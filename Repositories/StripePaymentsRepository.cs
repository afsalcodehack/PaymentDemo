using Entity.Entities;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class StripePaymentsRepository : IStripePaymentsRepository
    {
        private stripeContext stripeContext;
        public StripePaymentsRepository(stripeContext stripeContext)
        {
            this.stripeContext = stripeContext;
        }
        public void CreateTransaction(Transaction transaction)
        {
            stripeContext.Transactions.Add(transaction);
            stripeContext.SaveChanges();
        }

        public void UpdateTransaction(Transaction transaction)
        {
            var updateTransaction = stripeContext.Transactions.Where(x => x.TransactionId == transaction.TransactionId).FirstOrDefault();
            if (updateTransaction != null)
            {
                updateTransaction.Email = transaction.Email;
                updateTransaction.Currency = transaction.Currency;
                updateTransaction.Amount = transaction.Amount;
                updateTransaction.CreatedDate = transaction.CreatedDate;
                updateTransaction.Type = transaction.Type;
                updateTransaction.Status = "Completed";
                stripeContext.Transactions.Update(updateTransaction);
                stripeContext.SaveChanges();
            }
        }
    }
}
