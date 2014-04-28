using System.Data.Entity;

using VGRental.Model;

namespace VGRental
{
    public class VGRentalEntities : DbContext
    {
        public VGRentalEntities() : base(@"Data source=ec2-54-187-62-7.us-west-2.compute.amazonaws.com,1433;Integrated Security=False;User ID=sa;Password=o.KhNu2Lo?;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False") { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatusCodes { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<VGRental.Model.System> Systems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Game
            modelBuilder.Entity<Game>().HasKey(g => new { g.SystemName, g.GameName });

            modelBuilder.Entity<Game>().HasRequired(g => g.System)
                .WithMany(s => s.Games).HasForeignKey(g => g.SystemName);

            modelBuilder.Entity<Game>().Property(g => g.Multiplayer).IsOptional();
            modelBuilder.Entity<Game>().Property(g => g.NumberOfPlayers).IsOptional();
            #endregion

            modelBuilder.Entity<OrderDetails>().HasKey(o => new { o.OrderID });

            //modelBuilder.Entity<OrderDetails>().HasRequired(o => o.Order)
            //    .WithMany(o => o.OrderID)

            //modelBuilder.Entity<OrderDetails>().HasRequired(o => o.Order)
            //    .WithRequiredPrincipal(o => o.OrderDetails);
            //modelBuilder.Entity<Order>().HasRequired(o => o.OrderDetails)
            //    .WithRequiredDependent(o => o.Order);
                //.WithRequiredPrincipal(o => o.Order);

            modelBuilder.Entity<OrderDetails>().Property(o => o.Status).IsRequired();
            modelBuilder.Entity<OrderDetails>().Property(o => o.ShipDate).IsOptional();

            #region Promotion
            modelBuilder.Entity<Promotion>()
                .HasKey(p => new { p.OrderID, p.PromotionID });

            modelBuilder.Entity<Promotion>().HasRequired(p => p.Order)
                .WithOptional(p => p.Promotion);

            modelBuilder.Entity<Promotion>().HasRequired(p => p.OrderDetails)
                .WithOptional(p => p.Promotion);
            #endregion

            #region subscription
            //modelBuilder.Entity<Subscription>()
            //    .HasKey(s => new { s.customerID, s.SubscriptionID });

            modelBuilder.Entity<Subscription>().HasRequired(s => s.Customer)
                .WithMany(c => c.Subscriptions).HasForeignKey(s => s.customerID);
            #endregion

            #region Order
            modelBuilder.Entity<Order>().HasKey(o => new { o.OrderID });
            //modelBuilder.Entity<Order>().HasKey(o => new { o.OrderID, customerID = o.CustomerID });

            modelBuilder.Entity<Order>().HasRequired(o => o.Customer)
                .WithMany(c => c.Orders).HasForeignKey(o => o.CustomerID);

            modelBuilder.Entity<Order>().HasRequired(o => o.Game)
                .WithMany(g => g.Orders).HasForeignKey(g => new { g.SystemName, g.GameName });
            #endregion

            modelBuilder.Entity<VGRental.Model.System>().HasKey(s => s.Name);

            #region customer
            modelBuilder.Entity<Customer>().Property(c => c.DOB).IsOptional();
            modelBuilder.Entity<Customer>().Property(c => c.Email).IsRequired();
            modelBuilder.Entity<Customer>().Property(c => c.Email).HasMaxLength(254);
            modelBuilder.Entity<Customer>().Property(c => c.Name).IsRequired();
            modelBuilder.Entity<Customer>().Property(c => c.Name).HasMaxLength(70);
            modelBuilder.Entity<Customer>().Property(c => c.UserName).IsRequired();
            modelBuilder.Entity<Customer>().Property(c => c.UserName).HasMaxLength(254);
            modelBuilder.Entity<Customer>().Property(c => c.Password).IsRequired();
            modelBuilder.Entity<Customer>().Property(c => c.ShippingAddress).IsRequired();
            modelBuilder.Entity<Customer>().Property(c => c.ShippingAddress).HasMaxLength(95);
            modelBuilder.Entity<Customer>().Property(c => c.City).IsRequired();
            modelBuilder.Entity<Customer>().Property(c => c.City).HasMaxLength(35);
            modelBuilder.Entity<Customer>().Property(c => c.State).IsRequired();
            modelBuilder.Entity<Customer>().Property(c => c.State).HasMaxLength(20);
            #endregion

            modelBuilder.Entity<OrderStatus>().HasKey(c => c.Status);
            modelBuilder.Entity<OrderStatus>().Property(c => c.Status).IsRequired();
        }
    }
}
