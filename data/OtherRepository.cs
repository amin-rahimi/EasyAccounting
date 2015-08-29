using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyAccounting.util;

namespace EasyAccounting.data
{
    class OtherRepository
    {
        eadbEntities entities = new eadbEntities();

        public object getCustomerJoinContract(DateTime date)
        {
            PersianDateFormatter pdf = new PersianDateFormatter();
            int dateinteger = pdf.getDateInteger(date);
            var customerJoinContract = entities.Customers.Join(entities.Contracts, customer => customer.Id, contract => contract.CustomerId, (customer, contract)
                => new
                {
                    CustomerId = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    PhoneNumber = customer.PhoneNumber,
                    Time = contract.NextAppointmentTime,
                    Date = contract.NextAppointmentDate

                }).Where(a => a.Date == dateinteger).OrderBy(b => b.Time);
            return customerJoinContract.ToList();
        }

        public object getCustomerJoinContract(int date)
        {
            PersianDateFormatter pdf = new PersianDateFormatter();
            //int dateinteger = pdf.getDateInteger(date);
            var customerJoinContract = entities.Customers.Join(entities.Contracts, customer => customer.Id, contract => contract.CustomerId, (customer, contract)
                => new
                {
                    CustomerId = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    PhoneNumber = customer.PhoneNumber,
                    Time = contract.NextAppointmentTime,
                    Date = contract.NextAppointmentDate

                }).Where(a => a.Date == date).OrderBy(b => b.Time);
            return customerJoinContract.ToList();
        }

        public object getCustomersPayment()
        {
            var customerspayment = (from p in entities.Payments
                                    join con in entities.Contracts on p.ContractId equals con.Id
                                    join c in entities.Customers on con.CustomerId equals c.Id
                                    select new
                                    {
                                        FirstName = c.FirstName,
                                        LastName = c.LastName,
                                        PhoneNumber = c.PhoneNumber,
                                        Amount = p.AmountOfPayment,
                                        Date = p.DateOfPayment
                                    }).OrderByDescending(o => o.Date);
            return customerspayment.ToList();
        }

        public List<DayReport> getGroupByDay(int year, int month)
        {
            var query = entities.Payments.Where(p => p.Year == year && p.Month == month)
                .GroupBy(g => g.Day).Select(s => new DayReport{Day = s.FirstOrDefault().Day.Value, Income = s.Sum(f => f.AmountOfPayment).Value});
            
            return query.ToList();
        }

        public List<MonthReport> getGroupByMonth(int year)
        {
            var query = entities.Payments.Where(p => p.Year == year)
                .GroupBy(g => g.Month).Select(s => new MonthReport { Month = s.FirstOrDefault().Month.Value, Income = s.Sum(f => f.AmountOfPayment).Value });
            return query.ToList();
        }

        public List<YearReport> getGroupByYear()
        {
            var query = entities.Payments.GroupBy(g => g.Year).Select(s => new YearReport { Year = s.FirstOrDefault().Year.Value, Income = s.Sum(f => f.AmountOfPayment).Value });
            return query.ToList();
        }
    }
}
