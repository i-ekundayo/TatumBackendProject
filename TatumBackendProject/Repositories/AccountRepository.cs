using Microsoft.EntityFrameworkCore;
using TatumBackendProject.Common.Models;
using TatumBackendProject.Data;
using TatumBackendProject.Entities;

namespace TatumBackendProject.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Account>> GetFilteredAsync(
            Guid? id = null,
            Guid? customerId = null,
            string? accountNumber = null)
        {
            var query = _dbSet.AsNoTracking().AsQueryable();
            if (id.HasValue)
                query = query.Where(a => a.Id == id.Value);
            if (customerId.HasValue)
                query = query.Where(a => a.CustomerId == customerId.Value);
            if (!string.IsNullOrEmpty(accountNumber))
                accountNumber = accountNumber.Trim();
                query = query.Where(a => a.AccountNumber == accountNumber);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Account>> GetByCustomerIdAsync(
            IEnumerable<Guid> customerIds,
            CancellationToken ct = default)
        {
            var ids = customerIds.ToList();

            if(!ids.Any())
            {
                return Enumerable.Empty<Account>();
            }
            return await _dbSet
                .AsNoTracking()
                .Where(a => ids.Contains(a.CustomerId))
                .ToListAsync(ct);
        }

        public async Task<PagedResult<Account>> GetPagedAsync(
            Guid? id,
            Guid? customerId,
            string? accountNumber,
            PaginationParameters pagination,
            CancellationToken ct = default)
        {
            var query = _context.Accounts.AsNoTracking().AsQueryable();

            // ==============================
            // ACCOUNT ID
            // ==============================
            if (id.HasValue)
                query = query.Where(a => a.Id == id.Value);

            // ==============================
            // Customer ID
            // ==============================
            if (customerId.HasValue)
                query = query.Where(x => x.CustomerId == customerId.Value);

            // ==============================
            // Account Number
            // ==============================
            if (!string.IsNullOrEmpty(accountNumber))
            {
                var number = accountNumber.Trim();
                query = query.Where(x => x.AccountNumber == number);
            }
                
            // ==============================
            // Count
            // ==============================
            var totalCount = await query.CountAsync(ct);

            // ==============================
            // Pages
            // ==============================
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize);

            // =============================
            // Data
            // =============================
            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync(ct);

            return new PagedResult<Account>
            {
                Items = items,
                PageNumber = pagination.PageNumber,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
        }
    }
}
