namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFieldsFOrElectrofishing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Electrofishing_Header", "Pass1TimeBegin", c => c.DateTime(nullable: false));
            AddColumn("dbo.Electrofishing_Header", "Pass1TimeEnd", c => c.DateTime(nullable: false));
            AddColumn("dbo.Electrofishing_Header", "Pass1TotalSecondsEF", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass1WaterTempBegin", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass1WaterTempStop", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass1Hertz", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass1Freq", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass1Volts", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass2TimeBegin", c => c.DateTime(nullable: false));
            AddColumn("dbo.Electrofishing_Header", "Pass2TimeEnd", c => c.DateTime(nullable: false));
            AddColumn("dbo.Electrofishing_Header", "Pass2TotalSecondsEF", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass2WaterTempBegin", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass2WaterTempStop", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass2Hertz", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass2Freq", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass2Volts", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass3TimeBegin", c => c.DateTime(nullable: false));
            AddColumn("dbo.Electrofishing_Header", "Pass3TimeEnd", c => c.DateTime(nullable: false));
            AddColumn("dbo.Electrofishing_Header", "Pass3TotalSecondsEF", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass3WaterTempBegin", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass3WaterTempStop", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass3Hertz", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass3Freq", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass3Volts", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass4TimeBegin", c => c.DateTime(nullable: false));
            AddColumn("dbo.Electrofishing_Header", "Pass4TimeEnd", c => c.DateTime(nullable: false));
            AddColumn("dbo.Electrofishing_Header", "Pass4TotalSecondsEF", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass4WaterTempBegin", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass4WaterTempStop", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass4Hertz", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass4Freq", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass4Volts", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass5TimeBegin", c => c.DateTime(nullable: false));
            AddColumn("dbo.Electrofishing_Header", "Pass5TimeEnd", c => c.DateTime(nullable: false));
            AddColumn("dbo.Electrofishing_Header", "Pass5TotalSecondsEF", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass5WaterTempBegin", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass5WaterTempStop", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass5Hertz", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass5Freq", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass5Volts", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass6TimeBegin", c => c.DateTime(nullable: false));
            AddColumn("dbo.Electrofishing_Header", "Pass6TimeEnd", c => c.DateTime(nullable: false));
            AddColumn("dbo.Electrofishing_Header", "Pass6TotalSecondsEF", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass6WaterTempBegin", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass6WaterTempStop", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass6Hertz", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass6Freq", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Pass6Volts", c => c.Double());
            DropColumn("dbo.Electrofishing_Header", "PassNumber");
            DropColumn("dbo.Electrofishing_Header", "TimeBegin");
            DropColumn("dbo.Electrofishing_Header", "TimeEnd");
            DropColumn("dbo.Electrofishing_Header", "TotalSecondsEF");
            DropColumn("dbo.Electrofishing_Header", "WaterTempBegin");
            DropColumn("dbo.Electrofishing_Header", "WaterTempStop");
            DropColumn("dbo.Electrofishing_Header", "Hertz");
            DropColumn("dbo.Electrofishing_Header", "Freq");
            DropColumn("dbo.Electrofishing_Header", "Volts");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Electrofishing_Header", "Volts", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Freq", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "Hertz", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "WaterTempStop", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "WaterTempBegin", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "TotalSecondsEF", c => c.Double());
            AddColumn("dbo.Electrofishing_Header", "TimeEnd", c => c.String());
            AddColumn("dbo.Electrofishing_Header", "TimeBegin", c => c.String());
            AddColumn("dbo.Electrofishing_Header", "PassNumber", c => c.String());
            DropColumn("dbo.Electrofishing_Header", "Pass6Volts");
            DropColumn("dbo.Electrofishing_Header", "Pass6Freq");
            DropColumn("dbo.Electrofishing_Header", "Pass6Hertz");
            DropColumn("dbo.Electrofishing_Header", "Pass6WaterTempStop");
            DropColumn("dbo.Electrofishing_Header", "Pass6WaterTempBegin");
            DropColumn("dbo.Electrofishing_Header", "Pass6TotalSecondsEF");
            DropColumn("dbo.Electrofishing_Header", "Pass6TimeEnd");
            DropColumn("dbo.Electrofishing_Header", "Pass6TimeBegin");
            DropColumn("dbo.Electrofishing_Header", "Pass5Volts");
            DropColumn("dbo.Electrofishing_Header", "Pass5Freq");
            DropColumn("dbo.Electrofishing_Header", "Pass5Hertz");
            DropColumn("dbo.Electrofishing_Header", "Pass5WaterTempStop");
            DropColumn("dbo.Electrofishing_Header", "Pass5WaterTempBegin");
            DropColumn("dbo.Electrofishing_Header", "Pass5TotalSecondsEF");
            DropColumn("dbo.Electrofishing_Header", "Pass5TimeEnd");
            DropColumn("dbo.Electrofishing_Header", "Pass5TimeBegin");
            DropColumn("dbo.Electrofishing_Header", "Pass4Volts");
            DropColumn("dbo.Electrofishing_Header", "Pass4Freq");
            DropColumn("dbo.Electrofishing_Header", "Pass4Hertz");
            DropColumn("dbo.Electrofishing_Header", "Pass4WaterTempStop");
            DropColumn("dbo.Electrofishing_Header", "Pass4WaterTempBegin");
            DropColumn("dbo.Electrofishing_Header", "Pass4TotalSecondsEF");
            DropColumn("dbo.Electrofishing_Header", "Pass4TimeEnd");
            DropColumn("dbo.Electrofishing_Header", "Pass4TimeBegin");
            DropColumn("dbo.Electrofishing_Header", "Pass3Volts");
            DropColumn("dbo.Electrofishing_Header", "Pass3Freq");
            DropColumn("dbo.Electrofishing_Header", "Pass3Hertz");
            DropColumn("dbo.Electrofishing_Header", "Pass3WaterTempStop");
            DropColumn("dbo.Electrofishing_Header", "Pass3WaterTempBegin");
            DropColumn("dbo.Electrofishing_Header", "Pass3TotalSecondsEF");
            DropColumn("dbo.Electrofishing_Header", "Pass3TimeEnd");
            DropColumn("dbo.Electrofishing_Header", "Pass3TimeBegin");
            DropColumn("dbo.Electrofishing_Header", "Pass2Volts");
            DropColumn("dbo.Electrofishing_Header", "Pass2Freq");
            DropColumn("dbo.Electrofishing_Header", "Pass2Hertz");
            DropColumn("dbo.Electrofishing_Header", "Pass2WaterTempStop");
            DropColumn("dbo.Electrofishing_Header", "Pass2WaterTempBegin");
            DropColumn("dbo.Electrofishing_Header", "Pass2TotalSecondsEF");
            DropColumn("dbo.Electrofishing_Header", "Pass2TimeEnd");
            DropColumn("dbo.Electrofishing_Header", "Pass2TimeBegin");
            DropColumn("dbo.Electrofishing_Header", "Pass1Volts");
            DropColumn("dbo.Electrofishing_Header", "Pass1Freq");
            DropColumn("dbo.Electrofishing_Header", "Pass1Hertz");
            DropColumn("dbo.Electrofishing_Header", "Pass1WaterTempStop");
            DropColumn("dbo.Electrofishing_Header", "Pass1WaterTempBegin");
            DropColumn("dbo.Electrofishing_Header", "Pass1TotalSecondsEF");
            DropColumn("dbo.Electrofishing_Header", "Pass1TimeEnd");
            DropColumn("dbo.Electrofishing_Header", "Pass1TimeBegin");
        }
    }
}
