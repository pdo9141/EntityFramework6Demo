using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using NinjaDomain.Classes;
using NinjaDomain.DataModel;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            // SETTING THE INITIALIZER TO NULL WILL TELLS EF TO ONLY EXECUTE DESIRED COMMAND, E.G., INSERT NINJA SQL
            Database.SetInitializer(new NullDatabaseInitializer<NinjaContext>());
            //InsertNinja();
            //InsertMultipleNinjas();
            //InsertNinjaWithEquipment();
            //SimpleNinjaQueries();
            //SimpleNinjaGraphQuery();
            //QueryAndUpdateNinja();
            //QueryAndUpdateNinjaDisconnected();
            //RetrieveDataWithFind();
            //RetrieveDataWithStoredProc();
            //DeleteNinja();
            //DeleteNinjaWithStoredProc();
            ProjectionQuery();

            Console.ReadLine();
        }

        private static void ProjectionQuery()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninjas = context.Ninjas
                    .Select(n => new { n.Name, n.DateOfBirth, n.EquipmentOwned })
                    .ToList();
            }
        }

        private static void InsertNinja()
        {
            var ninja = new Ninja
            {
                Name = "SampsonSan",
                ServedInOniwaban = false,
                DateOfBirth = new DateTime(2008, 1, 28),
                ClanId = 1
            };

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.Add(ninja);
                context.SaveChanges();
            }
        }

        private static void InsertMultipleNinjas()
        {
            var ninja1 = new Ninja
            {
                Name = "Leonardo",
                ServedInOniwaban = false,
                DateOfBirth = new DateTime(1984, 1, 1),
                ClanId = 1
            };

            var ninja2 = new Ninja
            {
                Name = "Raphael",
                ServedInOniwaban = false,
                DateOfBirth = new DateTime(1985, 1, 1),
                ClanId = 1
            };

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.AddRange(new List<Ninja> { ninja1, ninja2 });
                context.SaveChanges();
            }
        }

        private static void InsertNinjaWithEquipment()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var ninja = new Ninja
                {
                    Name = "Kacy Catanzaro",
                    ServedInOniwaban = false,
                    DateOfBirth = new DateTime(1990, 1, 14),
                    ClanId = 1
                };

                var muscles = new NinjaEquipment
                {
                    Name = "Muscles",
                    Type = EquipmentType.Tool
                };

                var spunk = new NinjaEquipment
                {
                    Name = "Spunk",
                    Type = EquipmentType.Weapon
                };

                context.Ninjas.Add(ninja);
                ninja.EquipmentOwned.Add(muscles);
                ninja.EquipmentOwned.Add(spunk);
                context.SaveChanges();
            }
        }

        private static void SimpleNinjaQueries()
        {
            using (var context = new NinjaContext())
            {
                var ninjas = context.Ninjas.ToList();

                /*
                var query = context.Ninjas;
                var someNinjas = query.ToList();

                foreach (var ninja in query)
                {
                    // AVOID DOING LOTS OF WORK IN AN ENUMERATION
                    // THAT IS ALSO RESPONSIBLE FOR QUERY EXECUTION
                    // DATABASE CONNECTION WILL BE HELD OPEN FOR A LONG TIME
                    Console.WriteLine(ninja.Name);
                }
                */
            }

            using (var context = new NinjaContext())
            {
                // WHERE IS NOT AN EXECUTING METHOD, SO DB COMMANDS WON'T GET EXECUTED UNTIL ITERATING NINJAS IN FOREACH
                var ninjas = context.Ninjas.Where(n => n.Name == "Raphael");
                foreach (var ninja in ninjas)
                    Console.WriteLine(ninja.Name);
            }

            Console.WriteLine("");

            using (var context = new NinjaContext())
            {
                var ninjas = context.Ninjas.Where(n => n.DateOfBirth >= new DateTime(1984, 1, 1));
                foreach (var ninja in ninjas)
                    Console.WriteLine(ninja.Name);
            }

            Console.WriteLine("");

            using (var context = new NinjaContext())
            {
                // FIRSTORDEFAULT IS AN EXECUTING METHOD
                var ninja = context.Ninjas.Where(n => n.DateOfBirth >= new DateTime(1984, 1, 1)).FirstOrDefault();
                if (ninja != null)
                    Console.WriteLine(ninja.Name);
            }

            Console.WriteLine("");

            using (var context = new NinjaContext())
            {
                // BE CAREFUL OF WHICH COMMANDS ARE EXECUTING COMMANDS
                // YOU MIGHT RUN INTO PERFORMANCE ISSUES WHEN YOUR FILTERING IS DONE IN MEMORY INSTEAD OF THE DB
                // THIS HAPPENS WHEN YOU RETURN THE LIST FIRST BY UNKNOWINGLY RUNNING AN EXECUTING METHOD
                // THEN FILTER USING WHERE AFTERWARDS
                var ninja = context.Ninjas
                    .Where(n => n.DateOfBirth >= new DateTime(1984, 1, 1))
                    .OrderBy(n => n.Name)
                    .Skip(1)
                    .Take(1)
                    .FirstOrDefault();

                if (ninja != null)
                    Console.WriteLine(ninja.Name);
            }
        }

        private static void SimpleNinjaGraphQuery()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                // USING THE INCLUDE METHOD ENABLES EAGER LOADING
                // BE CAREFUL WHEN USING TOO MANY INCLUDES FOR DEEP GRAPHS SINCE QUERY PERFORMANCE WILL DEGRADE
                var ninja = context.Ninjas.Include(n => n.EquipmentOwned)
                    .FirstOrDefault(n => n.Name.StartsWith("Kacy"));
            }

            // LEVERAGE EXPLICIT LOADING IF YOU DON'T NEED THE GRAPH TILL LATER
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var ninja = context.Ninjas
                    .FirstOrDefault(n => n.Name.StartsWith("Kacy"));

                Console.WriteLine("Ninja Retrieved:" + ninja.Name);
                context.Entry(ninja).Collection(n => n.EquipmentOwned).Load();
            }

            // LEVERAGE LAZY LOADING BY MARKING POCO FIELD WITH VIRTUAL KEYWORD
            // CALLING THE COUNT() METHOD ON EQUIPMENTOWNED WILL TRIGGER THE LOAD
            // ALL LOT OF FOLKS ARE NOT A BIG FAN OF LAZY LOADING, BETTER TO KNOW WHAT YOU'RE EXPECTING AND EXPLICIT LOAD WHAT YOU NEED
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var ninja = context.Ninjas
                    .FirstOrDefault(n => n.Name.StartsWith("Kacy"));

                Console.WriteLine("Ninja Retrieved: " + ninja.Name);
                Console.WriteLine("Ninja Equipment Cound: {0}", ninja.EquipmentOwned.Count());
            }
        }

        private static void QueryAndUpdateNinja()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.FirstOrDefault();
                ninja.ServedInOniwaban = !ninja.ServedInOniwaban;
                context.SaveChanges();
            }
        }

        private static void QueryAndUpdateNinjaDisconnected()
        {
            Ninja ninja;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
            }

            ninja.ServedInOniwaban = !ninja.ServedInOniwaban;

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.Attach(ninja);
                context.Entry(ninja).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        private static void RetrieveDataWithFind()
        {
            // BENEFIT OF FIND IS THAT IF THE OBJECT WAS ALREADY QUERIED AND CACHED TO MEMORY IT WILL NOT MAKE ANOTHER TRIP TO THE DATABASE
            var keyval = 4;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.Find(keyval);
                Console.WriteLine("After Find#1: " + ninja.Name);

                var someNinja = context.Ninjas.Find(keyval);
                Console.WriteLine("After Find#2: " + someNinja.Name);
                ninja = null;
            }
        }

        private static void RetrieveDataWithStoredProc()
        {
            //CREATE PROCEDURE GetOldNinjas AS SELECT * FROM Ninjas WHERE DateOfBirth <= '1/1/1980'
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninjas = context.Ninjas.SqlQuery("EXEC GetOldNinjas");
                foreach (var ninja in ninjas)
                    Console.WriteLine(ninja.Name);
            }
        }

        private static void DeleteNinja()
        {
            Ninja ninja;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
                //context.Ninjas.Remove(ninja);
                //context.SaveChanges();
            }

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                //context.Ninjas.Attach(ninja);
                //context.Ninjas.Remove(ninja);
                context.Entry(ninja).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        private static void DeleteNinjaWithStoredProc()
        {
            //CREATE PROCEDURE DeleteNinjaViaId @Id INT AS DELETE Ninjas WHERE Id = @Id
            var keyval = 2;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Database.ExecuteSqlCommand("EXEC DeleteNinjaViaId {0}", keyval);
            }

            /*
            using (var context = new ServicesDBContext())
            {
                var entity = new SigningProviderInfoRequestEntity
                {
                    TaskId = Guid.NewGuid()
                };
                db.SigningProviderInfoRequestEntities.AddObject(entity);
                db.SaveChanges();

                var param1 = new SqlParameter("@SigningProviderInfoRequestId", entity.Id);
                var param2 = new SqlParameter("@ClientId", 91);

                db.ExecuteStoreCommand("SP_name @SigningProviderInfoRequestId, @ClientId", param1, param2)

                var param3 = new SqlParameter("@EntityTypeId", 1);
                var param4 = new SqlParameter("@EntityId", 2);

                var results = db
                    .ExecuteStoreQuery<AuditAccessData>("AuditAccessData_Sel @EntityTypeId, @EntityId", param3, param4)
                    .ToList();

                results = db
                    .ExecuteStoreQuery<AuditAccessData>("AuditAccessData_Sel @EntityTypeId, @EntityId", param3, param4)
                    .Select(r => new { Id = r.Id, UserName = r.UserName })
                    .ToList();
            }
            */
        }

        private static void JoinSamples()
        {
            /*
            using (var db = new OrderDBContext())
            {
                var query = (from a in db.Addresses
                             from s in db.States
                             from c in db.Counties
                             where a.City == "Irvine" && a.State == s.Abbr
                             && c.Name == a.County && c.StateFips == s.StateFips
                             select new
                             {
                                 AddressId = a.AddressId,
                                 State = s.Name,
                                 County = c.Name
                             }).Take(9);

                query = (from a in db.Addresses
                         join s in db.States on a.State equals s.Abbr
                         join c in db.Counties on new { County = a.County, StateFips = s.StateFips } equals new { County = c.Name, StateFips = c.StateFips }
                         where a.City.Equals("Irvine")
                         select new
                         {
                             AddressId = a.AddressId,
                             State = s.Name,
                             County = c.Name
                         }).Take(9);
            }
            */
        }
    }
}
