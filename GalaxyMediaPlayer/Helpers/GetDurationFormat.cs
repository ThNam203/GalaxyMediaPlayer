using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyMediaPlayer.Helpers
{
    internal class DurationFormatHelper
    {
        public static string GetDurationFormatFromTotalSeconds(double totalTimeInSecond)
        {
            // Nam: format string for each song duration
            string dayFormat = "dddd.hh\\:mm\\:ss";
            string hourFormat = "hh\\:mm\\:ss";
            string minuteFormat = "mm\\:ss";
            string secondFormat = "ss";

            // Nam: set durationFormat for beautiful ui
            if (totalTimeInSecond < 60) return secondFormat;
            else if (totalTimeInSecond < 3600) return minuteFormat;
            else if (totalTimeInSecond < 86400) return hourFormat;
            else return dayFormat;
        }
    }
    
}
