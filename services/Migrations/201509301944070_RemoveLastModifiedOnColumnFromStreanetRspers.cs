namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveLastModifiedOnColumnFromStreanetRspers : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.StreamNet_RperS_Detail", "LastModifiedOn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StreamNet_RperS_Detail", "LastModifiedOn", c => c.String());
        }
    }
}
