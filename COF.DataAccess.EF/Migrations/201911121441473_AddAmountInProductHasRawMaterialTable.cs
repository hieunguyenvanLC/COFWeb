namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAmountInProductHasRawMaterialTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductHasRawMaterial", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductHasRawMaterial", "Amount");
        }
    }
}
