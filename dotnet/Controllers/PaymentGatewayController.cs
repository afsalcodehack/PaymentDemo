using Ardalis.GuardClauses;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public AuthorizeCaptureResponse Authorize(PaymentIntentCreateRequest request)
        {
            Guard.Against.Null(request, nameof(request));
            return authorizeCapturePaymentGateway.Authorize(request);


        }

        [HttpPost("capture")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public AuthorizeCaptureResponse Capture(PaymentIntentCreateRequest request)
        {
            Guard.Against.Null(request, nameof(request));
            return authorizeCapturePaymentGateway.Capture(request);
        }
    }
}
