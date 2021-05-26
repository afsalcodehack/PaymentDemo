using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Repository;

namespace server.Service
{
    public interface IExternalPaymentGW
    {
        string Authorize();

        string Capture();

    }

    public class StripePaymentGW : IExternalPaymentGW
    {
        private readonly IPaymentGWRepository _paymentGWRepository;

        public StripePaymentGW(IPaymentGWRepository paymentGWRepository)
        {
            _paymentGWRepository = paymentGWRepository;
        }

        public string Authorize()
        {
            throw new NotImplementedException();
        }

        public string Capture()
        {
            throw new NotImplementedException();
        }
    }
}


