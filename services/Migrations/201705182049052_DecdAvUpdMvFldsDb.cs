namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DecdAvUpdMvFldsDb : DbMigration
    {
        public override void Up()
        {
            Sql(@"
-- Update table Fields
update dbo.[Fields] set ControlType = 'text' where FieldCategoryId = 5 and DbColumnName = 'HighestAndBestUse'
update dbo.[Fields] set ControlType = 'file' where FieldCategoryId = 5 and DbColumnName = 'RegionalOfficeReviewFiles'
update dbo.[Fields] set Name = 'Effective Date' where FieldCategoryId = 5 and DbColumnName = 'AppraisalValuationDate'
update dbo.[Fields] set PossibleValues = '[""Approved"",""Acceptable"",""Marginally Acceptable"",""Not Acceptable""]' where FieldCategoryId = 5 and DbColumnName = 'NwroComments'

-- Update table DatasetFields
update dbo.[DatasetFields] set ControlType = 'file', FieldRoleId = 2, OrderIndex = 150 where DatasetId = 1193 and DbColumnName = 'RegionalOfficeReviewFiles'
update dbo.[DatasetFields] set OrderIndex = 130, SourceId = 1 where DatasetId = 1193 and DbColumnName = 'TypeOfTransaction' -- Filling in null values
update dbo.[DatasetFields] set OrderIndex = 140, SourceId = 1 where DatasetId = 1193 and DbColumnName = 'PartiesInvolved' -- Filling in null values
update dbo.[DatasetFields] set FieldRoleId = 2, OrderIndex = 55, ControlType = 'text' where DatasetId = 1193 and DbColumnName = 'HighestAndBestUse'
update dbo.[DatasetFields] set Label = 'Effective Date', OrderIndex = 135 where DatasetId = 1193 and DbColumnName = 'AppraisalValuationDate'
update dbo.[DatasetFields] set FieldRoleId = 2, OrderIndex = 5 where DatasetId = 1193 and DbColumnName = 'LastAppraisalRequestDate'
update dbo.[DatasetFields] set FieldRoleId = 1, OrderIndex = 200 where DatasetId = 1193 and DbColumnName = 'OtherPermitLeases'
            ");
        }

        public override void Down()
        {
            Sql(@"
-- Update table Fields
update dbo.[Fields] set ControlType = 'select' where FieldCategoryId = 5 and DbColumnName = 'HighestAndBestUse'
update dbo.[Fields] set ControlType = 'file' where FieldCategoryId = 5 and DbColumnName = 'RegionalOfficeReviewFiles'
update dbo.[Fields] set Name = 'Valuation Date' where FieldCategoryId = 5 and DbColumnName = 'AppraisalValuationDate'
update dbo.[Fields] set PossibleValues = '[""Approved"",""Acceptable"",""Not Acceptable""]' where FieldCategoryId = 5 and DbColumnName = 'NwroComments'

-- Update table DatasetFields
update dbo.[DatasetFields] set ControlType = 'file', FieldRoleId = 1, OrderIndex = 150 where DatasetId = 1193 and DbColumnName = 'RegionalOfficeReviewFiles'
--update dbo.[DatasetFields] set OrderIndex = 130, SourceId = 1 where DatasetId = 1193 and DbColumnName = 'TypeOfTransaction' -- Leave this as it is
--update dbo.[DatasetFields] set OrderIndex = 140, SourceId = 1 where DatasetId = 1193 and DbColumnName = 'PartiesInvolved' -- Leave this as it is
update dbo.[DatasetFields] set FieldRoleId = 2, OrderIndex = 55, ControlType = 'text' where DatasetId = 1193 and DbColumnName = 'HighestAndBestUse'
update dbo.[DatasetFields] set Label = 'Valuation Date', OrderIndex = 135 where DatasetId = 1193 and DbColumnName = 'AppraisalValuationDate'
update dbo.[DatasetFields] set FieldRoleId = 1, OrderIndex = 5 where DatasetId = 1193 and DbColumnName = 'LastAppraisalRequestDate'
update dbo.[DatasetFields] set FieldRoleId = 2, OrderIndex = 200 where DatasetId = 1193 and DbColumnName = 'OtherPermitLeases'
            ");
        }
    }
}
