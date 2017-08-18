namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RifflesAndPoolsOhMy : DbMigration
    {
        public override void Up()
        {
            Sql(@"
    update fields set PossibleValues = '[""1 - Riffles and Pools"",""2 - Riffles"",""3 - Neither Riffles nor Pools""]' where name  in ('Water Visibility') 
");
        }
        
        public override void Down()
        {
            Sql(@"
    update fields set PossibleValues = '[""Riffles and Pools"",""Riffles"",""Neither Riffles or Pools""]' where name  in ('Water Visibility') 
");

        }
    }
}
