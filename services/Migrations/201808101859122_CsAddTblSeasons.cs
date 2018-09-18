namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CsAddTblSeasons : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Seasons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Species = c.String(),
                        Season = c.Int(),
                        OpenDate = c.DateTime(),
                        CloseDate = c.DateTime(),
                        TotalDays = c.Int(),
                        DatasetId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Seasons");
        }
    }
}
