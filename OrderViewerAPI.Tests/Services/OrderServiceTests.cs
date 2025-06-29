using Xunit;
using Moq;
using OrderViewer.API.Models;
using OrderViewer.API.DTOs;
using OrderViewer.API.Services;
using OrderViewer.API.Repositories;

namespace OrderViewerAPI.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _mockRepo;
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _mockRepo = new Mock<IOrderRepository>();
            _service = new OrderService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetFilteredOrdersAsync_ReturnsMappedDtos()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { Id = Guid.NewGuid(), CustomerName = "Alice", Status = "Pending", Total = 100, CreatedDate = DateTime.UtcNow },
                new Order { Id = Guid.NewGuid(), CustomerName = "Bob", Status = "Shipped", Total = 150, CreatedDate = DateTime.UtcNow }
            };

            var filter = new FilterOrdersDto { Statuses = new[] { "Pending", "Shipped" } };
            _mockRepo.Setup(r => r.GetFilteredOrdersAsync(filter)).ReturnsAsync(orders);

            // Act
            var result = await _service.GetFilteredOrdersAsync(filter);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, o => o.CustomerName == "Alice");
            Assert.Contains(result, o => o.CustomerName == "Bob");
        }

        [Fact]
        public async Task GetOrderDetailsAsync_ReturnsMappedOrderDetails()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                Id = orderId,
                CustomerName = "Emily",
                Status = "Processing",
                Total = 300,
                CreatedDate = DateTime.UtcNow,
                Items = new List<OrderItem>
                {
                    new OrderItem { ProductName = "Keyboard", Quantity = 1, Price = 100 },
                    new OrderItem { ProductName = "Mouse", Quantity = 2, Price = 100 }
                }
            };

            _mockRepo.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(order);

            // Act
            var result = await _service.GetOrderDetailsAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Emily", result.CustomerName);
            Assert.Equal(2, result.Items.Count);
        }

        [Fact]
        public async Task GetOrderDetailsAsync_OrderNotFound_ReturnsNull()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync((Order?)null);

            // Act
            var result = await _service.GetOrderDetailsAsync(orderId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task MarkAsPaidAsync_ValidId_ReturnsTrue()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order { Id = orderId };
            _mockRepo.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(order);
            _mockRepo.Setup(r => r.MarkAsPaidAsync(orderId)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.MarkAsPaidAsync(orderId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task MarkAsPaidAsync_InvalidId_ReturnsFalse()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync((Order?)null);

            // Act
            var result = await _service.MarkAsPaidAsync(orderId);

            // Assert
            Assert.False(result);
        }
    }
}
