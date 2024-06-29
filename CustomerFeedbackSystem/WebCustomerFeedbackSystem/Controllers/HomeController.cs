using Microsoft.AspNetCore.Mvc;
using WebCustomerFeedbackSystem.Models;
using WebCustomerFeedbackSystem.EF;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using System.Text;


namespace WebCustomerFeedbackSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFeedback _feedback;
        private readonly CustomerFeedbackSystemContext _context;

        private readonly IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger, IFeedback feedback, IHttpClientFactory clientFactory, CustomerFeedbackSystemContext context)
        {
            _logger = logger;
            _feedback = feedback;
            _clientFactory = clientFactory;
            _context = context;
        }

        [HttpPost]

        public async Task<IActionResult> ReviewFeedback(int id)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var feedback = _context.Feedbacks.FirstOrDefault(f => f.FeedbackId == id);
                if (feedback == null)
                {
                    _logger.LogError($"Feedback with id {id} not found.");
                    return NotFound();
                }

                feedback.Status = "Reviewed";

                var content = new StringContent(JsonConvert.SerializeObject(feedback), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"https://localhost:7009/api/Feedback/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return Ok();
                }
                else
                {
                    _logger.LogError($"Failed to update feedback with id {id}. Status code: {response.StatusCode}");
                    return StatusCode((int)response.StatusCode, "Error updating feedback status");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating feedback status.");
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7009/api/Feedback");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var fb = JsonConvert.DeserializeObject<List<Feedback>>(json);

                var feedbacks = _feedback.GetAll();
                ViewBag.feed = feedbacks;
                return View(fb);
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Error fetching data");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
