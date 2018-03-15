namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;

    public partial class MultiselectViewFixes : DbMigration
    {
        public override void Up()
        {
            //fix the BSample view - it needs to strip out the array code characters
            var sqlfile = "20180301_multiselectviewfix.sql";
            var filepath = Path.Combine(Configuration.GetMigrationBasePath(), sqlfile);
            SqlFile(filepath);

            //fix the SRRCode - turns out this wasn't a multiselect fix but rather an alias update
            sqlfile = "20180301_SRRCodeNameChange.sql";
            filepath = Path.Combine(Configuration.GetMigrationBasePath(), sqlfile);
            SqlFile(filepath);
        }
        
        public override void Down()
        {
        }
    }
}
