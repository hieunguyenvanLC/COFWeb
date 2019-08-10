namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditDb : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderDetail", "ProductId", "dbo.Product");
            DropIndex("dbo.OrderDetail", new[] { "ProductId" });
            AddColumn("dbo.OrderDetail", "ProductSizeId", c => c.Int(nullable: false));
            CreateIndex("dbo.OrderDetail", "ProductSizeId");
            AddForeignKey("dbo.OrderDetail", "ProductSizeId", "dbo.ProductSize", "Id");
            DropColumn("dbo.OrderDetail", "ProductId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderDetail", "ProductId", c => c.Int(nullable: false));
            DropForeignKey("dbo.OrderDetail", "ProductSizeId", "dbo.ProductSize");
            DropIndex("dbo.OrderDetail", new[] { "ProductSizeId" });
            DropColumn("dbo.OrderDetail", "ProductSizeId");
            CreateIndex("dbo.OrderDetail", "ProductId");
            AddForeignKey("dbo.OrderDetail", "ProductId", "dbo.Product", "Id");
        }
    }
}
