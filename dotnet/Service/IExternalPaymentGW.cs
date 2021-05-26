using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Repository;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using Result;

namespace server.Service
{
    public interface IExternalPaymentGW
    {
        string Authorize();

        string Capture();

        Task<Result<ConfigResponse>> GetConfig();

        Task<Session> GetCheckoutSession(string sessionId);

        Task<CreateCheckoutSessionResponse> CreateCheckoutSession();


    }

    public class StripePaymentGW : IExternalPaymentGW
    {
        private readonly IPaymentGWRepository _paymentGWRepository;
        private readonly IStripeClient _stripeClient;
        private readonly IOptions<StripeOptions> _options;

        public StripePaymentGW(IPaymentGWRepository paymentGWRepository, IStripeClient stripeClient
            , IOptions<StripeOptions> options)
        {
            _paymentGWRepository = paymentGWRepository;
            _options = options;
            _stripeClient = new StripeClient(this._options.Value.SecretKey);
            
        }

        

        public async Task<Session> GetCheckoutSession(string sessionId)
        {
            var service = new SessionService(this._stripeClient);
            var session = await service.GetAsync(sessionId);
            return session;
        }
        public async Task<CreateCheckoutSessionResponse> CreateCheckoutSession()
        {
            
            List<string> paymentMethodTypes = this._options.Value.PaymentMethodTypes;

            
            var options = new SessionCreateOptions
            {
                SuccessUrl = $"{this._options.Value.Domain}/success.html?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{this._options.Value.Domain}/canceled.html",
                PaymentMethodTypes = paymentMethodTypes,
                Mode = "payment",
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Quantity = 1,
                        Price = this._options.Value.Price,
                    },
                },
            };

            var service = new SessionService(this._stripeClient);
            var session = await service.CreateAsync(options);

            return new CreateCheckoutSessionResponse
            {
                SessionId = session.Id,
            };
        }

        public string Authorize()
        {
            throw new NotImplementedException();
        }

        public string Capture()
        {
            throw new NotImplementedException();
        }

        public async  Task<Result<ConfigResponse>> GetConfig()
        {
            var service = new PriceService(this._stripeClient);
            var price = await service.GetAsync(this._options.Value.Price);

            if (service == null || price == null)
                Result<ConfigResponse>.Failure();

            // return json: publishableKey (`./.env`), unitAmount, currency
            var response =  new ConfigResponse
            {
                PublishableKey = this._options.Value.PublishableKey,
                UnitAmount = price.UnitAmount,
                Currency = price.Currency,
            };

            return Result<ConfigResponse>.Success(response);
        }
    }
}


