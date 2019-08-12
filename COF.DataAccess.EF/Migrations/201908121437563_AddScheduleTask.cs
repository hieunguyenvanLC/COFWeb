namespace COF.DataAccess.EF.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddScheduleTask : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ScheduleTask",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Seconds = c.Int(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                        LastSuccessOnUtc = c.DateTime(),
                        LastStartedOnUtc = c.DateTime(),
                        IsRunning = c.Boolean(nullable: false),
                        Description = c.String(),
                        HourStartOnEst = c.Int(),
                        HourEndOnEst = c.Int(),
                        CreatedOnUtc = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedOnUtc = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ScheduleTask_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ScheduleTask",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ScheduleTask_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
