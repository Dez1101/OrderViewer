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
            try
            {
                if (filter == null)
                    throw new ArgumentNullException(nameof(filter), "Filter cannot be null");

                if (filter.StartDate.HasValue && filter.EndDate.HasValue && filter.StartDate > filter.EndDate)
                    throw new ArgumentException("StartDate cannot be later than EndDate");

                if (filter.MinTotal.HasValue && filter.MaxTotal.HasValue && filter.MinTotal > filter.MaxTotal)
                    throw new ArgumentException("MinTotal cannot be greater than MaxTotal");

                if (filter.Statuses?.Any(s => string.IsNullOrWhiteSpace(s)) == true)
                    throw new ArgumentException("Statuses cannot contain empty or whitespace values");

                var validSortFields = new[] { "customername", "createddate", "total" };
                if (!string.IsNullOrWhiteSpace(filter.SortBy) &&
                    !validSortFields.Contains(filter.SortBy.ToLower()))
                {
                    filter.SortBy = "createddate"; // default fallback
                    filter.SortDirection = "asc";
                }

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
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve filtered orders: {ex.Message}", ex);
            }
        }

        public async Task<OrderDetailsDto?> GetOrderDetailsAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new ArgumentException("Invalid order ID", nameof(id));

                var order = await _repo.GetByIdAsync(id);
                if (order == null)
                    return null;

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
            catch (Exception ex)
            {
                // Log
                throw new InvalidOperationException($"Failed to retrieve order details: {ex.Message}", ex);
            }
        }

        public async Task<bool> MarkAsPaidAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new ArgumentException("Invalid order ID", nameof(id));

                var order = await _repo.GetByIdAsync(id);
                if (order == null)
                    return false;

                if (order.IsPaid)
                    throw new InvalidOperationException("Order is already marked as paid");

                await _repo.MarkAsPaidAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                // Log 
                throw new InvalidOperationException($"Failed to mark order as paid: {ex.Message}", ex);
            }
        }
    }
}
