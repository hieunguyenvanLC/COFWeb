namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRmQtyDataType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RawMaterial", "AutoTotalQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.RawMaterial", "UserInputTotalQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.RawMaterialHistory", "Quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.RawMaterialHistory", "TotalQtyAtTimeAccess", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RawMaterialHistory", "TotalQtyAtTimeAccess", c => c.Int(nullable: false));
            AlterColumn("dbo.RawMaterialHistory", "Quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.RawMaterial", "UserInputTotalQty", c => c.Int(nullable: false));
            AlterColumn("dbo.RawMaterial", "AutoTotalQty", c => c.Int(nullable: false));
        }
    }
}
