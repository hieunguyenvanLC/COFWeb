namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVouchourSupport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Order", "DiscountType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Order", "DiscountType");
        }
    }
}
