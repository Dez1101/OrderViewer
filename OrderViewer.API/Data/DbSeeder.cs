using OrderViewer.API.Models;

namespace OrderViewer.API.Data
{
    public static class DbSeeder
    {
        public static void Seed(OrderViewerDbContext context)
        {
            if (context.Orders.Any()) return;

            var rand = new Random();
            var statuses = new[] { "Pending", "Processing", "Shipped", "Cancelled" };
            var customers = new[]
            {
            "John Smith", "Alice Johnson", "Michael Brown", "Emily Davis",
            "David Wilson", "Sophia Taylor", "James Anderson", "Olivia Martin",
            "William Thomas", "Isabella Moore"
        };
            var products = new[]
            {
            "Bluetooth Speaker", "Gaming Mouse", "Mechanical Keyboard",
            "Wireless Headphones", "USB-C Cable", "Webcam", "Portable SSD",
            "Laptop Stand", "Smartwatch", "LED Desk Lamp"
        };

            for (int i = 1; i <= 50; i++)
            {
                var order = new Order
                {
                    CustomerName = customers[rand.Next(customers.Length)],
                    Status = statuses[rand.Next(statuses.Length)],
                    CreatedDate = DateTime.UtcNow.AddDays(-rand.Next(0, 30)),
                    Total = 0,
                    Items = new List<OrderItem>()
                };

                int itemCount = rand.Next(1, 5);
                for (int j = 0; j < itemCount; j++)
                {
                    var quantity = rand.Next(1, 4);
                    var price = (decimal)(rand.NextDouble() * 300 + 50); // 50–350
                    var productName = products[rand.Next(products.Length)];

                    order.Items.Add(new OrderItem
                    {
                        ProductName = productName,
                        Quantity = quantity,
                        Price = Math.Round(price, 2)
                    });

                    order.Total += quantity * price;
                }

                order.Total = Math.Round(order.Total, 2);
                context.Orders.Add(order);
            }

            context.SaveChanges();
        }
    }

}
