using Entity.Entities;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stripe;
using server.Models;

namespace server.Service
{
    public class AuthorizeCapturePaymentGateway : IAuthorizeCapturePaymentGateway
    {
        private readonly IStripePaymentsRepository stripePaymentsRepository;

        public AuthorizeCapturePaymentGateway(IStripePaymentsRepository stripePaymentsRepository)
        {
            this.stripePaymentsRepository = stripePaymentsRepository;
        }

        public AuthorizeCaptureResponse Authorize(PaymentIntentCreateRequest request)
        {
            

            var options2 = new PaymentIntentCreateOptions
            {
                Amount = request.Amount,
                Currency = request.Currency,
                PaymentMethodTypes = new List<string>
                {
                "card",
                },
                CaptureMethod = "manual"
            };

            var paymentIntents = new PaymentIntentService();

            var paymentIntent = paymentIntents.Create(options2);

            Transaction transaction = new Transaction();
            transaction.Id = Convert.ToString(Guid.NewGuid());
            transaction.CreatedDate = DateTime.Now;
            transaction.Currency = request.Currency;
            transaction.Status = "Pending";
            transaction.TransactionId = paymentIntent.Id;
            stripePaymentsRepository.CreateTransaction(transaction);

            return new AuthorizeCaptureResponse() { clientSecret = paymentIntent.ClientSecret };
           
        }

        public AuthorizeCaptureResponse Capture(PaymentIntentCreateRequest request)
        {
            var options = new PaymentIntentCaptureOptions
            {
                AmountToCapture = request.Amount,
            };

            var service = new PaymentIntentService();
            var paymentIntent = service.Capture(request.payment_id, options);

            Transaction updateTransaction = new Transaction();
            updateTransaction.TransactionId = paymentIntent.Id;
            updateTransaction.Currency = paymentIntent.Currency;
            updateTransaction.Amount = request.Amount;
            updateTransaction.Status = "Completed";
            updateTransaction.Email = "aneez@gmail.com";
            stripePaymentsRepository.UpdateTransaction(updateTransaction);

            return new AuthorizeCaptureResponse() { clientSecret = paymentIntent.ClientSecret };


        }
    }
}
