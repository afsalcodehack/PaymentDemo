using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Repository
{
    public interface IPaymentGWRepository
    {
        int AddTransaction();
        int UpdateTransaction();
    }

    public class PaymentGWRepo : IPaymentGWRepository
    {
        public int AddTransaction()
        {
            throw new NotImplementedException();
        }

        public int UpdateTransaction()
        {
            throw new NotImplementedException();
        }
    }
}
