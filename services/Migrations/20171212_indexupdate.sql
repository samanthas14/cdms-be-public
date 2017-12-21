CREATE NONCLUSTERED INDEX WATERTEMP_READINGDATETIME_IDX ON [dbo].[WaterTemp_Detail](	[ReadingDateTime] ASC );

CREATE NONCLUSTERED INDEX WATERTEMP_VIEW_IDX ON[dbo].[WaterTemp_Detail] 
([RowId],[RowStatusId],[ActivityId],[EffDt])
INCLUDE([Id],[ReadingDateTime],[WaterTemperature],[WaterTemperatureF],[WaterLevel],[TempAToD],[BatteryVolts],[ByUserId],[QAStatusId],[AirTemperature],[AirTemperatureF],[GMTReadingDateTime],[Conductivity],[PSI],[AbsolutePressure]);

CREATE NONCLUSTERED INDEX DATASET_FIELDS_IDX ON [dbo].[DatasetFields]
(
	[DatasetId] ASC
)
INCLUDE ( 	[Id],
	[FieldId],
	[FieldRoleId],
	[CreateDateTime],
	[Label],
	[DbColumnName],
	[Validation],
	[SourceId],
	[InstrumentId],
	[OrderIndex],
	[ControlType],
	[Rule])
