namespace TatumBackendProject.DTOs
{
    public class TransactionResponseDto
    {
        public Guid Id { get; set; }

        public string Reference { get; set; } = null!;

        public string Type { get; set; } = null!;

        public string Status { get; set; } = null!;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = null!;

        public string? BillerName { get; set; }

        public string? ProductName { get; set; }

        public string? ProductItemName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? CompletedAt { get; set; }
    }
}
