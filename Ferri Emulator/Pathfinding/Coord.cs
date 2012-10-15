using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferri.Kernel.Pathfinding
{
    public class Coord
    {
        public int X;
        public int Y;

        public int PositionDistance;
        public int ReversedPositionDistance;

        public Coord(int x, int y)
        {
            X = x;
            Y = y;
            PositionDistance = 1000;
            ReversedPositionDistance = 1000;
        }
    }
}
