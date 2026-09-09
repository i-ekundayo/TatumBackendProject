using TatumBackendProject.Common.Models;
using TatumBackendProject.Entities;

namespace TatumBackendProject.Repositories
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);
        Task<User?> GetUserWithTokenAsync(Guid userId);
        Task<User?> GetByPasswordSetupTokenAsync(string token);
        Task<bool> ExistsByStaffIdAsync(string staffId);
        Task<PagedResult<User>>
        GetPagedAsync(
            Guid? userId,
            PaginationParameters pagination,
            CancellationToken ct = default);
    }
}