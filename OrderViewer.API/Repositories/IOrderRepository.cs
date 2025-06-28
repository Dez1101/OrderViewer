using OrderViewer.API.DTOs;
using OrderViewer.API.Models;

namespace OrderViewer.API.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetFilteredOrdersAsync(FilterOrdersDto filter);
        Task<Order?> GetByIdAsync(Guid id);
    }
}
