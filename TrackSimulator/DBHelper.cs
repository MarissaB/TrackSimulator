using System;
using Microsoft.Data.Sqlite;
using Windows.Storage;

namespace TrackSimulator
{
    public class DBHelper
    {
        private static readonly string DatabaseFileName = "TrackSimulator.db";
        private static readonly string DatabaseFilePath = ApplicationData.Current.LocalFolder.Path + "\\" + DatabaseFileName;
        public static SqliteConnection DatabaseFile { get; set; }
        
        /// <summary>
        /// Creates all database tables in a new connection.
        /// </summary>
        /// <param name="purge">Whether to purge all tables in the database</param>
        internal static void CreateDatabaseTables(bool purge)
        {
            try
            {
                DatabaseFile = new SqliteConnection("Filename=" + DatabaseFilePath);

                using (SqliteConnection db = DatabaseFile)
                {
                    db.Open();

                    SqliteCommand command = new SqliteCommand();
                    command.Connection = db;
                    if (purge)
                    {
                        command.CommandText = @"DROP TABLE IF EXISTS drivers";
                        _ = command.ExecuteNonQuery();
                    }

                    command.CommandText = @"CREATE TABLE drivers(id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT)";
                    _ = command.ExecuteNonQuery();
                    Logging.Log("Created database tables.", Logging.LogType.INFO);
                }
            }
            catch (Exception ex)
            {
                Logging.Log("Sqlite Database Tables for DRIVERS could not be created || " + ex.Message, Logging.LogType.ERROR);
            }
        }
    }
}