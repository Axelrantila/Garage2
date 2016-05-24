namespace Garage2.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Garage2.DataAccessLayer.VehicleContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed( Garage2.DataAccessLayer.VehicleContext context )
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

            context.TypeOfVehicles.AddOrUpdate(
                t => t.Name,
                new TypeOfVehicle { Name = "Car" },
                new TypeOfVehicle { Name = "SUV" },
                new TypeOfVehicle { Name = "Motorcycle" },
                new TypeOfVehicle { Name = "CarTrailer" },
                new TypeOfVehicle { Name = "Truck" },
                new TypeOfVehicle { Name = "Bus" }
                );

            context.Members.AddOrUpdate(
                t => new { t.Name, t.EmailAddress, t.PhoneNr},
                new Member { Name = "Axel Räntilä", EmailAddress = "axelrantila@axelrantila.com", PhoneNr = "+461234567890123456789"},
                new Member { Name = "Lorem Ipsum", EmailAddress = "lorem@ipsum.li", PhoneNr = "+6666666666666666666666666"},
                new Member { Name = "Reno Jackson", EmailAddress = "reno@leagueofexplorers.com", PhoneNr = "+3046646"}
                );
        }
    }
}
