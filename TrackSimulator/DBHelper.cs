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

        #region Driver Commands
        /// <summary>
        /// Creates the driver table
        /// </summary>
        /// <param name="purge">Whether to purge the table from the database</param>
        internal static void CreateDriversTable(bool purge)
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

                    command.CommandText =
                        @"CREATE TABLE IF NOT EXISTS 'drivers' (
                                            'ID'    INTEGER,
	                                        'FirstName' TEXT,
	                                        'LastName'  TEXT,
	                                        'City'  TEXT,
	                                        'State' TEXT,
	                                        'Car_Make'  TEXT,
	                                        'Car_Model' TEXT,
	                                        'Car_Year'  INTEGER,
	                                        'DriverNumber'    TEXT,
                                            'Active'    TEXT NOT NULL DEFAULT 'true',
                                            PRIMARY KEY('ID' AUTOINCREMENT));";
                    _ = command.ExecuteNonQuery();
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                Logging.Log("CreateDriverTable() failed || " + ex.Message, Logging.LogType.ERROR);
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
        /// <param name="count">Number of drivers to return</param>
        /// <param name="includeInactives">Whether to include inactive drivers</param>
        /// <returns>List of corresponding drivers</returns>
        public static List<Driver> SearchDrivers(Driver driver, int count, bool includeInactives)
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
                    if (!includeInactives)
                    {
                        command.CommandText += " and Active LIKE 'true'";
                    }
                    if (count > 0)
                    {
                        command.CommandText += " LIMIT " + count.ToString();
                    }
                    SqliteDataReader query = command.ExecuteReader();

                    while (query.Read())
                    {
                        Driver foundDriver = new Driver(query);
                        drivers.Add(foundDriver);
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
                    command.CommandText = "INSERT INTO drivers (FirstName, LastName, City, State, Car_Make, Car_Model, Car_Year, DriverNumber, Active) VALUES (@firstName, @lastName, @city, @state, @car_make, @car_model, @car_year, @driverNumber, @active);";
                    command.CommandText += " select last_insert_rowid();";
                    command.Parameters.AddWithValue("@firstName", driver.FirstName);
                    command.Parameters.AddWithValue("@lastName", driver.LastName);
                    command.Parameters.AddWithValue("@city", driver.City);
                    command.Parameters.AddWithValue("@state", driver.State);
                    command.Parameters.AddWithValue("@car_make", driver.Car_Make);
                    command.Parameters.AddWithValue("@car_model", driver.Car_Model);
                    command.Parameters.AddWithValue("@car_year", driver.Car_Year);
                    command.Parameters.AddWithValue("@driverNumber", driver.DriverNumber);
                    command.Parameters.AddWithValue("@active", driver.Active.ToString());

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

        /// <summary>
        /// Updates a driver record in the database
        /// </summary>
        /// <param name="driver">Driver to update</param>
        /// <returns>Boolean for whether the command was successful</returns>
        public static bool UpdateDriver(Driver driver)
        {
            bool isSuccessful = false;
            try
            {
                using (SqliteConnection db = DatabaseFile)
                {
                    db.Open();
                    SqliteCommand command = new SqliteCommand();
                    command.Connection = db;
                    command.CommandText = "UPDATE drivers SET " +
                        "FirstName = @firstName, " +
                        "LastName = @lastName, " +
                        "City = @city, " +
                        "State = @state, " +
                        "Car_Make = @car_make, " +
                        "Car_Model = @car_model, " +
                        "Car_Year = @car_year, " +
                        "DriverNumber = @driverNumber," +
                        "Active = @active " +
                        "WHERE ID = @id";
                    command.Parameters.AddWithValue("@firstName", driver.FirstName);
                    command.Parameters.AddWithValue("@lastName", driver.LastName);
                    command.Parameters.AddWithValue("@city", driver.City);
                    command.Parameters.AddWithValue("@state", driver.State);
                    command.Parameters.AddWithValue("@car_make", driver.Car_Make);
                    command.Parameters.AddWithValue("@car_model", driver.Car_Model);
                    command.Parameters.AddWithValue("@car_year", driver.Car_Year);
                    command.Parameters.AddWithValue("@driverNumber", driver.DriverNumber);
                    command.Parameters.AddWithValue("@active", driver.Active.ToString());
                    command.Parameters.AddWithValue("@id", driver.ID);

                    SqliteDataReader query = command.ExecuteReader();
                    db.Close();
                    isSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                Logging.Log("UpdateDriver() failed || " + ex.Message, Logging.LogType.ERROR);
            }
            return isSuccessful;
        }
        #endregion

        #region Category Commands
        /// <summary>
        /// Creates the categories table
        /// </summary>
        /// <param name="purge">Whether to purge the table from the database</param>
        internal static void CreateCategoriesTable(bool purge)
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
                        command.CommandText = @"DROP TABLE IF EXISTS categories";
                        _ = command.ExecuteNonQuery();
                    }

                    command.CommandText =
                        @"CREATE TABLE IF NOT EXISTS 'categories' (
                                            'ID'    INTEGER,
	                                        'Name' TEXT,
                                            'Length' INTEGER,
	                                        'Qualifying'  TEXT,
	                                        'Light'  TEXT,
	                                        'Ladder' TEXT,
                                            'Active'    TEXT NOT NULL DEFAULT 'true',
                                            PRIMARY KEY('ID' AUTOINCREMENT));";
                    _ = command.ExecuteNonQuery();
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                Logging.Log("CreateCategoriesTable() failed || " + ex.Message, Logging.LogType.ERROR);
            }
        }

        /// <summary>
        /// Loads all categories from the database
        /// </summary>
        /// <returns>List of Drivers</returns>
        public static List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();
            try
            {
                using (SqliteConnection db = DatabaseFile)
                {
                    db.Open();
                    SqliteCommand command = new SqliteCommand();
                    command.Connection = db;
                    command.CommandText = "SELECT * FROM categories";
                    SqliteDataReader query = command.ExecuteReader();

                    while (query.Read())
                    {
                        Category category = new Category(query);
                        categories.Add(category);
                    }
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                Logging.Log("GetAllCategories() failed || " + ex.Message, Logging.LogType.ERROR);
            }
            return categories;
        }

        /// <summary>
        /// Creates a new Driver entry in the database
        /// </summary>
        /// <param name="driver">Driver to be created</param>
        /// <returns>Driver with new row ID</returns>
        public static Category CreateCategory(Category category)
        {
            try
            {
                using (SqliteConnection db = DatabaseFile)
                {
                    db.Open();
                    SqliteCommand command = new SqliteCommand();
                    command.Connection = db;
                    command.CommandText = "INSERT INTO categories (Name, Length, Qualifying, Light, Ladder, Active) VALUES (@name, @length, @qualifying, @light, @ladder, @active);";
                    command.CommandText += " select last_insert_rowid();";
                    command.Parameters.AddWithValue("@name", category.Name);
                    command.Parameters.AddWithValue("@length", category.Length.ToString());
                    command.Parameters.AddWithValue("@qualifying", category.Qualifying);
                    command.Parameters.AddWithValue("@light", category.Light);
                    command.Parameters.AddWithValue("@ladder", category.Ladder);
                    command.Parameters.AddWithValue("@active", category.Active.ToString());

                    SqliteDataReader query = command.ExecuteReader();

                    while (query.Read())
                    {
                        category.ID = Convert.ToInt32(query.GetValue(0));
                    }
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                Logging.Log("CreateCategory() failed || " + ex.Message, Logging.LogType.ERROR);
            }
            return category;
        }

        /// <summary>
        /// Searches the database for categories
        /// </summary>
        /// <param name="category">Category to search on</param>
        /// <returns>List of corresponding categories</returns>
        public static List<Category> SearchCategories(Category category, bool includeInactives)
        {
            List<Category> categories = new List<Category>();
            try
            {
                using (SqliteConnection db = DatabaseFile)
                {
                    db.Open();
                    SqliteCommand command = new SqliteCommand();
                    command.Connection = db;
                    command.CommandText = "SELECT * FROM categories WHERE Name LIKE '%" + category.Name + "%'";
                    if (includeInactives)
                    {
                        command.CommandText += " and Active LIKE 'true'";
                    }
                    SqliteDataReader query = command.ExecuteReader();

                    while (query.Read())
                    {
                        Category foundCategory = new Category(query);
                        categories.Add(foundCategory);
                    }
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                Logging.Log("SearchCategories() failed || " + ex.Message, Logging.LogType.ERROR);
            }
            return categories;
        }

        /// <summary>
        /// Updates a category record in the database
        /// </summary>
        /// <param name="category">Category to update</param>
        /// <returns>Boolean for whether the command was successful</returns>
        public static bool UpdateCategory(Category category)
        {
            bool isSuccessful = false;
            try
            {
                using (SqliteConnection db = DatabaseFile)
                {
                    db.Open();
                    SqliteCommand command = new SqliteCommand();
                    command.Connection = db;
                    command.CommandText = "UPDATE categories SET " +
                        "Name = @name, " +
                        "Length = @length, " +
                        "Qualifying = @qualifying, " +
                        "Light = @light, " +
                        "Ladder = @ladder, " +
                        "Active = @active " +
                        "WHERE ID = @id";
                    command.Parameters.AddWithValue("@name", category.Name);
                    command.Parameters.AddWithValue("@length", category.Length.ToString());
                    command.Parameters.AddWithValue("@qualifying", category.Qualifying);
                    command.Parameters.AddWithValue("@light", category.Light);
                    command.Parameters.AddWithValue("@ladder", category.Ladder);
                    command.Parameters.AddWithValue("@active", category.Active.ToString());
                    command.Parameters.AddWithValue("@id", category.ID.ToString());

                    SqliteDataReader query = command.ExecuteReader();
                    db.Close();
                    isSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                Logging.Log("UpdateCategory() failed || " + ex.Message, Logging.LogType.ERROR);
            }
            return isSuccessful;
        }

        #endregion

        #region Run Commands
        /// <summary>
        /// Creates the runs table
        /// </summary>
        /// <param name="purge">Whether to purge the table from the database</param>
        internal static void CreateRunTable(bool purge)
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
                        command.CommandText = @"DROP TABLE IF EXISTS runs";
                        _ = command.ExecuteNonQuery();
                    }

                    command.CommandText =
                        @"CREATE TABLE IF NOT EXISTS 'runs' (
                                            'ID'    INTEGER,
                                            'Lane'  TEXT,
                                            'Winner'    TEXT,
                                            'Dial'  NUMERIC,
                                            'Start' TEXT,
                                            'Time_Reaction' NUMERIC,
                                            'Time_60'   NUMERIC,
                                            'Time_330'  NUMERIC,
                                            'Time_660'  NUMERIC,
                                            'Time_990'  NUMERIC,
                                            'Time_1320' NUMERIC,
                                            'Speed_660' INTEGER,
                                            'Speed_1320'    INTEGER,
                                            'DriverID'  INTEGER NOT NULL,
                                            PRIMARY KEY('ID' AUTOINCREMENT));";
                    _ = command.ExecuteNonQuery();
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                Logging.Log("CreateRunTable() failed || " + ex.Message, Logging.LogType.ERROR);
            }
        }

        /// <summary>
        /// Creates a new Run entry in the database
        /// </summary>
        /// <param name="run">Run to be created</param>
        /// <returns>Run with new row ID</returns>
        public static Run CreateRun(Run run)
        {
            try
            {
                using (SqliteConnection db = DatabaseFile)
                {
                    db.Open();
                    SqliteCommand command = new SqliteCommand();
                    command.Connection = db;
                    command.CommandText = "INSERT INTO runs (Lane, Winner, Dial, Start, Time_Reaction, Time_60, Time_330, Time_660, Time_990, Time_1320, Speed_660, Speed_1320, DriverID) VALUES (@lane, @winner, @dial, @start, @time_Reaction, @time_60, @time_330, @time_660, @time_990, @time_1320, @speed_660, @speed_1320, @driverID);";
                    command.CommandText += " select last_insert_rowid();";
                    command.Parameters.AddWithValue("@lane", run.Lane);
                    command.Parameters.AddWithValue("@winner", run.Winner.ToString());
                    command.Parameters.AddWithValue("@dial", run.Dial);
                    command.Parameters.AddWithValue("@start", run.Start.ToString());
                    command.Parameters.AddWithValue("@time_60", run.Time_60);
                    command.Parameters.AddWithValue("@time_330", run.Time_60);
                    command.Parameters.AddWithValue("@time_660", run.Time_60);
                    command.Parameters.AddWithValue("@time_990", run.Time_60);
                    command.Parameters.AddWithValue("@time_1320", run.Time_60);
                    command.Parameters.AddWithValue("@speed_660", run.Time_60);
                    command.Parameters.AddWithValue("@speed_1320", run.Time_60);
                    command.Parameters.AddWithValue("@driverID", run.Driver.ID);

                    SqliteDataReader query = command.ExecuteReader();

                    while (query.Read())
                    {
                        run.ID = Convert.ToInt32(query.GetValue(0));
                    }
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                Logging.Log("CreateRun() failed || " + ex.Message, Logging.LogType.ERROR);
            }
            return run;
        }

        /// <summary>
        /// Loads most recent runs from the database
        /// </summary>
        /// <returns>List of Runs</returns>
        public static List<Run> GetRecentRuns()
        {
            List<Run> runs = new List<Run>();
            try
            {
                using (SqliteConnection db = DatabaseFile)
                {
                    db.Open();
                    SqliteCommand command = new SqliteCommand();
                    command.Connection = db;
                    command.CommandText = "SELECT * FROM runs LIMIT 30";
                    SqliteDataReader query = command.ExecuteReader();

                    while (query.Read())
                    {
                        Run run = new Run(query);
                        runs.Add(run);
                    }
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                Logging.Log("GetAllRuns() failed || " + ex.Message, Logging.LogType.ERROR);
            }
            return runs;
        }
        #endregion

        #region Race Commands
        /// <summary>
        /// Creates the races table
        /// </summary>
        /// <param name="purge">Whether to purge the table from the database</param>
        internal static void CreateRaceTable(bool purge)
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
                        command.CommandText = @"DROP TABLE IF EXISTS races";
                        _ = command.ExecuteNonQuery();
                    }

                    command.CommandText =
                        @"CREATE TABLE IF NOT EXISTS 'races' (
                                            'ID'    INTEGER,
	                                        'LeftRunID' INTEGER,
	                                        'RightRunID'    INTEGER,
	                                        'WinningRunID'  INTEGER,
	                                        'CategoryID'    INTEGER,
	                                        'Round' INTEGER,
	                                        'Elimination'   TEXT,
                                            PRIMARY KEY('ID' AUTOINCREMENT));";
                    _ = command.ExecuteNonQuery();
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                Logging.Log("CreateRaceTable() failed || " + ex.Message, Logging.LogType.ERROR);
            }
        }

        /// <summary>
        /// Creates a new Race entry in the database
        /// </summary>
        /// <param name="race">Run to be created</param>
        /// <returns>Race with new row ID</returns>
        public static Race CreateRace(Race race)
        {
            try
            {
                using (SqliteConnection db = DatabaseFile)
                {
                    db.Open();
                    SqliteCommand command = new SqliteCommand();
                    command.Connection = db;
                    command.CommandText = "INSERT INTO races (LeftRunID, RightRunID, WinningRunID, CategoryID, Round, Elimination) VALUES (@leftRunID, @rightRunID, @winningRunID, @categoryID, @round, @elimination);";
                    command.CommandText += " select last_insert_rowid();";
                    command.Parameters.AddWithValue("@leftRunID", race.LeftRunID);
                    command.Parameters.AddWithValue("@rightRunID", race.RightRunID);
                    command.Parameters.AddWithValue("@winningRunID", race.WinningRunID);
                    command.Parameters.AddWithValue("@categoryID", race.Category.ID);
                    command.Parameters.AddWithValue("@round", race.Round);
                    command.Parameters.AddWithValue("@elimination", race.Elimination.ToString());

                    SqliteDataReader query = command.ExecuteReader();

                    while (query.Read())
                    {
                        race.ID = Convert.ToInt32(query.GetValue(0));
                    }
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                Logging.Log("CreateRace() failed || " + ex.Message, Logging.LogType.ERROR);
            }
            return race;
        }
        #endregion
    }
}