namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIdentity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RolePermission",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CanView = c.Boolean(nullable: false),
                        CanModify = c.Boolean(nullable: false),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        PermissionId = c.Int(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedOnUtc = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Permission", t => t.PermissionId, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.PermissionId);
            
            CreateTable(
                "dbo.Permission",
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
                "dbo.UserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        AppUser_Id = c.String(maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.User", t => t.AppUser_Id)
                .ForeignKey("dbo.Role", t => t.IdentityRole_Id)
                .Index(t => t.AppUser_Id)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.ShopHasUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        ShopId = c.Int(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedOnUtc = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shop", t => t.ShopId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ShopId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(maxLength: 256),
                        Address = c.String(maxLength: 256),
                        Avatar = c.String(),
                        BirthDay = c.DateTime(),
                        City = c.String(),
                        State = c.String(),
                        Gender = c.Boolean(),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        AppUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.AppUser_Id)
                .Index(t => t.AppUser_Id);
            
            CreateTable(
                "dbo.UserLogin",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        AppUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.User", t => t.AppUser_Id)
                .Index(t => t.AppUser_Id);
            
            CreateTable(
                "dbo.UserWorkingTime",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        UserId = c.String(nullable: false, maxLength: 128),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedOnUtc = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Customer", "ActiveBonusPoint", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BonusPointHistory", "ActiveBonusPoint", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BonusPointHistory", "OrderId", c => c.Int(nullable: false));
            AlterColumn("dbo.Order", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Order", "UserId");
            CreateIndex("dbo.BonusPointHistory", "OrderId");
            AddForeignKey("dbo.BonusPointHistory", "OrderId", "dbo.Order", "Id");
            AddForeignKey("dbo.Order", "UserId", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRole", "IdentityRole_Id", "dbo.Role");
            DropForeignKey("dbo.Order", "UserId", "dbo.User");
            DropForeignKey("dbo.UserWorkingTime", "UserId", "dbo.User");
            DropForeignKey("dbo.ShopHasUser", "UserId", "dbo.User");
            DropForeignKey("dbo.UserRole", "AppUser_Id", "dbo.User");
            DropForeignKey("dbo.UserLogin", "AppUser_Id", "dbo.User");
            DropForeignKey("dbo.UserClaim", "AppUser_Id", "dbo.User");
            DropForeignKey("dbo.ShopHasUser", "ShopId", "dbo.Shop");
            DropForeignKey("dbo.BonusPointHistory", "OrderId", "dbo.Order");
            DropForeignKey("dbo.RolePermission", "RoleId", "dbo.Role");
            DropForeignKey("dbo.RolePermission", "PermissionId", "dbo.Permission");
            DropIndex("dbo.UserWorkingTime", new[] { "UserId" });
            DropIndex("dbo.UserLogin", new[] { "AppUser_Id" });
            DropIndex("dbo.UserClaim", new[] { "AppUser_Id" });
            DropIndex("dbo.ShopHasUser", new[] { "ShopId" });
            DropIndex("dbo.ShopHasUser", new[] { "UserId" });
            DropIndex("dbo.BonusPointHistory", new[] { "OrderId" });
            DropIndex("dbo.Order", new[] { "UserId" });
            DropIndex("dbo.UserRole", new[] { "IdentityRole_Id" });
            DropIndex("dbo.UserRole", new[] { "AppUser_Id" });
            DropIndex("dbo.RolePermission", new[] { "PermissionId" });
            DropIndex("dbo.RolePermission", new[] { "RoleId" });
            AlterColumn("dbo.Order", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.BonusPointHistory", "OrderId");
            DropColumn("dbo.BonusPointHistory", "ActiveBonusPoint");
            DropColumn("dbo.Customer", "ActiveBonusPoint");
            DropTable("dbo.UserWorkingTime");
            DropTable("dbo.UserLogin");
            DropTable("dbo.UserClaim");
            DropTable("dbo.User");
            DropTable("dbo.ShopHasUser");
            DropTable("dbo.UserRole");
            DropTable("dbo.Permission");
            DropTable("dbo.RolePermission");
            DropTable("dbo.Role");
        }
    }
}
