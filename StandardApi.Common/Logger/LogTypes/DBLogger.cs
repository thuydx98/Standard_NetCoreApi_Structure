namespace StandardApi.Common.Logger.LogTypes
{
    public class DBLogger : LogBase
    {
        string connectionString = string.Empty;
        public override void Log(string message)
        {
            lock (key)
            {
                //Code to log data to the database
            }
        }
    }
}
