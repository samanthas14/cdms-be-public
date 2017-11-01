using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Resources
{
    public class StringHelper
    {
        //removes newlines, tabs and double spaces.
        public static string removeFormattingChars(string aString)
        {
            aString = aString.Replace(System.Environment.NewLine, "");
            aString = aString.Replace("\t", "");
            aString = aString.Replace("  ", " ");

            return aString;
        }

        //pad a number with a certain number of zeros. defaults to 10
        private string padNumber(string strNumber, int intDigits = 10)
        {
            if (strNumber.Length < intDigits)
                strNumber = "0" + strNumber;

            return strNumber;
        }
    }
}