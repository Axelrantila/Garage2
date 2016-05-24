namespace Garage2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewMemberProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "EmailAddress", c => c.String());
            AddColumn("dbo.Members", "PhoneNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "PhoneNumber");
            DropColumn("dbo.Members", "EmailAddress");
        }
    }
}
