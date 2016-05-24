namespace Garage2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewMemberProperties3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "PhoneNr", c => c.String());
            DropColumn("dbo.Members", "PhoneNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Members", "PhoneNumber", c => c.Int(nullable: false));
            DropColumn("dbo.Members", "PhoneNr");
        }
    }
}
