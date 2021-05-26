using Entity.Configuration;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class StripePaymentRepository : IStripePaymentRepository
    {
        public readonly IOptions<StripeOptions> options;
        private readonly IStripeClient client;
        public StripePaymentRepository()
        {
            
        }
        
    }
}
