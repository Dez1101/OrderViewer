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
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.Fail("Invalid filter parameters"));

                var result = await _service.GetFilteredOrdersAsync(filter);
                return Ok(ApiResponse<List<OrderDto>>.SuccessResponse(result));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<string>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                // log
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<string>.Fail($"An error occurred: {ex.Message}"));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOrderDetails(Guid id)
        {
            try
            {
                var result = await _service.GetOrderDetailsAsync(id);
                if (result == null)
                    return NotFound(ApiResponse<string>.Fail("Order not found"));
                return Ok(ApiResponse<OrderDetailsDto>.SuccessResponse(result));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<string>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                // Log 
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<string>.Fail($"An error occurred: {ex.Message}"));
            }
        }

        [HttpPost("{id}/mark-paid")]
        public async Task<IActionResult> MarkAsPaid(Guid id)
        {
            try
            {
                var success = await _service.MarkAsPaidAsync(id);
                if (!success)
                    return NotFound(ApiResponse<string>.Fail("Order not found"));
                return Ok(ApiResponse<string>.SuccessResponse("Order marked as paid"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<string>.Fail(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<string>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                // Log
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<string>.Fail($"An error occurred: {ex.Message}"));
            }
        }
    }

}
