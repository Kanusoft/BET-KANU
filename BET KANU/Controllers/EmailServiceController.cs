using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BET_KANU.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BET_KANU.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailServiceController : ControllerBase
    {
        private readonly EmailService _emailService;
        private readonly ILogger<EmailServiceController> _logger;

        private static readonly HashSet<string> AllowedHelpTypes = new(StringComparer.Ordinal)
        {
            "Sponsor a project",
            "Provide ongoing funding",
            "Offer services or resources",
            "Collaborate",
            "Other"
        };

        public EmailServiceController(
            EmailService emailService,
            ILogger<EmailServiceController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        // Existing Contact Us route: POST /api/EmailService
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmailModel value)
        {
            if (value == null ||
                string.IsNullOrWhiteSpace(value.email) ||
                string.IsNullOrWhiteSpace(value.message))
            {
                return BadRequest(new
                {
                    result = false,
                    message = "Please provide a valid email and message."
                });
            }

            try
            {
                // Do not use Task.Run: wait for SendGrid to accept or reject the request.
                await _emailService.SendMailAsync(value);
                return Ok(new
                {
                    result = true,
                    message = "Thank you! Your message has been submitted."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BET KANU contact email submission failed");
                return StatusCode(502, new
                {
                    result = false,
                    message = "Unable to send your message right now. Please try again later."
                });
            }
        }

        // Keep BOTH existing partnership routes:
        // POST /api/EmailService/partnership
        // POST /api/partnership-inquiry  <-- your existing modal URL
        [HttpPost("partnership")]
        [HttpPost("/api/partnership-inquiry")]
        public async Task<IActionResult> SubmitPartnership([FromBody] PartnershipInquiryModel model)
        {
            if (model == null ||
                string.IsNullOrWhiteSpace(model.Name) ||
                string.IsNullOrWhiteSpace(model.Organization) ||
                string.IsNullOrWhiteSpace(model.Email))
            {
                return BadRequest(new
                {
                    result = false,
                    message = "Please provide your name, organization name, and a valid email."
                });
            }

            if (model.HelpTypes != null &&
                (model.HelpTypes.Count > AllowedHelpTypes.Count ||
                 model.HelpTypes.Any(x => x == null || !AllowedHelpTypes.Contains(x))))
            {
                return BadRequest(new
                {
                    result = false,
                    message = "One or more selected partnership options are invalid."
                });
            }

            try
            {
                await _emailService.SendPartnershipInquiryAsync(model);

                return Ok(new
                {
                    result = true,
                    message = "Thank you for your interest in supporting BET KANU! Our team will contact you to discuss the possibilities."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BET KANU partnership email submission failed");
                return StatusCode(502, new
                {
                    result = false,
                    message = "Unable to send your partnership inquiry right now. Please try again later."
                });
            }
        }
    }

    // Existing payload shape preserved: Name, Organization, Email, HelpTypes, Message
    public class PartnershipInquiryModel
    {
        [Required, StringLength(120)]
        public string? Name { get; set; }

        [Required, StringLength(160)]
        public string? Organization { get; set; }

        [Required, EmailAddress, StringLength(254)]
        public string? Email { get; set; }

        public List<string>? HelpTypes { get; set; }

        [StringLength(4000)]
        public string? Message { get; set; }
    }

    // Existing contact form JSON property names preserved.
    public class EmailModel
    {
        [StringLength(120)]
        public string? name { get; set; }

        [Required, EmailAddress, StringLength(254)]
        public string? email { get; set; }

        [StringLength(180)]
        public string? subject { get; set; }

        // Kept for backward compatibility, but a public form may NOT set recipients.
        public string? toEmail { get; set; }

        [Required, StringLength(4000)]
        public string? message { get; set; }
    }
}
