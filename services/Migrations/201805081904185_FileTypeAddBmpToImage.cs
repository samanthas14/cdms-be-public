namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileTypeAddBmpToImage : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update dbo.FileTypes
set Extensions = 'jpg,gif,png,bmp'
where [Name] = 'Image'
            ");
        }
        
        public override void Down()
        {
            Sql(@"
update dbo.FileTypes
set Extensions = 'jpg,gif,png'
where [Name] = 'Image'
            ");
        }
    }
}
