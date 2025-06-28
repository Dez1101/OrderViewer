using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderViewer.API.DTOs;
using OrderViewer.API.Responses;
using OrderViewer.API.Services;

namespace OrderViewer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> GetFilteredOrders()
        {
            var result = await _service.GetFilteredOrdersAsync();
            return Ok(ApiResponse<List<OrderDto>>.SuccessResponse(result));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOrderDetails(Guid id)
        {
            var result = await _service.GetOrderDetailsAsync(id);
            if (result == null)
                return NotFound(ApiResponse<string>.Fail("Order not found"));
            return Ok(ApiResponse<OrderDetailsDto>.SuccessResponse(result));
        }
        
    }

}
