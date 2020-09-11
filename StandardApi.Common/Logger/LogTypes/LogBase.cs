namespace StandardApi.Common.Logger.LogTypes
{
    public abstract class LogBase
    {
        protected readonly object key = new object();
        public abstract void Log(string message);
    }
}
