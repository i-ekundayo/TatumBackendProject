using TatumBackendProject.Common.Constants;
using TatumBackendProject.Entities;

namespace TatumBackendProject.DTOs
{
    public class TransactionFilterDto
    {
        public Guid? TransactionId { get; set; }
        public Guid? AccountId { get; set; }

        public string? AccountNumber { get; set; }

        public Guid? UserId { get; set; }

        public Guid? BillerId { get; set; }

        public Guid? ProductId { get; set; }

        public TransactionStatus? Status { get; set; }

        public ProductCategory? Category { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
