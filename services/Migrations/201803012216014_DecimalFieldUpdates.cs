namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DecimalFieldUpdates : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JvRearing_Detail", "Result", c => c.Decimal(precision: 9, scale: 4));
            AlterColumn("dbo.Appraisal_Detail", "AppraisalValue", c => c.Decimal(precision: 9, scale: 2));

            Sql(@"
            update DatasetFields set ControlType = 'text' where id = 1177;
            update DatasetFields set ControlType = 'text' where id = 1371;
            ");

        }
        
        public override void Down()
        {
            AlterColumn("dbo.Appraisal_Detail", "AppraisalValue", c => c.Int());
            AlterColumn("dbo.JvRearing_Detail", "Result", c => c.Single());
        }
    }
}
