using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using Stripe;
using Stripe.Checkout;

namespace server.Controllers
{
    //[Route("[controller]")]
    //[ApiController]
    public class PaymentsController : Controller
    {
        private readonly IStripePaymentRepository _stripePaymentRepository;

        public PaymentsController(IStripePaymentRepository stripePaymentRepository)
        {
            _stripePaymentRepository = stripePaymentRepository;
        }

        //[Authorize]
        [HttpGet("config")]
        public Task<Entity.Configuration.ConfigResponse> GetConfig()
        {
            return _stripePaymentRepository.GetConfig();
        }

        [HttpGet("checkout-session")]
        public Task<Session> GetCheckoutSession(string sessionId)
        {
            return _stripePaymentRepository.GetCheckoutSession(sessionId);
        }

        [Authorize]
        [HttpPost("create-checkout-session")]
        public Task<Entity.Configuration.CreateCheckoutSessionResponse> CreateCheckoutSession()
        {
            return _stripePaymentRepository.CreateCheckoutSession();
        }
    }
}
