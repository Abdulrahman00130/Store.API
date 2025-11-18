using Microsoft.AspNetCore.Mvc;
using Store.API.Services.Abstractions;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpPost("{basketId}")]
        public async Task<IActionResult> CreatePaymentIntent(string basketId)
        {
            var result = await _serviceManager.PaymentService.CreatePaymentIntentAsync(basketId);

            return Ok(result);
        }

        //stripe listen --forward-to https://localhost:7018/api/Payments/webhook
        [Route("webhook")]
        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            const string endpointSecret = "whsec_155f0cd7a068048bbc26f48f49ee7ea8ec637d60e6802de76c1272754ffb9a26";

            var stripeEvent = EventUtility.ParseEvent(json);
            var signatureHeader = Request.Headers["Stripe-Signature"];

            stripeEvent = EventUtility.ConstructEvent(json,
                    signatureHeader, endpointSecret);

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

            // If on SDK version < 46, use class Events instead of EventTypes
            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                //update order status to succeeded
                _serviceManager.PaymentService.UpdatePaymentIntentForSucceedOrFailed(paymentIntent.Id, true);

            }
            else if (stripeEvent.Type == "payment_intent.payment_failed")
            {
                //update order status to failed
                _serviceManager.PaymentService.UpdatePaymentIntentForSucceedOrFailed(paymentIntent.Id, false);

            }
            else
            {
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }
            return Ok();

        }
    }
}
