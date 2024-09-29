using System;
using System.IO;

namespace UserRekongition.Model
{
    public class Logger
    {
        private readonly string logFilePath;

        public Logger(string filePath)
        {
            logFilePath = filePath;
        }

        // Ghi log thông tin ngoại lệ
        public void LogException(string exceptionType, Exception ex)
        {
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {exceptionType}: {ex.Message}" +
                                $"\nStack Trace: {ex.StackTrace}\n";
            WriteLog(logMessage);
        }

        // Ghi log thông tin khác
        public void LogInfo(string message)
        {
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] INFO: {message}\n";
            WriteLog(logMessage);
        }

        // Ghi nội dung vào file log
        private void WriteLog(string message)
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine(message);
            }
        }
    }
}
