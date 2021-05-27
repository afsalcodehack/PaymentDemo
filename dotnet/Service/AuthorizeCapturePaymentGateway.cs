using Entity.Entities;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stripe;
using server.Models;
using Ardalis.GuardClauses;

namespace server.Service
{
    public class AuthorizeCapturePaymentGateway : IAuthorizeCapturePaymentGateway
    {
        private readonly IStripePaymentsRepository stripePaymentsRepository;

        private const string status = "completed";
        private const string email = "aneezm12@gmail.com";
        private const string transactionStatus = "pending";

        public AuthorizeCapturePaymentGateway(IStripePaymentsRepository stripePaymentsRepository)
        {
            this.stripePaymentsRepository = stripePaymentsRepository;
        }


        /// <summary>
        /// Implemented Authorize method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public AuthorizeCaptureResponse Authorize(PaymentIntentCreateRequest request)
        {
            Guard.Against.Null(request, nameof(request));

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

            Transaction transaction = new Transaction
            {
                Id = Convert.ToString(Guid.NewGuid()),
                CreatedDate = DateTime.Now,
                Currency = request.Currency,
                Status = transactionStatus,
                TransactionId = paymentIntent.Id,
            };
            //transaction.Id = Convert.ToString(Guid.NewGuid());
            //transaction.CreatedDate = DateTime.Now;
            //transaction.Currency = request.Currency;
            //transaction.Status = transactionStatus;
            //transaction.TransactionId = paymentIntent.Id;
            stripePaymentsRepository.CreateTransaction(transaction);

            return new AuthorizeCaptureResponse() { clientSecret = paymentIntent.ClientSecret };
           
        }


        /// <summary>
        /// Implemented capture method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public AuthorizeCaptureResponse Capture(PaymentIntentCreateRequest request)
        {
            var options = new PaymentIntentCaptureOptions
            {
                AmountToCapture = request.Amount,
            };

            var service = new PaymentIntentService();
            var paymentIntent = service.Capture(request.Payment_id, options);

            Transaction updateTransaction = new Transaction
            {
                TransactionId = paymentIntent.Id,
                Currency = paymentIntent.Currency,
                Amount = request.Amount,
                Status = status,
                Email = email
            };
            
            stripePaymentsRepository.UpdateTransaction(updateTransaction);

            return new AuthorizeCaptureResponse() { clientSecret = paymentIntent.ClientSecret };


        }
    }
}
