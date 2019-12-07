namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePrecisionForUnitCost : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RawMaterial", "UnitCost", c => c.Decimal(nullable: false, precision: 18, scale: 0));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RawMaterial", "UnitCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
