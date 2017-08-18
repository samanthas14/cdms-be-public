namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreSpeciesToOtherSpecies : DbMigration
    {
        public override void Up()
        {
            Sql(@"update fields set PossibleValues = '{""COHO"":""Coho"",""DACE"":""Dace"",""SCULPIN"":""Sculpin"",""SUCKER"":""Sucker"",""SHINER"":""Shiner"",""NP Minnow"":""NP Minnow"",""CRAYFISH"":""Crayfish"",""BULLHEAD CATFISH"":""Bullhead Catfish"",""WHITE FISH"":""White Fish"",""HATCHERY CHS"":""Hatchery CHS"",""HATCHERY STS"":""Hatchery STS"",""CHINOOK"":""Chinook Fry"",""STEELHEAD"":""Steelhead Fry"",""LAMPREY-AMMO"":""Lamprey-Ammo"",""LAMPREY-MACRO"":""Lamprey-Macro"",""SALMONIDS"":""Salmonids"", ""LAMPREY"":""Lamprey"", ""BASS"":""Bass"", ""CARP"":""Carp"", ""CHISEL MOUTH"":""Chisel Mouth"",""OTHER"":""Other""}'
                  where name = 'Other Species'");
        }
        
        public override void Down()
        {
            Sql(@"update fields set PossibleValues = '{""COHO"":""Coho"",""DACE"":""Dace"",""SCULPIN"":""Sculpin"",""SUCKER"":""Sucker"",""SHINER"":""Shiner"",""NP Minnow"":""NP Minnow"",""CRAYFISH"":""Crayfish"",""BULLHEAD CATFISH"":""Bullhead Catfish"",""WHITE FISH"":""White Fish"",""HATCHERY CHS"":""Hatchery CHS"",""HATCHERY STS"":""Hatchery STS"",""CHINOOK"":""Chinook Fry"",""STEELHEAD"":""Steelhead Fry"",""LAMPREY-AMMO"":""Lamprey-Ammo"",""LAMPREY-MACRO"":""Lamprey-Macro"",""OTHER"":""Other""}'
                  where name = 'Other Species'");
        }
    }
}
