using CsvHelper;
using Newtonsoft.Json.Linq;
using services.Models;
using services.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace services.Controllers
{
    public class ExportController : CDMSController
    {
        //Writes csv export file out to a file and returns the url.
        // POST /api/v1/export/exportdatasetactivities
        [HttpPost]
        public ExportResult ExportDatasetActivities(JObject jsonData)
        {
            var db = ServicesContext.Current;

            dynamic json = jsonData;

            User me = AuthorizationManager.getCurrentUser();

            //grab a reference to this dataset so we can parse incoming fields
            Dataset dataset = db.Datasets.Find(json.DatasetId.ToObject<int>());
            if (dataset == null || me == null)
                throw new Exception("Configuration error. Please try again.");

            logger.Debug("Alright!  we are working with dataset: " + dataset.Id);

            //DataTable dt = getQueryResults(dataset, json);
            DataTable dt = QueryHelper.getQueryResults(dataset, json, "Export");

            logger.Debug("Download data -- we have a result back.");

            string Filename = json.Filename;
            Filename = Filename.Replace("\"", string.Empty);
            Filename = Filename.Replace("\\", string.Empty);
            Filename = Filename.Replace("/", string.Empty);

            logger.Debug("Incoming filename specified: " + Filename);

            string root = System.Web.HttpContext.Current.Server.MapPath("~/exports");
            string the_file = root + @"\" + dataset.Id + @"_" + me.Id + @"\" + Filename;

            logger.Debug("saving file to location: " + the_file);

            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(the_file)); //will create if necessary.

            //TODO: better is to get it from config
            string rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);
            //rootUrl += "/services/exports/" + dataset.Id + "_" + me.Id + "/" + Filename;
            rootUrl += "/" + System.Configuration.ConfigurationManager.AppSettings["ExecutingEnvironment"] + "exports/" + dataset.Id + "_" + me.Id + "/" + Filename;
            logger.Debug("rootUrl again = " + rootUrl);

            using (TextWriter writer = System.IO.File.CreateText(the_file)) //will overwrite = good
            {
                using (var csv = new CsvWriter(writer))
                {
                    IEnumerable<string> columnNames = dataset.getExportLabelsList();//dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);

                    string strHeader = "Waypoints File";
                    int intHeaderLength = strHeader.Length;
                    int intFieldHeaderLength = 0;
                    logger.Debug("dataset.Datastore.TablePrefix = " + dataset.Datastore.TablePrefix);

                    //columns
                    foreach (var header in columnNames)
                    {
                        //logger.Debug("header = " + header);
                        intFieldHeaderLength = header.Length;
                        if (dataset.Datastore.TablePrefix == "NPT_SGS_carcass")
                        {
                            if ((intFieldHeaderLength >= intHeaderLength) && (header.IndexOf(strHeader) > -1))
                            {
                                // For Spawning Ground Survey, the "Waypoints File" header is unnecessary,
                                // because the data is not saved.  The header is only used in the front end.
                                // Consequently, it causes the data items to be skewed; everything from Channel to the right,
                                // gets shifted to the left one, because there is no Waypoint File entry.
                                // Therefore, we will skip adding this header.
                                logger.Debug("Skipping Waypoints File Header...");
                            }
                            else
                            {
                                csv.WriteField(header);
                            }
                        }
                        else
                        {
                            csv.WriteField(header);
                        }
                    }
                    csv.NextRecord();

                    //fields
                    foreach (DataRow row in dt.Rows)
                    {
                        IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                        foreach (var field in fields)
                        {
                            //logger.Debug("field before replace = " + field);
                            //replace out the multiselect array stuff.
                            var f = field.Replace("[]", string.Empty).Replace("[\"", string.Empty).Replace("\"]", string.Empty).Replace("\",\"", ",");
                            //logger.Debug("field after replace = " + f);
                            csv.WriteField(f);
                        }
                        csv.NextRecord();
                    }
                }
            }

            //TODO-- error handling?

            ExportResult result = new ExportResult();
            result.success = true;
            result.file = rootUrl;
            result.errors = null;

            return result;

        }
    }
}
