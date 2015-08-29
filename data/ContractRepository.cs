using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccounting.data
{
    class ContractRepository
    {
        eadbEntities entities = new eadbEntities();
        public Contract getContract(int id)
        {
            return entities.Contracts.Where(c => c.Id == id).First();
        }

        public void addContract(Contract c)
        {
            entities.Contracts.Add(c);
            entities.SaveChanges();

        }

        public void deleteContract(int id)
        {
            Contract c = this.getContract(id);
            entities.Contracts.Remove(c);
            entities.SaveChanges();
        }

        public void updateContract(Contract c)
        {
            entities.Contracts.Attach(c);
            entities.Entry(c).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public IQueryable<Contract> getContractsByCustomerId(int customerId)
        {
            return entities.Contracts.Where(c => c.CustomerId == customerId).OrderByDescending(o => o.ContractStartDate);
        }
    }
}
