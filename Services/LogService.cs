using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalManagementSystem.Services
{
    public class LogService
    {
        public const string FileSrc = "Logs.txt";

        public const string LogInfo = "Information";
        public const string LogWarning = "Warning";
        public const string LogError = "Error";

        public void WriteLog(String LogType, String Caller, String Message)
        {
            File.AppendAllText(FileSrc, String.Format(
                "[{0}] => {3} : ({1}) -> {2}",
                DateTime.Now.ToString(),
                Caller,
                Message,
                LogType));
        }
    }
}
