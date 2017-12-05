-- AdultWeir
update dbo.Fields set [Validation] = 't', [Units] = 'HH:MM' where FieldCategoryId = 1 and DbColumnName = 'TimeStart'
update dbo.Fields set [Validation] = 't', [Units] = 'HH:MM' where FieldCategoryId = 1 and DbColumnName = 'TimeEnd'
update dbo.Fields set [Validation] = 't', [Units] = 'HH:MM' where FieldCategoryId = 1 and DbColumnName = 'PassageTime'

update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1004 and DbColumnName = 'TimeStart' -- ISK
update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1004 and DbColumnName = 'TimeEnd' -- ISK

update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1005 and DbColumnName = 'TimeStart' -- PL
update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1005 and DbColumnName = 'PassageTime' -- PL

-- SpawningGroundSurvey
update dbo.Fields set [Validation] = 't', [Units] = 'HH:MM' where FieldCategoryId = 6 and DbColumnName = 'DateTime'

update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1207 and DbColumnName = 'DateTime' -- BIOM
update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1208 and DbColumnName = 'DateTime' -- GRME
update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1209 and DbColumnName = 'DateTime' -- UMME
update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1210 and DbColumnName = 'DateTime' -- WWME
update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1231 and DbColumnName = 'DateTime' -- Um Broodstock

-- SnorkelFish
update dbo.Fields set [Validation] = 't', [Units] = 'HH:MM' where FieldCategoryId = 8 and DbColumnName = 'StartTime'
update dbo.Fields set [Validation] = 't', [Units] = 'HH:MM' where FieldCategoryId = 8 and DbColumnName = 'EndTime'

update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1212 and DbColumnName = 'StartTime' -- GRME
update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1212 and DbColumnName = 'EndTime' -- GRME

update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1213 and DbColumnName = 'StartTime' -- BioM
update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1213 and DbColumnName = 'EndTime' -- BioM

-- ScrewTrap
update dbo.Fields set [Validation] = 't' where FieldCategoryId = 9 and DbColumnName = 'ArrivalTime'
update dbo.Fields set [Validation] = 't' where FieldCategoryId = 9 and DbColumnName = 'DepartTime'
update dbo.Fields set [Validation] = 't' where FieldCategoryId = 9 and DbColumnName = 'HubometerTime'

update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1214 and DbColumnName = 'ArrivalTime' -- GRME

update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1215 and DbColumnName = 'ArrivalTime' -- UMME
update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1215 and DbColumnName = 'DepartTime' -- UMME
update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1215 and DbColumnName = 'HubometerTime' -- UMME

update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1216 and DbColumnName = 'ArrivalTime' -- WWME
update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1216 and DbColumnName = 'DepartTime' -- WWME
update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1216 and DbColumnName = 'HubometerTime' -- WWME

update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1217 and DbColumnName = 'ArrivalTime' -- PL
update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1217 and DbColumnName = 'DepartTime' -- PL
update dbo.DatasetFields set [Validation] = 't' where DatasetId = 1217 and DbColumnName = 'HubometerTime' -- PL
