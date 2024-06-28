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

        public IActionResult ReviewFeedback(int id)
        {
            try
            {
                var feedback = _context.Feedbacks.FirstOrDefault(f => f.FeedbackId == id);
                if (feedback == null)
                {
                    _logger.LogError($"Feedback with id {id} not found.");
                    return NotFound();
                }

                feedback.Status = "Reviewed";

                // Manual update query
                _context.Database.ExecuteSqlRaw("UPDATE Feedback SET Status = {0} WHERE FeedbackId = {1}", feedback.Status, feedback.FeedbackId);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating feedback status.");
                return StatusCode(500, "Internal server error");
            }

            //Versi API;
            //try
            //{
            //    var client = _clientFactory.CreateClient();
            //    var feedBack = new Feedback
            //    {
            //        Status = fb.Status,
            //        FeedbackId = fb.FeedbackId,
            //        FeedbackText = fb.FeedbackText,
            //        CustomerId = fb.CustomerId,
            //    };
            //    var json = JsonConvert.SerializeObject(feedBack);
            //    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            //    var response = await client.PostAsync("https://localhost:7009/api/Feedback/", content);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        TempData["SuccessMessage"] = "Transaction created successfully.";
            //    }
            //    else
            //    {
            //        TempData["ErrorMessage"] = "Failed to create transaction.";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error creating transaction");
            //    TempData["ErrorMessage"] = "An error occurred while creating transaction.";
            //}

            //return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7009/api/Feedback");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var transactions = JsonConvert.DeserializeObject<List<Feedback>>(json);
                // Ambil data produk untuk dropdown
                var products = _feedback.GetAll();
                ViewBag.feed = products;
                return View(transactions);
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
