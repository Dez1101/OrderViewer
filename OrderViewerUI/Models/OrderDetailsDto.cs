namespace OrderViewerUI.Models
{
    public class OrderDetailsDto : OrderDto
    {
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
