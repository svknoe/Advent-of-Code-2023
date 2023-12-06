using System.Drawing;

namespace Tools
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Interval
    {
        private int _a;

        /// <summary>
        /// Value denoting the lower bound of the interval.
        /// </summary>
        public int A
        {
            get => _a;
            set
            {
                if (B == default || value <= B) _a = value;
                else
                {
                    _a = B;
                    _b = value;
                }
            }
        }

        private int _b;

        /// <summary>
        /// Value denoting the upper bound of the interval.
        /// </summary>
        public int B
        {
            get => _b;
            set
            {
                if (value >= A) _b = value;
                else
                {
                    _b = A;
                    _a = value;
                }
            }
        }

        /// <summary>
        /// Length of the interval.
        /// </summary>
        public int Length => B - A;

        public Interval()
        {
            Initialize(0, 0);
        }

        /// <summary>
        /// An interval from a to b.
        /// Interval is reversed if a > b.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Interval(int a, int b)
        {
            Initialize(a, b);
        }

        /// <summary>
        /// An interval from point.X to point.Y.
        /// Interval is reversed if point.X > point.Y.
        /// </summary>
        /// <param name="point"></param>
        public Interval(Point point)
        {
            Initialize(point.X, point.Y);
        }

        private void Initialize(int a, int b)
        {
            if (a < b)
            {
                _a = a;
                _b = b;
            }
            else
            {
                _a = b;
                _b = a;
            }
        }

        /// <summary>
        /// Checks whether this Interval contains x.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public bool Contains(int x)
        {
            var contains = A <= x && x <= B;
            return contains;
        }

        /// <summary>
        /// Checks whether this Interval contains other interval.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Contains(Interval other)
        {
            return Contains(other.A) && Contains(other.B);
        }

        /// <summary>
        /// Checks whether this Interval overlaps another.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Overlaps(Interval other)
        {
            if (Contains(other.A)) return true;
            if (Contains(other.B)) return true;
            if (other.Contains(A)) return true;
            if (other.Contains(B)) return true;
            return false;
        }

        /// <summary>
        /// Returns the distance from this interval to the given value.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public int DistanceTo(int x)
        {
            if (Contains(x)) return 0;

            var distance = Math.Min(Math.Abs(A - x), Math.Abs(A - x));
            return distance;
        }

        /// <summary>
        /// Returns the distance from this interval to the given Interval.
        /// </summary>
        /// <param name="otherInterval"></param>
        /// <returns></returns>
        public int DistanceTo(Interval otherInterval)
        {
            if (Overlaps(otherInterval)) return 0;

            var distance = Math.Min(DistanceTo(otherInterval.A), DistanceTo(otherInterval.B));
            return distance;
        }

        /// <summary>
        /// Returns the union of this Interval and another.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="requireOverlap">If this is false then the Interval spanning both Intervals is returned.</param>
        /// <returns></returns>
        public IntervalCollection? GetUnion(Interval other, bool requireOverlap = false)
        {
            if (!Overlaps(other))
            {
                return (requireOverlap ? null : new IntervalCollection(new() { this, other }));
            }

            var interval = new Interval(Math.Min(A, other.A), Math.Max(B, other.B));
            var union = new IntervalCollection(new() { interval });
            return union;
        }

        /// <summary>
        /// Returns the intersection of this Interval and another.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Interval? GetIntersection(Interval other)
        {
            if (!Overlaps(other)) return null;
            var maxA = Math.Max(A, other.A);
            var minB = Math.Min(B, other.B);
            var intersection = new Interval(maxA, minB);
            var intersectionOrNull = intersection.A <= intersection.B ? intersection : null;
            return intersectionOrNull;
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is not Interval other) return false;
            var areEqual = A == other.A && B == other.B;
            return areEqual;
        }

        /// <summary>
        /// Returns the Interval that spans both x and this Interval.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public Interval ExtendTo(int x)
        {
            var extendedInterval = new Interval(Math.Min(A, x), Math.Max(B, x));
            return extendedInterval;
        }

        /// <summary>
        /// Returns a copy of this Interval.
        /// </summary>
        /// <returns></returns>
        public Interval Clone()
        {
            return new(A, B);
        }

        /// <summary>
        /// Returns an IntervalCollection whose intervals contain exactly those values that are contained in this Interval, but no tin otherInterval.
        /// </summary>
        /// <param name="intervalToSubtract"></param>
        /// <returns></returns>
        public IntervalCollection GetSubtract(Interval intervalToSubtract)
        {
            var intervalCollection = new IntervalCollection(new() { this });
            intervalCollection.Subtract(intervalToSubtract);
            return intervalCollection;
        }

        /// <summary>
        /// Returns the an IntervalCollection containing exactly those values that this interval does not contain.
        /// </summary>
        /// <returns></returns>
        public IntervalCollection GetComplement()
        {
            var complementIntervals = new List<Interval>();

            if (int.MinValue < A) complementIntervals.Add(new(int.MinValue, A));
            if (B < int.MaxValue) complementIntervals.Add(new(B, int.MaxValue));
            var complement = new IntervalCollection(complementIntervals);
            return complement;
        }
    }
}