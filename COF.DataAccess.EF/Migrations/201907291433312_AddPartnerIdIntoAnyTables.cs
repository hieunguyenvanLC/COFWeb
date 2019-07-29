namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPartnerIdIntoAnyTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Partner",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                        ParticipationDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedOnUtc = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Category", "PartnerId", c => c.Int(nullable: false));
            AddColumn("dbo.Product", "PartnerId", c => c.Int(nullable: false));
            AddColumn("dbo.OrderDetail", "PartnerId", c => c.Int(nullable: false));
            AddColumn("dbo.Order", "PartnerId", c => c.Int(nullable: false));
            AddColumn("dbo.BonusPointHistory", "PartnerId", c => c.Int(nullable: false));
            AddColumn("dbo.Customer", "PartnerId", c => c.Int(nullable: false));
            AddColumn("dbo.Shop", "PartnerId", c => c.Int(nullable: false));
            AddColumn("dbo.RawMaterial", "PartnerId", c => c.Int(nullable: false));
            AddColumn("dbo.ProductHasRawMaterial", "PartnerId", c => c.Int(nullable: false));
            AddColumn("dbo.RawMaterialHistory", "PartnerId", c => c.Int(nullable: false));
            AddColumn("dbo.ShopHasUser", "PartnerId", c => c.Int(nullable: false));
            AddColumn("dbo.User", "PartnerId", c => c.Int());
            AddColumn("dbo.UserWorkingTime", "PartnerId", c => c.Int(nullable: false));
            AddColumn("dbo.Table", "PartnerId", c => c.Int(nullable: false));
            AddColumn("dbo.TableHasOrder", "PartnerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Shop", "PartnerId");
            AddForeignKey("dbo.Shop", "PartnerId", "dbo.Partner", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shop", "PartnerId", "dbo.Partner");
            DropIndex("dbo.Shop", new[] { "PartnerId" });
            DropColumn("dbo.TableHasOrder", "PartnerId");
            DropColumn("dbo.Table", "PartnerId");
            DropColumn("dbo.UserWorkingTime", "PartnerId");
            DropColumn("dbo.User", "PartnerId");
            DropColumn("dbo.ShopHasUser", "PartnerId");
            DropColumn("dbo.RawMaterialHistory", "PartnerId");
            DropColumn("dbo.ProductHasRawMaterial", "PartnerId");
            DropColumn("dbo.RawMaterial", "PartnerId");
            DropColumn("dbo.Shop", "PartnerId");
            DropColumn("dbo.Customer", "PartnerId");
            DropColumn("dbo.BonusPointHistory", "PartnerId");
            DropColumn("dbo.Order", "PartnerId");
            DropColumn("dbo.OrderDetail", "PartnerId");
            DropColumn("dbo.Product", "PartnerId");
            DropColumn("dbo.Category", "PartnerId");
            DropTable("dbo.Partner");
        }
    }
}
