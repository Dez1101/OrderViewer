namespace OrderViewer.API.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CustomerName { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public decimal Total { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsPaid { get; set; } = false;

        public List<OrderItem> Items { get; set; } = new();
    }
}
