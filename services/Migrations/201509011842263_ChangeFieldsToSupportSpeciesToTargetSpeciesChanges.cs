namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeFieldsToSupportSpeciesToTargetSpeciesChanges : DbMigration
    {
        public override void Up()
        {
            Sql(@"
    update fields        set DbColumnName = 'TargetSpecies' where name  = 'Target Species'
    update datasetfields set DbColumnName = 'TargetSpecies' where label = 'Target Species'
");
        }
        
        public override void Down()
        {
            Sql(@"
    update fields        set DbColumnName = 'Species' where name  = 'Target Species'
    update datasetfields set DbColumnName = 'Species' where label = 'Target Species'
");
        }
    }
}
