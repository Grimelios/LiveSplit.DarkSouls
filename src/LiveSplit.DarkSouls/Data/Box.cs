using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSoulsMemory;

namespace LiveSplit.DarkSouls.Data
{
    public class Box
    {
        public Box(Vector3f lowerBound, Vector3f upperBound)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
        }

        public Vector3f LowerBound { get; set; }
        public Vector3f UpperBound { get; set; }
    }
}
