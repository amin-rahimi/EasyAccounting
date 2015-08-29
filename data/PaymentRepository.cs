using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccounting.data
{
    class PaymentRepository
    {
        eadbEntities entities = new eadbEntities();

        public Payment getPayment(int id)
        {
            return entities.Payments.Where(p => p.Id == id).First();
        }

        public void addPayment(Payment p)
        {
            entities.Payments.Add(p);
            entities.SaveChanges();

        }

        public void deletePayment(int id)
        {
            Payment p = this.getPayment(id);
            entities.Payments.Remove(p);
            entities.SaveChanges();
        }

        public void updatePayment(Payment p)
        {
            entities.Payments.Attach(p);
            entities.Entry(p).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public IQueryable<Payment> getPaymentsByContractId(int contractId)
        {
            return entities.Payments.Where(p => p.ContractId == contractId).OrderByDescending(o => o.DateOfPayment);
        }
    }
}
