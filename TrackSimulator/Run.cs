using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackSimulator
{
    /// <summary>
    /// A timed pass down the track in the left or right lane.
    /// </summary>
    public class Run
    {
        public int ID { get; set; }
        /// <summary>
        /// L for left lane, R for right lane
        /// </summary>
        public char Lane { get; set; }
        public bool Winner { get; set; }
        public decimal Dial { get; set; }
        /// <summary>
        /// The moment that the green light came on to signal the start of the run.
        /// </summary>
        public DateTime Start { get; set; }
        /// <summary>
        /// The seconds between the Start and driver crossing the start line (reacting).
        /// </summary>
        public decimal Time_Reaction { get; set; }
        /// <summary>
        /// Seconds between the ReactionTime and driver crossing the 60ft line.
        /// </summary>
        public decimal Time_60 { get; set; }
        /// <summary>
        /// Seconds between the ReactionTime and driver crossing the 330ft line.
        /// </summary>
        public decimal Time_330 { get; set; }
        /// <summary>
        /// Seconds between the ReactionTime and driver crossing the 660ft line.
        /// </summary>
        public decimal Time_660 { get; set; }
        /// <summary>
        /// Seconds between the ReactionTime and driver crossing the 990ft line.
        /// </summary>
        public decimal Time_990 { get; set; }
        /// <summary>
        /// Seconds between the ReactionTime and driver crossing the 1320ft line.
        /// </summary>
        public decimal Time_1320 { get; set; }
        public int Speed_660 { get; set; }
        public int Speed_1320 { get; set; }
        public Race Race { get; set; }
        public Driver Driver { get; set; }

        public Run()
        {
            SetEmptyRun();
            Driver = new Driver();
            Race = new Race();
        }

        public Run(string lane, int driverID, string driverFirst, string driverLast, string driverNumber, int categoryID, string categoryName, int round, bool elimination)
        {
            SetLane(lane);
            SetEmptyRun();
            Driver = new Driver(driverFirst, driverLast, driverNumber)
            {
                ID = driverID
            };
            Category category = new Category
            {
                ID = categoryID,
                Name = categoryName
            };
            Race = new Race
            {
                Category = category,
                Round = round,
                Elimination = elimination
            };
        }

        /// <summary>
        /// Constructor using SqliteDataReader result
        /// </summary>
        /// <param name="queryResult"></param>
        public Run(SqliteDataReader queryResult)
        {
            ID = Convert.ToInt32(queryResult.GetValue(0));
            Lane = (char)queryResult.GetValue(1);
            string winnerResult = (string)queryResult.GetValue(2);
            if (winnerResult == "true")
            {
                Winner = true;
            }
            else { Winner = false; }

            Dial = (decimal)queryResult.GetValue(3);
            // TODO: Finish parsing Run from queryResult
        }

        /// <summary>
        /// Zeroes out all sensor values for the Run.
        /// </summary>
        public void SetEmptyRun()
        {
            Time_1320 = 0;
            Time_990 = 0;
            Time_660 = 0;
            Time_330 = 0;
            Time_60 = 0;

            Speed_1320 = 0;
            Speed_660 = 0;
        }

        /// <summary>
        /// Lane defaults to 'right' because it's the primary track lane for single passes.
        /// </summary>
        /// <param name="lane">'Left' or 'right' lane</param>
        /// <returns></returns>
        private void SetLane(string lane)
        {
            if (lane == "L" || lane == "l" || lane.ToUpper() == "LEFT")
            {
                Lane = 'L';
            }
            else
            { 
                Lane = 'R';
            }
        }

        /// <summary>
        /// Sets the recorded times (in seconds) for each value
        /// </summary>
        /// <param name="start"></param>
        /// <param name="sensor_reaction"></param>
        /// <param name="sensor_60"></param>
        /// <param name="sensor_330"></param>
        /// <param name="sensor_660"></param>
        /// <param name="sensor_990"></param>
        /// <param name="sensor_1320"></param>
        public void SetTimes(DateTime start, DateTime sensor_reaction, DateTime sensor_60, DateTime sensor_330, DateTime sensor_660, DateTime sensor_990, DateTime sensor_1320)
        {
            Start = start;
            Time_Reaction = CalculateTimeInSeconds(start, sensor_reaction);

            // Times are set from the moment of reaction (leaving the starting line) to the moment crossing the distance sensor.
            Time_60 = CalculateTimeInSeconds(sensor_reaction, sensor_60);
            Time_330 = CalculateTimeInSeconds(sensor_reaction, sensor_330);
            Time_660 = CalculateTimeInSeconds(sensor_reaction, sensor_660);
            Time_990 = CalculateTimeInSeconds(sensor_reaction, sensor_990);
            Time_1320 = CalculateTimeInSeconds(sensor_reaction, sensor_1320);
        }

        /// <summary>
        /// Calculates the time in seconds it took the vehicle to reach the sensor after the reaction time.
        /// </summary>
        /// <param name="reaction">Timestamp of driver reaction</param>
        /// <param name="sensor">Timestamp the driver passed a sensor</param>
        /// <returns>Seconds it took to reach the sensor. Returns 0 if sensor was not reached.</returns>
        public decimal CalculateTimeInSeconds(DateTime reaction, DateTime sensor)
        {
            decimal result = 0;
            if (sensor != null && sensor > reaction)
            {
                result = (decimal)(sensor - reaction).TotalSeconds;
            }

            return result;
        }

        
    }
}

