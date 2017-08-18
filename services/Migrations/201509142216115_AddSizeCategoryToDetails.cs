namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSizeCategoryToDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Electrofishing_Detail", "SizeCategory", c => c.String());
            DropColumn("dbo.Electrofishing_Detail", "FishSize");

            Sql(@"
-- In details change Fish Size (mm) to Size Category
update DatasetFields set label = 'Size Category', dbcolumnName = 'SizeCategory', controltype = 'text' where fieldid in(
                                                                                select id from fields where name = 'Fish Size' and fieldcategoryid = (select id from fieldcategories where name = 'Electrofishing'))
update fields        set name  = 'Size Category', dbcolumnName = 'SizeCategory', controltype = 'text', units = NULL, possibleValues = NULL where name = 'Fish Size' and fieldcategoryid = (select id from fieldcategories where name = 'Electrofishing')

");

        }
        
        public override void Down()
        {
            AddColumn("dbo.Electrofishing_Detail", "FishSize", c => c.String());
            DropColumn("dbo.Electrofishing_Detail", "SizeCategory");

            Sql(@"
update DatasetFields set label = 'Fish Size', dbcolumnName = 'FishSize', controltype = 'text' where fieldid in(
                                                                        select id from fields where name = 'Size Category' and fieldcategoryid = (select id from fieldcategories where name = 'Electrofishing'))
update fields        set name  = 'Fish Size', dbcolumnName = 'FishSize', controltype = 'text', units = 'mm', possibleValues = '[""CH <50"",""CH 50-80"",""BUT/STS <70"",""BUT/STS 70-129"",""BUT/STS 130-2""]' 
                                                                                              where name = 'Size Category' and fieldcategoryid = (select id from fieldcategories where name = 'Electrofishing')

");


        }
    }
}
