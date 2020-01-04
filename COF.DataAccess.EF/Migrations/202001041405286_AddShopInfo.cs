namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShopInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shop", "Image", c => c.String());
            AddColumn("dbo.Shop", "StartTime", c => c.String());
            AddColumn("dbo.Shop", "EndTime", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shop", "EndTime");
            DropColumn("dbo.Shop", "StartTime");
            DropColumn("dbo.Shop", "Image");
        }
    }
}
