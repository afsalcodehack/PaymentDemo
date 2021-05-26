using Entity.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe.Checkout;

namespace Repositories.IRepositories
{
    public interface IStripePaymentRepository
    {
        Task<ConfigResponse> GetConfig();
        Task<Session> GetCheckoutSession(string sessionId);
        Task<CreateCheckoutSessionResponse> CreateCheckoutSession();
    }

}
