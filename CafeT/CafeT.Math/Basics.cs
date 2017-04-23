using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord;
using Accord.Math;
using MathNet;
using MathNet.Numerics;

namespace CafeT.Math
{
    public static class Basics
    {
        /// <summary>
        /// f(x) = ax + b
        /// </summary>
        /// <param name="x"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float GetValueOfLinear(float x, float a, float b)
        {
            return a*x + b;
        }

        public static float GetPoint(int n)
        {            
            if (n >= 100 && n < 200) return 1;
            return 0;
        }
    }
}
