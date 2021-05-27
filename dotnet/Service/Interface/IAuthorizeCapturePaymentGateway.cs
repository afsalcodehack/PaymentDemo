using server.Models;
using System.Threading.Tasks;

namespace server.Service
{
    public interface IAuthorizeCapturePaymentGateway
    {
        Task<AuthorizeCaptureResponse> Authorize(PaymentIntentCreateRequest request);

        AuthorizeCaptureResponse Capture(PaymentIntentCreateRequest request);

    }

   
}


