using System;
using System.IO;
using System.Reflection;

namespace QuizNumbersAndSum
{
    public class LogWriter
    {
        private string logPath;
        private string logName;
        public LogWriter(string logPath, string logName)
        {
            this.logPath = logPath;
            this.logName = logName;
        }
        public void Write(string logMessage)
        {
            try
            {
                using (StreamWriter w = File.AppendText(logPath + "\\" + logName))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0}", DateTime.Now);
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }

        public void ClearLog()
        {
            File.WriteAllText(logPath + "\\" + logName, string.Empty);
        }
    }
}
