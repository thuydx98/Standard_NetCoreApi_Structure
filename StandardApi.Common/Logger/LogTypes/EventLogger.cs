namespace StandardApi.Common.Logger.LogTypes
{
    public class EventLogger : LogBase
    {
        public override void Log(string message)
        {
            lock (key)
            {
                //EventLog eventLog = new EventLog(“”);
                //eventLog.Source = "IDGEventLog";
                //eventLog.WriteEntry(message);
            }
        }
    }
}
