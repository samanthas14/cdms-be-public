using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using services.Resources.Attributes;

namespace services.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }

        public int LocationTypeId { get; set; }
        public int? WaterBodyId { get; set; }
        public int? SdeFeatureClassId { get; set; }
        public int? SdeObjectId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int? UserId { get; set; } //ok if this isn't set


        public int? Elevation { get; set; }

        public int Status { get; set; }

        [DecimalPrecision(18,8)]
        public decimal? GPSEasting { get; set; }

        [DecimalPrecision(18, 8)]
        public decimal? GPSNorthing { get; set; }

        public string Projection { get; set; }
        public string UTMZone { get; set; }

        [DecimalPrecision(18, 13)]
        public decimal? Latitude { get; set; }

        [DecimalPrecision(18, 13)]
        public decimal? Longitude { get; set; }

        public string OtherAgencyId { get; set; }
        public string ImageLink { get; set; }

        //these are very specific to water locations... is there a better way to abstract this?
        public float? WettedWidth { get; set; }
        public float? WettedDepth { get; set; }

        [DecimalPrecision(5, 2)]
        public decimal? RiverMile { get; set; }
        public string StudyDesign { get; set; }

        public int? ProjectId { get; set; } // Blank, but when we de-normalize, we will need it.
        public int? SubprojectId { get; set; }



        [JsonIgnore]
        public virtual List<Project> Projects { get; set; }

        public virtual LocationType LocationType { get; set; }
        public virtual SdeFeatureClass SdeFeatureClass { get; set; }
        
        public virtual WaterBody WaterBody { get; set; }

        public Location()
        {
            //set some defaults for our constructor
            LocationTypeId = LocationType.DEFAULT_LOCATIONTYPEID;
            CreateDateTime = DateTime.Now;
            SdeFeatureClassId = SdeFeatureClass.DEFAULT_FEATURECLASSID;
        }
    }

    public class WaterBody
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StreamName { get; set; }
        public string TribToName { get; set; }
        public string Description { get; set; }
//        public int? SdeObjectId { get; set; }
        public string GNIS_ID { get; set; }
    }
}
