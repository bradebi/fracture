using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UTK.Math
{
    public static class Scalar
    {
        public static bool AreEqual(float value1,float value2, float delta)
        {
            return System.Math.Abs(value1 - value2) <= delta;
        }

        public static bool AreEqual(double value1, double value2, double delta)
        {
            return System.Math.Abs(value1 - value2) <= delta;
        }

    }
}
