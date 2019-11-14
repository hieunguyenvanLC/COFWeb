namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescriptionForRmHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RawMaterialHistory", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RawMaterialHistory", "Description");
        }
    }
}
