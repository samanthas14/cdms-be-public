namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpawningGroundSurveyFixes : DbMigration
    {
        public override void Up()
        {
            Sql(@"
-- Add extra species
update fields set PossibleValues = '[""BUT"", ""CHF"", ""COHO"", ""STS"", ""CHS"", ""PL""]'
    where Description = 'Species the survey is targeting' or Description = 'Species of the carcass found'

-- Change label
update fields        set name  = 'Target Species' where Description = 'Species the survey is targeting'
update datasetfields set label = 'Target Species' where fieldid in (select id from fields where Description = 'Species the survey is targeting')

-- Add location
update fields set PossibleValues = '[""On Redd (OR)"", ""Near Redd (NR)"", ""Off Redd (OR)""]' where name = 'Fish Location'

-- Make marks multiselect
update fields        set ControlType = 'multiselect' where name  = 'Marks'
update datasetfields set ControlType = 'multiselect' where label = 'Marks'

-- Fix name
update fields set Name         = 'Percent Retained' where Name = 'Spawning Status'
update datasetFields set Label = 'Percent Retained' where label = 'Spawning Status'



-- Set datatype to time
update fields        set controltype = 'time'  where name = 'Start Time' or name = 'End Time' or name = 'Time'
update datasetfields set controltype = 'time'  where fieldid in (select id from fields where name in( 'Start Time','End Time','Time'))

-- Fix coding error
update fields set datatype = 'string' where name = 'Waypoint Date/Time'
update fields        set dbcolumnname = 'DateTime'  where name = 'Waypoint Date/Time'
update datasetfields set  dbcolumnname = 'DateTime' where label = 'Waypoint Date/Time'

");
        }
        
        public override void Down()
        {
            //Sql(@"--");
        }
    }
}
