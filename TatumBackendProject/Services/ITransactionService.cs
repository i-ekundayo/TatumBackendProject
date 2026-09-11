using TatumBackendProject.Common.Models;
using TatumBackendProject.DTOs;
using TatumBackendProject.Responses;

namespace TatumBackendProject.Services
{
    public interface ITransactionService
    {
        Task<ApiResponse<TransactionResponseDto>>
        PurchaseAsync(
            ProductPurchaseRequestDto request,
            CancellationToken ct = default);

        Task<ApiResponse<PagedResult<TransactionDto>>>
            GetTransactionsAsync(
                PaginationParameters pagination,
                TransactionFilterDto? filter = null);

        Task<ApiResponse<TransactionDto>>
            GetTransactionByIdAsync(Guid id);

        Task<ApiResponse<TransactionSummaryDto>>
            GetAdminTransactionSummaryAsync(
                TransactionSummaryFilterDto filter);
    }
}
