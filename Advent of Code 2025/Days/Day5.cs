using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2025.Days
{
    public class Day5
    {

        AdventOfCode2025Parser parser = new AdventOfCode2025Parser();

        public long Day5Part1Solver(string filename)
        {
            List<string> input = parser.ParseInputAsSingleArrayOfStrings(filename);

            List<(long, long)> freshRanges = input.Where(f => f.Contains("-")).Select(e => (long.Parse(e.Split('-')[0]), long.Parse(e.Split('-')[1]))).ToList();

            List<long> ingrediants = input.Where((e, idx) => idx > input.IndexOf("")).Select(f => long.Parse(f)).ToList();

            long counter = 0;

            ingrediants.ForEach(e => counter = freshRanges.Any(f => f.Item1 <= e && f.Item2 >= e) ? counter+ 1 : counter);

            return counter;
        }

        public long Day5Part2Solver(string filename)
        {
            List<string> input = parser.ParseInputAsSingleArrayOfStrings(filename);

            List<(long, long)> freshRanges = input.Where(f => f.Contains("-")).Select(e => (long.Parse(e.Split('-')[0]), long.Parse(e.Split('-')[1]))).ToList();

            List<(long, long)> condensedRanges = new List<(long, long)>();

            long counter = 0;

            for (int i = 0; i < freshRanges.Count; ++i)
            {
                long rangeStart = freshRanges[i].Item1;
                long rangeEnd = freshRanges[i].Item2;

                bool modifiedRange = false;

                for (int j = 0; j < condensedRanges.Count; ++j)
                {
                    (long, long) curRange = condensedRanges[j];
                    if (rangeEnd >= curRange.Item1 && rangeStart <= curRange.Item2 && (rangeStart != curRange.Item1 || rangeEnd != curRange.Item2)) 
                    {
                        rangeStart = Math.Min(rangeStart, curRange.Item1);
                        rangeEnd = Math.Max(rangeEnd, curRange.Item2);
                        condensedRanges.RemoveAt(j);
                        condensedRanges.Add((rangeStart, rangeEnd));
                        j = 0;
                        modifiedRange = true;
                    }
                }

                if (!modifiedRange)
                {
                    condensedRanges.Add((rangeStart, rangeEnd));
                }
            }

            condensedRanges = condensedRanges.ToHashSet().ToList();

            condensedRanges.ForEach(e => counter += e.Item2 - e.Item1 + 1);

            return counter;
        }
    }
}
