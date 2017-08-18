namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreFieldsForStreamNetRperS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StreamNet_RperS_Detail", "Age10Adults", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "Age11PlusAdults", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "Age1Juvs", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "Age2Adults", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "Age2Juvs", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "Age3Adults", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "Age3Juvs", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "Age4Adults", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "Age4PlusJuvs", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "Age5Adults", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "Age6Adults", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "Age7Adults", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "Age8Adults", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "Age9Adults", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "CBFWApopName", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "CommonPopName", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "CompilerRecordID", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "DataEntry", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "DataEntryNotes", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "ESU_DPS", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "HarvestAdj", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "HatcherySpawners", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "HatcherySpawnersAlpha", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "HatcherySpawnersLowerLimit", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "HatcherySpawnersUpperLimit", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "IndicatorLocation", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "LastUpdated", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "MainstemHarvest", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "MajorPopGroup", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "MeasureLocation", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "MetaComments", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "MethodAdjustments", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "MetricLocation", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "NOBroodStockRemoved", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "OceanHarvest", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "PopFitNotes", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "PopID", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "ProtMethDocumentation", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "ProtMethName", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "ProtMethURL", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "Publish", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "RecoveryDomain", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "Recruits", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "RecruitsAlpha", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "RecruitsLowerLimit", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "RecruitsMissing", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "RecruitsMissingExplanation", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "RecruitsUpperLimit", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "RefID", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "RperSAlpha", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "RperSLowerLimit", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "RperSUpperLimit", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "SubmitAgency", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "TotalSpawners", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "TotalSpawnersAlpha", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "TotalSpawnersLowerLimit", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "TotalSpawnersUpperLimit", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "TribHarvest", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "UpdDate", c => c.String());
            AddColumn("dbo.StreamNet_RperS_Detail", "YOY", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StreamNet_RperS_Detail", "YOY");
            DropColumn("dbo.StreamNet_RperS_Detail", "UpdDate");
            DropColumn("dbo.StreamNet_RperS_Detail", "TribHarvest");
            DropColumn("dbo.StreamNet_RperS_Detail", "TotalSpawnersUpperLimit");
            DropColumn("dbo.StreamNet_RperS_Detail", "TotalSpawnersLowerLimit");
            DropColumn("dbo.StreamNet_RperS_Detail", "TotalSpawnersAlpha");
            DropColumn("dbo.StreamNet_RperS_Detail", "TotalSpawners");
            DropColumn("dbo.StreamNet_RperS_Detail", "SubmitAgency");
            DropColumn("dbo.StreamNet_RperS_Detail", "RperSUpperLimit");
            DropColumn("dbo.StreamNet_RperS_Detail", "RperSLowerLimit");
            DropColumn("dbo.StreamNet_RperS_Detail", "RperSAlpha");
            DropColumn("dbo.StreamNet_RperS_Detail", "RefID");
            DropColumn("dbo.StreamNet_RperS_Detail", "RecruitsUpperLimit");
            DropColumn("dbo.StreamNet_RperS_Detail", "RecruitsMissingExplanation");
            DropColumn("dbo.StreamNet_RperS_Detail", "RecruitsMissing");
            DropColumn("dbo.StreamNet_RperS_Detail", "RecruitsLowerLimit");
            DropColumn("dbo.StreamNet_RperS_Detail", "RecruitsAlpha");
            DropColumn("dbo.StreamNet_RperS_Detail", "Recruits");
            DropColumn("dbo.StreamNet_RperS_Detail", "RecoveryDomain");
            DropColumn("dbo.StreamNet_RperS_Detail", "Publish");
            DropColumn("dbo.StreamNet_RperS_Detail", "ProtMethURL");
            DropColumn("dbo.StreamNet_RperS_Detail", "ProtMethName");
            DropColumn("dbo.StreamNet_RperS_Detail", "ProtMethDocumentation");
            DropColumn("dbo.StreamNet_RperS_Detail", "PopID");
            DropColumn("dbo.StreamNet_RperS_Detail", "PopFitNotes");
            DropColumn("dbo.StreamNet_RperS_Detail", "OceanHarvest");
            DropColumn("dbo.StreamNet_RperS_Detail", "NOBroodStockRemoved");
            DropColumn("dbo.StreamNet_RperS_Detail", "MetricLocation");
            DropColumn("dbo.StreamNet_RperS_Detail", "MethodAdjustments");
            DropColumn("dbo.StreamNet_RperS_Detail", "MetaComments");
            DropColumn("dbo.StreamNet_RperS_Detail", "MeasureLocation");
            DropColumn("dbo.StreamNet_RperS_Detail", "MajorPopGroup");
            DropColumn("dbo.StreamNet_RperS_Detail", "MainstemHarvest");
            DropColumn("dbo.StreamNet_RperS_Detail", "LastUpdated");
            DropColumn("dbo.StreamNet_RperS_Detail", "IndicatorLocation");
            DropColumn("dbo.StreamNet_RperS_Detail", "HatcherySpawnersUpperLimit");
            DropColumn("dbo.StreamNet_RperS_Detail", "HatcherySpawnersLowerLimit");
            DropColumn("dbo.StreamNet_RperS_Detail", "HatcherySpawnersAlpha");
            DropColumn("dbo.StreamNet_RperS_Detail", "HatcherySpawners");
            DropColumn("dbo.StreamNet_RperS_Detail", "HarvestAdj");
            DropColumn("dbo.StreamNet_RperS_Detail", "ESU_DPS");
            DropColumn("dbo.StreamNet_RperS_Detail", "DataEntryNotes");
            DropColumn("dbo.StreamNet_RperS_Detail", "DataEntry");
            DropColumn("dbo.StreamNet_RperS_Detail", "CompilerRecordID");
            DropColumn("dbo.StreamNet_RperS_Detail", "CommonPopName");
            DropColumn("dbo.StreamNet_RperS_Detail", "CBFWApopName");
            DropColumn("dbo.StreamNet_RperS_Detail", "Age9Adults");
            DropColumn("dbo.StreamNet_RperS_Detail", "Age8Adults");
            DropColumn("dbo.StreamNet_RperS_Detail", "Age7Adults");
            DropColumn("dbo.StreamNet_RperS_Detail", "Age6Adults");
            DropColumn("dbo.StreamNet_RperS_Detail", "Age5Adults");
            DropColumn("dbo.StreamNet_RperS_Detail", "Age4PlusJuvs");
            DropColumn("dbo.StreamNet_RperS_Detail", "Age4Adults");
            DropColumn("dbo.StreamNet_RperS_Detail", "Age3Juvs");
            DropColumn("dbo.StreamNet_RperS_Detail", "Age3Adults");
            DropColumn("dbo.StreamNet_RperS_Detail", "Age2Juvs");
            DropColumn("dbo.StreamNet_RperS_Detail", "Age2Adults");
            DropColumn("dbo.StreamNet_RperS_Detail", "Age1Juvs");
            DropColumn("dbo.StreamNet_RperS_Detail", "Age11PlusAdults");
            DropColumn("dbo.StreamNet_RperS_Detail", "Age10Adults");
        }
    }
}
