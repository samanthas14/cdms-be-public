using System.Data.SqlClient;

namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCharacteristicToWaterQualityDataset : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update fieldCategories set name = 'Water Quality with Labs' where id in (select max(id) from FieldCategories where name = 'Water Quality')      -- Make name unique
update datasets set name = 'Water Quality with Labs' where name = 'Water Quality'      -- Make name consistent with field category above

CREATE TABLE #NewFieldInfo (id int, fieldName nvarchar(max), DbColumnName nvarchar(max), Validation nvarchar(max), ControlType nvarchar(max), [Rule] nvarchar(max), FieldRoleId int, OrderIndex int IDENTITY(1,1))    
select id into #NewDatasetIds from datasets where name = 'Water Quality with Labs'


INSERT INTO dbo.Fields (FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule])
OUTPUT INSERTED.id, INSERTED.Name, INSERTED.DbColumnName, INSERTED.Validation, INSERTED.ControlType , INSERTED.[Rule], NULL INTO #NewFieldInfo

SELECT   
        FieldCategoryId = (select id from FieldCategories where name = 'Water Quality with Labs'), 
        Name = 'Characteristic Name',
        Description = 'Name of characteristic measured',
        Units = NULL,
        Validation = NULL,
        DataType = 'string',
        PossibleValues = '{""1,2,3,4,6,7,8-HPCDD"":""1,2,3,4,6,7,8-HpCDD"", ""1,2,3,4,6,7,8-HPCDF"":""1,2,3,4,6,7,8-HpCDF"", ""1,2,3,4,7,8,9-HPCDF"":""1,2,3,4,7,8,9-HpCDF"", ""1,2,3,4,7,8-HXCDD"":""1,2,3,4,7,8-HxCDD"", ""1,2,3,4,7,8-HXCDF"":""1,2,3,4,7,8-HxCDF"", ""1,2,3,6,7,8-HXCDD"":""1,2,3,6,7,8-HxCDD"", ""1,2,3,6,7,8-HXCDF"":""1,2,3,6,7,8-HxCDF"", ""1,2,3,7,8,9-HXCDD"":""1,2,3,7,8,9-HxCDD"", ""1,2,3,7,8,9-HXCDF"":""1,2,3,7,8,9-HxCDF"", ""1,2,3,7,8-PECDD"":""1,2,3,7,8-PeCDD"", ""1,2,3,7,8-PECDF"":""1,2,3,7,8-PeCDF"", ""1-METHYLPHENANTHRENE"":""1-Methylphenanthrene"", ""1-METHYNAPHTHALENE"":""1-Methynaphthalene"", ""2,2'',4,4'',5,5''-HEXACHLOROBIPHENYL (BZ 153)"":""2,2'',4,4'',5,5''-Hexachlorobiphenyl (BZ 153)"", ""2,2`,3,3`,4,4`,5,5`,6-NONACHLOROBIPHENYL (BZ 206)"":""2,2`,3,3`,4,4`,5,5`,6-Nonachlorobiphenyl (BZ 206)"", ""2,2`,3,3`,4,4`,5,5`,6-NONACHLOROBIPHENYL(BZ 206)"":""2,2`,3,3`,4,4`,5,5`,6-Nonachlorobiphenyl(BZ 206)"", ""2,2`,3,3`,4,4`,5,6-OCTACHLOROBIPHENYL (BZ 195)"":""2,2`,3,3`,4,4`,5,6-Octachlorobiphenyl (BZ 195)"", ""2,2`,3,3`,4,4`,5-HEPTACHLOROBIPHENYL (BZ 170)"":""2,2`,3,3`,4,4`,5-Heptachlorobiphenyl (BZ 170)"", ""2,2`,3,3`,4,4`-HEXACHLOROBIPHENYL (BZ 128)"":""2,2`,3,3`,4,4`-Hexachlorobiphenyl (BZ 128)"", ""2,2`,3,4,4`,5,5`-HEPTACHLOROBIPHENYL (BZ 180)"":""2,2`,3,4,4`,5,5`-Heptachlorobiphenyl (BZ 180)"", ""2,2`,3,4,4`,5`-HEXACHLOROBIPHENYL (BZ 138)"":""2,2`,3,4,4`,5`-Hexachlorobiphenyl (BZ 138)"", ""2,2`,3,4`,5,5`,6-HEPTACHLOROBIPHENYL (BZ 187)"":""2,2`,3,4`,5,5`,6-Heptachlorobiphenyl (BZ 187)"", ""2,2`,3,5`-TETRACHLOROBIPHENYL (BZ 44)"":""2,2`,3,5`-Tetrachlorobiphenyl (BZ 44)"", ""2,2`,4,5,5`-PENTACHLOROBIPHENYL (BZ 101)"":""2,2`,4,5,5`-Pentachlorobiphenyl (BZ 101)"", ""2,2`,5,5`-TETRACHLOROBIPHENYL (BZ 52)"":""2,2`,5,5`-Tetrachlorobiphenyl (BZ 52)"", ""2,2`,5-TRICHLOROBIPHENYL (BZ 18)"":""2,2`,5-Trichlorobiphenyl (BZ 18)"", ""2,3,3`,4,4`-PENTACHLOROBIPHENYL (BZ 105)"":""2,3,3`,4,4`-Pentachlorobiphenyl (BZ 105)"", ""2,3,3`,4`,6-PENTACHLOROBIPHENYL (BZ 110)"":""2,3,3`,4`,6-Pentachlorobiphenyl (BZ 110)"", ""2,3,4,6,7,8-HXCDF"":""2,3,4,6,7,8-HxCDF"", ""2,3,4,7,8-PECDF"":""2,3,4,7,8-PeCDF"", ""2,3,5-TRIMETHYLNAPHTHALENE"":""2,3,5-Trimethylnaphthalene"", ""2,3,7,8-TCDD"":""2,3,7,8-TCDD"", ""2,3,7,8-TCDF"":""2,3,7,8-TCDF"", ""2,3`,4,4`,5-PENTACHLOROBIPHENYL (BZ 118)"":""2,3`,4,4`,5-Pentachlorobiphenyl (BZ 118)"", ""2,3`,4,4`-TETRACHLOROBIPHENYL (BZ 66)"":""2,3`,4,4`-Tetrachlorobiphenyl (BZ 66)"", ""2,4,4`-TRICHLOROBIPHENYL (BZ 28)"":""2,4,4`-Trichlorobiphenyl (BZ 28)"", ""2,4,5-T"":""2,4,5-T"", ""2,4,5-TP (SILVEX)"":""2,4,5-TP (Silvex)"", ""2,4-D"":""2,4-D"", ""2,4`-DDD"":""2,4`-DDD"", ""2,4`-DDE"":""2,4`-DDE"", ""2,4`-DDT"":""2,4`-DDT"", ""2,4`-DICHLOROBIPHENYL (BZ 8)"":""2,4`-Dichlorobiphenyl (BZ 8)"", ""2,6-DIMETHYLNAPHTHALENE"":""2,6-Dimethylnaphthalene"", ""2-METHYLNAPTHALENE"":""2-Methylnapthalene"", ""2M4-DB"":""2m4-DB"", ""3,3`,4,4`,5-PENTACHLOROBIPHENYL (BZ 126)"":""3,3`,4,4`,5-Pentachlorobiphenyl (BZ 126)"", ""3,3`,4,4`-TETRACHLOROBIPHENYL (BZ 77)"":""3,3`,4,4`-Tetrachlorobiphenyl (BZ 77)"", ""4,4''-DDE"":""4,4''-DDE"", ""4,4-DDD"":""4,4-DDD"", ""4,4-DDE"":""4,4-DDE"", ""4,4-DDT"":""4,4-DDT"", ""4,4`-DDD"":""4,4`-DDD"", ""4,4`-DDE"":""4,4`-DDE"", ""4,4`-DDT"":""4,4`-DDT"", ""4-NITROPHENOL"":""4-Nitrophenol"", ""ACENAPHTHENE"":""Acenaphthene"", ""ACENAPHTHYLENE"":""Acenaphthylene"", ""ALDRIN"":""Aldrin"", ""ALPHA CHLORDANE"":""alpha Chlordane"", ""ALPHA-BHC"":""alpha-BHC"", ""ALUMINUM, TOTAL"":""Aluminum, Total"", ""AMMONIA, TOTAL AS N"":""Ammonia, Total as N"", ""ANTHRACENE"":""Anthracene"", ""ANTIMONY, TOTAL"":""Antimony, Total"", ""AROCLOR 1016"":""Aroclor 1016"", ""AROCLOR 1221"":""Aroclor 1221"", ""AROCLOR 1232"":""Aroclor 1232"", ""AROCLOR 1242"":""Aroclor 1242"", ""AROCLOR 1248"":""Aroclor 1248"", ""AROCLOR 1254"":""Aroclor 1254"", ""AROCLOR 1260"":""Aroclor 1260"", ""ARSENIC"":""Arsenic"", ""ARSENIC, TOTAL"":""Arsenic, Total"", ""AZINPHOS-METHYL"":""Azinphos-methyl"", ""BACTERIA - E.COLI"":""Bacteria - E.coli"", ""BACTERIA - FECAL"":""Bacteria - Fecal"", ""BACTERIA - TOTAL COLIFORM"":""Bacteria - Total Coliform"", ""BARIUM"":""Barium"", ""BARIUM, TOTAL"":""Barium, Total"", ""BENZO[A]ANTHRACENE"":""Benzo[a]anthracene"", ""BENZO[A]PYRENE"":""Benzo[a]pyrene"", ""BENZO[B]FLUORANTHENE"":""Benzo[b]fluoranthene"", ""BENZO[G,H,I]PERYLENE"":""Benzo[g,h,i]perylene"", ""BENZO[K]FLUORANTHENE"":""Benzo[k]fluoranthene"", ""BERYLLIUM, TOTAL"":""Beryllium, Total"", ""BETA-BHC"":""beta-BHC"", ""BICARBONATE"":""Bicarbonate"", ""BIOCHEMICAL OXYGEN DEMAND-5 DAY"":""Biochemical Oxygen Demand-5 day"", ""BIPHENYL"":""Biphenyl"", ""BOLSTAR"":""Bolstar"", ""BROMOXYNIL OCTANOATE"":""Bromoxynil octanoate"", ""CADMIUM"":""Cadmium"", ""CADMIUM, TOTAL"":""Cadmium, Total"", ""CALCIUM"":""Calcium"", ""CALCIUM, TOTAL"":""Calcium, Total"", ""CARBONATE"":""Carbonate"", ""CHLORDANE"":""Chlordane"", ""CHLORIDE"":""Chloride"", ""CHLOROPHYLL-A"":""Chlorophyll-a"", ""CHLORPYRIFOS"":""Chlorpyrifos"", ""CHROMIUM"":""Chromium"", ""CHROMIUM, TOTAL"":""Chromium, Total"", ""CHRYSENE"":""Chrysene"", ""COBALT, TOTAL"":""Cobalt, Total"", ""COLOR"":""Color"", ""CONDUCTIVITY, LAB"":""Conductivity, Lab"", ""COPPER"":""Copper"", ""COPPER, TOTAL"":""Copper, Total"", ""COUMAPHOS"":""Coumaphos"", ""DALAPON"":""Dalapon"", ""DECACHLOROBIPHENYL (BZ 209)"":""Decachlorobiphenyl (BZ 209)"", ""DECACHLOROBIPHENYL (SURR.)"":""Decachlorobiphenyl (Surr.)"", ""DELTA-BHC"":""delta-BHC"", ""DEMETON 0-S"":""Demeton 0-S"", ""DIAZINON"":""Diazinon"", ""DIBENZ[A,H]ANTHRACENE"":""Dibenz[a,h]anthracene"", ""DIBENZOTHIOPHENE"":""Dibenzothiophene"", ""DICAMBA"":""Dicamba"", ""DICHLORPROP"":""Dichlorprop"", ""DICHLORVOS"":""Dichlorvos"", ""DIELDRIN"":""Dieldrin"", ""DIMETHOATE"":""Dimethoate"", ""DINOSEB"":""Dinoseb"", ""DISULFOTON"":""Disulfoton"", ""ELEMENTARY MERCURY"":""Elementary Mercury"", ""ENDOSULFAN I"":""Endosulfan I"", ""ENDOSULFAN II"":""Endosulfan II"", ""ENDOSULFAN SULFATE"":""Endosulfan sulfate"", ""ENDRIN"":""Endrin"", ""ENDRIN ALDEHYDE"":""Endrin Aldehyde"", ""ENDRIN KETONE"":""Endrin Ketone"", ""EPN"":""EPN"", ""ETHOPROP"":""Ethoprop"", ""FENSULFOTHION"":""Fensulfothion"", ""FENTHION"":""Fenthion"", ""FLUORANTHENE"":""Fluoranthene"", ""FLUORENE"":""Fluorene"", ""FLUORIDE"":""Fluoride"", ""FLUORIDE, DISSOLVED"":""Fluoride, Dissolved"", ""GAMMA-BHC (LINDANE)"":""gamma-BHC (Lindane)"", ""GLYPHOSATE"":""Glyphosate"", ""HARDNESS"":""Hardness"", ""HEPTACHLOR"":""Heptachlor"", ""HEPTACHLOR EPOXIDE"":""Heptachlor epoxide"", ""HEXACHLOROBENZENE"":""Hexachlorobenzene"", ""INDENO[1,2,3-CD]PYRENE"":""Indeno[1,2,3-cd]pyrene"", ""IRON"":""Iron"", ""IRON, TOTAL"":""Iron, Total"", ""LEAD"":""Lead"", ""LEAD, TOTAL"":""Lead, Total"", ""LITHIUM, TOTAL"":""Lithium, Total"", ""MAGNESIUM"":""Magnesium"", ""MAGNESIUM, TOTAL"":""Magnesium, Total"", ""MALATHION"":""Malathion"", ""MANGANESE"":""Manganese"", ""MANGANESE, TOTAL"":""Manganese, Total"", ""MECURY"":""Mecury"", ""MERCURY"":""Mercury"", ""MERCURY, TOTAL"":""Mercury, Total"", ""MERPHOS"":""Merphos"", ""METHOXYCHLOR"":""Methoxychlor"", ""METHYL MERCURY"":""Methyl Mercury"", ""MEVINPHOS"":""Mevinphos"", ""MIREX"":""Mirex"", ""MOLYBDENUM, TOTAL"":""Molybdenum, Total"", ""NALED"":""Naled"", ""NAPHTHALENE"":""Naphthalene"", ""NICKEL, TOTAL"":""Nickel, Total"", ""NITRATE"":""Nitrate"", ""NITRATE + NITRITE AS N"":""Nitrate + Nitrite as N"", ""NITRATE AS N"":""Nitrate as N"", ""NITRITE"":""Nitrite"", ""NITRITE AS N"":""Nitrite as N"", ""OCDD"":""OCDD"", ""OCDF"":""OCDF"", ""ORTHO PHOSPHORUS AS P"":""Ortho Phosphorus as P"", ""PARATHION ETHYL"":""Parathion ethyl"", ""PARATHION METHYL"":""Parathion methyl"", ""PCB105"":""PCB105"", ""PCB114"":""PCB114"", ""PCB157"":""PCB157"", ""PCB167"":""PCB167"", ""PCB28"":""PCB28"", ""PCB77"":""PCB77"", ""PENTACHLORONITROBENZENE (SURR.)"":""Pentachloronitrobenzene (Surr.)"", ""PENTACHLOROPHENOL"":""Pentachlorophenol"", ""PH"":""pH"", ""PH, LABORATORY"":""pH, Laboratory"", ""PHENANTHRENE"":""Phenanthrene"", ""PHORATE"":""Phorate"", ""POTASSIUM"":""Potassium"", ""POTASSIUM, TOTAL"":""Potassium, Total"", ""PYRENE"":""Pyrene"", ""RONNEL"":""Ronnel"", ""SAND"":""Sand"", ""SELENIUM"":""Selenium"", ""SELENIUM, TOTAL"":""Selenium, Total"", ""SILICA - COLORIMETRIC"":""Silica - Colorimetric"", ""SILVER, TOTAL"":""Silver, Total"", ""SODIUM"":""Sodium"", ""SODIUM, TOTAL"":""Sodium, Total"", ""STIROFOS"":""Stirofos"", ""STRONTIUM, TOTAL"":""Strontium, Total"", ""SULFATE"":""Sulfate"", ""SULFOTEPP"":""Sulfotepp"", ""THALLIUM, TOTAL"":""Thallium, Total"", ""TIN, TOTAL"":""Tin, Total"", ""TITANIUM, TOTAL"":""Titanium, Total"", ""TOKUTHION"":""Tokuthion"", ""TOTAL ALKALINITY AS CACO3"":""Total Alkalinity as CaCO3"", ""TOTAL DISSOLVED SOLIDS - 180"":""Total Dissolved Solids - 180"", ""TOTAL DISSOLVED SOLIDS - SUM"":""Total Dissolved Solids - Sum"", ""TOTAL HPCDD"":""Total HpCDD"", ""TOTAL HPCDF"":""Total HpCDF"", ""TOTAL HXCDD"":""Total HxCDD"", ""TOTAL HXCDF"":""Total HxCDF"", ""TOTAL KJELDAHL NITROGEN"":""Total Kjeldahl Nitrogen"", ""TOTAL KJELDAHL NITROGEN AS N"":""Total Kjeldahl Nitrogen as N"", ""TOTAL ORGANIC CARBON"":""Total Organic Carbon"", ""TOTAL PECDD"":""Total PeCDD"", ""TOTAL PECDF"":""Total PeCDF"", ""TOTAL PHOSPHORUS"":""Total Phosphorus"", ""TOTAL PHOSPHORUS AS P"":""Total Phosphorus as P"", ""TOTAL SOLIDS"":""Total Solids"", ""TOTAL SUSPENDED SOLIDS"":""Total Suspended Solids"", ""TOTAL TCDD"":""Total TCDD"", ""TOTAL TCDF"":""Total TCDF"", ""TOXAPHENE"":""Toxaphene"", ""TRANS-NONACHLOR"":""trans-Nonachlor"", ""TRICHLORONATE"":""Trichloronate"", ""TURBIDITY"":""Turbidity"", ""VANADIUM, TOTAL"":""Vanadium, Total"", ""ZINC"":""Zinc"", ""ZINC, TOTAL"":""Zinc, Total"", ""ZIRCONIUM, TOTAL"":""Zirconium, Total""}',
        DbColumnName = 'CharacteristicName',
        ControlType = 'select',
        [Rule] = NULL

update #NewFieldInfo set FieldRoleId = 2    -- 2 == data


INSERT INTO dbo.DatasetFields(DatasetId, FieldId, FieldRoleId, CreateDateTime, Label, DbColumnName, Validation, SourceId, InstrumentId, OrderIndex, ControlType, [Rule])
SELECT
    DatasetId      = d.id,
    FieldId        = f.id,
    FieldRoleId    = f.FieldRoleId,
    CreateDateTime = GetDate(),
    Label          = f.fieldName,
    DbColumnName   = f.DbColumnName,
    Validation     = f.Validation,
    SourceId       = 1,
    InstrumentId   = NULL,
    OrderIndex     = 5, 
    ControlType    = f.ControlType,
    [Rule]         = f.[Rule]
FROM #NewDatasetIds as d, #NewFieldInfo as f



drop table #NewFieldInfo
drop table #NewDatasetIds


");
        }

        public override void Down()
        {
            Sql(@"
    update fieldCategories set name = 'Water Quality' where  name = 'Water Quality with Labs'   
    update datasets set name = 'Water Quality' where  name = 'Water Quality with Labs'   

    delete from datasetfields where label in ('Characteristic Name')
    delete from fields where name in('Characteristic Name')
");
        }
    }
}