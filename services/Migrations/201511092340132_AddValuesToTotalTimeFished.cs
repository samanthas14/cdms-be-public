namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddValuesToTotalTimeFished : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update dbo.[Fields]
set [PossibleValues]=
'[""00:00"",""00:15"",""00:30"",""00:45"",""01:00"",
""01:15"",""01:30"",""01:45"",""02:00"",
""02:15"",""02:30"",""02:45"",""03:00"",
""03:15"",""03:30"",""03:45"",""04:00"",
""04:15"",""04:30"",""04:45"",""05:00"",
""05:15"",""05:30"",""05:45"",""06:00"",
""06:15"",""06:30"",""06:45"",""07:00"",
""07:15"",""07:30"",""07:45"",""08:00"",
""08:15"",""08:30"",""08:45"",""09:00"",
""09:15"",""09:30"",""09:45"",""10:00"",
""10:15"",""10:30"",""10:45"",""11:00"",
""11:15"",""11:30"",""11:45"",""12:00"",
""12:15"",""12:30"",""12:45"",""13:00"",
""13:15"",""13:30"",""13:45"",""14:00"",
""14:15"",""14:30"",""14:45"",""15:00"",
""15:15"",""15:30"",""15:45"",""16:00"",
""16:15"",""16:30"",""16:45"",""17:00"",
""17:15"",""17:30"",""17:45"",""18:00""]'
where DbColumnName = 'TotalTimeFished'
go

insert into dbo.[Fishermen](FirstName, Aka, LastName, DateAdded, FullName, StatusId, OkToCallId)
values('Unknown', 'UNK', 'Fisherman', GETDATE(), 'Unknown (UNK) Fisherman', 0, 0)
go

insert into dbo.[FishermanProjects](Fisherman_Id, Project_Id)
values (1, 1217)
go
            ");
        }
        
        public override void Down()
        {
            Sql(@"
update dbo.[Fields]
set [PossibleValues]=NULL
where DbColumnName = 'TotalTimeFished'
go

delete from dbo.[Fishermen]
go

delete from dbo.[FishermanProjects]
go
            ");
        }
    }
}
