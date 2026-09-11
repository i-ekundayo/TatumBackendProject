namespace TatumBackendProject.DTOs
{
    public class TransactionSummaryDto
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int TotalTransactions { get; set; }

        public int SuccessfulTransactions { get; set; }

        public int FailedTransactions { get; set; }

        public int PendingTransactions { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal SuccessfulAmount { get; set; }

        public decimal FailedAmount { get; set; }

        public decimal PendingAmount { get; set; }

        public PercentageChangeDto
            TransactionCountChange
        { get; set; }
            = new();

        public PercentageChangeDto
            TransactionAmountChange
        { get; set; }
            = new();

        public List<TransactionCategorySummaryDto>
            ByCategory
        { get; set; } = new();

        public List<TransactionBillerSummaryDto>
            ByBiller
        { get; set; } = new();
    }
}
