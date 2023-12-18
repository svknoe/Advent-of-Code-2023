using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    public class Group
    {
        public List<char> Springs { get; set; } = new();
        public List<int> DamagedGroupSizes { get; set; } = new();

        public override string ToString()
        {
            var asString = "";
            Springs.ForEach(x => asString += x);

            return asString;
        }

        public int GetArrangementCount()
        {

        }
    }
}
