namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transmitter_type_dbcolumnname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SGS_Carcass_Detail", "RadioTag", c => c.String());
            DropColumn("dbo.SGS_Carcass_Detail", "TransmitterType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SGS_Carcass_Detail", "TransmitterType", c => c.String());
            DropColumn("dbo.SGS_Carcass_Detail", "RadioTag");
        }
    }
}
