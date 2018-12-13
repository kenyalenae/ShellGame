﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLogic
{
    //NetRandom inherits for \/ (LMRRandom) 
    public class NetRandom : LMRRandom
    {
        // These are used for random placement of the "pea" in the boxes, it what we will call when checking if the box contians something.
        private Random rand;

        public NetRandom(int seed)
        {
            rand = new Random(seed);
        }

        public NetRandom()
        {
            rand = new Random();
        }

        public override int Next()
        {
            return rand.Next();
        }

        public override int Next(int minValue, int maxValue)
        {
            return rand.Next(minValue, maxValue);
        }

        public override int Next(int maxValue)
        {
            return rand.Next(maxValue);
        }

        public override void NextBytes(byte[] buffer)
        {
            rand.NextBytes(buffer);
        }

        public override double NextDouble()
        {
            return rand.NextDouble();
        }
    }
}
