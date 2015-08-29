using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccounting.data
{
    class AppointmentRepository
    {
        eadbEntities entities = new eadbEntities();
        public Appointment getAppointment(int id)
        {
            return entities.Appointments.Where(a => a.Id == id).First();
        }

        public void addAppointment(Appointment a)
        {
            entities.Appointments.Add(a);
            entities.SaveChanges();

        }

        public void deleteAppointment(int id)
        {
            Appointment a = this.getAppointment(id);
            entities.Appointments.Remove(a);
            entities.SaveChanges();
        }

        public void updateAppointment(Appointment a)
        {
            entities.Appointments.Attach(a);
            entities.Entry(a).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public IQueryable<Appointment> getAppointmentByContractId(int contractId)
        {
            return entities.Appointments.Where(p => p.ContractId == contractId).OrderByDescending(o => o.AppointmentDate);
        }
    }
}
