namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dna_spelling : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SGS_Carcass_Detail", "DNACollected", c => c.String());
            DropColumn("dbo.SGS_Carcass_Detail", "DNAColleceted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SGS_Carcass_Detail", "DNAColleceted", c => c.String());
            DropColumn("dbo.SGS_Carcass_Detail", "DNACollected");
        }
    }
}
