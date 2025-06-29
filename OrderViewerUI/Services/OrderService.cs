using OrderViewerUI.Models;
using OrderViewerUI.Responses;
using System.Net.Http.Json;

namespace OrderViewerUI.Services
{
    public class OrderService
    {
        private readonly HttpClient _http;

        public OrderService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<OrderDto>> GetOrdersAsync(FilterOrdersDto filter)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/orders/filter", filter);

                if (!response.IsSuccessStatusCode)
                {
                    return new List<OrderDto>();
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<OrderDto>>>();
                return result?.Data ?? new List<OrderDto>();
            }
            catch (Exception ex)
            {
                //Console.Error.WriteLine($"Error fetching orders: {ex.Message}");
                return new List<OrderDto>();
            }
        }

        public async Task<OrderDetailsDto?> GetOrderDetailsAsync(Guid id)
        {
            try
            {
                var result = await _http.GetFromJsonAsync<ApiResponse<OrderDetailsDto>>($"api/orders/{id}");
                return result?.Data;
            }
            catch (Exception ex)
            {
                //Console.Error.WriteLine($"Error fetching order details: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> MarkAsPaidAsync(Guid id)
        {
            try
            {
                var response = await _http.PostAsync($"api/orders/{id}/mark-paid", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                //Console.Error.WriteLine($"Error marking order as paid: {ex.Message}");
                return false;
            }
        }
    }
}
