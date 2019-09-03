namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVouchourSupportDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetail", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetail", "Description");
        }
    }
}
