using Microsoft.Data.Sqlite;
using System;
using System.Diagnostics;
using Windows.ApplicationModel.Resources;

namespace TrackSimulator
{
    public class DBHelper
    {
        public static SqliteConnection DatabaseFile { get; set; }
        internal static bool CreateDatabaseTables(string databaseName)
        {
            DatabaseFile = new SqliteConnection("Filename=" + databaseName + ".db");

            bool isSuccessful = false;
            ResourceLoader resourceLoader = ResourceLoader.GetForViewIndependentUse();

            using (SqliteConnection db = DatabaseFile)
            {
                db.Open();

                string driversTableCommand = "CREATE TABLE IF NOT EXISTS `DRIVERS`";
                SqliteCommand createDriversTable = new SqliteCommand(driversTableCommand, db);

                try
                {
                    createDriversTable.ExecuteReader();
                    isSuccessful = true;
                }
                catch (SqliteException e)
                {
                    Debug.WriteLine("Sqlite Database Tables for DRIVERS could not be created. " + e.Message);
                }
            }

            return isSuccessful;
        }
    }
}