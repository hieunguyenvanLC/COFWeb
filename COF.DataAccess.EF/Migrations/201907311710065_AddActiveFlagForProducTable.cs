namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActiveFlagForProducTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "IsActive");
        }
    }
}
