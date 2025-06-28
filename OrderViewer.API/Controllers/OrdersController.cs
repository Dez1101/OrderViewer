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

        [HttpPost("filter")]
        public async Task<IActionResult> GetFilteredOrders([FromBody] FilterOrdersDto filter)
        {
            var result = await _service.GetFilteredOrdersAsync(filter);
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

        [HttpPost("{id}/mark-paid")]
        public async Task<IActionResult> MarkAsPaid(Guid id)
        {
            var success = await _service.MarkAsPaidAsync(id);
            if (!success)
                return NotFound(ApiResponse<string>.Fail("Order not found"));
            return Ok(ApiResponse<string>.SuccessResponse("Order marked as paid"));
        }

    }

}
