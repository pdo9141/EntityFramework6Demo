using System;
using System.Linq;
using System.Data.Entity;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NinjaDomain.Classes;

namespace NinjaDomain.DataModel
{
    public class DisconnectedRepository
    {
        public List<Ninja> GetNinjasWithClan(int id)
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
        /*
        public Ninja GetNinjaById(int id)
        {
            return _context.Ninjas.Find(id);
        }

        public List<Ninja> GetNinjas()
        {
            return _context.Ninjas.ToList();
        }

        public IEnumerable GetClanList()
        {
            return _context.Clans.OrderBy(c => c.ClanName).Select(c => new { c.Id, c.ClanName }).ToList();
        }

        public ObservableCollection<Ninja> NinjasInMemory()
        {
            if (_context.Ninjas.Local.Count == 0)
            {
                GetNinjas();
            }

            return _context.Ninjas.Local;
        }

        public void Save()
        {
            RemoveEmptyNewNinjas();
            _context.SaveChanges();
        }

        private Ninja NewNinja()
        {
            var ninja = new Ninja();
            _context.Ninjas.Add(ninja);
            return ninja;
        }

        private void RemoveEmptyNewNinjas()
        {
            // YOU CAN'T REMOVE FROM OR ADD TO A COLLECTION IN A FOREACH LOOP
            for (var i = _context.Ninjas.Local.Count; i > 0; i--)
            {
                var ninja = _context.Ninjas.Local[i - 1];
                if (_context.Entry(ninja).State == EntityState.Added && !ninja.IsDirty)
                {
                    _context.Ninjas.Remove(ninja);
                }
            }
        }

        public void DeleteCurrentNinja(Ninja ninja)
        {
            _context.Ninjas.Remove(ninja);
            Save();
        }

        public void DeleteEquipment(ICollection equipmentList)
        {
            foreach (NinjaEquipment equip in equipmentList)
            {
                _context.Equipment.Remove(equip);
            }
        }
        */
    }
}
