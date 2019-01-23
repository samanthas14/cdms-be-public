namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixed_juvsurvival : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NPT_JuvSurvival_Detail", "Survival", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.NPT_JuvSurvival_Detail", "Lower95", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.NPT_JuvSurvival_Detail", "Upper95", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NPT_JuvSurvival_Detail", "Upper95", c => c.Int());
            AlterColumn("dbo.NPT_JuvSurvival_Detail", "Lower95", c => c.Int());
            AlterColumn("dbo.NPT_JuvSurvival_Detail", "Survival", c => c.Int());
        }
    }
}
