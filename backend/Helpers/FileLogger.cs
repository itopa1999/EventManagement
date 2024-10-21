using System;
using System.IO;
using System.Threading.Tasks;

namespace backend.Helpers
{
    public class FileLogger
    {
        private readonly string _logFilePath;

        public FileLogger(string logFilePath)
        {
            _logFilePath = logFilePath;
            if (!File.Exists(_logFilePath))
            {
                File.Create(_logFilePath).Close();
            }
        }

        public async Task LogErrorAsync(Exception ex)
        {
            var logEntry = $"{DateTime.UtcNow}: Error occurred: {ex.Message}{Environment.NewLine}" +
                        $"Stack Trace: {ex.StackTrace}{Environment.NewLine}" +
                        $"Inner Exception: {ex.InnerException?.Message}{Environment.NewLine}" +
                        $"-------------------------------------------------------------------{Environment.NewLine}";

            await File.AppendAllTextAsync(_logFilePath, logEntry);
        }
    }
}