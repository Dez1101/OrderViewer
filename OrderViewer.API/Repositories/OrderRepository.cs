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

        public async Task<List<Order>> GetFilteredOrdersAsync()
        {
            var query = _context.Orders.Include(o => o.Items).AsQueryable();

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
