namespace CodeModelFromDB
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AWModel : DbContext
    {
        public AWModel()
            : base("name=AWModel")
        {
        }

        public virtual DbSet<Borrower> Borrowers { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Borrower>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Borrower>()
                .Property(e => e.MiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<Borrower>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.Value)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.ReferenceNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .HasOptional(e => e.OrderDetail)
                .WithRequired(e => e.Order);
        }
    }
}
