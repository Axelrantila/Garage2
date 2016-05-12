namespace Garage2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Owner = c.String(),
                        LicenseNr = c.String(),
                        TypeOfVehicle = c.Int(nullable: false),
                        Length = c.Single(nullable: false),
                        Weight = c.Single(nullable: false),
                        TimeParked = c.DateTime(nullable: false),
                        Parked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Vehicles");
        }
    }
}
