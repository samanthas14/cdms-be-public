namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class redd_dataset_fix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SGS_Carcass_Detail", "CarcassWPT", c => c.String());
            AddColumn("dbo.SGS_Carcass_Header", "Datum", c => c.String());
            AddColumn("dbo.SGS_Redd_Detail", "WPTType", c => c.String());
            AddColumn("dbo.SGS_Redd_Detail", "WPTComments", c => c.String());
            AddColumn("dbo.SGS_Redd_Header", "Datum", c => c.String());
            AddColumn("dbo.SGS_Redd_Header", "PreviousRedds", c => c.Int(nullable: false));
            AddColumn("dbo.SGS_Redd_Header", "NewRedds", c => c.Int(nullable: false));
            DropColumn("dbo.SGS_Carcass_Detail", "WPTName");
            DropColumn("dbo.SGS_Carcass_Detail", "Datum");
            DropColumn("dbo.SGS_Redd_Detail", "ReddType");
            DropColumn("dbo.SGS_Redd_Detail", "ReddComments");
            DropColumn("dbo.SGS_Redd_Detail", "Datum");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SGS_Redd_Detail", "Datum", c => c.String());
            AddColumn("dbo.SGS_Redd_Detail", "ReddComments", c => c.String());
            AddColumn("dbo.SGS_Redd_Detail", "ReddType", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "Datum", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "WPTName", c => c.String());
            DropColumn("dbo.SGS_Redd_Header", "NewRedds");
            DropColumn("dbo.SGS_Redd_Header", "PreviousRedds");
            DropColumn("dbo.SGS_Redd_Header", "Datum");
            DropColumn("dbo.SGS_Redd_Detail", "WPTComments");
            DropColumn("dbo.SGS_Redd_Detail", "WPTType");
            DropColumn("dbo.SGS_Carcass_Header", "Datum");
            DropColumn("dbo.SGS_Carcass_Detail", "CarcassWPT");
        }
    }
}
