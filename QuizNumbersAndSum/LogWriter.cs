using System;
using System.IO;
using System.Reflection;

namespace QuizNumbersAndSum
{
    public class LogWriter
    {
        private string logPath;
        private string logName;
        private object locker;// for multithreading
        public LogWriter(string logPath, string logName)
        {
            this.logPath = logPath;
            this.logName = logName;
            locker = new object();
        }

        public void Write(string logMessage)
        {
            try
            {
                lock (locker)
                {
                    using (StreamWriter w = File.AppendText(logPath + "\\" + logName))
                    {
                        Log(logMessage, w);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
            try
            {
                lock (locker)
                {
                    File.WriteAllText(logPath + "\\" + logName, string.Empty);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
