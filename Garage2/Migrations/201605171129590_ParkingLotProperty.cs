namespace Garage2.Migrations
{
    using DataAccessLayer;
    using Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public partial class ParkingLotProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "ParkingLot", c => c.Int(nullable: false));

            //VehicleContext db = new VehicleContext();
            //int lot = Garage.MinLotNr;
            //foreach(var v in db.Vehicles.ToList()) {
            //    if ( v.Parked ) {
            //        v.ParkingLot = lot;
            //        lot += (int)Garage.NrOfLotsRequired[v.TypeOfVehicle];
            //    }
            //    else
            //        v.ParkingLot = 0;
            //}
            //db.SaveChanges();
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vehicles", "ParkingLot");
        }
    }
}
