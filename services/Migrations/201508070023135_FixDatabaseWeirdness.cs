namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.InstrumentProjects", newName: "ProjectInstruments");
            //DropPrimaryKey("dbo.ProjectInstruments");
            //AddPrimaryKey("dbo.ProjectInstruments", new[] { "Project_Id", "Instrument_Id" });
        }
        
        public override void Down()
        {
            //DropPrimaryKey("dbo.ProjectInstruments");
            //AddPrimaryKey("dbo.ProjectInstruments", new[] { "Instrument_Id", "Project_Id" });
            //RenameTable(name: "dbo.ProjectInstruments", newName: "InstrumentProjects");
        }
    }
}
