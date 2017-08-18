namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocTblStudyDesign : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "StudyDesign", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Locations", "StudyDesign");
        }
    }
}
