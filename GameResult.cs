using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caro_PhanMinhHưng
{
    public class GameResult
    {
        public DateTime StartTime { get; set; }
        public string Player1Name{ get; set; }
        public int Player1Time { get; set; }
        public int Player1Undo { get; set; }
        public string Player2Name { get; set; }
        public int Player2Time { get; set; }
        public int Player2Undo { get; set; }
        public int Winner { get; set; }

        public GameResult(DateTime startTime, string player1Name, int player1Time, int player1Undo, string player2Name, int player2Time, int player2Undo, int winner)
        {
            StartTime = startTime;
            Player1Name = player1Name;
            Player1Time = player1Time;
            Player1Undo = player1Undo;
            Player2Name = player2Name;
            Player2Time = player2Time;
            Player2Undo = player2Undo;
            Winner = winner;
        }
    }
}
