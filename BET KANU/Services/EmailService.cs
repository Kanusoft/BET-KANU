using BET_KANU.Controllers;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Net.Mime;

namespace BET_KANU.Services
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.Factory.StartNew(() =>
            {
                SendMail(message);
            });
        }

        private void SendMail(IdentityMessage message)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("info@betkanu.com");
            msg.To.Add(new MailAddress("info@betkanu.com"));
            msg.Subject = message.Destination + ": " + message.Subject;
            string email = message.Body;
            msg.AlternateViews.Add(
                AlternateView.CreateAlternateViewFromString(email, null, MediaTypeNames.Text.Plain));

            //SmtpClient smtpClient = new SmtpClient("mail.betkanu.com", 25);
            SmtpClient smtpClient = new SmtpClient("ws9.win.arvixe.com", 465);
            smtpClient.EnableSsl = true;
            //System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("info@betkanu.com", "KanuBK123");
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("info@betkanu.com", "GalyoB123");
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = credentials;
            smtpClient.Send(msg);
        }

        public void SendMail(EmailModel emailVM)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("info@betkanu.com", "BET KANU Website Contact");
            
            // Clean up toEmail if present
            string recipient = "info@betkanu.com";
            if (!string.IsNullOrWhiteSpace(emailVM.toEmail))
            {
                string target = emailVM.toEmail;
                if (target.Contains("<") && target.Contains(">"))
                {
                    int start = target.IndexOf("<") + 1;
                    int end = target.IndexOf(">");
                    target = target.Substring(start, end - start).Trim();
                }
                if (!string.IsNullOrWhiteSpace(target) && target.Contains("@"))
                {
                    recipient = target;
                }
            }

            msg.To.Add(new MailAddress(recipient));

            // Ensure a copy is always delivered to info@betkanu.com
            if (!recipient.Equals("info@betkanu.com", StringComparison.OrdinalIgnoreCase))
            {
                msg.CC.Add(new MailAddress("info@betkanu.com"));
            }

            // Add reply-to header so clicking Reply responds to the submitter
            if (!string.IsNullOrWhiteSpace(emailVM.email) && emailVM.email.Contains("@"))
            {
                try
                {
                    msg.ReplyToList.Add(new MailAddress(emailVM.email, emailVM.name ?? ""));
                }
                catch { }
            }

            msg.Subject = $"[BET KANU Contact] {emailVM.subject ?? "Website Message"}";
            string body = $"From: {emailVM.name ?? "-"}\nEmail: {emailVM.email ?? "-"}\nSubject: {emailVM.subject ?? "-"}\n\nMessage:\n{emailVM.message ?? ""}";
            msg.AlternateViews.Add(
                AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Plain));

            SmtpClient smtpClient = new SmtpClient("ws9.win.arvixe.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Timeout = 8000;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential("info@betkanu.com", "GalyoB123");

            try
            {
                smtpClient.Send(msg);
            }
            catch (Exception ex1)
            {
                Console.WriteLine($"[SMTP Port 587 failed] {ex1.Message}");
                try
                {
                    smtpClient.Port = 25;
                    smtpClient.Send(msg);
                }
                catch (Exception ex2)
                {
                    Console.WriteLine($"[SMTP Port 25 failed] {ex2.Message}");
                    try
                    {
                        smtpClient.Port = 465;
                        smtpClient.Send(msg);
                    }
                    catch (Exception ex3)
                    {
                        Console.WriteLine($"[SMTP Port 465 failed] {ex3.Message}");
                    }
                }
            }
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
