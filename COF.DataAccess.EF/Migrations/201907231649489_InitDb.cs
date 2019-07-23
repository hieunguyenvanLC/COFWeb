namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderNumber = c.String(maxLength: 45),
                        ItemName = c.String(maxLength: 45),
                        Date = c.String(maxLength: 45),
                        TimeCreated = c.String(maxLength: 45),
                        TimeCompleted = c.String(maxLength: 45),
                        Description = c.String(maxLength: 45),
                        UserId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        OrderStatus = c.String(),
                        ShopId = c.Int(nullable: false),
                        PaymentType = c.String(),
                        BonusPoint = c.String(),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedOnUtc = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shop", t => t.ShopId)
                .Index(t => t.ShopId);
            
            CreateTable(
                "dbo.OrderDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedOnUtc = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Order", t => t.OrderId)
                .ForeignKey("dbo.Product", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductName = c.String(maxLength: 45),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(maxLength: 45),
                        ShopId = c.Int(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedOnUtc = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shop", t => t.ShopId)
                .Index(t => t.ShopId);
            
            CreateTable(
                "dbo.ProductHasRawMaterials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RawMaterialId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        RawMaterialUnitId = c.Int(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedOnUtc = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.RawMaterial", t => t.RawMaterialId, cascadeDelete: true)
                .Index(t => t.RawMaterialId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.RawMaterial",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        AutoTotalQty = c.Int(nullable: false),
                        RawMaterialUnitId = c.Int(nullable: false),
                        UserInputTotalQty = c.Int(nullable: false),
                        ShopId = c.Int(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedOnUtc = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RawMaterialUnit", t => t.RawMaterialUnitId, cascadeDelete: true)
                .ForeignKey("dbo.Shop", t => t.ShopId, cascadeDelete: true)
                .Index(t => t.RawMaterialUnitId)
                .Index(t => t.ShopId);
            
            CreateTable(
                "dbo.RawMaterialUnit",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedOnUtc = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Shop",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShopName = c.String(maxLength: 45),
                        PhoneNumber = c.String(maxLength: 45),
                        Address = c.String(maxLength: 45),
                        City = c.String(maxLength: 45),
                        State = c.String(maxLength: 45),
                        ZipCode = c.String(maxLength: 45),
                        Description = c.String(maxLength: 45),
                        Status = c.String(maxLength: 45),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedOnUtc = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Table",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TableNumber = c.String(maxLength: 45),
                        Description = c.String(maxLength: 45),
                        Status = c.String(maxLength: 45),
                        ShopId = c.Int(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedOnUtc = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shop", t => t.ShopId)
                .Index(t => t.ShopId);
            
            CreateTable(
                "dbo.TableHasOrder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TableId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedOnUtc = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Order", t => t.OrderId)
                .ForeignKey("dbo.Table", t => t.TableId)
                .Index(t => t.TableId)
                .Index(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Order", "ShopId", "dbo.Shop");
            DropForeignKey("dbo.OrderDetail", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Product", "ShopId", "dbo.Shop");
            DropForeignKey("dbo.TableHasOrder", "TableId", "dbo.Table");
            DropForeignKey("dbo.TableHasOrder", "OrderId", "dbo.Order");
            DropForeignKey("dbo.Table", "ShopId", "dbo.Shop");
            DropForeignKey("dbo.RawMaterial", "ShopId", "dbo.Shop");
            DropForeignKey("dbo.RawMaterial", "RawMaterialUnitId", "dbo.RawMaterialUnit");
            DropForeignKey("dbo.ProductHasRawMaterials", "RawMaterialId", "dbo.RawMaterial");
            DropForeignKey("dbo.ProductHasRawMaterials", "ProductId", "dbo.Product");
            DropForeignKey("dbo.OrderDetail", "OrderId", "dbo.Order");
            DropIndex("dbo.TableHasOrder", new[] { "OrderId" });
            DropIndex("dbo.TableHasOrder", new[] { "TableId" });
            DropIndex("dbo.Table", new[] { "ShopId" });
            DropIndex("dbo.RawMaterial", new[] { "ShopId" });
            DropIndex("dbo.RawMaterial", new[] { "RawMaterialUnitId" });
            DropIndex("dbo.ProductHasRawMaterials", new[] { "ProductId" });
            DropIndex("dbo.ProductHasRawMaterials", new[] { "RawMaterialId" });
            DropIndex("dbo.Product", new[] { "ShopId" });
            DropIndex("dbo.OrderDetail", new[] { "OrderId" });
            DropIndex("dbo.OrderDetail", new[] { "ProductId" });
            DropIndex("dbo.Order", new[] { "ShopId" });
            DropTable("dbo.TableHasOrder");
            DropTable("dbo.Table");
            DropTable("dbo.Shop");
            DropTable("dbo.RawMaterialUnit");
            DropTable("dbo.RawMaterial");
            DropTable("dbo.ProductHasRawMaterials");
            DropTable("dbo.Product");
            DropTable("dbo.OrderDetail");
            DropTable("dbo.Order");
        }
    }
}
