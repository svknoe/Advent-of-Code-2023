using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public static class MathEx
    {
        public static bool EqualDoubles(double a, double b, double tol = 1e-9)
        {
            return (a - tol <= b && b <= a + tol)
                   || (double.IsPositiveInfinity(a) && double.IsPositiveInfinity(b))
                   || (double.IsNegativeInfinity(a) && double.IsNegativeInfinity(b))
                   || (double.IsNaN(a) && double.IsNaN(b));
        }

        public static bool EqualDoubles(double? a, double? b, double tol = 1e-9)
        {
            if (a is double doubleA && b is double doubleB) return EqualDoubles(doubleA, doubleB, tol);
            if (a is null && b is null) return true;
            return false;
        }
    }
}
