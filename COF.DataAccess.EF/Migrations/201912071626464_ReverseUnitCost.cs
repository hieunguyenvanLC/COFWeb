namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReverseUnitCost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RawMaterial", "UnitCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.RawMaterialUnit", "UnitCost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RawMaterialUnit", "UnitCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.RawMaterial", "UnitCost");
        }
    }
}
