namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedActivityQA : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActivityQAs", "QAStatusName", c => c.String());
            AddColumn("dbo.ActivityQAs", "QAStatusDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ActivityQAs", "QAStatusDescription");
            DropColumn("dbo.ActivityQAs", "QAStatusName");
        }
    }
}
