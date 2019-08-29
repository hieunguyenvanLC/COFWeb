namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCategoryId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetail", "CategoryId", c => c.Int(nullable: false));
            AddColumn("dbo.Category", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Category", "Type");
            DropColumn("dbo.OrderDetail", "CategoryId");
        }
    }
}
