namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edited_carcass_detail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SGS_Carcass_Detail", "CWTCode", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "DNAColleceted", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "TagsPetersonDisk", c => c.String());
            DropColumn("dbo.SGS_Carcass_Detail", "DNACollecetd");
            DropColumn("dbo.SGS_Carcass_Detail", "TagsStreamer");
            DropColumn("dbo.SGS_Carcass_Detail", "TagsPetersonDisc");
            DropColumn("dbo.SGS_Carcass_Detail", "MarksAnalFin");
            DropColumn("dbo.SGS_Carcass_Detail", "MarksCaudalFin");
            DropColumn("dbo.SGS_Carcass_Detail", "MarksPectoralFin");
            DropColumn("dbo.SGS_Carcass_Detail", "MarksMaxillary");
            DropColumn("dbo.SGS_Carcass_Detail", "MarksFreezeBrand");
            DropColumn("dbo.SGS_Carcass_Detail", "MarksGRIT");
            DropColumn("dbo.SGS_Carcass_Detail", "MarksOTC");
            DropColumn("dbo.SGS_Carcass_Detail", "MarksDorsalScar");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SGS_Carcass_Detail", "MarksDorsalScar", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "MarksOTC", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "MarksGRIT", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "MarksFreezeBrand", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "MarksMaxillary", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "MarksPectoralFin", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "MarksCaudalFin", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "MarksAnalFin", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "TagsPetersonDisc", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "TagsStreamer", c => c.String());
            AddColumn("dbo.SGS_Carcass_Detail", "DNACollecetd", c => c.String());
            DropColumn("dbo.SGS_Carcass_Detail", "TagsPetersonDisk");
            DropColumn("dbo.SGS_Carcass_Detail", "DNAColleceted");
            DropColumn("dbo.SGS_Carcass_Detail", "CWTCode");
        }
    }
}
