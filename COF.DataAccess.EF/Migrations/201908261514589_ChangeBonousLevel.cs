namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeBonousLevel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BonusLevel", "StartPointToReach", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BonusLevel", "EndPointToReach", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.BonusLevel", "PointToReach");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BonusLevel", "PointToReach", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.BonusLevel", "EndPointToReach");
            DropColumn("dbo.BonusLevel", "StartPointToReach");
        }
    }
}
