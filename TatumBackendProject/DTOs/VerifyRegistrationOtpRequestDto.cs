using System.ComponentModel.DataAnnotations;

namespace TatumBackendProject.DTOs
{
    public class VerifyRegistrationOtpRequestDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "A valid email address is required.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Verification code is required.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Verification code must be 6 digits.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Verification code must contain only digits.")]
        public string Otp { get; set; } = string.Empty;
    }
}
