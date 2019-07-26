namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeMaxLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Permission", "Name", c => c.String(maxLength: 256));
            AlterColumn("dbo.Order", "OrderNumber", c => c.String(maxLength: 256));
            AlterColumn("dbo.Order", "ItemName", c => c.String(maxLength: 256));
            AlterColumn("dbo.Order", "Date", c => c.String(maxLength: 256));
            AlterColumn("dbo.Order", "TimeCreated", c => c.String(maxLength: 256));
            AlterColumn("dbo.Order", "TimeCompleted", c => c.String(maxLength: 256));
            AlterColumn("dbo.Order", "Description", c => c.String(maxLength: 256));
            AlterColumn("dbo.Product", "ProductName", c => c.String(maxLength: 256));
            AlterColumn("dbo.Product", "Description", c => c.String());
            AlterColumn("dbo.RawMaterialUnit", "Name", c => c.String(maxLength: 256));
            AlterColumn("dbo.Shop", "ShopName", c => c.String(maxLength: 250));
            AlterColumn("dbo.Shop", "Address", c => c.String(maxLength: 250));
            AlterColumn("dbo.Shop", "City", c => c.String(maxLength: 250));
            AlterColumn("dbo.Shop", "State", c => c.String(maxLength: 250));
            AlterColumn("dbo.Shop", "ZipCode", c => c.String(maxLength: 250));
            AlterColumn("dbo.Shop", "Description", c => c.String());
            AlterColumn("dbo.Shop", "Status", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Shop", "Status", c => c.String(maxLength: 45));
            AlterColumn("dbo.Shop", "Description", c => c.String(maxLength: 45));
            AlterColumn("dbo.Shop", "ZipCode", c => c.String(maxLength: 45));
            AlterColumn("dbo.Shop", "State", c => c.String(maxLength: 45));
            AlterColumn("dbo.Shop", "City", c => c.String(maxLength: 45));
            AlterColumn("dbo.Shop", "Address", c => c.String(maxLength: 45));
            AlterColumn("dbo.Shop", "ShopName", c => c.String(maxLength: 45));
            AlterColumn("dbo.RawMaterialUnit", "Name", c => c.String());
            AlterColumn("dbo.Product", "Description", c => c.String(maxLength: 45));
            AlterColumn("dbo.Product", "ProductName", c => c.String(maxLength: 45));
            AlterColumn("dbo.Order", "Description", c => c.String(maxLength: 45));
            AlterColumn("dbo.Order", "TimeCompleted", c => c.String(maxLength: 45));
            AlterColumn("dbo.Order", "TimeCreated", c => c.String(maxLength: 45));
            AlterColumn("dbo.Order", "Date", c => c.String(maxLength: 45));
            AlterColumn("dbo.Order", "ItemName", c => c.String(maxLength: 45));
            AlterColumn("dbo.Order", "OrderNumber", c => c.String(maxLength: 45));
            AlterColumn("dbo.Permission", "Name", c => c.String());
        }
    }
}
