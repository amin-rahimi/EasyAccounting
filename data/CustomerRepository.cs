using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccounting.data
{
    class CustomerRepository
    {
        private eadbEntities entities = new eadbEntities();

        public Customer getCustomer(int id)
        {
            return entities.Customers.Where(c => c.Id == id).First();
        }

        public IOrderedQueryable<Customer> getAllCustomers()
        {
            return entities.Customers.OrderByDescending(c => c.CreatedDate);
        }

        public void addCustomer(Customer c)
        {
            
            entities.Customers.Add(c);
            entities.SaveChanges();
         
        }

        public void deleteCustomer(int id)
        {
            Customer c = this.getCustomer(id);
            entities.Customers.Remove(c);
            entities.SaveChanges();
        }

        public void updateCustomer(Customer c)
        {
            entities.Customers.Attach(c);
            entities.Entry(c).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public IQueryable<Customer> searchCustomers(String name, String lastName, String phoneNumber)
        {
            IQueryable<Customer> query = entities.Customers.Where(c => c.FirstName.Contains(name)).Where(c => c.LastName.Contains(lastName)).Where(c => c.PhoneNumber.Contains(phoneNumber));
            //if (!String.IsNullOrWhiteSpace(name))
            //{
            //    query.Where(c => c.FirstName.Contains(name));
            //}
            //if (!String.IsNullOrWhiteSpace(lastName))
            //{
            //    query.Where(c => c.FirstName.Contains(lastName));
            //}
            //if (!String.IsNullOrWhiteSpace(phoneNumber))
            //{
            //    query.Where(c => c.FirstName.Contains(phoneNumber));
            //}
            return query;

        }

    }
}
