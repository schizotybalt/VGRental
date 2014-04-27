using System;
using System.Data.Entity.Migrations;
using System.IO;

using VGRental.Model;

namespace VGRental.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<VGRental.VGRentalEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(VGRentalEntities context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();

            context.Systems.Add(new VGRental.Model.System
            {
                Name = "Playstation 4",
                Manf = "Sony Inc."
            });
            context.Systems.Add(new VGRental.Model.System
            {
                Name = "Playstation 3",
                Manf = "Sony Inc."
            });
            context.Systems.Add(new VGRental.Model.System
            {
                Name = "Xbox One",
                Manf = "Microsoft Inc."
            });
            context.Systems.Add(new VGRental.Model.System
            {
                Name = "Xbox 360",
                Manf = "Microsoft Inc."
            });
            context.Systems.Add(new VGRental.Model.System
            {
                Name = "Wii U",
                Manf = "Nintendo Inc."
            });
            context.Systems.Add(new VGRental.Model.System
            {
                Name = "Wii",
                Manf = "Nintendo Inc."
            });

            context.OrderStatusCodes.Add(new OrderStatus { Status = "Submitted" });
            context.OrderStatusCodes.Add(new OrderStatus { Status = "Cancelled" });
            context.OrderStatusCodes.Add(new OrderStatus { Status = "Shipped" });
            context.OrderStatusCodes.Add(new OrderStatus { Status = "Delivered" });

            ParsePS4Games(context);

            context.Database.ExecuteSqlCommand("ALTER TABLE[dbo].[Customers] ADD UNIQUE(Email)");
            context.Database.ExecuteSqlCommand("ALTER TABLE[dbo].[Customers] ADD UNIQUE(UserName)");
            context.Database.ExecuteSqlCommand("alter table Games add constraint CopiesAvailable check (QuantityAvailable >= 0)");
            context.Database.ExecuteSqlCommand("delete from AspNetUsers");
        }

        private void ParsePS4Games(VGRentalEntities context)
        {
            StreamReader reader
                = File.OpenText(@"C:\Users\tiliska\Documents\Visual Studio 2013\Projects\VGRental\ps4games.txt");
            string line;
            Random rnd1 = new Random();

            while ((line = reader.ReadLine()) != null)
            {
                string[] items = line.Split('\t');
                int total_quantity = rnd1.Next(100, 500);
                int quantity_available = rnd1.Next(total_quantity);

                DateTime? release_date = null;
                try
                {
                    release_date = Convert.ToDateTime(items[7]);
                }
                catch (FormatException) { }

                context.Games.Add(new Game
                    {
                        SystemName = "Playstation 4",
                        GameName = items[0],
                        Genre = items[1],
                        Developer = items[2],
                        Publisher = items[3],
                        ReleaseDate = release_date,
                        TotalQuantity=total_quantity,
                        QuantityAvailable = quantity_available,
                    });
            }
        }
    }
}
