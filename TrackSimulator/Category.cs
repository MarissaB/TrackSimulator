using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackSimulator
{
    public class Category
    {
        /// <summary>
        /// Database ID. An unsaved Category has an ID of 0.
        /// </summary>
        public int ID { get; set; }
        public string Name { get; set; }
        public int Length { get; set; }
        public string Qualifying { get; set; }
        public string Light { get; set; }
        public string Ladder { get; set; }
        /// <summary>
        /// Whether the category is active in the system or has been deactivated.
        /// Categories are retired/decommissioned rather than deleted from the DB to preserve records.
        /// </summary>
        public bool Active { get; set; }

        public Category() { }

        public Category(string name, int length, string qualifying, string light, string ladder)
        {
            ID = 0;
            Name = name;
            Length = length;
            Qualifying = qualifying;
            Light = light;
            Ladder = ladder;
            Active = true;
        }

        public Category(SqliteDataReader queryResult)
        {
            ID = Convert.ToInt32(queryResult.GetValue(0));
            Name = (string)queryResult.GetValue(1);
            Length = Convert.ToInt32(queryResult.GetValue(2));
            Qualifying = (string)queryResult.GetValue(3);
            Light = (string)queryResult.GetValue(4);
            Ladder = (string)queryResult.GetValue(5);
            string activeResult = (string)queryResult.GetValue(6);
            if (activeResult == "false")
            {
                Active = false;
            }
            else
            {
                Active = true;
            }
        }

    }
}
