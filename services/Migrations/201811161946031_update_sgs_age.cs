namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_sgs_age : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SGS_Carcass_Detail", "TargetFish");
            DropColumn("dbo.SGS_Carcass_Detail", "Recapture");
            DropColumn("dbo.SGS_Carcass_Detail", "VerifiedOrigin");
            DropColumn("dbo.SGS_Carcass_Detail", "UDF1");
            DropColumn("dbo.SGS_Carcass_Detail", "UDF2");
            DropColumn("dbo.SGS_Carcass_Detail", "UDF3");
            DropColumn("dbo.SGS_Carcass_Detail", "UDF4");
            DropColumn("dbo.SGS_Carcass_Detail", "UDF5");
            DropColumn("dbo.SGS_Redd_Detail", "UDF1");
            DropColumn("dbo.SGS_Redd_Detail", "UDF2");
            DropColumn("dbo.SGS_Redd_Detail", "UDF3");
            DropColumn("dbo.SGS_Redd_Detail", "UDF4");
            DropColumn("dbo.SGS_Redd_Detail", "UDF5");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SGS_Redd_Detail", "UDF5", c => c.String());
            AddColumn("dbo.SGS_Redd_Detail", "UDF4", c => c.String());
            AddColumn("dbo.SGS_Redd_Detail", "UDF3", c => c.String());
            AddColumn("dbo.SGS_Redd_Detail", "UDF2", c => c.String());
            AddColumn("dbo.SGS_Redd_Detail", "UDF1", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "UDF5", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "UDF4", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "UDF3", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "UDF2", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "UDF1", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "VerifiedOrigin", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "Recapture", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "TargetFish", c => c.String());
        }
    }
}
