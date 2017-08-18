namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CleanOutOldTables : DbMigration
    {
        public override void Up()
        {
            Sql(@"
if OBJECT_ID('dbo.ActivitiesNewDecd', 'U') is not null drop table dbo.ActivitiesNewDecd
if OBJECT_ID('dbo.ActivitiesTmp', 'U') is not null drop table dbo.ActivitiesTmp
if OBJECT_ID('dbo.Appraisal_DetailBu', 'U') is not null drop table dbo.Appraisal_DetailBu
if OBJECT_ID('dbo.Appraisal_DetailNew', 'U') is not null drop table dbo.Appraisal_DetailNew
if OBJECT_ID('dbo.Appraisal_DetailTmp', 'U') is not null drop table dbo.Appraisal_DetailTmp
if OBJECT_ID('dbo.Appraisal_HeaderNew', 'U') is not null drop table dbo.Appraisal_HeaderNew
if OBJECT_ID('dbo.Appraisal_HeaderTmp', 'U') is not null drop table dbo.Appraisal_HeaderTmp
if OBJECT_ID('dbo.FilesNewDecd', 'U') is not null drop table dbo.FilesNewDecd
if OBJECT_ID('dbo.LocationsNewDecd', 'U') is not null drop table dbo.LocationsNewDecd

if exists(select name from sys.databases where name = 'CDMS_DECDtmp') drop database CDMS_DECDtmp
if exists(select name from sys.databases where name = 'CDMS_PRODtmp') drop database CDMS_PRODtmp

update dbo.[DatasetFields] set ControlType = 'file' where DatasetId = 1193 and DbColumnName = 'MapFiles'
update dbo.[Fields] set ControlType = 'file' where FieldCategoryId = 5 and DbColumnName = 'MapFiles'
            ");
        }
        
        public override void Down()
        {
            Sql(@"
update dbo.[DatasetFields] set ControlType = 'link' where DatasetId = 1193 and DbColumnName = 'MapFiles'
update dbo.[Fields] set ControlType = 'link' where FieldCategoryId = 5 and DbColumnName = 'MapFiles'
            ");
        }
    }
}
