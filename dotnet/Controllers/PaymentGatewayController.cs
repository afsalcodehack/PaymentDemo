using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using server.Models;
using server.Service;
using Stripe;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentGatewayController : ControllerBase
    {
        private readonly IAuthorizeCapturePaymentGateway authorizeCapturePaymentGateway;
        private readonly IStripeClient client;
        public readonly IOptions<Entity.Configuration.StripeOptions> options;

        public PaymentGatewayController(IOptions<Entity.Configuration.StripeOptions> options, IAuthorizeCapturePaymentGateway authorizeCapturePaymentGateway)
        {
            this.options = options;
            this.authorizeCapturePaymentGateway = authorizeCapturePaymentGateway;
            this.client = new StripeClient(this.options.Value.SecretKey);
        }

        [HttpGet("getconfig")]
        public Config GetConfig()
        {
            var service = new PriceService(this.client);

            return new Config
            {
                PublishableKey = this.options.Value.PublishableKey,
            };
        }

        [HttpPost("authorize")]
        public AuthorizeCaptureResponse Authorize(PaymentIntentCreateRequest request)
        {
            return authorizeCapturePaymentGateway.Authorize(request);
        }

        [HttpPost("capture")]
        public AuthorizeCaptureResponse Capture(PaymentIntentCreateRequest request)
        {
            return authorizeCapturePaymentGateway.Capture(request);
        }
    }
}
