
using System.Net;
using BET_KANU.Controllers;
using Microsoft.AspNet.Identity; 
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BET_KANU.Services
{
    public class EmailService : IIdentityMessageService
    {
        private readonly SendGridClient _client;
        private readonly ILogger<EmailService> _logger;
        private readonly EmailAddress _from;
        private readonly EmailAddress _inbox;
        private readonly EmailAddress? _partnershipCc;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _logger = logger;

            var apiKey = configuration["SendGrid:ApiKey"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                apiKey = configuration["SENDGRID_API_KEY"];
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException(
                    "SendGrid API key is not configured.");
            }

            _client = new SendGridClient(apiKey.Trim());
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("SENDGRID_API_KEY is not configured.");

            _client = new SendGridClient(apiKey.Trim());
            _from = new EmailAddress(
                GetSetting(configuration, "SENDGRID_FROM_EMAIL", "info@kanusoft.com"),
                "BET KANU");
            _inbox = new EmailAddress(
                GetSetting(configuration, "PARTNERSHIP_INQUIRY_TO_EMAIL", "info@betkanu.com"));

            // Optional CC for partnership inquiries. Do not send duplicates to the same inbox.
            var ccAddress = configuration["PARTNERSHIP_INQUIRY_CC_EMAIL"]?.Trim();
            if (!string.IsNullOrWhiteSpace(ccAddress) &&
                !string.Equals(ccAddress, _inbox.Email, StringComparison.OrdinalIgnoreCase))
            {
                _partnershipCc = new EmailAddress(ccAddress);
            }
        }

        // Existing contact form.
        public async Task SendMailAsync(EmailModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var name = Clean(model.name);
            var address = Clean(model.email);
            var subject = OneLine(string.IsNullOrWhiteSpace(model.subject)
                ? "Website Message"
                : model.subject);
            var message = Clean(model.message);

            var plainText =
                "New BET KANU Contact Message\n\n" +
                $"Name: {name}\nEmail: {address}\nSubject: {subject}\n\nMessage:\n{message}";

            var html =
                "<h2>New BET KANU Contact Message</h2>" +
                $"<p><strong>Name:</strong> {Html(name)}</p>" +
                $"<p><strong>Email:</strong> {Html(address)}</p>" +
                $"<p><strong>Subject:</strong> {Html(subject)}</p>" +
                $"<p><strong>Message:</strong><br>{HtmlLines(message)}</p>";

            // Do not use model.toEmail: a public form must not choose email recipients.
            var email = MailHelper.CreateSingleEmail(
                _from, _inbox, "[BET KANU Contact] " + subject, plainText, html);

            email.SetReplyTo(new EmailAddress(address, OneLine(name)));
            await SendGridAsync(email);
        }

        // Compatibility for other existing callers; new controller uses SendMailAsync.
        public void SendMail(EmailModel model) =>
            SendMailAsync(model).GetAwaiter().GetResult();

        // New partnership form. Uses the model in your existing controller.
        public async Task SendPartnershipInquiryAsync(PartnershipInquiryModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var name = Clean(model.Name);
            var org = Clean(model.Organization);
            var address = Clean(model.Email);
            var message = string.IsNullOrWhiteSpace(model.Message)
                ? "(No message provided)"
                : Clean(model.Message);
            var helpTypes = model.HelpTypes != null && model.HelpTypes.Count > 0
                ? string.Join(", ", model.HelpTypes)
                : "None selected";

            var subject = "[BET KANU] New Partnership Inquiry: " + OneLine(org);
            var plainText =
                "New Partnership Inquiry Received\n\n" +
                $"Name: {name}\n" +
                $"Organization / Business: {org}\n" +
                $"Email: {address}\n" +
                $"How would you like to help: {helpTypes}\n\n" +
                $"Message:\n{message}";

            var html =
                "<h2>New Partnership Inquiry</h2>" +
                $"<p><strong>Name:</strong> {Html(name)}</p>" +
                $"<p><strong>Organization / Business:</strong> {Html(org)}</p>" +
                $"<p><strong>Email:</strong> {Html(address)}</p>" +
                $"<p><strong>How would he like to help:</strong> {Html(helpTypes)}</p>" +
                $"<p><strong>Message:</strong><br>{HtmlLines(message)}</p>";

            var email = MailHelper.CreateSingleEmail(
                _from, _inbox, subject, plainText, html);

            // CC gets the same partnership inquiry as the primary BET KANU inbox.
            if (_partnershipCc != null)
                email.AddCc(_partnershipCc);

            // From must be verified with SendGrid; Reply-To is the visitor.
            email.SetReplyTo(new EmailAddress(address, OneLine(name)));
            await SendGridAsync(email);
        }

        // Preserve legacy ASP.NET Identity email functionality if used elsewhere.
        // Note: This uses Microsoft.AspNet.Identity from your original EmailService.cs.
        public async Task SendAsync(IdentityMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (string.IsNullOrWhiteSpace(message.Destination))
                throw new ArgumentException("Identity message destination is required.", nameof(message));

            var email = MailHelper.CreateSingleEmail(
                _from,
                new EmailAddress(message.Destination.Trim()),
                OneLine(message.Subject ?? "BET KANU"),
                message.Body ?? " ",
                null);

            await SendGridAsync(email);
        }

        private async Task SendGridAsync(SendGridMessage email)
        {
            var response = await _client.SendEmailAsync(email);
            if (response.StatusCode == HttpStatusCode.Accepted)
                return;

            // Log the failure privately. Never send SendGrid's error details to visitors.
            var details = response.Body == null
                ? string.Empty
                : await response.Body.ReadAsStringAsync();

            _logger.LogError(
                "SendGrid rejected BET KANU message: HTTP {StatusCode}. Body: {ErrorBody}",
                (int)response.StatusCode, details);

            throw new InvalidOperationException(
                $"SendGrid did not accept the message. HTTP {(int)response.StatusCode}.");
        }

        private static string GetSetting(IConfiguration configuration, string key, string fallback) =>
            string.IsNullOrWhiteSpace(configuration[key]) ? fallback : configuration[key]!.Trim();

        private static string Clean(string? input) => (input ?? string.Empty).Trim();
        private static string OneLine(string? input) =>
            Clean(input).Replace("\r", " ").Replace("\n", " ");
        private static string Html(string? input) => WebUtility.HtmlEncode(input ?? string.Empty);
        private static string HtmlLines(string? input) =>
            Html(input).Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "<br>");
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message) => Task.CompletedTask;
    }
}
