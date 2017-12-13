-- Add rules for catching duplicates
-- WaterTemp
update dbo.[Fields]
set [Rule] = '{"OnChange":"activities.errors = undefined; removeRowErrorsBeforeRecheck(); checkForDuplicates();}'
where FieldCategoryId = 2 and DbColumnName = 'ReadingDateTime'

update dbo.[DatasetFields]
set [Rule] = '{"OnChange":"activities.errors = undefined; removeRowErrorsBeforeRecheck(); checkForDuplicates();}'
where DatasetId = 1190 and DbColumnName = 'ReadingDateTime'