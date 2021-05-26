using Entity.Configuration;
using Entity.Entities;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{
    public class StripePaymentRepository : IStripePaymentRepository
    {
        public readonly IOptions<StripeOptions> options;
        private readonly IStripeClient client;
        private stripeContext stripeContext;
        public StripePaymentRepository(IOptions<StripeOptions> options, stripeContext stripeContext)
        {
            this.options = options;
            this.client = new StripeClient(this.options.Value.SecretKey);
            this.stripeContext = stripeContext;
        }

        public async Task<ConfigResponse> GetConfig()
        {
            // Fetch price from the API
            var service = new PriceService(this.client);
            var price = await service.GetAsync(this.options.Value.Price);

            // return json: publishableKey (`./.env`), unitAmount, currency
            return new ConfigResponse
            {
                PublishableKey = this.options.Value.PublishableKey,
                UnitAmount = price.UnitAmount,
                Currency = price.Currency,
            };
        }

        public async Task<Session> GetCheckoutSession(string sessionId)
        {
            var service = new SessionService(this.client);
            var session = await service.GetAsync(sessionId);

            //update db 
            var transaction = stripeContext.Transactions.Where(x => x.TransactionId == sessionId).FirstOrDefault();
            transaction.Amount = session.AmountTotal;
            transaction.CreatedDate = DateTime.Now;
            transaction.Type = session.PaymentMethodTypes[0];
            transaction.Currency = session.Currency;
            transaction.Email = session.CustomerDetails.Email;
            transaction.Status = "Completed";
            stripeContext.Update(transaction);
            stripeContext.SaveChanges();
            return session;
        }

        public async Task<CreateCheckoutSessionResponse> CreateCheckoutSession()
        {
            // Pulled from environment variables in the `.env` file. In practice,
            // users often hard code this list of strings representing the types of
            // payment methods that are accepted.
            List<string> paymentMethodTypes = this.options.Value.PaymentMethodTypes;

            // Create new Checkout Session for the order
            // Other optional params include:
            //  [billing_address_collection] - to display billing address details on the page
            //  [customer] - if you have an existing Stripe Customer ID
            //  [customer_email] - lets you prefill the email input in the form
            //  For full details see https:#stripe.com/docs/api/checkout/sessions/create

            //  ?session_id={CHECKOUT_SESSION_ID} means the redirect will have the session ID set as a query param
            var options = new SessionCreateOptions
            {
                SuccessUrl = $"{this.options.Value.Domain}/success.html?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{this.options.Value.Domain}/canceled.html",
                PaymentMethodTypes = paymentMethodTypes,
                Mode = "payment",
                
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Quantity = 1,
                        Price = this.options.Value.Price,
                    },
                },
            };

            var service = new SessionService(this.client);
            var session = await service.CreateAsync(options);

            Transaction transaction = new Transaction();
            transaction.Id = Convert.ToString(Guid.NewGuid());
            transaction.TransactionId = session.Id;
            transaction.Status = "Pending";
            stripeContext.Transactions.Add(transaction);
            stripeContext.SaveChanges();
           
            return new CreateCheckoutSessionResponse
            {
                SessionId = session.Id,
            };
        }

    }
}
