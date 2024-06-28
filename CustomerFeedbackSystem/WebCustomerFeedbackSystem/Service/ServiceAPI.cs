using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebCustomerFeedbackSystem.Models;

namespace WebCustomerFeedbackSystem.Service
{
    public class ServiceAPI
    {
        private readonly HttpClient _httpClient;

        public ServiceAPI(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Feedback>> GetFeedbackAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7009/api/Feedback");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var feedbacks = JsonSerializer.Deserialize<IEnumerable<Feedback>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return feedbacks;
            }
            catch (Exception ex)
            {
                // Logging atau penanganan kesalahan
                Console.WriteLine($"Error fetching data from API: {ex.Message}");
                return new List<Feedback>();
            }
        }
        public async Task<bool> UpdateFeedbackStatusAsync(int id, string status)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { Status = status }), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"https://localhost:7009/api/Feedback/{id}", content);

            return response.IsSuccessStatusCode;
        }
    }
}
