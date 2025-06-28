using Microsoft.EntityFrameworkCore;
using OrderViewer.API.Data;
using OrderViewer.API.DTOs;
using OrderViewer.API.Models;

namespace OrderViewer.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderViewerDbContext _context;

        public OrderRepository(OrderViewerDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetFilteredOrdersAsync(FilterOrdersDto filter)
        {
            var query = _context.Orders.Include(o => o.Items).AsQueryable();

            if (filter.StartDate.HasValue)
                query = query.Where(o => o.CreatedDate >= filter.StartDate.Value);
            if (filter.EndDate.HasValue)
                query = query.Where(o => o.CreatedDate <= filter.EndDate.Value);
            if (filter.Statuses?.Any() == true)
                query = query.Where(o => filter.Statuses.Contains(o.Status));
            if (filter.MinTotal.HasValue)
                query = query.Where(o => o.Total >= filter.MinTotal.Value);
            if (filter.MaxTotal.HasValue)
                query = query.Where(o => o.Total <= filter.MaxTotal.Value);

            return await query.ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(Guid id) =>
            await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);

        public async Task MarkAsPaidAsync(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order is not null)
            {
                order.IsPaid = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
