namespace services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BenUpdData : DbMigration
    {
        public override void Up()
        {
            Sql(@"
update dbo.[DatasetFields]
set DbColumnName = 'MsPollutionTolerant', Label = 'MS Pollution Tolerant', [Validation] = null where DatasetId = 1252 and DbColumnName = 'MsPollutionTolerantPercent'
update dbo.[DatasetFields]
set DbColumnName = 'MsPredator', Label = 'MS Predator', [Validation] = null where DatasetId = 1252 and DbColumnName = 'MsPredatorPercent'
update dbo.[DatasetFields]
set DbColumnName = 'MsDominantTaxa3', Label = 'MS Dominant Taxa3', [Validation] = null where DatasetId = 1252 and DbColumnName = 'MsDominantTaxa3Percent'

update dbo.[Fields]
set DbColumnName = 'MsPollutionTolerant', Name = 'MS Pollution Tolerant', [Validation] = null, Units = null where FieldCategoryId = 22 and DbColumnName = 'MsPollutionTolerantPercent'
update dbo.[Fields]
set DbColumnName = 'MsPredator', Name = 'MS Predator', [Validation] = null, Units = null where FieldCategoryId = 22 and DbColumnName = 'MsPredatorPercent'
update dbo.[Fields]
set DbColumnName = 'MsDominantTaxa3', Name = 'MS Dominant Taxa3', [Validation] = null, Units = null where FieldCategoryId = 22 and DbColumnName = 'MsDominantTaxa3Percent'
            ");
        }
        
        public override void Down()
        {
            Sql(@"
update dbo.[DatasetFields]
set DbColumnName = 'MsPollutionTolerantPercent', Label = 'MS Pollution Tolerant Percent', [Validation] = null where DatasetId = 1252 and DbColumnName = 'MsPollutionTolerant'
update dbo.[DatasetFields]
set DbColumnName = 'MsPredatorPercent', Label = 'MS Predator Percent', [Validation] = null where DatasetId = 1252 and DbColumnName = 'MsPredator'
update dbo.[DatasetFields]
set DbColumnName = 'MsDominantTaxa3Percent', Label = 'MS Dominant Taxa3 Percent', [Validation] = null where DatasetId = 1252 and DbColumnName = 'MsDominantTaxa3'

update dbo.[Fields]
set DbColumnName = 'MsPollutionTolerantPercent', Name = 'MS Pollution Tolerant Percent', [Validation] = 'p2d', '%' = null where FieldCategoryId = 22 and DbColumnName = 'MsPollutionTolerant'
update dbo.[Fields]
set DbColumnName = 'MsPredatorPercent', Name = 'MS Predator Percent', [Validation] = 'p2d', Units = '%' where FieldCategoryId = 22 and DbColumnName = 'MsPredator'
update dbo.[Fields]
set DbColumnName = 'MsDominantTaxa3Percent', Name = 'MS Dominant Taxa3 Percent', [Validation] = 'p2d', Units = '%' where FieldCategoryId = 22 and DbColumnName = 'MsDominantTaxa3'
            ");
        }
    }
}
