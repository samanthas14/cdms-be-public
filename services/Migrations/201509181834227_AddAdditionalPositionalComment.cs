namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdditionalPositionalComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Electrofishing_Detail", "AdditionalPositionalComment", c => c.String());
            DropColumn("dbo.Electrofishing_Detail", "FishComments");

            Sql(@"
    update datasetfields set label = 'Additional Positional Comment', dbcolumnname = 'AdditionalPositionalComment' 
        where fieldid in 
	        (select id from fields where name = 'Fish Comments' and fieldcategoryid in (select id from fieldcategories where name = 'Electrofishing'))

    update fields set name = 'Additional Positional Comment', DbColumnName = 'AdditionalPositionalComment', Description = 'Comment about fish position' 
        where name = 'Fish Comments' and fieldcategoryid in (select id from fieldcategories where name = 'Electrofishing')
");
        }
        
        public override void Down()
        {

            Sql(@"
    update datasetfields set label = 'Fish Comments', dbcolumnname = 'FishComments' 
        where fieldid in 
	        (select id from fields where name = 'Additional Positional Comment' and fieldcategoryid in (select id from fieldcategories where name = 'Electrofishing'))

    update fields set name = 'Fish Comments', DbColumnName = 'FishComments', Description = 'Comments about the individual fish' 
        where name = 'Additional Positional Comment' and fieldcategoryid in (select id from fieldcategories where name = 'Electrofishing')
");

            AddColumn("dbo.Electrofishing_Detail", "FishComments", c => c.String());
            DropColumn("dbo.Electrofishing_Detail", "AdditionalPositionalComment");
        }
    }
}
