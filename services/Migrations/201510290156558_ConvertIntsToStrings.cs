namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConvertIntsToStrings : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SnorkelFish_Header", "Visibility", c => c.String());
            Sql(@"
    update fields set datatype = 'string' where fieldcategoryid in (select id from FieldCategories where name = 'Snorkel Fish') and name = 'visibility' 
");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SnorkelFish_Header", "Visibility", c => c.Int());

            Sql(@"
    update fields set datatype = 'number' where fieldcategoryid in (select id from FieldCategories where name = 'Snorkel Fish') and name = 'visibility' 
");

        }
    }
}
