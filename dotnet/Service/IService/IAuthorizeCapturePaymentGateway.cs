using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Service
{
    public interface IAuthorizeCapturePaymentGateway
    {
        AuthorizeCaptureResponse Authorize(PaymentIntentCreateRequest request);

        AuthorizeCaptureResponse Capture(PaymentIntentCreateRequest request);

    }

   
}


