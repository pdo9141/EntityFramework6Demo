using System;
using System.Linq;
using System.Data.Entity;
using System.Collections;
using System.Collections.Generic;
using NinjaDomain.Classes;

namespace NinjaDomain.DataModel
{
    public class DisconnectedRepository
    {
        public List<Ninja> GetNinjasWithClan()
        {
            using (var context = new NinjaContext())
            {
                // FOR PERFORMANCE REASONS, IN DISCONNECT APPS LIKE MVC, USE THE ASNOTRACKING METHOD TO SAVE EF FROM TRACKING ENTITIES
                //return context.Ninjas.Include(n => n.Clan).ToList();
                return context.Ninjas.AsNoTracking().Include(n => n.Clan).ToList();
            }
        }

        public Ninja GetNinjaWithEquipment(int id)
        {
            using (var context = new NinjaContext())
            {
                return context.Ninjas.AsNoTracking().Include(n => n.EquipmentOwned).FirstOrDefault(n => n.Id == id);
            }
        }

        public Ninja GetNinjaWithEquipmentAndClan(int id)
        {
            using (var context = new NinjaContext())
            {
                return context.Ninjas.AsNoTracking().Include(n => n.EquipmentOwned)
                    .Include(n => n.Clan)
                    .FirstOrDefault(n => n.Id == id);
            }
        }

        public IEnumerable GetClanList()
        {
            using (var context = new NinjaContext())
            {
                return context.Clans.AsNoTracking().OrderBy(c => c.ClanName)
                    .Select(c => new { c.Id, c.ClanName }).ToList();
            }
        }

        public Ninja GetNinjaById(int id)
        {
            using (var context = new NinjaContext())
            {
                //return context.Ninjas.Find(id);
                return context.Ninjas.AsNoTracking().SingleOrDefault(n => n.Id == id);
            }
        }

        public void SaveUpdatedNinja(Ninja ninja)
        {
            using (var context = new NinjaContext())
            {
                context.Entry(ninja).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void SaveNewNinja(Ninja ninja)
        {
            using (var context = new NinjaContext())
            {
                context.Ninjas.Add(ninja);
                context.SaveChanges();
            }
        }

        public object GetEquipmentById(int value)
        {
            throw new NotImplementedException();
        }

        public void DeleteNinja(int ninjaId)
        {
            using (var context = new NinjaContext())
            {
                var ninja = context.Ninjas.Find(ninjaId);
                context.Entry(ninja).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public void SaveNewEquipment(NinjaEquipment equipment, int ninjaId)
        {
            // PAYING THE PRICE OF NOT HAVING A FOREIGN KEY HERE.
            // REASON #857 WHY I PREFER FOREIGN KEYS!
            using (var context = new NinjaContext())
            {
                var ninja = context.Ninjas.Find(ninjaId);
                ninja.EquipmentOwned.Add(equipment);
                context.SaveChanges();
            }
        }

        public void SaveUpdatedEquipment(NinjaEquipment equipment, int ninjaId)
        {
            // PAYING THE PRICE OF NOT HAVING A FOREIGN KEY HERE.
            // REASON #858 WHY I PREFER FOREIGN KEYS!
            using (var context = new NinjaContext())
            {
                var equipmentWithNinjaFromDatabase =
                    context.Equipment.Include(n => n.Ninja).FirstOrDefault(e => e.Id == equipment.Id);

                context.Entry(equipmentWithNinjaFromDatabase).CurrentValues.SetValues(equipment);

                context.SaveChanges();
            }
        }
    }
}
