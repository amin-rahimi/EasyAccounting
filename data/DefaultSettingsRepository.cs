using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccounting.data
{
    class DefaultSettingsRepository
    {
        eadbEntities entities = new eadbEntities();

        public void AddSettings(DefaultSetting setting)
        {
            entities.DefaultSettings.Add(setting);
            entities.SaveChanges();
        }

        public DefaultSetting GetSetting(String name)
        {
            return entities.DefaultSettings.FirstOrDefault(d => d.Name == name);
        }

        public List<DefaultSetting> GetSetingsByType(String type)
        {
            return entities.DefaultSettings.Where(d => d.Type == type).ToList();
        }

        public void delete(string name)
        {
            DefaultSetting ds = this.GetSetting(name);
            entities.DefaultSettings.Remove(ds);
            entities.SaveChanges();
        }

        public void UpdateSetting(DefaultSetting s)
        {
            entities.DefaultSettings.Attach(s);
            entities.Entry(s).State = EntityState.Modified;
            entities.SaveChanges();
        }
    }
}
