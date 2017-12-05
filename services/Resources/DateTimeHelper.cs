using System;
using NLog;

namespace services.Resources
{
    public class DateTimeHelper
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public DateTime dtDateTime1;
        public DateTime dtDateTime2;
        public string strDateTime1;
        public string strDateTime2;

        public DateTimeHelper(DateTime aDate)
        {
            setQueryDateBounds(aDate);
        }

        public void setQueryDateBounds(DateTime aDate)
        {
            logger.Debug("Inside DateTimeHelper.cs, setQueryDateBounds...");

            /* When we save a date/time in the database, we usually store it like this:  2017-11-30 10:30:15.125
             * Later when we query for a date, we must do it like this:  StartDate = '2017-11-30 10:30:15.125'
             * 
             * If we try to save another record later, using the same date and other parameter material (a duplicate),
             * the system sets the milliseconds, so the dateTime is actually different.
             * In order to get around this issue, we search for the YYYY-MM-DD only.
             * 
             * However, in order to search for a date in that manner, we must specify a lower and upper bounds.
             * For example, if we search for 2017-10-30 10:30:15.125,
             * the lower bound would be >= 2017-10-30 00:00:00.000
             * and the upper bound would be < 2017-10-31 00:00:00.000
             * 
             * With all the above in mind, this method receives a datetime item, and then extracts the upper and lower bounds should be.
             */

            DateTime dtStart;
            DateTime dtEnd;
            //string strBounds = "";
            int intYear = 0;
            int intMonth = 0;
            int intDay = 0;

            intYear = aDate.Year;
            intMonth = aDate.Month;
            intDay = aDate.Day;

            dtStart = new DateTime(intYear, intMonth, intDay);
            logger.Debug("dtStart = " + dtStart.ToString());

            intDay++;
            dtEnd = new DateTime(intYear, intMonth, intDay);
            logger.Debug("dtEnd = " + dtEnd.ToString());

            string strDtStart = dtStart.ToString("u");
            strDtStart = strDtStart.Replace("Z", "");
            strDtStart += ".000";
            logger.Debug("strDtStart = " + strDtStart);

            string strDtEnd = dtEnd.ToString("u");
            strDtEnd = strDtEnd.Replace("Z", "");
            strDtEnd += ".000";
            logger.Debug("strDtEnd = " + strDtEnd);

            //strBounds = strDtStart + "," + strDtEnd;

            //return strBounds;

            strDateTime1 = strDtStart;
            strDateTime2 = strDtEnd;
        }
    }
}