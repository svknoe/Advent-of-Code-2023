using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day07
{
    public class HandComparer(bool jokerEnabled) : IComparer<Hand>
    {
        private bool JokerEnabled = jokerEnabled;

        public int Compare(Hand aHand, Hand bHand)
        {
            var aStrength = GetStrength(aHand.Cards);
            var bStrength = GetStrength(bHand.Cards);

            if (aStrength > bStrength) return 1;
            if (aStrength < bStrength) return -1;

            for (var i = 0; i < aHand.Cards.Count; i++)
            {
                if (IsHigherThan(aHand.Cards[i], bHand.Cards[i]) is bool aIsStronger) return aIsStronger ? 1 : -1;
            }

            return 0;

            int GetStrength(List<char> chards)
            {
                if (IsNOrMoreOfAKind(chards, 5)) return 6; // Five of a kind
                else if (IsNOrMoreOfAKind(chards, 4)) return 5; // Four of a kind
                else if (IsHouse(chards, 3)) return 4; // Full house
                else if (IsNOrMoreOfAKind(chards, 3)) return 3; // Three of a kind
                else if (IsHouse(chards, 2)) return 2; // Two pairs
                else if (IsNOrMoreOfAKind(chards, 2)) return 1; // One pair
                else return 0; // High card
            }
        }

        private bool IsNOrMoreOfAKind(List<char> chards, int n) => chards.Any(chard => chards.Count(x => x.Equals(chard) || (JokerEnabled && x.Equals('J'))) >= n);

        private bool IsHouse(List<char> chards, int houseSize)
        {
            if (JokerEnabled && chards[0] == 'K' && chards[1] == '8' && chards[2] == '3' && chards[3] == 'J' && chards[4] == '3')
            {
                // K83J3
            }


            if (!IsNOrMoreOfAKind(chards, houseSize)) return false;

            var mostCommonChard = chards.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
            var reducedHand = chards.Where(x => !x.Equals(mostCommonChard)).ToList();

            if (JokerEnabled)
            {
                var deficiency = houseSize - chards.Count(chard => chard.Equals(mostCommonChard));

                while (deficiency > 0)
                {
                    reducedHand.Remove('J');
                    deficiency--;
                }
            }

            var isHouse = IsNOrMoreOfAKind(reducedHand, 2);
            return isHouse;
        }

        private bool? IsHigherThan(char aChard, char bChard)
        {
            var thisStrength = GetStrength(aChard);
            var otherStrength = GetStrength(bChard);

            if (thisStrength > otherStrength) return true;
            if (thisStrength < otherStrength) return false;
            return null;

            int GetStrength(char chard)
            {
                return chard switch
                {
                    'A' => 14,
                    'K' => 13,
                    'Q' => 12,
                    'J' => JokerEnabled ? 1 : 11,
                    'T' => 10,
                    '9' => 9,
                    '8' => 8,
                    '7' => 7,
                    '6' => 6,
                    '5' => 5,
                    '4' => 4,
                    '3' => 3,
                    '2' => 2,
                    _ => throw new ArgumentOutOfRangeException(nameof(chard), chard, null)
                };
            }
        }
    }
}