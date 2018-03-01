ALTER VIEW dbo.Screwtrap_vw
AS
SELECT        h.FileTitle, h.ClipFiles, h.Tagger, h.LivewellTemp, h.TaggingTemp, 
h.PostTaggingTemp, h.ReleaseTemp, h.ArrivalTime, h.DepartTime, h.ArrivalRPMs, 
                         h.DepartureRPMs, h.Hubometer, 
						 h.HubometerTime, h.TrapStopped,
						 h.TrapStarted, h.FishCollected, 
						 h.FishReleased, h.Flow, h.Turbidity, 
						 h.TrapDebris, h.RiverDebris, 
                         h.ActivityComments, h.Unit, h.DailyFinClips, h.Crew, h.TrapStatus, 
						 h.Weather, d.Sequence, d.PitTagCode, d.ForkLength, d.Weight, d.OtherSpecies, d.FishCount, 
                         d.ConditionalComment, d.TextualComments, d.Note, d.ReleaseLocation, 
						 d.FishComments, d.RowId, d.RowStatusId, d.TagStatus, d.ClipStatus, 
                         d.AdditionalPositionalComments, d.EventType, d.SecondPitTag, 
						 d.RacewayTransectTank, d.LifeStage, d.GeneticId, d.CodedWireTag, d.BroodYear, d.MigrationYear, 
                         d.SizeOfCount, d.ScaleId, 
						 d.Containment, 
						 CASE 
						 WHEN SpeciesRunRearing IN ('00U', '25H', '25W', '3RH', '3RW', '7RW', '90U', 'A0W', 'D0W', 'ERU', 'G0W') THEN '' 
						 WHEN d .ForkLength < 100 THEN 'Small' WHEN d .ForkLength > 120 THEN 'Large' ELSE 'Medium' 
						 END AS SizeClass, 
                         CASE 
						 WHEN SpeciesRunRearing = '00U' THEN 'Unknown' 
						 WHEN SpeciesRunRearing = '11H' THEN 'Hat. Spring Chinook' 
						 WHEN SpeciesRunRearing = '11U' THEN 'Spring Chinook (unknown r/t)'
                          WHEN SpeciesRunRearing = '11W' THEN 'Wild Spring Chinook' 
						  WHEN SpeciesRunRearing = '13H' THEN 'Hat. Fall Chinook' 
						  WHEN SpeciesRunRearing = '13W' THEN 'Wild Fall Chinook' 
						  WHEN SpeciesRunRearing = '25H' THEN 'Hat. Coho' 
						  WHEN SpeciesRunRearing = '25W' THEN 'Wild Coho' 
						  WHEN SpeciesRunRearing = '32H' THEN 'Hat. Summer Steelhead' 
						  WHEN SpeciesRunRearing = '32W' THEN 'Wild Summer Steelhead' 
						  WHEN SpeciesRunRearing = '3RH' THEN 'Hat. Rainbow Trout' 
						  WHEN SpeciesRunRearing = '3RW' THEN 'Wild Rainbow Trout' 
						  WHEN SpeciesRunRearing = '7RW' THEN 'Bull Trout' 
						  WHEN SpeciesRunRearing = '90U' THEN 'Other' 
						  WHEN SpeciesRunRearing = 'A0W' THEN 'Lamprey' 
						  WHEN SpeciesRunRearing = 'D0W' THEN 'Northern Pikeminnow' 
						  WHEN SpeciesRunRearing = 'ERU' THEN 'Brook Trout' 
						  WHEN SpeciesRunRearing = 'G0W' THEN 'Mountain Whitefish' 
						  ELSE 'Other' 
						  END AS SpeciesRunRearingLabel, 
						  CASE WHEN SpeciesRunRearing IN ('11H', '13H', '25H', '32H', '3RH') 
                         THEN 'HAT' WHEN OtherSpecies IN ('HATCHERY CHS', 'HATCHERY STS') THEN 'HAT' WHEN SpeciesRunRearing IN ('11U', '11W', '13W', '25W', '32W', '3RW') 
                         THEN 'NAT' WHEN OtherSpecies IN ('CHINOOK', 'STEELHEAD') THEN 'NAT' ELSE 'UNK' END AS HatNat, a.Id AS ActivityId, a.DatasetId, a.InstrumentId, 
                         a.LaboratoryId, a.ActivityDate, a.CreateDate, w.Id AS WaterbodyId, w.Name AS WaterbodyName, l.Id AS LocationId, aq.QAStatusName, 
                         aq.Comments AS ActivityQAComments, aq.QAStatusId, aq.QAStatusId AS ActivityQAStatusId, l.Label AS LocationLabel, 
						 d.SpeciesRunRearing
FROM            dbo.Screwtrap_Detail_VW AS d INNER JOIN
                         dbo.Screwtrap_Header_VW AS h ON d.ActivityId = h.ActivityId INNER JOIN
                         dbo.Activities AS a ON a.Id = h.ActivityId LEFT OUTER JOIN
                         dbo.Locations AS l ON l.Id = a.LocationId LEFT OUTER JOIN
                         dbo.WaterBodies AS w ON w.Id = l.WaterBodyId INNER JOIN
                         dbo.ActivityQAs_VW AS aq ON aq.ActivityId = a.Id
						 WHERE DatasetId = 1214;