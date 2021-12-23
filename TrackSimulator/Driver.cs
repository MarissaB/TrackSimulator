using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackSimulator
{
    public class Driver
    {
        public int ID;
        public string FirstName;
        public string LastName;
        public string City;
        public string State;
        public string Car_Make;
        public string Car_Model;
        public int Car_Year;
        public string RaceNumber;

        public Driver()
        {

        }

        public Driver(string firstName, string lastName, string raceNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            RaceNumber = raceNumber;
            City = "";
            State = "";
            Car_Make = "";
            Car_Model = "";
            Car_Year = 9999;
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
            RaceNumber = (string)queryResult.GetValue(8);
        }

        public string FullName()
        {
            return FirstName + " " + LastName;
        }

        public string SearchTerms()
        {
            string searchTerm = " FirstName LIKE '%" + FirstName + "%' and ";
            searchTerm += " LastName LIKE '%" + LastName + "%' and ";
            searchTerm += " RaceNumber LIKE '%" + RaceNumber + "%'";
            return searchTerm;
        }

    }
}
