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

        /// <summary>
        /// Loads all drivers from the database
        /// </summary>
        /// <returns>List of Drivers</returns>
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

        /// <summary>
        /// Searches the database for drivers
        /// </summary>
        /// <param name="driver">Driver to search on</param>
        /// <returns>List of corresponding drivers</returns>
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

        /// <summary>
        /// Creates a new Driver entry in the database
        /// </summary>
        /// <param name="driver">Driver to be created</param>
        /// <returns>Driver with new row ID</returns>
        public static Driver CreateDriver(Driver driver)
        {
            try
            {
                using (SqliteConnection db = DatabaseFile)
                {
                    db.Open();
                    SqliteCommand command = new SqliteCommand();
                    command.Connection = db;
                    command.CommandText = "INSERT INTO drivers (FirstName, LastName, City, State, Car_Make, Car_Model, Car_Year, RaceNumber) VALUES (@firstName, @lastName, @city, @state, @car_make, @car_model, @car_year, @raceNumber);";
                    command.CommandText += " select last_insert_rowid();";
                    command.Parameters.AddWithValue("@firstName", driver.FirstName);
                    command.Parameters.AddWithValue("@lastName", driver.LastName);
                    command.Parameters.AddWithValue("@city", driver.City);
                    command.Parameters.AddWithValue("@state", driver.State);
                    command.Parameters.AddWithValue("@car_make", driver.Car_Make);
                    command.Parameters.AddWithValue("@car_model", driver.Car_Model);
                    command.Parameters.AddWithValue("@car_year", driver.Car_Year);
                    command.Parameters.AddWithValue("@raceNumber", driver.RaceNumber);
                                        
                    SqliteDataReader query = command.ExecuteReader();

                    while (query.Read())
                    {
                        driver.ID = Convert.ToInt32(query.GetValue(0));
                    }
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                Logging.Log("CreateDriver() failed || " + ex.Message, Logging.LogType.ERROR);
            }
            return driver;
        }
    }
}