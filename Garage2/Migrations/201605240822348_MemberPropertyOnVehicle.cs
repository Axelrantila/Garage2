namespace Garage2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MemberPropertyOnVehicle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "MemberId", c => c.Int());
            CreateIndex("dbo.Vehicles", "MemberId");
            AddForeignKey("dbo.Vehicles", "MemberId", "dbo.Members", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vehicles", "MemberId", "dbo.Members");
            DropIndex("dbo.Vehicles", new[] { "MemberId" });
            DropColumn("dbo.Vehicles", "MemberId");
        }
    }
}
