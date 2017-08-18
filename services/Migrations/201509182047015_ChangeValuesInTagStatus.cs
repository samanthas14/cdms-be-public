namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeValuesInTagStatus : DbMigration
    {
        public override void Up()
        {
            // This will update electrofishing and screw trap
            Sql(@"
    update fields set PossibleValues = '{""NEW"":""New"", ""RECAP"":""Recap"", ""NONE"":""None""}' 
        where name = 'Tag Status'

    update fields set PossibleValues = '{""DEPLETION"":""Depletion"", ""MARK"":""Mark"", ""RECAP"":""Recapture"", ""SALVAGE"":""Salvage"", ""SINGLE-PASS"":""Single-pass"", ""OTHER"":""Other""}' 
        where name = 'Event Type'

");
        }
        
        public override void Down()
        {
            Sql(@"
    update fields set PossibleValues = '[""New"", ""Recap"", ""None""]' 
        where name = 'Tag Status'

    update fields set PossibleValues = '[""Depletion"",""Mark"",""Recapture"",""Salvage"",""Single-pass"",""Other""]' 
        where name = 'Event Type'
");

        }
    }
}