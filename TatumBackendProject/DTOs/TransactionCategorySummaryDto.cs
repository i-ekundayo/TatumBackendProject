namespace TatumBackendProject.DTOs
{
    public class TransactionCategorySummaryDto
    {
        public string Category { get; set; } = string.Empty;

        public int Count { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal PercentageOfTransactions { get; set; }
    }
}
