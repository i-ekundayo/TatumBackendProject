using TatumBackendProject.Data;
using TatumBackendProject.Entities;

namespace TatumBackendProject.Repositories
{
    public class NotificationRepository: Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(AppDbContext context): base(context) { }
    }
}
