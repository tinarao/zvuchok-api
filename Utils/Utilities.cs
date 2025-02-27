namespace api.Utils
{
    public class Utilities
    {
        public enum AvailabilityStatus
        {
            Public,
            OnModeration,
            Rejected,
            OnAnalysis
        }

        public static string NormalizeString(string str)
        {
            return str.ToLower();
        }
    }
}