using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.WebPages;
using Newtonsoft.Json.Linq;

namespace services.Resources
{
    /**
     * Reads a TSV (tab separated) file and return
     */ 
    public class PitagisReader
    {
        private readonly string fileName;
        private string[] colVals;

        const string RECORD_ID        = "RECORD ID";
        const string PIT_TAG_NUMBER   = "PIT TAG NUMBER";
        const string FORK_LENGTH      = "FORK LENGTH";
        const string WEIGHT           = "WEIGHT";
        const string TYPE_RUN_REARING = "TYPE/RUN/REARING";
        const string COMMENT_1        = "COMMENT 1";
        const string COMMENT_2        = "COMMENT 2";
        const string COMMENT_3        = "COMMENT 3";
        const string FISH_COUNT       = "Fish Count";           // This field will contain a default value, injected in code below

        private static List<string> colNames;


        public PitagisReader(string a_fileName)
        {
            fileName = a_fileName;

            colNames = new List<string> {
                "FILE TYPE", "PROGRAM VERSION", "FILE TITLE", "TAG DATE", "TAGGER", "HATCHERY SITE", 
                "STOCK", "BROOD YR", "MIGRATORY YR", "TAG SITE", "RACEWAY/TRANSECT", "CAPTURE METHOD", 
                "TAGGING TEMP", "POST TAGGING TEMP", "RELEASE WATER TEMP", "TAGGING METHOD", "ORGANIZATION", 
                "COORDINATOR ID", "RELEASE DATE", "RELEASE SITE", "RELEASE RIVER KM", RECORD_ID,
                PIT_TAG_NUMBER, FORK_LENGTH, WEIGHT, TYPE_RUN_REARING, COMMENT_1, COMMENT_2, COMMENT_3, FISH_COUNT
            };            
            
            colVals = new string[colNames.Count];
        }


        internal ImportDataResult getImportDataResult()
        {

            // The first 24 lines are the "header" and need to be parsed out:

            // FILE TYPE                      : TAGGING
            // PROGRAM VERSION                : PITTAG3 1.5.4
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // MEACHAM CONTROL UNIT #2 RECAP
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // FILE TITLE                     : CRC14169.R02
            // TAG DATE                       : 06/18/14 10:28
            // TAGGER                         : THOMPSON D
            // HATCHERY SITE                  : 
            // STOCK                          : 
            // BROOD YR                       : 
            // MIGRATORY YR                   : 14
            // TAG SITE                       : MEACHC
            // RACEWAY/TRANSECT               : 
            // CAPTURE METHOD                 : SHOCK
            // TAGGING TEMP                   : 10.5
            // POST TAGGING TEMP              : 10.7
            // RELEASE WATER TEMP             : 10.2
            // TAGGING METHOD                 : HAND
            // ORGANIZATION                   : CTUIR
            // COORDINATOR ID                 : CRC
            // RELEASE DATE                   : 06/18/14 10:40
            // RELEASE SITE                   : MEACHC
            // RELEASE RIVER KM               : 465.127.014


            ImportDataResult dataresult = new ImportDataResult();
            var rows = new List<Dictionary<string, string>>();

            dataresult.columns = colNames;

            using (var reader = new StreamReader(fileName))
            {
                // Skip any leading blank lines... shouldn't be any, but you never know

                string line = "";

                // Inject some default values
                Store(new Tuple<string, string>(FISH_COUNT, "1"));

                while (line.Trim().IsEmpty())
                    line = reader.ReadLine();

                // Now process lines until we hit another blank line

                while (!line.Trim().IsEmpty())
                {
                    Store(GetNameAndValue(line));
                    line = reader.ReadLine();
                }

                // Fix up dates in the header
                StandardizeDateTime("TAG DATE");
                StandardizeDateTime("RELEASE DATE");

                // After the 2nd blank line, we're in the data section... Read until we hit a non-blank.
                while (line.Trim().IsEmpty())
                    line = reader.ReadLine();


                // Again process until we hit a blank line
                while (!line.Trim().IsEmpty())
                {
                    // Now we have column-delineated data:
                    // 4  384.3B23AF7939      95       9.3  32W  ||Unit 2 Scale 42
                    // RECORD_ID, PIT_TAG_NUMBER, FORK_LENGTH, WEIGHT, TYPE_RUN_REARING, COMMENT_1, COMMENT_2, COMMENT_3

                    Store(new Tuple<string,string>(RECORD_ID, line.Substring(0, 4).Trim()));
                    Store(new Tuple<string, string>(PIT_TAG_NUMBER, line.Substring(5, 15).Trim()));
                    Store(new Tuple<string, string>(FORK_LENGTH, line.Substring(23, 6).Trim()));
                    Store(new Tuple<string, string>(WEIGHT, line.Substring(30, 9).Trim()));
                    Store(new Tuple<string, string>(TYPE_RUN_REARING, line.Substring(40, 4).Trim()));

                    // Now comes the pipe-delimited stuff...
                    var rest = line.Substring(45).Trim().Split('|');

                    Store(new Tuple<string, string>(COMMENT_1, rest.Length > 0 ? rest[0] : ""));
                    Store(new Tuple<string, string>(COMMENT_2, rest.Length > 1 ? rest[1] : ""));
                    Store(new Tuple<string, string>(COMMENT_3, rest.Length > 2 ? rest[2] : ""));

                    var row = new Dictionary<string, string>();

                    for (var i = 0; i < colNames.Count; i++)
                    {
                        var colName = RenameHeaderValue(colNames[i]);

                        row[colName] = colVals[i];
                    }

                    rows.Add(row);

                    line = reader.ReadLine();
                }
            }

            RenameHeaders();

            dataresult.columns = colNames;
            dataresult.rows = rows;

            return dataresult;
        }

