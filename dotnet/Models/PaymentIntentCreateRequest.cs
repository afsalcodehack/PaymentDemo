using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class PaymentIntentCreateRequest
    {
        public long? Amount { get; set; }

        public string Currency { get; set; }

        public List<string> PaymentMethodTypes { get; set; }
        public string CaptureMethod { get; set; }

        public string payment_id { get; set; }
    }
}
