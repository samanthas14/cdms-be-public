namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class release_fix_2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Release_Data_Header", "SubmissionDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Release_Data_Header", "LastReleaseDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Release_Data_Header", "LastReleaseDate", c => c.String());
            AlterColumn("dbo.Release_Data_Header", "SubmissionDate", c => c.String());
        }
    }
}
