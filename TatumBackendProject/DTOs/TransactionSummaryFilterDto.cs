using TatumBackendProject.Common.Constants;

namespace TatumBackendProject.DTOs
{
    public class TransactionSummaryFilterDto
    {
        public TransactionPeriod? Period { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
