using OrderViewer.API.DTOs;

namespace OrderViewer.API.Services
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetFilteredOrdersAsync(FilterOrdersDto filter);
        Task<OrderDetailsDto?> GetOrderDetailsAsync(Guid id);
        Task<bool> MarkAsPaidAsync(Guid id);
    }
}
