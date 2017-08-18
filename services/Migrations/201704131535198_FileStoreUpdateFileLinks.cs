namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileStoreUpdateFileLinks : DbMigration
    {
        public override void Up()
        {
            Sql(@"
--Update the link for non-CRPP files, from the web server, to the CDMS-Share.
update dbo.[Files] set Link = replace(Link, 'http://cdms.ctuir.org/services/uploads', '\\gis-data01\CDMS-Share\Prod\P') where ProjectId != 2247
GO
update dbo.[Files] set Link = replace(Link, 'https://cdms.ctuir.org/services/uploads', '\\gis-data01\CDMS-Share\Prod\P') where ProjectId != 2247
GO
update dbo.[Files] set Link = replace(Link, '/', '\') where ProjectId != 2247
GO

--Update the link only for CRPP files, from CRPP-GIS-Share, to CDMS-Share.
update dbo.[Files] set Link = replace(Link, '\\gis-data01\CRPP-GIS-Share\CDMS\Prod', '\\gis-data01\CDMS-Share\Prod\P\2247\S') where ProjectId = 2247
GO
            ");
        }
        
        public override void Down()
        {
            Sql(@"
update dbo.[Files] set Link = replace(Link, '\\gis-data01\CDMS-Share\Prod\P', 'https://cdms.ctuir.org/services/uploads') where ProjectId != 2247
GO
update dbo.[Files] set Link = replace(Link, '\', '/') where ProjectId != 2247
GO

update dbo.[Files] set Link = replace(Link, '\\gis-data01\CDMS-Share\Prod\P\2247\S', '\\gis-data01\CRPP-GIS-Share\CDMS\Prod') where ProjectId = 2247
GO
            ");
        }
    }
}
