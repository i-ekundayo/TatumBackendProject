namespace TatumBackendProject.DTOs
{
    public class TransactionBillerSummaryDto
    {
        public Guid BillerId { get; set; }

        public string BillerName { get; set; } = string.Empty;

        public string BillerCode { get; set; } = string.Empty;

        public int Count { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal PercentageOfTransactions { get; set; }
    }
}
