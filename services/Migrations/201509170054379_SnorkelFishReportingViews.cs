namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SnorkelFishReportingViews : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SnorkelFish_Detail", "ChannelUnitType", c => c.String());
            AddColumn("dbo.SnorkelFish_Detail", "AEMHabitatType", c => c.String());
            AddColumn("dbo.SnorkelFish_Detail", "AEMLength", c => c.Int(nullable: true));
            AddColumn("dbo.SnorkelFish_Header", "IsAEM", c => c.String());

            Sql(@"
create view SnorkelFish_vw as
select
    h.Team, h.NoteTaker, h.StartTime, h.EndTime, h.WaterTemperature, h.Visibility, h.WeatherConditions, h.VisitId, h.Comments, h.CollectionType, 
    h.DominantSpecies, h.CommonSpecies, h.RareSpecies, h.Unit, iif(h.IsAEM='YES',AEMHabitatType,ChannelUnitType) as UnitType,

    d.NoSnorklers, d.FishID, d.ChannelUnit, d.ChannelUnitNumber, d.Lane, d.Type, d.HabitatType, d.ChannelAverageDepth, d.ChannelLength, 
    d.ChannelWidth, d.ChannelMaxDepth, d.ChannelLength *  d.ChannelWidth as ChannelArea, d.FishCount, d.Species, d.SizeClass, d.Length, 
    d.UnidentifiedSalmonID, d.OtherSpeciesPres, d.NaturalWoodUsed, 
    d.PlacedWoodUsed, d.NaturalBoulderUsed, d.PlacedBoulderUsed, d.NaturalOffChannelUsed, d.CreatedOffChannelUsed, d.NewSideChannelUsed, 
    d.NoStructureUsed, d.AmbientTemp, d.MinimumTemp, d.FieldNotes, d.ChannelUnitType, d.AEMHabitatType, d.AEMLength,

    a.id as ActivityId, a.DatasetId, a.InstrumentId, a.LaboratoryId, a.ActivityDate, w.id as WaterbodyId, w.name as WaterbodyName, l.id as LocationId, 

    l.name as LocationName

from SnorkelFish_Detail d 
join SnorkelFish_Header h on d.ActivityId = h.ActivityId
join activities a on a.id = h.ActivityId
join locations l on l.id = a.locationid
join waterbodies w on w.id = l.waterbodyid



go
CREATE VIEW [dbo].[SnorkelFish_Detail_VW]
AS
SELECT        *
FROM            dbo.SnorkelFish_Detail AS d
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.SnorkelFish_Detail AS dd
                               WHERE        (ActivityId = d.ActivityId) AND (RowId = d.RowId)))
go

CREATE VIEW [dbo].[SnorkelFish_Header_VW]
AS
SELECT        *
FROM            dbo.SnorkelFish_Header AS h
WHERE        (EffDt =
                             (SELECT        MAX(EffDt) AS MaxEffDt
                               FROM            dbo.SnorkelFish_Header AS hh
                               WHERE        (ActivityId = h.ActivityId)));
");
        }

        public override void Down()
        {
            Sql(@"
drop view SnorkelFish_vw
go
drop view SnorkelFish_Detail_VW
go
drop view SnorkelFish_Header_VW
go");

            DropColumn("dbo.SnorkelFish_Detail", "AEMLength");
            DropColumn("dbo.SnorkelFish_Detail", "AEMHabitatType");
            DropColumn("dbo.SnorkelFish_Detail", "ChannelUnitType");
            DropColumn("dbo.SnorkelFish_Header", "IsAEM");



        }
    }
}
