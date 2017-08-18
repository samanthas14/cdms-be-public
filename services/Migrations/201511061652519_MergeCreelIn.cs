namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MergeCreelIn : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CreelSurvey_Carcass", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.CreelSurvey_Carcass", "ByUserId", "dbo.Users");
            DropForeignKey("dbo.CreelSurvey_Carcass", "QAStatusId", "dbo.QAStatus");
            DropIndex("dbo.CreelSurvey_Carcass", new[] { "ActivityId" });
            DropIndex("dbo.CreelSurvey_Carcass", new[] { "ByUserId" });
            DropIndex("dbo.CreelSurvey_Carcass", new[] { "QAStatusId" });
            CreateTable(
                "dbo.Fishermen",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        Aka = c.String(),
                        LastName = c.String(),
                        PhoneNumber = c.String(),
                        DateAdded = c.DateTime(nullable: false),
                        DateInactive = c.DateTime(),
                        FullName = c.String(),
                        FishermanComments = c.String(),
                        StatusId = c.Int(nullable: false),
                        OkToCallId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FishermanProjects",
                c => new
                    {
                        Fisherman_Id = c.Int(nullable: false),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Fisherman_Id, t.Project_Id })
                .ForeignKey("dbo.Fishermen", t => t.Fisherman_Id, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.Fisherman_Id)
                .Index(t => t.Project_Id);
            
            AddColumn("dbo.CreelSurvey_Detail", "FishermanId", c => c.Int());
            AddColumn("dbo.CreelSurvey_Detail", "TotalTimeFished", c => c.Int());
            AddColumn("dbo.CreelSurvey_Detail", "FishCount", c => c.Int());
            AddColumn("dbo.CreelSurvey_Detail", "Species", c => c.String());
            AddColumn("dbo.CreelSurvey_Detail", "MethodCaught", c => c.String());
            AddColumn("dbo.CreelSurvey_Detail", "Disposition", c => c.String());
            AddColumn("dbo.CreelSurvey_Detail", "Sex", c => c.String());
            AddColumn("dbo.CreelSurvey_Detail", "Origin", c => c.String());
            AddColumn("dbo.CreelSurvey_Detail", "FinClip", c => c.String());
            AddColumn("dbo.CreelSurvey_Detail", "Marks", c => c.String());
            AddColumn("dbo.CreelSurvey_Detail", "ForkLength", c => c.Int());
            AddColumn("dbo.CreelSurvey_Detail", "MeHPLength", c => c.Int());
            AddColumn("dbo.CreelSurvey_Detail", "SnoutId", c => c.String());
            AddColumn("dbo.CreelSurvey_Detail", "ScaleId", c => c.String());
            AddColumn("dbo.CreelSurvey_Detail", "CarcassComments", c => c.String());
            AddColumn("dbo.CreelSurvey_Header", "SurveySpecies", c => c.String());
            AddColumn("dbo.CreelSurvey_Header", "WorkShift", c => c.String());
            AddColumn("dbo.CreelSurvey_Header", "WeatherConditions", c => c.String());
            AddColumn("dbo.CreelSurvey_Header", "TimeStart", c => c.String());
            AddColumn("dbo.CreelSurvey_Header", "TimeEnd", c => c.String());
            AddColumn("dbo.CreelSurvey_Header", "SurveyComments", c => c.String());
            AddColumn("dbo.CreelSurvey_Header", "Direction", c => c.String());
            DropColumn("dbo.CreelSurvey_Detail", "FishermanName");
            DropColumn("dbo.CreelSurvey_Detail", "FishermanPhone");
            DropColumn("dbo.CreelSurvey_Detail", "HoursFished");
            DropColumn("dbo.CreelSurvey_Detail", "MinutesFished");
            DropColumn("dbo.CreelSurvey_Detail", "NumberFishCaught");
            DropColumn("dbo.CreelSurvey_Detail", "NumberFishKept");
            DropColumn("dbo.CreelSurvey_Detail", "TotalFishingTrips");
            DropColumn("dbo.CreelSurvey_Detail", "TotalCreelInterviews");
            DropColumn("dbo.CreelSurvey_Detail", "Projection");
            DropColumn("dbo.CreelSurvey_Detail", "UTMZone");
            DropColumn("dbo.CreelSurvey_Detail", "WaterBody");
            DropColumn("dbo.CreelSurvey_Detail", "SectionNumber");
            DropColumn("dbo.CreelSurvey_Header", "SurveyType");
            DropColumn("dbo.CreelSurvey_Header", "Species");
            DropColumn("dbo.CreelSurvey_Header", "Shift");
            DropColumn("dbo.CreelSurvey_Header", "Season");
            DropColumn("dbo.CreelSurvey_Header", "StartTime");
            DropColumn("dbo.CreelSurvey_Header", "EndTime");
            DropColumn("dbo.CreelSurvey_Header", "Comments");
            DropTable("dbo.CreelSurvey_Carcass");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CreelSurvey_Carcass",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Species = c.String(),
                        MethodCaught = c.String(),
                        Disposition = c.String(),
                        Sex = c.String(),
                        Origin = c.String(),
                        FinClips = c.String(),
                        Marks = c.String(),
                        ForkLength = c.Int(),
                        MeHPLength = c.Int(),
                        SnoutId = c.String(),
                        ScaleId = c.String(),
                        CarcassComments = c.String(),
                        ParentRowId = c.Int(nullable: false),
                        RowId = c.Int(nullable: false),
                        RowStatusId = c.Int(nullable: false),
                        ActivityId = c.Int(nullable: false),
                        ByUserId = c.Int(nullable: false),
                        QAStatusId = c.Int(nullable: false),
                        EffDt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.CreelSurvey_Header", "Comments", c => c.String());
            AddColumn("dbo.CreelSurvey_Header", "EndTime", c => c.String());
            AddColumn("dbo.CreelSurvey_Header", "StartTime", c => c.String());
            AddColumn("dbo.CreelSurvey_Header", "Season", c => c.String());
            AddColumn("dbo.CreelSurvey_Header", "Shift", c => c.String());
            AddColumn("dbo.CreelSurvey_Header", "Species", c => c.String());
            AddColumn("dbo.CreelSurvey_Header", "SurveyType", c => c.String());
            AddColumn("dbo.CreelSurvey_Detail", "SectionNumber", c => c.String());
            AddColumn("dbo.CreelSurvey_Detail", "WaterBody", c => c.String());
            AddColumn("dbo.CreelSurvey_Detail", "UTMZone", c => c.String());
            AddColumn("dbo.CreelSurvey_Detail", "Projection", c => c.String());
            AddColumn("dbo.CreelSurvey_Detail", "TotalCreelInterviews", c => c.Int());
            AddColumn("dbo.CreelSurvey_Detail", "TotalFishingTrips", c => c.Int());
            AddColumn("dbo.CreelSurvey_Detail", "NumberFishKept", c => c.Int());
            AddColumn("dbo.CreelSurvey_Detail", "NumberFishCaught", c => c.Int());
            AddColumn("dbo.CreelSurvey_Detail", "MinutesFished", c => c.Int());
            AddColumn("dbo.CreelSurvey_Detail", "HoursFished", c => c.Int());
            AddColumn("dbo.CreelSurvey_Detail", "FishermanPhone", c => c.String());
            AddColumn("dbo.CreelSurvey_Detail", "FishermanName", c => c.String());
            DropForeignKey("dbo.FishermanProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.FishermanProjects", "Fisherman_Id", "dbo.Fishermen");
            DropIndex("dbo.FishermanProjects", new[] { "Project_Id" });
            DropIndex("dbo.FishermanProjects", new[] { "Fisherman_Id" });
            DropColumn("dbo.CreelSurvey_Header", "Direction");
            DropColumn("dbo.CreelSurvey_Header", "SurveyComments");
            DropColumn("dbo.CreelSurvey_Header", "TimeEnd");
            DropColumn("dbo.CreelSurvey_Header", "TimeStart");
            DropColumn("dbo.CreelSurvey_Header", "WeatherConditions");
            DropColumn("dbo.CreelSurvey_Header", "WorkShift");
            DropColumn("dbo.CreelSurvey_Header", "SurveySpecies");
            DropColumn("dbo.CreelSurvey_Detail", "CarcassComments");
            DropColumn("dbo.CreelSurvey_Detail", "ScaleId");
            DropColumn("dbo.CreelSurvey_Detail", "SnoutId");
            DropColumn("dbo.CreelSurvey_Detail", "MeHPLength");
            DropColumn("dbo.CreelSurvey_Detail", "ForkLength");
            DropColumn("dbo.CreelSurvey_Detail", "Marks");
            DropColumn("dbo.CreelSurvey_Detail", "FinClip");
            DropColumn("dbo.CreelSurvey_Detail", "Origin");
            DropColumn("dbo.CreelSurvey_Detail", "Sex");
            DropColumn("dbo.CreelSurvey_Detail", "Disposition");
            DropColumn("dbo.CreelSurvey_Detail", "MethodCaught");
            DropColumn("dbo.CreelSurvey_Detail", "Species");
            DropColumn("dbo.CreelSurvey_Detail", "FishCount");
            DropColumn("dbo.CreelSurvey_Detail", "TotalTimeFished");
            DropColumn("dbo.CreelSurvey_Detail", "FishermanId");
            DropTable("dbo.FishermanProjects");
            DropTable("dbo.Fishermen");
            CreateIndex("dbo.CreelSurvey_Carcass", "QAStatusId");
            CreateIndex("dbo.CreelSurvey_Carcass", "ByUserId");
            CreateIndex("dbo.CreelSurvey_Carcass", "ActivityId");
            AddForeignKey("dbo.CreelSurvey_Carcass", "QAStatusId", "dbo.QAStatus", "Id");
            AddForeignKey("dbo.CreelSurvey_Carcass", "ByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.CreelSurvey_Carcass", "ActivityId", "dbo.Activities", "Id");
        }
    }
}
