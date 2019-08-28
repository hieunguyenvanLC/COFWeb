namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApiLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Order", "ApiLog", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Order", "ApiLog");
        }
    }
}