        //private static readonly List<string> colNames = new List<string> {
        //        "FILE TYPE", "PROGRAM VERSION", "FILE TITLE", "TAG DATE", "TAGGER", "HATCHERY SITE", 
        //        "STOCK", "BROOD YR", "MIGRATORY YR", "TAG SITE", "RACEWAY/TRANSECT", "CAPTURE METHOD", 
        //        "TAGGING TEMP", "POST TAGGING TEMP", "RELEASE WATER TEMP", "TAGGING METHOD", "ORGANIZATION", 
        //        "COORDINATOR ID", "RELEASE DATE", "RELEASE SITE", "RELEASE RIVER KM", RECORD_ID,
        //        PIT_TAG_NUMBER, FORK_LENGTH, WEIGHT, TYPE_RUN_REARING, COMMENT_1, COMMENT_2, COMMENT_3
        //    };

        private void RenameHeaders()
        {
            for (var i = 0; i < colNames.Count; i++)
                colNames[i] = RenameHeaderValue(colNames[i]);
        }


        private string RenameHeaderValue(string from)
        {
            if (from == "TAG DATE")           return "Tag DateTime";
            if (from == "MIGRATORY YR")       return "Migratory Year";
            if (from == "TAGGING TEMP")       return "Tagging Temp (C)";
            if (from == "POST TAGGING TEMP")  return "Post Tagging Temp (C)";
            if (from == "RELEASE WATER TEMP") return "Release Temp (C)";
            if (from == "RELEASE DATE")       return "Release DateTime";
            if (from == RECORD_ID)            return "Sequence";
            if (from == COMMENT_1)            return "Additional Positional Comments";
            if (from == COMMENT_2)            return "Conditional Comment";
            if (from == COMMENT_3)            return "Textual Comments";            
            if (from == PIT_TAG_NUMBER)       return "Pit Tag Code";
            if (from == FORK_LENGTH)          return "Fork Length (mm)";
            if (from == WEIGHT)               return "Weight (g)";
            if (from == TYPE_RUN_REARING)     return "Species Run Rearing";

            return from;
        }


        // Convert 04/05/15 09:17 to a more friendly format
        private void StandardizeDateTime(string field)
        {
            var index = colNames.FindIndex(x => x == field);

            if(index != -1)
                colVals[index] = DateTime.Parse(colVals[index]).ToString("MM/dd/yyyy");     
        }


        // Returns null if the line does not contain a single ':' char
        private Tuple<string, string> GetNameAndValue(string line)
        {
            var words = line.Split(new[] { ':' }, 2);

            if (words.Length != 2)
                return null;

            return new Tuple<string, string>(words[0].Trim(), words[1].Trim());
        }


        private void Store(Tuple<string, string> nameVal)
        {
            if (nameVal != null)
            {
                var index = colNames.FindIndex(x => x == nameVal.Item1);

                if (index > -1) // Ignore unknown values
                    colVals[index] = nameVal.Item2;
            }
        }

    }
}