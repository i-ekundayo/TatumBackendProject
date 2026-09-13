using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using TatumBackendProject.Auth;
using TatumBackendProject.Common.Constants;
using TatumBackendProject.DTOs;
using TatumBackendProject.Entities;
using TatumBackendProject.Repositories;
using TatumBackendProject.Responses;

namespace TatumBackendProject.Services
{
    public interface IAuthService
    {
        //Task<ApiResponse<LoginResponseDto>> LoginAsync(
        //    LoginRequestDto request,
        //    CancellationToken ct = default);
        
        Task<ApiResponse<UserDto>> RegisterAsync(
            RegisterRequestDto request,
            CancellationToken ct= default);

        //Task<ApiResponse<UserDto>> VerifyRegistrationOtpAsync(
        //    VerifyRegistrationOtpRequestDto request,
        //    CancellationToken ct= default);
    }
    public class AuthService: IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly JwtService _jwtService;
        private readonly JwtSettings _settings;
        private readonly INotificationService _notificationService;
        private readonly EmailSettings _emailSettings;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserRepository userRepository, IConfiguration configuration, 
            JwtService jwtService, IOptions<EmailSettings> emailOptions, IAccountRepository accountRepository,
            IOptions<JwtSettings> options, INotificationService notificationService)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _jwtService = jwtService;
            _settings = options.Value;
            _notificationService = notificationService;
            _emailSettings = emailOptions.Value;
            _configuration = configuration;
        }

        public async Task<ApiResponse<UserDto>> RegisterAsync(RegisterRequestDto request, CancellationToken ct = default)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var phone = request.Phone.Trim();

            // ===============================
            // VALIDATION
            // ===============================

            if (string.IsNullOrWhiteSpace(email)) {
                return ApiResponse<UserDto>.Fail(
                    "Email is required.",
                    new List<ApiError>
                    {
                        new(
                            "InvalidEmail",
                            "Email is required.")
                    });
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                return ApiResponse<UserDto>.Fail(
                    "Phone Number is required.",
                    new List<ApiError>
                    {
                        new(
                            "InvalidPhone",
                            "Phone number is required.")

                    });
            }

            // ===============================
            // CHECK EXISTING USER
            // ===============================

            var existingUser = await _userRepository.GetByEmailAsync(email);

                
            if(existingUser != null)
            // Allow an unverified registration to reques
            // another OTP instead of creating another user.
            {
                if (!existingUser.IsRegistrationVerified)
                {
                    return ApiResponse<UserDto>.Fail(
                        "Registration is already pending varification.",
                        new List<ApiError>
                        {
                            new(
                                "RegistrationPending",
                                "A registration already exists for thies email. Please verify the OTP.")
                        });
                }

                return ApiResponse<UserDto>.Fail(
                    "An account with this email already exists.",
                    new List<ApiError> {
                        new(
                            "EmailExists",
                            "An account with this email already exists.")
                    });
                }

                // ==============================
                // GENERATE OTP
                // ==============================

                var otp = GenerateOtp();

                var user = new Entities.User
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    Phone = phone,
                    FirstName = request.FirstName?.Trim(),
                    LastName = request.LastName?.Trim(),
                    PasswordHash = HashPassword(request.Password),
                    Role = UserRoles.Customer,

                    // IMPORTANT:
                    // Customer is not active until OTP verification.
                    IsActive = false,
                    IsRegistrationVerified = false,
                    RegistrationOtp = otp,
                    RegistrationOtpExpiresAt = DateTime.UtcNow.AddMinutes(10),
                    OtpAttempts = 0,
                    CreatedAt = DateTime.UtcNow,
                };

                await _userRepository.AddAsync(user);

                await _userRepository.SaveChangesAsync(ct);

                // ==============================
                // SEND OTP
                // ==============================

                await SendRegistrationOtpAsync(
                    user,
                    otp,
                    ct);

                // ==============================
                // RESPONSE
                // ==============================

                return ApiResponse<UserDto>.Ok(
                    MapToDto(user),
                    "Registration initiated successfully. " +
                    "A verification code has been sent to your eamil and phone.");
            }

            private static string GenerateOtp()
            {
                return RandomNumberGenerator
                    .GetInt32(100000, 1000000)
                    .ToString();
            }

            private async Task SendRegistrationOtpAsync(
                Entities.User user,
                string otp,
                CancellationToken ct)
            {
                var subject = "TatumConnect Registration Verification";
                var message = $"""
                    Hello {user.FirstName},
               
                    Welcome to TatumConnect.

                    Your registration code is: 
                
                    {otp}

                    this code expires in 10 minutes.

                    If you did not initiate this registration, pelase ignore this message.
                
                    Regards,
                    TatumConnect
                    """;

                // ==============================
                // EMAIL
                // ==============================

                await _notificationService.SendEmailAsync(
                    user.Email,
                    subject,
                    message,
                    ct);

                // ==============================
                // PHONE / SMS
                // ==============================

                // await _notificationService.SendSmsAsync(
                //     user.Phone!,
                //     $"Your TatumConnect verification code is {otp}. It expires in 10 minutes.",
                //      ct)
            }
        

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Department = user.Department,
                ProfileImageUrl = user.ProfileImageUrl,
                StaffId = user.StaffId,
                Role = user.Role,
                RegistrationOtp = user.RegistrationOtp,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
            };
        }

        // ==============================
        // PASSWORD HASH
        // ==============================

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private static bool VerifyPassword(string password, string passwordHash)
        {
            return HashPassword(password) == passwordHash;
        }

        private async Task<string> GenerateAccountNumberAsync(){
            while (true) {
                var accountNumber = RandomNumberGenerator.GetInt32(10000000, 100000000).ToString();

                //var exists = await _accountRepository.ExistsByAccountNumberAsync(accountNumber);

                var exists = await _accountRepository.GetFilteredAsync(accountNumber: accountNumber);

                if (!exists.Any())
                {
                    return accountNumber;
                }
            }
        }
    }
}
