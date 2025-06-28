using Microsoft.EntityFrameworkCore;
using OrderViewer.API.Models;

namespace OrderViewer.API.Data
{
    public class OrderViewerDbContext : DbContext
    {
        public OrderViewerDbContext(DbContextOptions<OrderViewerDbContext> options) : base(options) { }

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    }
}
