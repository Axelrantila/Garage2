namespace Garage2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VehicleTypeId_Added : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vehicles", "TypeOfVehicleNew_Id", "dbo.TypeOfVehicles");
            DropIndex("dbo.Vehicles", new[] { "TypeOfVehicleNew_Id" });
            RenameColumn(table: "dbo.Vehicles", name: "TypeOfVehicleNew_Id", newName: "TypeOfVehicleNewId");
            AlterColumn("dbo.Vehicles", "TypeOfVehicleNewId", c => c.Int(nullable: false));
            CreateIndex("dbo.Vehicles", "TypeOfVehicleNewId");
            AddForeignKey("dbo.Vehicles", "TypeOfVehicleNewId", "dbo.TypeOfVehicles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vehicles", "TypeOfVehicleNewId", "dbo.TypeOfVehicles");
            DropIndex("dbo.Vehicles", new[] { "TypeOfVehicleNewId" });
            AlterColumn("dbo.Vehicles", "TypeOfVehicleNewId", c => c.Int());
            RenameColumn(table: "dbo.Vehicles", name: "TypeOfVehicleNewId", newName: "TypeOfVehicleNew_Id");
            CreateIndex("dbo.Vehicles", "TypeOfVehicleNew_Id");
            AddForeignKey("dbo.Vehicles", "TypeOfVehicleNew_Id", "dbo.TypeOfVehicles", "Id");
        }
    }
}
