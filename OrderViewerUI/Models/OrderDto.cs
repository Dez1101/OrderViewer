namespace OrderViewerUI.Models
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsPaid { get; set; }
    }
}
