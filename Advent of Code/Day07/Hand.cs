using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day07
{
    public class Hand(List<char> cards, int bid)
    {
        public List<char> Cards { get; set; } = cards;
        public int Bid { get; set; } = bid;

        public override string ToString()
        {
            var displayString = "";
            cards.ForEach(chard => displayString += chard);
            displayString += " : " + Bid;
            return displayString;
        }
    }
}
