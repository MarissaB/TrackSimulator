using System;
using System.Collections.Generic;
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

                    command.CommandText = @"CREATE TABLE IF NOT EXISTS 'drivers' (
                                            'ID'    INTEGER,
	                                        'FirstName' TEXT,
	                                        'LastName'  TEXT,
	                                        'City'  TEXT,
	                                        'State' TEXT,
	                                        'Car_Make'  TEXT,
	                                        'Car_Model' TEXT,
	                                        'Car_Year'  INTEGER,
	                                        'RaceNumber'    TEXT,
	                                        PRIMARY KEY('ID' AUTOINCREMENT));";
                    _ = command.ExecuteNonQuery();
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                Logging.Log("CreateDatabaseTables() failed || " + ex.Message, Logging.LogType.ERROR);
            }
        }

        public static List<Driver> GetAllDrivers()
        {
            List<Driver> drivers = new List<Driver>();
            try
            {
                using (SqliteConnection db = DatabaseFile)
                {
                    db.Open();
                    SqliteCommand command = new SqliteCommand();
                    command.Connection = db;
                    command.CommandText = "SELECT * FROM drivers";
                    SqliteDataReader query = command.ExecuteReader();

                    while (query.Read())
                    {
                        Driver driver = new Driver(query);
                        drivers.Add(driver);
                    }
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                Logging.Log("GetAllDrivers() failed || " + ex.Message, Logging.LogType.ERROR);
            }
            return drivers;
        }

        public static List<Driver> SearchDrivers(Driver driver)
        {
            List<Driver> drivers = new List<Driver>();
            try
            {
                using (SqliteConnection db = DatabaseFile)
                {
                    db.Open();
                    SqliteCommand command = new SqliteCommand();
                    command.Connection = db;
                    command.CommandText = "SELECT * FROM drivers WHERE " + driver.SearchTerms();
                    SqliteDataReader query = command.ExecuteReader();

                    while (query.Read())
                    {
                        Driver foundDriver = new Driver(query);
                        drivers.Add(driver);
                    }
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                Logging.Log("SearchDrivers() failed || " + ex.Message, Logging.LogType.ERROR);
            }
            return drivers;
        }
    }
}