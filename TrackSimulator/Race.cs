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
        public int Round { get; set; }
        public bool Elimination { get; set; }
        public Category Category { get; set; }

        public Race() 
        {
            Category = new Category();
        }

        public Race(int leftRun, int rightRun, int winnerRun, int categoryID, string categoryName, int round, bool elimination)
        {
            LeftRunID = leftRun;
            RightRunID = rightRun;
            WinningRunID = winnerRun;
            Category = new Category
            {
                ID = categoryID,
                Name = categoryName
            };
            Round = round;
            Elimination = elimination;
        }
    }
}
