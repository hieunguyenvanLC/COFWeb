namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ProductHasRawMaterials", newName: "ProductHasRawMaterial");
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PhoneNumber = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        FullName = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        BonusLevelId = c.Int(nullable: false),
                        TotalBonusPoint = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedOnUtc = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BonusLevel", t => t.BonusLevelId)
                .Index(t => t.BonusLevelId);
            
            CreateTable(
                "dbo.BonusLevel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        PointToReach = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MoneyToOnePoint = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedOnUtc = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BonusPointHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeAccess = c.Int(nullable: false),
                        OldPoint = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Point = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OldLevel = c.String(),
                        Level = c.String(),
                        CustomerId = c.Int(nullable: false),
                        Description = c.String(),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedOnUtc = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.RawMaterialHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeAccess = c.DateTime(nullable: false),
                        TransactionTypeId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        InputTypeId = c.Int(nullable: false),
                        RawMaterialId = c.Int(nullable: false),
                        TotalQtyAtTimeAccess = c.Int(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedOnUtc = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RawMaterial", t => t.RawMaterialId, cascadeDelete: true)
                .Index(t => t.RawMaterialId);
            
            CreateIndex("dbo.Order", "CustomerId");
            AddForeignKey("dbo.Order", "CustomerId", "dbo.Customer", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RawMaterialHistory", "RawMaterialId", "dbo.RawMaterial");
            DropForeignKey("dbo.Order", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.BonusPointHistory", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.Customer", "BonusLevelId", "dbo.BonusLevel");
            DropIndex("dbo.RawMaterialHistory", new[] { "RawMaterialId" });
            DropIndex("dbo.BonusPointHistory", new[] { "CustomerId" });
            DropIndex("dbo.Customer", new[] { "BonusLevelId" });
            DropIndex("dbo.Order", new[] { "CustomerId" });
            DropTable("dbo.RawMaterialHistory");
            DropTable("dbo.BonusPointHistory");
            DropTable("dbo.BonusLevel");
            DropTable("dbo.Customer");
            RenameTable(name: "dbo.ProductHasRawMaterial", newName: "ProductHasRawMaterials");
        }
    }
}
