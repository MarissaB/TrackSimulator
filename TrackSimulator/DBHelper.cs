using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// <returns>List of corresponding drivers</returns>
        public static List<Driver> SearchDrivers(Driver driver, bool includeInactives)
        {
            List<Driver> drivers = new List<Driver>();
            try
            {
                using (SqliteConnection db = DatabaseFile)
                {
                    db.Open();
                    SqliteCommand command = new SqliteCommand();
                    command.Connection = db;
                    command.CommandText = "SELECT * FROM drivers WHERE " + driver.SearchTerms(includeInactives);
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

        #endregion
    }
}