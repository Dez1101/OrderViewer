using OrderViewer.API.DTOs;
using OrderViewer.API.Repositories;

namespace OrderViewer.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;

        public OrderService(IOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<OrderDto>> GetFilteredOrdersAsync(FilterOrdersDto filter)
        {
            var orders = await _repo.GetFilteredOrdersAsync(filter);
            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                CustomerName = o.CustomerName,
                Status = o.Status,
                Total = o.Total,
                CreatedDate = o.CreatedDate,
                IsPaid = o.IsPaid
            }).ToList();
        }

        public async Task<OrderDetailsDto?> GetOrderDetailsAsync(Guid id)
        {
            var order = await _repo.GetByIdAsync(id);
            if (order == null) return null;

            return new OrderDetailsDto
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                Status = order.Status,
                Total = order.Total,
                CreatedDate = order.CreatedDate,
                IsPaid = order.IsPaid,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };
        }

    }

}
