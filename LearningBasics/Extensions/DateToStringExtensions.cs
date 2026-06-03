namespace LearningBasics.Extensions
{
    public static class DateToStringExtensions
    {
        public static string ConvertDateToString(this DateTime date)
        {
            return date.ToString("F");
        }
    }
}
