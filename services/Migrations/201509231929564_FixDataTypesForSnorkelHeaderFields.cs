namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDataTypesForSnorkelHeaderFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SnorkelFish_Header", "DominantSpecies", c => c.String());
            AlterColumn("dbo.SnorkelFish_Header", "CommonSpecies", c => c.String());
            AlterColumn("dbo.SnorkelFish_Header", "RareSpecies", c => c.String());
            //DropColumn("dbo.ScrewTrap_Header", "DominantSpecies");
            //DropColumn("dbo.ScrewTrap_Header", "CommonSpecies");
            //DropColumn("dbo.ScrewTrap_Header", "RareSpecies");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ScrewTrap_Header", "RareSpecies", c => c.String());
            AddColumn("dbo.ScrewTrap_Header", "CommonSpecies", c => c.String());
            AddColumn("dbo.ScrewTrap_Header", "DominantSpecies", c => c.String());
            //AlterColumn("dbo.SnorkelFish_Header", "RareSpecies", c => c.Double());
            //AlterColumn("dbo.SnorkelFish_Header", "CommonSpecies", c => c.Double());
            //AlterColumn("dbo.SnorkelFish_Header", "DominantSpecies", c => c.Double());
        }
    }
}
