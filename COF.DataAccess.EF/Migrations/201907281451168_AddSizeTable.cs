namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSizeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductSize",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        SizeId = c.Int(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedOnUtc = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId)
                .ForeignKey("dbo.Size", t => t.SizeId)
                .Index(t => t.ProductId)
                .Index(t => t.SizeId);
            
            CreateTable(
                "dbo.Size",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedOnUtc = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Product", "Cost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Product", "Cost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropForeignKey("dbo.ProductSize", "SizeId", "dbo.Size");
            DropForeignKey("dbo.ProductSize", "ProductId", "dbo.Product");
            DropIndex("dbo.ProductSize", new[] { "SizeId" });
            DropIndex("dbo.ProductSize", new[] { "ProductId" });
            DropTable("dbo.Size");
            DropTable("dbo.ProductSize");
        }
    }
}
