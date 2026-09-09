using System.ComponentModel.DataAnnotations;
using TatumBackendProject.Common.Constants;

namespace TatumBackendProject.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = null!;

        public string? PasswordHash { get; set; }

        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        [MaxLength(30)]
        public string? Phone {  get; set; }

        [MaxLength(150)]
        public string? Department { get; set; }

        [MaxLength(500)]
        public string? ProfileImageUrl {  get; set; }

        [MaxLength(30)]
        public string? StaffId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = UserRoles.Customer;


        /// <summary>
        /// Indicates whether the user has completed password setup.
        /// </summary>
        public bool IsActive { get; set; } = false;


        // ===============================
        // PASSWORD SETUP / INVITATION
        // ==============================

        /// <summary>
        /// One-time token used by invited admins to set their password.
        /// </summary>
        public string? PasswordSetupToken { get; set; }

        public DateTime? PasswordSetupTokenExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? LastLoginAt { get; set; }

        // ===============================
        // OTP / REGISTRATION VERIFICATION
        // ==============================

        /// <summary>
        /// One-time passsword used to verify customer registration.
        /// </summary>

        [MaxLength(10)]
        public string? RegistrationOtp { get; set; }

        /// <summary>
        /// Expiration time for the registration OTP
        /// </summary>
        public DateTime? RegistrationOtpExpiresAt { get; set; }

        /// <summary>
        /// Number of OTP verification attempts.
        /// </summary>
        public int OtpAttempts { get; set; }

        /// <summary>
        /// Expiration time for the password reset OTP
        /// </summary>
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiresAt { get; set; }
        public string? PasswordResetOtp { get; set; }
        public DateTime? PasswordResetOtpExpiresAt { get; set; }


        ///<summary>
        /// Indicates whether the custome's registration has been verified.
        /// </summary>
        public bool IsRegistrationVerified { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
