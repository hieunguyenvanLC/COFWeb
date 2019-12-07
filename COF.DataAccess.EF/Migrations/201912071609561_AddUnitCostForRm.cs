namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUnitCostForRm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RawMaterialUnit", "UnitCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RawMaterialUnit", "UnitCost");
        }
    }
}
