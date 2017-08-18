namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DecdAvUpdCaptureOldData : DbMigration
    {
        public override void Up()
        {
            Sql(@"
-- Create a backup copy of table Appraisl_Header 
SELECT * 
INTO Appraisal_HeaderTmp 
FROM dbo.[Appraisal_Header]

-- Create a backup copy of table Appraisl_Detail 
select *
into dbo.Appraisal_DetailTmp
from dbo.Appraisal_Detail
            ");
        }

        public override void Down()
        {
            Sql(@"
drop table Appraisal_DetailTmp
go
drop table Appraisal_HeaderTmp
go
            ");
        }
    }
}
