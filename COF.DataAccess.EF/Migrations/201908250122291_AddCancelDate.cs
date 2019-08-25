namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCancelDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Order", "CancelDate", c => c.DateTime());
            AddColumn("dbo.Order", "CancelBy", c => c.String());
            DropColumn("dbo.Order", "CancelDateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Order", "CancelDateTime", c => c.DateTime());
            DropColumn("dbo.Order", "CancelBy");
            DropColumn("dbo.Order", "CancelDate");
        }
    }
}
