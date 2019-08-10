namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequired : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Customer", new[] { "BonusLevelId" });
            AlterColumn("dbo.Customer", "BonusLevelId", c => c.Int());
            CreateIndex("dbo.Customer", "BonusLevelId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Customer", new[] { "BonusLevelId" });
            AlterColumn("dbo.Customer", "BonusLevelId", c => c.Int(nullable: false));
            CreateIndex("dbo.Customer", "BonusLevelId");
        }
    }
}
