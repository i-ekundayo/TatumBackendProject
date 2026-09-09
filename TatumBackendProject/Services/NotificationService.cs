using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using TatumBackendProject.Common.Constants;
using TatumBackendProject.Repositories;
using TatumBackendProject.Responses;

namespace TatumBackendProject.Services
{
    public class NotificationService: INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmailSender _emailSender;
        private readonly HttpClient _httpClient;
        private readonly SmsSettings _smsSetings;
        public NotificationService(
            INotificationRepository notificationRepository, IEmailSender emailSender, IOptions<SmsSettings> smsOptions, HttpClient httpClient)
        {
            _notificationRepository = notificationRepository;
            _emailSender = emailSender;
            _smsSetings = smsOptions.Value;
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<object>> SendEmailAsync(
            string email, 
            string subject, 
            string htmlMessage,
            CancellationToken ct = default)
        {
            try
            {
                await _emailSender.SendAsync(
                    email,
                    subject,
                    htmlMessage,
                    ct);

                return ApiResponse<object>.Ok(
                    new
                    {
                        sent = true
                    },
                    "Email sent successfully.");
            }
            catch (Exception ex) 
            {
                return ApiResponse<object>.Fail(
                    "Faild to send email.",
                    new List<ApiError>
                    {
                        new(
                            "EmailSendFailed",
                            ex.Message)
                    });
            }
        }

        //public async Task<ApiResponse<object>> 
        //    SendAsync(
        //    )

        public async Task<ApiResponse<object>> SendSmsAsync(string phoneNumber, string message, CancellationToken ct)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    return ApiResponse<object>.Fail("Phone number is required.", new List<ApiError> { new("InvalidPhoneNumber", "A valid phone number is required.") });
                }

                var payload = new { api_key = _smsSetings.ApiKey, to = phoneNumber, from = _smsSetings.SenderId, sms = message, type = "plain", channel = "generic" };
                var json = JsonSerializer.Serialize(payload);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_smsSetings.BaseUrl}/sms/api/sms/send", content);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return ApiResponse<object>.Fail("Failed to send SMS.", new List<ApiError> { new("SmsSendFailed", error) });
                }

                return ApiResponse<object>.Ok((object?)"SMS sent successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.Fail("An error occrred while sending the SMS.", new List<ApiError> { new("SmsException", ex.Message) });
            }
        }
    }
}
