using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLogic
{
    public abstract class LMRRandom
    {

        public abstract int Next();

        public abstract int Next(int minValue, int maxValue);

        public abstract int Next(int maxValue);

        public abstract void NextBytes(byte[] buffer);

        public abstract double NextDouble();
    }
}
