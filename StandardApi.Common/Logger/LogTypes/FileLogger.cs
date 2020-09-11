using System.IO;

namespace StandardApi.Common.Logger.LogTypes
{
    public class FileLogger : LogBase
    {
        public string filePath = @"D:\IDGLog.txt";

        public override void Log(string message)
        {
            lock (key)
            {
                using (StreamWriter streamWriter = new StreamWriter(filePath))
                {
                    streamWriter.WriteLine(message);
                    streamWriter.Close();
                }
            }
        }
    }
}
