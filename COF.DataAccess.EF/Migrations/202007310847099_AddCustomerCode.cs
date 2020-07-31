namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "Code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer", "Code");
        }
    }
}
