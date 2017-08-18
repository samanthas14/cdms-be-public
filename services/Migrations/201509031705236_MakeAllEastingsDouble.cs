namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeAllEastingsDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "EastingUTM", c => c.Double());
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "NorthingUTM", c => c.Double());
            AlterColumn("dbo.SpawningGroundSurvey_Header", "StartEasting", c => c.Double());
            AlterColumn("dbo.SpawningGroundSurvey_Header", "StartNorthing", c => c.Double());
            AlterColumn("dbo.SpawningGroundSurvey_Header", "EndEasting", c => c.Double());
            AlterColumn("dbo.SpawningGroundSurvey_Header", "EndNorthing", c => c.Double());

            Sql(@"
update fields set datatype = 'double' where dbColumnName in ('StartNorthing','EndNorthing','NorthingUTM','StartEasting','EndEasting','EastingUTM')

");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SpawningGroundSurvey_Header", "EndNorthing", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Header", "EndEasting", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Header", "StartNorthing", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Header", "StartEasting", c => c.Int());
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "NorthingUTM", c => c.Single());
            AlterColumn("dbo.SpawningGroundSurvey_Detail", "EastingUTM", c => c.Single());

            Sql(@"
update fields set datatype = 'int' where dbColumnName in ('StartNorthing','EndNorthing','StartEasting','EndEasting')
update fields set datatype = 'float' where dbColumnName in ('NorthingUTM','EastingUTM')

");

        }
    }
}
