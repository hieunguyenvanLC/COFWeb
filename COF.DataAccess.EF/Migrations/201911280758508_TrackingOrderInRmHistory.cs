namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrackingOrderInRmHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RawMaterialHistory", "OrderId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RawMaterialHistory", "OrderId");
        }
    }
}
