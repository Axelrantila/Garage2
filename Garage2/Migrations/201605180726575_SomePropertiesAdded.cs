namespace Garage2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SomePropertiesAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "MakeAndModel", c => c.String());
            AddColumn("dbo.Vehicles", "Color", c => c.String());
            AddColumn("dbo.Vehicles", "NrOfWheels", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vehicles", "NrOfWheels");
            DropColumn("dbo.Vehicles", "Color");
            DropColumn("dbo.Vehicles", "MakeAndModel");
        }
    }
}
