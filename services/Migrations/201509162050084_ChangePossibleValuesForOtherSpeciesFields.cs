namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePossibleValuesForOtherSpeciesFields : DbMigration
    {
        public override void Up()
        {
            Sql(@"
    update fields set PossibleValues = '{""COHO"":""Coho"",""DACE"":""Dace"",""SCULPIN"":""Sculpin"",""SUCKER"":""Sucker"",""SHINER"":""Shiner"",""NP Minnow"":""NP Minnow"",""CRAYFISH"":""Crayfish"",""BULLHEAD CATFISH"":""Bullhead Catfish"",""WHITE FISH"":""White Fish"",""HATCHERY CHS"":""Hatchery CHS"",""HATCHERY STS"":""Hatchery STS"",""CHINOOK"":""Chinook Fry"",""STEELHEAD"":""Steelhead Fry"",""LAMPREY-AMMO"":""Lamprey-Ammo"",""LAMPREY-MACRO"":""Lamprey-Macro"",""OTHER"":""Other""}' where name = 'Other Species'
    delete from datasetfields where label = 'PassNumber'    -- No undo!
    delete from fields where name = 'PassNumber'            -- No undo!
");
        }
        
        public override void Down()
        {
            Sql(@"
    update fields set PossibleValues = '[""Dace"", ""Sculpin"", ""Sucker"", ""Shiner"", ""NP Minnow"", ""Crayfish"", ""Bullhead Catfish"", ""White Fish"", ""Hatchery CHS"", ""Hatchery STS"", ""Chinook Fry"", ""Steelhead Fry"", ""Lamprey-Ammo"", ""Lamprey-Macro"", ""Other""]' where name = 'Other Species'
");

        }
    }
}
