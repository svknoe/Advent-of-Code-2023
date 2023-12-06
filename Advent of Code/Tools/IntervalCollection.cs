using System.Collections;

namespace Tools
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    /// <summary>
    /// An IntervalCollection is a list of Intervals that automatically merges overlapping Intervals and stays ordered.
    /// </summary>
    public class IntervalCollection : IEnumerable<Interval>
    {
        public List<Interval> Intervals { get; private set; } = new();

        /// <summary>
        /// Length of IntervalCollection.
        /// This length is the sum of the lengths of all constituent Intervals.
        /// </summary>
        public long Length => Intervals.Select(x => x.Length).Sum();

        public IntervalCollection() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intervals"></param>
        public IntervalCollection(List<Interval> intervals)
        {
            var clonedIntervals = intervals.Select(x => x.Clone()).ToList();
            Add(clonedIntervals);
        }

        /// <summary>
        /// Checks whether this IntervalCollection contains a value x.
        /// It contains x if an interval contains x.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public bool Contains(long x)
        {
            var contains = Intervals.Any(interval => interval.Contains(x));
            return contains;
        }

        /// <summary>
        /// Returns the interval containing x or null if no interval does.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public Interval? GetIntervalContainingValue(long x)
        {
            var intervalContainingValue = Intervals.FirstOrDefault(interval => interval.Contains(x));
            return intervalContainingValue;
        }

        /// <summary>
        /// Add another Interval to the IntervalCollection.
        /// </summary>
        /// <param name="interval"></param>
        public void Add(Interval interval)
        {
            Intervals.Add(interval);
            Order();
        }

        /// <summary>
        /// Add more Intervals to the IntervalCollection.
        /// </summary>
        /// <param name="intervals"></param>
        public void Add(List<Interval> intervals)
        {
            Intervals.AddRange(intervals);
            Order();
        }

        /// <summary>
        /// If IntervalCollection contains specified interval then remove that interval.
        /// </summary>
        /// <param name="interval"></param>
        public void Remove(Interval interval)
        {
            Intervals.Remove(interval);
        }

        /// <summary>
        /// Removes the interval at the specified index.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            Intervals.RemoveAt(index);
        }

        public IEnumerator<Interval> GetEnumerator()
        {
            return Intervals.GetEnumerator();
        }

        /// <summary>
        /// Returns true if other IntervalCollection has the same number of Intervals and the intervals are pairwise equal.
        /// Does not account for differences in Tolerance.
        /// </summary>
        /// <param name="otherObject"></param>
        /// <returns></returns>
        public override bool Equals(object? otherObject)
        {
            if (otherObject is not IntervalCollection other) return false;
            if (Intervals.Count != other.Intervals.Count) return false;

            for (var i = 0; i < Intervals.Count; i++)
            {
                if (!Intervals[i].Equals(other.Intervals[i])) return false;
            }

            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Checks whether this IntervalCollection overlaps an Interval.
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public bool Overlaps(Interval interval)
        {
            var overlaps = Intervals.Any(interval.Overlaps);
            return overlaps;
        }

        /// <summary>
        /// Checks whether this IntervalCollection overlaps another.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Overlaps(IntervalCollection other)
        {
            var overlaps = Intervals.Any(other.Overlaps);
            return overlaps;
        }

        /// <summary>
        /// Returns the distance from this IntervalCollection to the given value.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public long DistanceTo(long x)
        {
            if (Contains(x)) return 0;

            var distance = Intervals.Select(subInterval => subInterval.DistanceTo(x)).Min();
            return distance;
        }

        /// <summary>
        /// Returns the distance from this IntervalCollection to the given Interval.
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public long DistanceTo(Interval interval)
        {
            if (Overlaps(interval)) return 0;

            var distance = Intervals.Select(interval.DistanceTo).Min();
            return distance;
        }

        /// <summary>
        /// Returns the distance from this IntervalCollection to the given Interval.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public long DistanceTo(IntervalCollection other)
        {
            if (Overlaps(other)) return 0;

            var distance = Intervals.Select(other.DistanceTo).Min();
            return distance;
        }

        /// <summary>
        /// Returns the union of this IntervalCollection and another.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public IntervalCollection GetUnion(IntervalCollection other)
        {
            var intervals = Intervals;
            intervals.AddRange(other.Intervals);
            var union = new IntervalCollection(intervals);
            return union;
        }

        /// <summary>
        /// Returns the intersection of this IntervalCollection and another.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public IntervalCollection GetIntersection(IntervalCollection other)
        {
            var intervalIntersections = new List<Interval>();

            foreach (var interval in Intervals)
            {
                foreach (var otherInterval in other.Intervals)
                {
                    var intervalIntersection = interval.GetIntersection(otherInterval);
                    if (intervalIntersection?.Length >= 0) intervalIntersections.Add(intervalIntersection);
                }
            }

            var intersection = new IntervalCollection(intervalIntersections);
            return intersection;
        }

        /// <summary>
        /// Removes all values contained in interval from this IntervalCollection.
        /// </summary>
        /// <param name="interval"></param>
        public void Subtract(Interval interval)
        {
            var complement = interval.GetComplement();
            var intersection = GetIntersection(complement);
            Intervals = intersection.Intervals;
            Order();
        }

        /// <summary>
        /// Removes all values contained in intervalCollection from this IntervalCollection.
        /// </summary>
        /// <param name="intervalCollection"></param>
        public void Subtract(IntervalCollection intervalCollection)
        {
            intervalCollection.Intervals.ForEach(Subtract);
        }

        /// <summary>
        /// Converts this AlignmentCollection to standard form and orders intervals.
        /// </summary>
        private void Order()
        {
            ConvertToStandardForm();
            Intervals = Intervals.OrderBy(x => x.A).ToList();
        }

        /// <summary>
        /// Returns true if this AlignmentCollection has no overlapping Intervals.
        /// </summary>
        /// <returns></returns>
        private bool IsOnStandardForm()
        {
            for (var i = 0; i < Intervals.Count - 1; i++)
            {
                var interval = Intervals[i];

                for (var j = i + 1; j < Intervals.Count; j++)
                {
                    if (Intervals[j].Contains(interval.A) || Intervals[j].Contains(interval.B)) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Replaces Intervals contained in this IntervalCollection with a new list of Intervals that contains the same numbers, but have no overlaps.
        /// </summary>
        private void ConvertToStandardForm()
        {
            if (IsOnStandardForm()) return;
            var unProcessedIntervals = Intervals.ToList();
            var nonOverlappingIntervals = new List<Interval>();

            var infiniteLoopDetectionCounter = 0;
            const long infiniteLoopDetectionThreshold = (int)1e4;
            while (unProcessedIntervals.Count > 0 && infiniteLoopDetectionCounter < infiniteLoopDetectionThreshold)
            {
                var intervalsMerged = false;
                infiniteLoopDetectionCounter++;

                var interval = unProcessedIntervals.First();
                unProcessedIntervals.RemoveAt(0);

                var processedIntervals = new List<Interval>();

                foreach (var otherInterval in unProcessedIntervals)
                {
                    if (!interval.Overlaps(otherInterval)) continue;

                    interval = interval.GetUnion(otherInterval, true).Intervals.First();
                    processedIntervals.Add(otherInterval);
                    intervalsMerged = true;
                }

                foreach (var processedInterval in processedIntervals)
                {
                    unProcessedIntervals.Remove(processedInterval);
                }

                if (intervalsMerged)
                {
                    unProcessedIntervals.Insert(0, interval);
                }
                else
                {
                    nonOverlappingIntervals.Add(interval);
                }
            }

            if (infiniteLoopDetectionCounter.Equals(infiniteLoopDetectionThreshold))
            {
                throw new($"{nameof(IntervalCollection)}.{nameof(ConvertToStandardForm)}: Infinite loop detected.");
            }

            Intervals = nonOverlappingIntervals;

            if (!IsOnStandardForm()) throw new($"{nameof(IntervalCollection)}.{nameof(ConvertToStandardForm)} failed.");
        }
    }
}