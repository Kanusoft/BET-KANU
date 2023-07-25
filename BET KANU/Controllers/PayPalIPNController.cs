using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;
using System.IO;
using System.Text;

namespace BET_KANU.Controllers
{
    public class PayPalIPNController : Controller
    {
        [HttpPost]
        public ActionResult Receive()
        {
            // Get the raw request body from the input stream
            string ipnData = string.Empty;
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                ipnData = reader.ReadToEnd();
            }

            // Verify the IPN message authenticity (you need to implement this part)
            bool isIPNAuthentic = VerifyIPNRequest(ipnData);

            if (isIPNAuthentic)
            {
                // Process the payment status
                string paymentStatus = HttpContext?.Request?.Form["payment_status"];

                if (paymentStatus == "Completed")
                {
                    // Payment is completed, redirect the user to the "Special Thanks" page
                    return RedirectToAction("Index", "SpecialThanks");
                }
                // Handle other payment statuses if needed
            }

            // IPN message not authentic or payment not completed, return an empty response to PayPal
            return new EmptyResult();
        }

        // Implement the IPN request verification (you need to define this method)
        private bool VerifyIPNRequest(string ipnData)
        {
            // Implement IPN verification logic here
            // Verify the IPN message using PayPal API or other methods
            // Return true if the IPN message is authentic, false otherwise
            return true;
        }
    }
}
