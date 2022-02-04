using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackSimulator
{
    public class Race
    {
        public int ID { get; set; }
        public int LeftRunID { get; set; }
        public int RightRunID { get; set; }
        public int WinningRunID { get; set; }
        public int CategoryID { get; set; }
        /// <summary>
        /// For app display only
        /// </summary>
        public string CategoryName { get; set; }
        public int CategoryRound { get; set; }
        public bool Elimination { get; set; }

        public Race() { }

        public Race(int leftRun, int rightRun, int winnerRun, int categoryID, string categoryName, int round, bool elimination)
        {
            LeftRunID = leftRun;
            RightRunID = rightRun;
            WinningRunID = winnerRun;
            CategoryID = categoryID;
            CategoryName = categoryName;
            CategoryRound = round;
            Elimination = elimination;
        }
    }
}
