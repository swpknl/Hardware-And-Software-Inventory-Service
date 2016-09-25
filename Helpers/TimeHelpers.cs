namespace Helpers
{
    using System;

    using Constants;

    /// <summary>
    /// The date time helper.
    /// </summary>
    public static class TimeSpanHelper
    {
        public static TimeSpan GetTimeSpan(string input)
        {
            var dateTimeArray = input.Split(Delimiters.DateTimeDelimiter);
            var hours = Convert.ToInt32(dateTimeArray[0]);
            var minutes = Convert.ToInt32(dateTimeArray[1]);
            return new TimeSpan(hours, minutes, 0);
        }

        public static TimeSpan GetReportingTime(TimeSpan baseTime, TimeSpan timeSpan)
        {
            var timeSpanHour = timeSpan.Hours;
            var random = new Random();
            var randomTime = (random.NextDouble() * (timeSpanHour - 0.1) + 0.1).ToString().Split('.');
            var reportingTimeHour = Convert.ToInt32(randomTime[0]) + baseTime.Hours;
            var reportingTimeMinute = Convert.ToInt32(Math.Round(Convert.ToDecimal(randomTime[1].Substring(0, 2)),2)) + baseTime.Minutes;
            return new TimeSpan(reportingTimeHour, reportingTimeMinute, 0);
        }

        public static bool IsCurrentTimeEqualToReportingTime(string inputTime)
        {
            var currentTimeSpan = inputTime.Split(Delimiters.DateTimeDelimiter);
            var currentTimeHour = Convert.ToInt32(currentTimeSpan[0]);
            var currentTimeMinute = Convert.ToInt32(currentTimeSpan[1]);
            var currentTime = new TimeSpan(currentTimeHour, currentTimeMinute, 0);
            return ReportingTime.Time.Equals(currentTime);
        }
    }
}
