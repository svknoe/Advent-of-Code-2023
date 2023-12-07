using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace Day05
{
    public class IntervalCollectionAction
    {
        public List<MapInterval> MapIntervals;

        public IntervalCollectionAction(List<MapLine> mapLines)
        {
            MapIntervals = new List<MapInterval>();

            foreach (var mapLine in mapLines)
            {
                var a = mapLine.SourceRangeStart;
                var b = mapLine.SourceRangeStart + mapLine.RangeLength - 1;
                var offset = mapLine.DestinationRangeStart - mapLine.SourceRangeStart;

                MapIntervals.Add(new MapInterval(a, b, offset));
            }

            var mapIntervalComplements = MapIntervals.Select(mapInterval => mapInterval.GetComplement()).ToList();
            var partialComplements = mapIntervalComplements.Select(x =>
                new IntervalCollection(x.Intervals.Select(y => MapInterval.FromInterval(y, 0)).Cast<Interval>().ToList())).ToList();

            var complement = new IntervalCollection { new Interval(long.MinValue / 10, long.MaxValue / 10) };
            complement = partialComplements.Aggregate(complement, (current, partialComplement) => current.GetIntersection(partialComplement));

            MapIntervals.AddRange(complement.Select(x => MapInterval.FromInterval(x, 0)));
            MapIntervals = MapIntervals.OrderBy(x => x.A).ToList();
        }

        public IntervalCollection Apply(IntervalCollection intervalCollection)
        {
            var destinationIntervals = new List<Interval>();

            foreach (var mapInterval in MapIntervals)
            {
                var mapIntervalCollection = new IntervalCollection(new List<Interval> { mapInterval });
                var intersections = intervalCollection.GetIntersection(mapIntervalCollection);

                var translatedIntervals = new List<Interval>();
                intersections.Intervals.ForEach(x => translatedIntervals.Add(new Interval(x.A + mapInterval.Offset, x.B + mapInterval.Offset)));

                destinationIntervals.AddRange(translatedIntervals);
            }

            //destinationIntervals = destinationIntervals.OrderBy(x => x.A).ToList();
            var transformedIntervalCollection = new IntervalCollection(destinationIntervals);
            return transformedIntervalCollection;
        }
    }

    public class MapLine(long destinationRangeStart, long sourceRangeStart, long rangeLength)
    {
        public long DestinationRangeStart = destinationRangeStart;
        public long SourceRangeStart = sourceRangeStart;
        public long RangeLength = rangeLength;
    }

    public class MapInterval(long a, long b, long offset) : Interval(a, b)
    {
        public long Offset { get; set; } = offset;

        public Interval GetMappedInterval()
        {
            return new Interval(A + Offset, B + Offset);
        }

        public static MapInterval FromInterval(Interval interval, long offset) => new(interval.A, interval.B, offset);
    }
}
