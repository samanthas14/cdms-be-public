namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWaterRightsSettle : DbMigration
    {
        public override void Up()
        {
            Sql(@"
insert into dbo.Datasets(ProjectId, DefaultRowQAStatusId, StatusId, CreateDateTime, Name, [Description], DefaultActivityQAStatusId, DatastoreId, Config)
values (
10025, 
1, 
1, 
'2017-01-23 00:00:00.000', 
'Water Rights Protection', 
'Do not use this dataset for anything else.  It is here so that the project views correctly', 
5, 
1, 
'{""RestrictRoles"":[""WRS""],""ActivitiesPage"": {""Route"": ""wrs""}, ""DataEntryPage"": {""HiddenFields"": [""Location"",""ActivityDate"",""QA"",""Instrument""]}}'
);

update dbo.Users set Roles = '[""Admin"",""DECD"",""CRPP"",""WRS""]' where [Id] = 1081
update dbo.Users set Roles = '[""WRS""]' where [Id] = 3137
update dbo.Users set Roles = '[""WRS""]' where [Id] = 1032
update dbo.Users set Roles = '[""WRS""]' where [Id] = 1084
            ");
        }
        
        public override void Down()
        {
            Sql(@"
update dbo.Users set Roles = '[""Admin"",""DECD"",""CRPP""]' where [Id] = 1081
update dbo.Users set Roles = null where [Id] = 3137
update dbo.Users set Roles = null where [Id] = 1032
update dbo.Users set Roles = null where [Id] = 1084

delete from dbo.Datasets where ProjectId = 10025 and Name = 'Water Rights Protection'
            ");
        }
    }
}
