using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using NLog;
using services.Resources;

namespace services.Models
{
    public class Dataset
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        public const int STATUS_ACTIVE = 1;
        public const int STATUS_INACTIVE = 0;
        public const int STATUS_DELETED = 2;

        public const int ACCESS_PUBLIC = 1;
        public const int ACCESS_PRIVATE = 2;
        public const int ACCESS_DEPARTMENT = 3; 

        public int Id { get; set; }
        public int ProjectId { get; set; }          //what project is this dataset related to?
        public int DefaultRowQAStatusId { get; set; }
        public int DefaultActivityQAStatusId { get; set; }
        public int StatusId { get; set; } //active, inactive, deleted
        public int? DatastoreId { get; set; }
        public string Config { get; set; } //config for this dataset...

        public DateTime CreateDateTime { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        
        //collection of QAStatuses - which statuses are possible for this dataset?
        public virtual QAStatus DefaultRowQAStatus { get; set; }

        public virtual List<DatasetField> Fields { get; set; }
        public virtual Datastore Datastore { get; set; }

        [InverseProperty("Datasets")]
        public virtual List<QAStatus> QAStatuses { get; set; } //dataset activity qa options

        [InverseProperty("QARowDatasets")]
        public virtual List<QAStatus> RowQAStatuses { get; set; } //dataset row qa options

        [NotMapped]
        public List<MetadataValue> Metadata
        {
            set
            {
                MetadataHelper.saveMetadata(this.Metadata, value, this.Id);
            }
            get
            {
                return MetadataHelper.getMetadata(this.Id, MetadataEntity.ENTITYTYPE_DATASET);
            }

        }

        /**
         * Deletes all metadata for this dataset
         */
        public void deleteMetadata()
        {
            MetadataHelper.deleteMetadata(this.Id, MetadataEntity.ENTITYTYPE_DATASET);
        }

        //TODO: really want to cache this somehow...
        //note: this is specific to datasetfields, so we can't use it for the global query.
        //internal string getExportSelectString()
        internal string getExportSelectString(string productTarget)
        {
            logger.Debug("Inside Dataset.cs, getExportSelectString...");
            logger.Debug("productTarget = " + productTarget);

            var header_fields = string.Join(",", this.Fields.Where(o => o.FieldRoleId == FieldRole.HEADER && o.DbColumnName != "").OrderBy(o => o.Label).Select(o => o.DbColumnName));
            //logger.Debug("header_fields1 = " + header_fields);
            header_fields += (header_fields == "") ? "" : ","; //add on the ending comma if applicable
            //logger.Debug("header_fields2 = " + header_fields);
            var detail_fields = string.Join(",",this.Fields.Where(o => o.FieldRoleId == FieldRole.DETAIL).OrderBy(o => o.Label).Select(o => o.DbColumnName)) + ",";

            // ReadingDate and ReadingTime are fields from old data.  We do not use these fields in CDMS; however, they are in the DatasetFields table.
            // Therefore, we will remove these fields from the query, if they got included; otherwise, the query will break.
            logger.Debug("detail_fields (before) = " + detail_fields);
            int intReadingDateLoc = detail_fields.IndexOf("ReadingDate,");
            if (intReadingDateLoc > -1)
            {
                detail_fields = detail_fields.Replace("ReadingDate,", "");
                logger.Debug("detail_fields (after removing ReadingDate) = " + detail_fields);
            }
            int intReadingTimeLoc = detail_fields.IndexOf("ReadingTime,");
            if (intReadingTimeLoc > -1)
            {
                detail_fields = detail_fields.Replace("ReadingTime,", "");
                logger.Debug("detail_fields (after removing ReadingTime) = " + detail_fields);
            }

            var system_fields = "";
            logger.Debug("this.Datastore.TablePrefix = " + this.Datastore.TablePrefix);
            if (this.Datastore.TablePrefix == "WaterTemp")
            {
                logger.Debug("This dataset is WaterTemp-related...");
                if (productTarget == "Query")
                {
                    logger.Debug("getExportSelectString called by:  Query...");
                    //system_fields = "CreateDate,QAStatusId,QAStatusName, ActivityQAComments, LocationId,ActivityQAStatusId,DatasetId,ActivityId,RowId, RowStatusId";
                    //system_fields = "CreateDate,QAStatusId,QAStatusName, ActivityQAComments, LocationId,ActivityQAStatusId,ActivityId,RowId, RowStatusId";
                    system_fields = "QAStatusId,QAStatusName,ActivityQAComments,LocationId,ActivityQAStatusId";
                }
                else // productTarget == "Export"
                {
                    logger.Debug("getExportSelectString called by:  Export");
                    system_fields = "QAStatusId,QAStatusName,ActivityQAComments,Label";
                }
            }
            else if (this.Datastore.TablePrefix == "Metrics")
            {
                logger.Debug("This dataset is Metrics-related...");
                system_fields = "CreateDate,LocationId,QAStatusId,DatasetId,ActivityId";
            }
            else if (this.Datastore.TablePrefix == "StreamNet_NOSA")
            {
                logger.Debug("This dataset is StreamNet_NOSA-Related...");
                system_fields = "QAStatusId,DatasetId,ActivityId";
            }
            else if (this.Datastore.TablePrefix == "StreamNet_RperS")
            {
                logger.Debug("This dataset is StreamNet_RperS-Related...");
                system_fields = "DatasetId,ActivityId";
            }
            else if (this.Datastore.TablePrefix == "StreamNet_SAR")
            {
                logger.Debug("This dataset is StreamNet_SAR-Related...");
                system_fields = "DatasetId,ActivityId";
            }
            else if (this.Datastore.TablePrefix == "CreelSurvey")
            {
                logger.Debug("This dataset is CreelSurvey-related...");
                if (productTarget == "Query")
                    system_fields = "ActivityQAStatusId,LocationId";        
                else
                    system_fields = "QAStatusName,LocationLabel";
            }
            else if ((this.Datastore.TablePrefix == "BSample") ||
                    (this.Datastore.TablePrefix == "JvRearing") ||
                    (this.Datastore.TablePrefix == "Genetic") ||
                    (this.Datastore.TablePrefix == "Benthic") ||
                    (this.Datastore.TablePrefix == "Drift") ||
                    (this.Datastore.TablePrefix == "AdultWeir"))
            {
                logger.Debug("This dataset is BiologicalSample-, or AdultWeir related...");
                if (productTarget == "Query")
                    system_fields = "ActivityQAStatusId,LocationId";
                else
                    system_fields = "QAStatusName,LocationLabel";
            }
            else
            {
                logger.Debug("This dataset IS NOT WaterTemp-related...");
                system_fields = "CreateDate,QAStatusName, ActivityQAComments, LocationId,ActivityQAStatusId,DatasetId,ActivityId,RowId, RowStatusId";
            }

            string strFishermanFields = "";
            if (Datastore.TablePrefix == "CreelSurvey")
            {
                logger.Debug("Adding fisherman fields to query string...");
                /*Type type = typeof(Models.Fisherman);
                foreach (PropertyInfo property in type.GetProperties())
                {
                    if (property.Name == "Id")
                        strFishermanFields = strFishermanFields + "FishermanId,";
                    else if ((property.Name != "OkToCallId") &&
                        (!property.Name.Contains("ProjectFishermen")))
                        strFishermanFields = strFishermanFields + property.Name + ",";

                }
                */
                if (productTarget == "Query")
                    strFishermanFields = strFishermanFields + "FishermanId,";
                else
                    strFishermanFields = strFishermanFields + "FullName,";

                logger.Debug("The Fisherman fields are:  " + strFishermanFields);
            }

            //add on any "system" fields we want to also return
            var activity_fields = "";
            if (productTarget == "Query")
                activity_fields = "ActivityDate, ActivityId, ";
            else
                activity_fields = "ActivityDate, ";

            //return activity_fields + header_fields + detail_fields + system_fields;
            // The following line gets set up above.
            //system_fields = "CreateDate,QAStatusId,QAStatusName, ActivityQAComments, LocationId,ActivityQAStatusId,DatasetId,ActivityId,RowId, RowStatusId";

            //return activity_fields + header_fields + detail_fields + system_fields;
            return activity_fields + header_fields + detail_fields + strFishermanFields + system_fields;
        }

        internal IEnumerable<string> getExportLabelsList()
        {
            logger.Debug("Inside Dataset.cs, getExportLabelsList...");
            IEnumerable<string> labels = null;

            string strFishermanFields = "";
            if (Datastore.TablePrefix == "CreelSurvey")
            {
                logger.Debug("Adding fisherman fields to export labels list...");
                /*Type type = typeof(Models.Fisherman);
                foreach (PropertyInfo property in type.GetProperties())
                {
                    if (property.Name == "Id")
                        strFishermanFields = strFishermanFields + "FishermanId,";
                    else if ((property.Name != "OkToCallId") &&
                        (!property.Name.Contains("ProjectFishermen")))
                        strFishermanFields = strFishermanFields + property.Name + ",";

                }
                */
                strFishermanFields = strFishermanFields + "FullName,";
                logger.Debug("The Fisherman fields are:  " + strFishermanFields);
            }

            try
            {
                labels = new string[] { "ActivityDate" };

                //labels = labels
                //  .Concat(this.Fields.Where(o => o.FieldRoleId == FieldRole.HEADER).OrderBy(o => o.Label).Select(o => o.Label + " " + o.Field.Units))
                //  .Concat(this.Fields.Where(o => o.FieldRoleId == FieldRole.DETAIL).OrderBy(o => o.Label).Select(o => o.Label + " " + o.Field.Units))
                    ////.Concat(new List<string>(new string[] { "CreateDate", "QAStatusId", "QAStatus", "ActivityQAComments", "LocationId", "ActivityQAStatusId", "DatasetId", "ActivityId","RowId","RowStatusId"}));
                //  .Concat(new List<string>(new string[] { "CreateDate", "QAStatus", "ActivityQAComments", "LocationId", "ActivityQAStatusId", "DatasetId", "ActivityId", "RowId", "RowStatusId" }));

                //system_fields = "QAStatusId, QAStatusName, ActivityQAComments, Label";
                if (this.Datastore.TablePrefix == "WaterTemp")
                {
                    labels = labels
                      .Concat(this.Fields.Where(o => o.FieldRoleId == FieldRole.HEADER).OrderBy(o => o.Label).Select(o => o.Label + " " + o.Field.Units))
                      .Concat(this.Fields.Where(o => o.FieldRoleId == FieldRole.DETAIL).OrderBy(o => o.Label).Select(o => o.Label + " " + o.Field.Units))
                      //.Concat(new List<string>(new string[] { "CreateDate", "QAStatusId", "QAStatus", "ActivityQAComments", "LocationName" }));
                      .Concat(new List<string>(new string[] { "QAStatusId", "QAStatus", "ActivityQAComments", "LocationName" }));

                }
                else if (this.Datastore.TablePrefix == "CreelSurvey")
                {
                    labels = labels
                      .Concat(this.Fields.Where(o => o.FieldRoleId == FieldRole.HEADER).OrderBy(o => o.Label).Select(o => o.Label + " " + o.Field.Units))
                      .Concat(this.Fields.Where(o => o.FieldRoleId == FieldRole.DETAIL).OrderBy(o => o.Label).Select(o => o.Label + " " + o.Field.Units))
                        //.Concat(new List<string>(new string[] { "CreateDate", "QAStatusId", "QAStatus", "ActivityQAComments", "LocationId", "ActivityQAStatusId", "DatasetId", "ActivityId","RowId","RowStatusId"}));
                      .Concat(new List<string>(new string[] {"FullName", "QAStatus", "Location"}));
                }
                else if ((this.Datastore.TablePrefix == "BSample") ||
                        (this.Datastore.TablePrefix == "JvRearing") ||
                        (this.Datastore.TablePrefix == "Genetic") ||
                        (this.Datastore.TablePrefix == "Benthic") ||
                        (this.Datastore.TablePrefix == "Drift") ||
                        (this.Datastore.TablePrefix == "AdultWeir"))
                {
                    labels = labels
                      .Concat(this.Fields.Where(o => o.FieldRoleId == FieldRole.HEADER).OrderBy(o => o.Label).Select(o => o.Label + " " + o.Field.Units))
                      .Concat(this.Fields.Where(o => o.FieldRoleId == FieldRole.DETAIL).OrderBy(o => o.Label).Select(o => o.Label + " " + o.Field.Units))
                      .Concat(new List<string>(new string[] { "QAStatus", "Location" }));
                }
                else
                {
                    labels = labels
                      .Concat(this.Fields.Where(o => o.FieldRoleId == FieldRole.HEADER).OrderBy(o => o.Label).Select(o => o.Label + " " + o.Field.Units))
                      .Concat(this.Fields.Where(o => o.FieldRoleId == FieldRole.DETAIL).OrderBy(o => o.Label).Select(o => o.Label + " " + o.Field.Units))
                        //.Concat(new List<string>(new string[] { "CreateDate", "QAStatusId", "QAStatus", "ActivityQAComments", "LocationId", "ActivityQAStatusId", "DatasetId", "ActivityId","RowId","RowStatusId"}));
                      .Concat(new List<string>(new string[] { "CreateDate", "QAStatus", "ActivityQAComments", "LocationId", "ActivityQAStatusId", "DatasetId", "ActivityId", "RowId", "RowStatusId" }));
                }

                foreach (var item in labels)
                {
                    //logger.Debug(item);    
                }
                
            }
            catch (Exception e)
            {
                logger.Debug("Error!");
                logger.Debug(e);
            }
            return labels;

        }

      }

    

}