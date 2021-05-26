using System;
using System.Collections.Generic;

#nullable disable

namespace Entity.Entities
{
    public partial class Transaction
    {
        public string Id { get; set; }
        public double? Amount { get; set; }
        public string TransactionId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Type { get; set; }
    }
}
