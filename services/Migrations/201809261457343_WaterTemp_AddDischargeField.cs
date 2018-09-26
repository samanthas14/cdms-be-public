namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WaterTemp_AddDischargeField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WaterTemp_Detail", "Discharge", c => c.Double());

            Sql(@"
                INSERT INTO dbo.Fields(FieldCategoryId, Name, [Description], Units, Validation, DataType, PossibleValues, DbColumnName, ControlType, [Rule]) 
                VALUES
                (2,'Discharge','Volumetric flow rate of water that is transported through a given location. It includes any suspended solids, dissolved chemicals, or biologic material in addition to the water itself',
                    'cfs','[0,5000]','float',null,'Discharge','number',null)
           ");
        }
        
        public override void Down()
        {
            DropColumn("dbo.WaterTemp_Detail", "Discharge");
        }
    }
}
