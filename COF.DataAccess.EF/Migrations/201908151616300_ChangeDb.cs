namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Order", "SourceId", c => c.Int());
            AddColumn("dbo.Order", "TableId", c => c.Int());
            AddColumn("dbo.Order", "IsFixedPrice", c => c.Boolean(nullable: false));
            AddColumn("dbo.Order", "SourceType", c => c.Int(nullable: false));
            AddColumn("dbo.Order", "LastRecordDate", c => c.DateTime());
            AddColumn("dbo.Order", "ServedPerson", c => c.String());
            AddColumn("dbo.Order", "DeliveryAddress", c => c.String());
            AddColumn("dbo.Order", "DeliveryStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Order", "DeliveryPhone", c => c.String());
            AddColumn("dbo.Order", "DeliveryCustomer", c => c.String());
            AddColumn("dbo.Order", "TotalInvoicePrint", c => c.Int(nullable: false));
            AddColumn("dbo.Order", "VAT", c => c.Double(nullable: false));
            AddColumn("dbo.Order", "VATAmount", c => c.Double(nullable: false));
            AddColumn("dbo.Order", "NumberOfGuest", c => c.Int(nullable: false));
            AddColumn("dbo.Order", "Att1", c => c.String());
            AddColumn("dbo.Order", "Att2", c => c.String());
            AddColumn("dbo.Order", "Att3", c => c.String());
            AddColumn("dbo.Order", "Att4", c => c.String());
            AddColumn("dbo.Order", "Att5", c => c.String());
            AddColumn("dbo.Order", "GroupPaymentStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Order", "StoreId", c => c.Int(nullable: false));
            AddColumn("dbo.Order", "LastModifiedPayment", c => c.DateTime());
            AddColumn("dbo.Order", "LastModifiedOrderDetail", c => c.DateTime());
            AddColumn("dbo.Order", "OrderCode", c => c.String());
            AddColumn("dbo.Order", "CheckInDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Order", "CheckOutDate", c => c.DateTime());
            AddColumn("dbo.Order", "ApproveDate", c => c.DateTime());
            AddColumn("dbo.Order", "TotalAmount", c => c.Double(nullable: false));
            AddColumn("dbo.Order", "FinalAmount", c => c.Double(nullable: false));
            AddColumn("dbo.Order", "Notes", c => c.String());
            AddColumn("dbo.Order", "FeeDescription", c => c.String());
            AddColumn("dbo.Order", "CheckInPerson", c => c.String());
            AddColumn("dbo.Order", "CheckOutPerson", c => c.String());
            AddColumn("dbo.Order", "ApprovePerson", c => c.String());
            AlterColumn("dbo.Order", "OrderStatus", c => c.Int(nullable: false));
            DropColumn("dbo.Order", "OrderNumber");
            DropColumn("dbo.Order", "ItemName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Order", "ItemName", c => c.String(maxLength: 256));
            AddColumn("dbo.Order", "OrderNumber", c => c.String(maxLength: 256));
            AlterColumn("dbo.Order", "OrderStatus", c => c.String());
            DropColumn("dbo.Order", "ApprovePerson");
            DropColumn("dbo.Order", "CheckOutPerson");
            DropColumn("dbo.Order", "CheckInPerson");
            DropColumn("dbo.Order", "FeeDescription");
            DropColumn("dbo.Order", "Notes");
            DropColumn("dbo.Order", "FinalAmount");
            DropColumn("dbo.Order", "TotalAmount");
            DropColumn("dbo.Order", "ApproveDate");
            DropColumn("dbo.Order", "CheckOutDate");
            DropColumn("dbo.Order", "CheckInDate");
            DropColumn("dbo.Order", "OrderCode");
            DropColumn("dbo.Order", "LastModifiedOrderDetail");
            DropColumn("dbo.Order", "LastModifiedPayment");
            DropColumn("dbo.Order", "StoreId");
            DropColumn("dbo.Order", "GroupPaymentStatus");
            DropColumn("dbo.Order", "Att5");
            DropColumn("dbo.Order", "Att4");
            DropColumn("dbo.Order", "Att3");
            DropColumn("dbo.Order", "Att2");
            DropColumn("dbo.Order", "Att1");
            DropColumn("dbo.Order", "NumberOfGuest");
            DropColumn("dbo.Order", "VATAmount");
            DropColumn("dbo.Order", "VAT");
            DropColumn("dbo.Order", "TotalInvoicePrint");
            DropColumn("dbo.Order", "DeliveryCustomer");
            DropColumn("dbo.Order", "DeliveryPhone");
            DropColumn("dbo.Order", "DeliveryStatus");
            DropColumn("dbo.Order", "DeliveryAddress");
            DropColumn("dbo.Order", "ServedPerson");
            DropColumn("dbo.Order", "LastRecordDate");
            DropColumn("dbo.Order", "SourceType");
            DropColumn("dbo.Order", "IsFixedPrice");
            DropColumn("dbo.Order", "TableId");
            DropColumn("dbo.Order", "SourceId");
        }
    }
}
