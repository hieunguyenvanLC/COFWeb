namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "ProductImage", c => c.String());
            AddColumn("dbo.Category", "CategoryImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Category", "CategoryImage");
            DropColumn("dbo.Product", "ProductImage");
        }
    }
}
