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
            msg.From = new MailAddress("EmailService@BETKANU.com");
            msg.To.Add(new MailAddress(emailVM.toEmail));
            msg.CC.Add(new MailAddress("info@betkanu.com"));
            msg.Subject = emailVM.email + ": " + emailVM.subject;
            string body = $"From: {emailVM.name ?? "-"} \n" + $"Email: {emailVM.email} \n\n" + $"{emailVM.message} ";
            msg.AlternateViews.Add(
                AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Plain));

            //SmtpClient smtpClient = new SmtpClient("mail.betkanu.com", 25);
            SmtpClient smtpClient = new SmtpClient("ws9.win.arvixe.com", 465);
            smtpClient.EnableSsl = true;
            //System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("info@betkanu.com", "KanuBK123");
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("info@betkanu.com", "GalyoB123");
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = credentials;
            smtpClient.Send(msg);
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
