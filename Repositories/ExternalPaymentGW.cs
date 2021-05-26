using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ExternalPaymentGW : IExternalPaymentGW
    {

        public ExternalPaymentGW(IStripePaymentRepository paymentRepository)
        {
            PaymentRepository = paymentRepository;
        }

        public IStripePaymentRepository PaymentRepository { get; }

        public void Authorize()
        {
            throw new NotImplementedException();
        }

        public void Capture()
        {
            throw new NotImplementedException();
        }
    }
}
