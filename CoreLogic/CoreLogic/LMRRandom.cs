using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLogic
{
    public abstract class LMRRandom
    {

        // Returns a non-negative random integer 
        public abstract int Next();

        // Returns a random integer between your specified ranges
        public abstract int Next(int minValue, int maxValue);

        // Gives you a number (not negative) and less then what you set for maxValue. 
        public abstract int Next(int maxValue);

        // Fills an array with random numbers
        public abstract void NextBytes(byte[] buffer);

        // returns a floating-point number that is equal or greater than 0.0 and less then 1.0
        public abstract double NextDouble();
    }
}
