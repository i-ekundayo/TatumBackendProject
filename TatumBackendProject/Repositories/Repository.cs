using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TatumBackendProject.Common.Models;
using TatumBackendProject.Data;

namespace TatumBackendProject.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;
        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        //==============================
        // GET BY ID
        //==============================

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        //==============================
        // GET ALL
        //==============================
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }

        //==============================
        // ADD
        //==============================
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        //==============================
        // UPDATE
        //==============================
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        //==============================
        // DELETE
        //==============================
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        //=============================
        // COUNT
        //=============================
        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }
        public async Task<int> CountAsync(
            Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        //=============================
        // EXISTS
        //=============================
        public async Task<bool> ExistsAsync(
            Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        //=============================
        // PAGINATION
        //=============================
        public async Task<PagedResult<T>> GetPagedAsync(
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken ct = default)
        {
            //Protect against invalid values
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            //Optional maximum page size
            if (pageSize > 100)
            {
                pageSize = 100;
            }

            var query = _dbSet.AsNoTracking();
            var totalCount = await query.CountAsync(ct);
            var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)pageSize);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
            return new PagedResult<T>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
            };
        }

        //=============================
        // PAGINATION WITH FILTER
        //=============================
        public async Task<PagedResult<T>> GetPagedAsync(
            Expression<Func<T, bool>> predicate,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken ct = default)
        {
            //Protect against invalid values
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;
            //Optional maximum page size
            if (pageSize > 100)
            {
                pageSize = 100;
            }
            var query = _dbSet.AsNoTracking().Where(predicate);
            var totalCount = await query.CountAsync(ct);
            var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)pageSize);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
            return new PagedResult<T>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
            };
        }

        public async Task<int> SaveChangesAsync(
            CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }
    }
}
