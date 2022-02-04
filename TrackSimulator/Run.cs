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
        /// The moment that the green light was enabled after the lights dropped.
        /// </summary>
        public DateTime Start { get; set; }
        public decimal ReactionTime { get; set; }
        public decimal Time_60 { get; set; }
        public decimal Time_330 { get; set; }
        public decimal Time_660 { get; set; }
        public decimal Time_990 { get; set; }
        public decimal Time_1320 { get; set; }
        public decimal Speed_660 { get; set; }
        public decimal Speed_1320 { get; set; }
        public int CategoryID { get; set; }
        /// <summary>
        /// Used for app display only
        /// </summary>
        public string CategoryName { get; set; }
        public decimal CategoryRound { get; set; }
        /// <summary>
        /// Whether the run should be logged as an elimination pass instead of a time trial
        /// </summary>
        public bool Elimination { get; set; }
        public int DriverID { get; set; }
        /// <summary>
        /// Used for app display only
        /// </summary>
        public string DriverFullName { get; set; }
        /// <summary>
        /// Used for app display only
        /// </summary>
        public string DriverNumber { get; set; }

        public Run() { }

        public Run(string lane, int driverID, string driverFullName, string driverNumber, int categoryID, string categoryName, decimal categoryRound, bool elimination)
        {
            Lane = GetLane(lane);
            DriverID = driverID;
            DriverFullName = driverFullName;
            DriverNumber = driverNumber;
            CategoryID = categoryID;
            CategoryName = categoryName;
            CategoryRound = categoryRound;
            Elimination = elimination;
        }

        private char GetLane(string lane)
        {
            if (lane == "R" || lane == "r" || lane.ToUpper() == "RIGHT")
            {
                return 'R';
            }
            else
            {
                return 'L';
            }
        }
    }
}

