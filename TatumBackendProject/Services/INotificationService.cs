using TatumBackendProject.DTOs;
using TatumBackendProject.Responses;

namespace TatumBackendProject.Services
{
    public interface INotificationService
    {
        //Task<ApiResponse<NotificationDto>> SendAsync(
        //    Guid customerId,
        //    string subject,
        //    string message,
        //    string type = "General",
        //    CancellationToken c = default);

        //Task<ApiResponse<object>> SendAsync(
        //    Guid customerId,
        //    string subject,
        //    string message);

        Task<ApiResponse<object>> SendEmailAsync(
            string email,
            string subject,
            string htmlMessage,
            CancellationToken c = default);

        Task<ApiResponse<object>> SendSmsAsync(
            string phone,
            string message,
            CancellationToken c = default);

        //Task<ApiResponse<NotificationDto>> GetByCustomerAsync(
        //    Guid customerId,
        //    CancellationToken c = default);


    }
}
