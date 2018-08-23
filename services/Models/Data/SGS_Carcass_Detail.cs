using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class SGS_Carcass_Detail : DataDetail
    {
        //Redd data
            // public string ReddSpecies { get; set; }
            // public string ReddType { get; set; }
            // public int? Count { get; set; }
            // public string ReddComments { get; set; }
        
        //Carcass data
        public string SampleNumber { get; set; }
        public string HistoricSampleNumber { get; set; }
        //public string IDFGNumber { get; set; }
        public string CarcassSpecies { get; set; }
        public string Sex { get; set; }
        public int? ForkLength { get; set; }
        public string SpawnedOut { get; set; }
        public int PercentSpawned { get; set; }
        public string OpercleLeft { get; set; }
        public string OpercleRight { get; set; }
        public string PITScanned { get; set; }
        public string PITCode { get; set; }
        public string AdiposeFinClipped { get; set; }
        public string CWTScanned { get; set; }
        public string SnoutCollected { get; set; }
        public string DNACollecetd { get; set; }
        public string Fins { get; set; }
        public string Scales { get; set; }
        public string Otolith { get; set; }
        public string TargetFish { get; set; }
        public string Recapture { get; set; }
        //public string FieldOrigin { get; set; } //calculated value based on presence of an ad clip and/or CWT
        public string VerifiedOrigin { get; set; }
        public int? Count { get; set; }
        public string CarcassComments { get; set; }

        //WPT data - coalesced from CarcassWPT and ReddWPT tables
        public string WPTName { get; set; }
        public string Datum { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        //TagsMarks data
        public string TransmitterType { get; set; } //combined radio and acoustic tag fields
        public string Vendor { get; set; }
        public string SerialNumber { get; set; }
        public string Frequency { get; set; }
        public string Channel { get; set; }
        public string Code { get; set; }
        public string TagsFloy { get; set; }
        public string TagsVIE { get; set; }
        public string TagsJaw { get; set; }
        public string TagsStaple { get; set; }
        public string TagsSpaghetti { get; set; }
        public string TagsStreamer { get; set; } //no associated records in SGS db, suggest omitting altogether
        public string TagsPetersonDisc { get; set; }
        public string MarksAnalFin { get; set; } //no associated records in SGS db, suggest omitting altogether
        public string MarksCaudalFin { get; set; } //no associated records in SGS db, suggest omitting altogether
        public string MarksPectoralFin { get; set; } //no associated records in SGS db, suggest omitting altogether
        public string MarksVentralFin { get; set; }
        public string MarksMaxillary { get; set; } //no associated records in SGS db, suggest omitting altogether
        public string MarksFreezeBrand { get; set; } //no associated records in SGS db, suggest omitting altogether
        public string MarksGRIT { get; set; } //no associated records in SGS db, suggest omitting altogether
        public string MarksOTC { get; set; } //no associated records in SGS db, suggest omitting altogether
        public string MarksDorsalScar { get; set; } //no associated records in SGS db, suggest omitting altogether
        public string Notes { get; set; } 

        //User Defined Fields
        public string UDF1 { get; set; }
        public string UDF2 { get; set; }
        public string UDF3 { get; set; }
        public string UDF4 { get; set; }
        public string UDF5 { get; set; }
    }
}