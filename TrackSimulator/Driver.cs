using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackSimulator
{
    public class Driver
    {
        /// <summary>
        /// Database ID. An unsaved Driver has an ID of 0.
        /// </summary>
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Car_Make { get; set; }
        public string Car_Model { get; set; }
        public int Car_Year { get; set; }
        /// <summary>
        /// The alphanumeric number displayed on whatever vehicle(s) a driver is racing.
        /// One person can have multiple driver numbers to account for several vehicles, or they can 
        /// have just a personal number that they use no matter what vehicle they're in.
        /// </summary>
        public string DriverNumber { get; set; }
        /// <summary>
        /// Whether the driver is active in the system or has been deactivated.
        /// Drivers that have retired or left are deactivated rather than deleted from the DB to preserve records.
        /// </summary>
        public bool Active { get; set; }

        public Driver() { }

        public Driver(string firstName, string lastName, string driverNumber)
        {
            ID = 0;
            FirstName = firstName;
            LastName = lastName;
            DriverNumber = driverNumber;
            City = "";
            State = "";
            Car_Make = "";
            Car_Model = "";
            Car_Year = 0;
            Active = true;
        }

        public Driver(SqliteDataReader queryResult)
        {
            ID = Convert.ToInt32(queryResult.GetValue(0));
            FirstName = (string)queryResult.GetValue(1);
            LastName = (string)queryResult.GetValue(2);
            City = (string)queryResult.GetValue(3);
            State = (string)queryResult.GetValue(4);
            Car_Make = (string)queryResult.GetValue(5);
            Car_Model = (string)queryResult.GetValue(6);
            Car_Year = Convert.ToInt32(queryResult.GetValue(7));
            DriverNumber = (string)queryResult.GetValue(8);
            string activeResult = (string)queryResult.GetValue(9);
            if (activeResult == "false")
            {
                Active = false;
            }
            else
            {
                Active = true;
            }
        }

        /// <summary>
        /// Build the search terms for a driver based on FirstName, LastName, and DriverNumber
        /// </summary>
        /// <param name="includeInactives">Whether to include inactive records in the search</param>
        /// <returns>search string</returns>
        public string SearchTerms()
        {
            string searchTerm = " FirstName LIKE '%" + FirstName + "%' and ";
            searchTerm += " LastName LIKE '%" + LastName + "%' and ";
            searchTerm += " DriverNumber LIKE '%" + DriverNumber + "%'";
            return searchTerm;
        }

    }
}
