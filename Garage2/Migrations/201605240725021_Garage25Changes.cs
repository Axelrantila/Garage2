namespace Garage2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Garage25Changes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TypeOfVehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Vehicles", "TypeOfVehicleNew_Id", c => c.Int());
            CreateIndex("dbo.Vehicles", "TypeOfVehicleNew_Id");
            AddForeignKey("dbo.Vehicles", "TypeOfVehicleNew_Id", "dbo.TypeOfVehicles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vehicles", "TypeOfVehicleNew_Id", "dbo.TypeOfVehicles");
            DropIndex("dbo.Vehicles", new[] { "TypeOfVehicleNew_Id" });
            DropColumn("dbo.Vehicles", "TypeOfVehicleNew_Id");
            DropTable("dbo.TypeOfVehicles");
        }
    }
}
