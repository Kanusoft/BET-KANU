using BET_KANU.Services;
using Microsoft.AspNetCore.Mvc;

namespace BET_KANU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailServiceController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] EmailModel value)
        {
            try
            {
                if (value == null || (string.IsNullOrWhiteSpace(value.email) && string.IsNullOrWhiteSpace(value.message)))
                {
                    return BadRequest(new { result = false, message = "Please provide your email and message." });
                }

                // Dispatch email sending in background task so UI doesn't block on SMTP network timeouts
                _ = Task.Run(() =>
                {
                    try
                    {
                        Console.WriteLine($"[Contact Form Received] From: {value.name} ({value.email}), Subject: {value.subject}");
                        EmailService emailService = new EmailService();
                        emailService.SendMail(value);
                        Console.WriteLine($"[Contact Form Email Dispatched Successfully] To: info@betkanu.com");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Contact Form Email Failed] {ex.Message}");
                    }
                });

                return Ok(new { result = true, message = "Thank you! Your message has been sent successfully. A copy has been delivered to info@betkanu.com." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { result = false, message = ex.Message });
            }
        }
    }

    public class EmailModel
    {
        public string? name { get; set; }

        public string? email { get; set; }

        public string? subject { get; set; }

        public string? toEmail { get; set; }

        public string? message { get; set; }
    }
}

