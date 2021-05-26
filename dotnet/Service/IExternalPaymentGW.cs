using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Service
{
    public interface IExternalPaymentGW
    {
        void Authorize();

        void Capture();

    }

    public class StripePaymentGW : IExternalPaymentGW
    {

        public StripePaymentGW()
        {

        }

        void IExternalPaymentGW.Authorize()
        {
            throw new NotImplementedException();
        }

        void IExternalPaymentGW.Capture()
        {
            throw new NotImplementedException();
        }
    }
}


