using APICustomerFeedbackSystem.EF;
using APICustomerFeedbackSystem.Models;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APICustomerFeedbackSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedback _feedback;
        private readonly CustomerFeedbackSystemContext _context;

        public FeedbackController(IFeedback feedback, CustomerFeedbackSystemContext context)
        {
            _feedback = feedback;
            _context = context;
        }
        // GET: api/<FeedbackController>
        [HttpGet]
        public IEnumerable<Feedback> Get()
        {
            //List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();
            var feedbacks = _feedback.GetAll();
            return feedbacks;
        }

        // GET api/<FeedbackController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FeedbackController>
        [HttpPost]
        public ActionResult Post(Feedback feedback)
        {
            try
            {
                Feedback fb = new Feedback
                {
                    Status = feedback.Status
                };
                var result = _feedback.Add(feedback);

                return CreatedAtAction(nameof(Get),feedback);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<FeedbackController>/5
        [HttpPut("{id}")]
        public IActionResult UpdateFeedback(int id,Feedback feedback)
        {
            try
            {
                var existingFeedback = _context.Feedbacks.FirstOrDefault(f => f.FeedbackId == id);
                if (existingFeedback == null)
                {
                    return NotFound();
                }

                existingFeedback.Status = feedback.Status;
                _context.SaveChanges(); // Save changes to database

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE api/<FeedbackController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        [HttpGet("ScheduleRecurringJob")]
        public string ScheduleRecurringJob()
        {
            RecurringJob.AddOrUpdate("recurring-job", () => Console.WriteLine("Review the feedback !!!"), Cron.Minutely);
            return "Recurring Job has been scheduled";
        }
    }
}
