using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using server.Models;
using server.Service;
using Stripe;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentGatewayController : BaseController
    {
        private readonly IAuthorizeCapturePaymentGateway authorizeCapturePaymentGateway;
        private readonly IStripeClient client;
        public readonly IOptions<Entity.Configuration.StripeOptions> options;

        public PaymentGatewayController(IOptions<Entity.Configuration.StripeOptions> options, IAuthorizeCapturePaymentGateway authorizeCapturePaymentGateway,ILogger<PaymentGatewayController> logger): base(logger)
        {
            this.options = options;
            this.authorizeCapturePaymentGateway = authorizeCapturePaymentGateway;
            client = new StripeClient(this.options.Value.SecretKey);
        }

        [HttpGet("getconfig")]
        public Config GetConfig()
        {
            
            return new Config
            {
                PublishableKey = options.Value.PublishableKey,
            };
        }

        [HttpPost("authorize")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public AuthorizeCaptureResponse Authorize(PaymentIntentCreateRequest request)
        {
            Guard.Against.Null(request, nameof(request));

            try
            {
                return authorizeCapturePaymentGateway.Authorize(request);
            }
            catch (Exception exception)
            {
                Logger.LogInformation(exception.Message);
                return null;
            }
        }

        [HttpPost("capture")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public AuthorizeCaptureResponse Capture(PaymentIntentCreateRequest request)
        {
            Guard.Against.Null(request, nameof(request));
            try
            {
                return authorizeCapturePaymentGateway.Capture(request);
            }
            catch (Exception exception)
            {
                Logger.LogInformation(exception.Message);
                return null;
            }

        }
    }
}
