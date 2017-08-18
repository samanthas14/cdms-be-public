namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSnrkelFishReportingView : DbMigration
    {
        public override void Up()
        {
            Sql(@"
drop view SnorkelFish_vw
go

create view SnorkelFish_vw as
select
    h.Team, h.NoteTaker, h.StartTime, h.EndTime, h.WaterTemperature, h.Visibility, h.WeatherConditions, h.VisitId, h.Comments, h.CollectionType, 
    h.DominantSpecies, h.CommonSpecies, h.RareSpecies, h.Unit, iif(h.IsAEM='YES',AEMHabitatType,ChannelUnitType) as UnitType,

    d.NoSnorklers, d.FishID, d.ChannelUnitNumber, d.Lane, d.Type, d.ChannelAverageDepth, d.ChannelLength, 
    d.ChannelWidth, d.ChannelMaxDepth, d.ChannelLength *  d.ChannelWidth as ChannelArea, d.FishCount, d.Species, d.SizeClass, 
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

");
        }

        public override void Down()
        {
            Sql(@"
drop view SnorkelFish_vw
go

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

");



        }
    }
}