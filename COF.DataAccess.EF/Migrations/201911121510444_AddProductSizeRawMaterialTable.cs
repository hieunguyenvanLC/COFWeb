namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductSizeRawMaterialTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductSizeRawMaterial",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductSizeId = c.Int(nullable: false),
                        RawMaterialId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedOnUtc = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProductSizeRawMaterial_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductSize", t => t.ProductSizeId, cascadeDelete: true)
                .ForeignKey("dbo.RawMaterial", t => t.RawMaterialId, cascadeDelete: true)
                .Index(t => t.ProductSizeId)
                .Index(t => t.RawMaterialId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductSizeRawMaterial", "RawMaterialId", "dbo.RawMaterial");
            DropForeignKey("dbo.ProductSizeRawMaterial", "ProductSizeId", "dbo.ProductSize");
            DropIndex("dbo.ProductSizeRawMaterial", new[] { "RawMaterialId" });
            DropIndex("dbo.ProductSizeRawMaterial", new[] { "ProductSizeId" });
            DropTable("dbo.ProductSizeRawMaterial",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProductSizeRawMaterial_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
