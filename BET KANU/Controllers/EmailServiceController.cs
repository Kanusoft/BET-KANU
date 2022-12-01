using BET_KANU.Services;
using System.Web.Http;


namespace BET_KANU.Controllers
{
    public class EmailServiceController : ApiController
    {
        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody] EmailModel value)
        {
            try
            {
                EmailService emailService = new EmailService();
                emailService.SendMail(value);
            }
            catch (Exception)
            {
                return;
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

