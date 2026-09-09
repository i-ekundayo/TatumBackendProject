using TatumBackendProject.Common.Models;
using TatumBackendProject.Entities;

namespace TatumBackendProject.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<List<Account>> GetFilteredAsync(
            Guid? id = null,
            Guid? customerId = null,
            string? accountNumber = null);

        Task<IEnumerable<Account>> GetByCustomerIdAsync(
            IEnumerable<Guid> customerIds,
            CancellationToken ct = default);

        Task<PagedResult<Account>>
            GetPagedAsync(
                Guid? id,
                Guid? customerId,
                string? accountNumber,
                PaginationParameters pagination,
                CancellationToken ct = default);
    }
}
