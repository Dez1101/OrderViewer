using OrderViewer.API.DTOs;

namespace OrderViewer.API.Services
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetFilteredOrdersAsync();
        Task<OrderDetailsDto?> GetOrderDetailsAsync(Guid id);
    }
}
