using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;
using System.IO;
using System.Text;
using System.Net;
using System.Collections.Specialized;

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


        private bool VerifyIPNRequest(string ipnData)
        {
            // Set the URL for PayPal's IPN verification endpoint
            string verificationUrl = "https://www.paypal.com/cgi-bin/webscr";

            // Set the request data to send back to PayPal for verification
            NameValueCollection requestData = new NameValueCollection
            {
                { "cmd", "_notify-validate" },
                { "ipnData", ipnData }
            };

            // Create a WebClient instance to send the verification request
            using (WebClient client = new WebClient())
            {
                // Set the encoding
                client.Encoding = Encoding.UTF8;

                // Post the verification request to PayPal
                byte[] responseBytes = client.UploadValues(verificationUrl, "POST", requestData);

                // Convert the response to a string
                string response = Encoding.UTF8.GetString(responseBytes);

                // Check if the response is "VERIFIED" to confirm the IPN authenticity
                return response.Equals("VERIFIED");
            }
        }
    }
}
