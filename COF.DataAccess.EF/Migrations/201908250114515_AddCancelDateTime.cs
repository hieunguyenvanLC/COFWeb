namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCancelDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Order", "CancelDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Order", "CancelDateTime");
        }
    }
}
