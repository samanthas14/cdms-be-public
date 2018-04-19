namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WtAddRowQa : DbMigration
    {
        public override void Up()
        {
            Sql(@"
insert into dbo.DatasetQAStatus1 (Dataset_Id, QAStatus_Id)
values
--For Meacham Restoration Project/WaterTemp 
(1256, 12), 
(1256, 13), 
(1256, 14), 
(1256, 15), 
(1256, 16), 
(1256, 17),
--For Tucannon Basin CHaMP Data
(1255, 12), 
(1255, 13), 
(1255, 14), 
(1255, 15), 
(1255, 16), 
(1255, 17)
            ");
        }
        
        public override void Down()
        {
            Sql(@"
delete from dbo.DatasetQAStatus1 
where Dataset_Id = 1256
and QAStatus_Id in (12, 13, 14, 15, 16, 17)

delete from dbo.DatasetQAStatus1 
where Dataset_Id = 1255
and QAStatus_Id in (12, 13, 14, 15, 16, 17)
            ");
        }
    }
}
