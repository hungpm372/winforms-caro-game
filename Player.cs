using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caro_PhanMinhHưng
{
    public class Player
    {
        public string Name { get; set; }
        public Bitmap Mark { get; set; }
        public int PlayTime { get; set; }
        public int NumberOfWins { get; set; }
        public int NumberOfUndo { get; set; }
        public Player(string name, Bitmap mark)
        {
            Name = name;
            Mark = mark;
            PlayTime = 0;
            NumberOfWins = 0;
            NumberOfUndo = 0;
        }

        public void IncreaseNumberOfWins()
        {
            NumberOfWins++;
        }

        public void IncreaseNumberOfUndos()
        {
            NumberOfUndo++;
        }
    }
}
