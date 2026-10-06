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

        [HttpPost("partnership")]
        [HttpPost("/api/partnership-inquiry")]
        public IActionResult SubmitPartnership([FromBody] PartnershipInquiryModel model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Organization))
                {
                    return BadRequest(new { result = false, message = "Please provide your name, organization name, and email address." });
                }

                var helpList = model.HelpTypes != null && model.HelpTypes.Any()
                    ? string.Join(", ", model.HelpTypes)
                    : "None selected";

                string subject = $"Partnership Inquiry: {model.Organization} ({model.Name})";
                string messageBody = $"New Partnership Inquiry Received:\n\n" +
                                     $"Name: {model.Name}\n" +
                                     $"Organization / Business: {model.Organization}\n" +
                                     $"Email: {model.Email}\n" +
                                     $"How would you like to help:\n  • {(model.HelpTypes != null && model.HelpTypes.Any() ? string.Join("\n  • ", model.HelpTypes) : "None selected")}\n\n" +
                                     $"Message:\n{(string.IsNullOrWhiteSpace(model.Message) ? "(No message provided)" : model.Message)}";

                var emailModel = new EmailModel
                {
                    name = model.Name,
                    email = model.Email,
                    subject = subject,
                    message = messageBody
                };

                _ = Task.Run(() =>
                {
                    try
                    {
                        Console.WriteLine($"[Partnership Form Received] From: {model.Name} - {model.Organization} ({model.Email})");
                        EmailService emailService = new EmailService();
                        emailService.SendMail(emailModel);
                        Console.WriteLine($"[Partnership Form Email Dispatched Successfully] To: info@betkanu.com");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Partnership Form Email Failed] {ex.Message}");
                    }
                });

                return Ok(new
                {
                    result = true,
                    message = "Thank you for your interest in supporting BET KANU! Our team will contact you to discuss the possibilities."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { result = false, message = ex.Message });
            }
        }
    }

    public class PartnershipInquiryModel
    {
        public string? Name { get; set; }
        public string? Organization { get; set; }
        public string? Email { get; set; }
        public List<string>? HelpTypes { get; set; }
        public string? Message { get; set; }
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

