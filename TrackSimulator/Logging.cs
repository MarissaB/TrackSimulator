using System;
using System.Diagnostics;
using System.IO;
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

        /// <summary>
        /// Record a message in the log
        /// </summary>
        /// <param name="message">String of message</param>
        /// <param name="type">LogType of message</param>
        public static void Log(string message, LogType type)
        {
            FileInfo logFile = new FileInfo(LogFilePath);

            if (logFile.Exists)
            {
                StreamWriter contents = logFile.AppendText();
                contents.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm") + " [" + type.ToString() + "] " + message);
                contents.Close();
            }
        }
    }
}
