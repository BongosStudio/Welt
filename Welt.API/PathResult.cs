using System;
using System.Collections.Generic;

namespace Welt.API
{
    public class PathResult
    {
        public PathResult()
        {
            Index = 0;
        }

        public IList<Vector3I> Waypoints;
        public int Index;
    }
}