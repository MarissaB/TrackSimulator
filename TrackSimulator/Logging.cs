using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace TrackSimulator
{
    public static class Logging
    {
        private static readonly string LogFileName = "TrackSimulator_Logging.txt";
        private static readonly string LogFilePath = ApplicationData.Current.LocalFolder.Path + "\\" + LogFileName;
        public enum LogType
        {
            ERROR,
            INFO,
            DEBUG
        }
        /// <summary>
        /// Checks for a log file and creates one if it doesn't exist.
        /// </summary>
        public static async void CheckLogFileAsync()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            FileInfo logFile = new FileInfo(LogFilePath);

            if (!logFile.Exists)
            {
                try
                {
                    StorageFile log = await localFolder.CreateFileAsync(LogFileName);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("InitializeLogFile() -- " + ex.Message);
                }
            }
        }

        public static void Log(string message, LogType type)
        {
            FileInfo logFile = new FileInfo(LogFilePath);

            if (logFile.Exists)
            {
                StreamWriter contents = logFile.AppendText();
                contents.WriteLine(DateTime.Now.ToString("YYYY-MM-dd HH:mm") + " [" + type.ToString() + "] " + message);
                contents.Close();
            }
        }
    }
}
