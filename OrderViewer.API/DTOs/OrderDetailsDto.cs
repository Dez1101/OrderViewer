namespace OrderViewer.API.DTOs
{
    public class OrderDetailsDto : OrderDto
    {
        public List<OrderItemDto> Items { get; set; } = new();
    }

}
