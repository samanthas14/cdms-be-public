namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateNullabilityOfDateFieldsInElectrofishing : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Electrofishing_Header", "Pass1TimeBegin", c => c.DateTime());
            AlterColumn("dbo.Electrofishing_Header", "Pass1TimeEnd", c => c.DateTime());
            AlterColumn("dbo.Electrofishing_Header", "Pass2TimeBegin", c => c.DateTime());
            AlterColumn("dbo.Electrofishing_Header", "Pass2TimeEnd", c => c.DateTime());
            AlterColumn("dbo.Electrofishing_Header", "Pass3TimeBegin", c => c.DateTime());
            AlterColumn("dbo.Electrofishing_Header", "Pass3TimeEnd", c => c.DateTime());
            AlterColumn("dbo.Electrofishing_Header", "Pass4TimeBegin", c => c.DateTime());
            AlterColumn("dbo.Electrofishing_Header", "Pass4TimeEnd", c => c.DateTime());
            AlterColumn("dbo.Electrofishing_Header", "Pass5TimeBegin", c => c.DateTime());
            AlterColumn("dbo.Electrofishing_Header", "Pass5TimeEnd", c => c.DateTime());
            AlterColumn("dbo.Electrofishing_Header", "Pass6TimeBegin", c => c.DateTime());
            AlterColumn("dbo.Electrofishing_Header", "Pass6TimeEnd", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Electrofishing_Header", "Pass6TimeEnd", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Electrofishing_Header", "Pass6TimeBegin", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Electrofishing_Header", "Pass5TimeEnd", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Electrofishing_Header", "Pass5TimeBegin", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Electrofishing_Header", "Pass4TimeEnd", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Electrofishing_Header", "Pass4TimeBegin", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Electrofishing_Header", "Pass3TimeEnd", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Electrofishing_Header", "Pass3TimeBegin", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Electrofishing_Header", "Pass2TimeEnd", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Electrofishing_Header", "Pass2TimeBegin", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Electrofishing_Header", "Pass1TimeEnd", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Electrofishing_Header", "Pass1TimeBegin", c => c.DateTime(nullable: false));
        }
    }
}
