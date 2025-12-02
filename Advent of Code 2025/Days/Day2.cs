using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2025.Days
{
    public class Day2
    {

        AdventOfCode2025Parser parser = new AdventOfCode2025Parser();


        public long Day2Part1Solver(string filename)
        {
            string input = parser.ParseInputAsSingleArrayOfStrings(filename)[0];

            var test2 = input.Split(',');

            List<(long, long)> ranges = input.Split(',').Select(e => (long.Parse(e.Split('-')[0]), long.Parse(e.Split('-')[1]))).ToList();

            long counter = 0;

            foreach (var tuple in ranges)
            {
                long rangeStart = tuple.Item1;
                long rangeEnd = tuple.Item2;

                for (long i = rangeStart; i <= rangeEnd; ++i)
                {
                    string curVal = i.ToString();

                    counter = curVal.Length % 2 == 0 && curVal.Substring(0, curVal.Length / 2) == curVal.Substring(curVal.Length / 2) ? counter + i : counter;
                }
            }

            return counter;
        }

        public long Day2Part2Solver(string filename)
        {
            string input = parser.ParseInputAsSingleArrayOfStrings(filename)[0];

            var test2 = input.Split(',');

            List<(long, long)> ranges = input.Split(',').Select(e => (long.Parse(e.Split('-')[0]), long.Parse(e.Split('-')[1]))).ToList();

            long counter = 0;

            foreach (var tuple in ranges)
            {
                long rangeStart = tuple.Item1;
                long rangeEnd = tuple.Item2;

                for (long i = rangeStart; i <= rangeEnd; ++i)
                {
                    string curVal = i.ToString();

                    List<char> curValList = curVal.ToCharArray().ToList();

                    bool foundInvalidId = false;

                    for (int curStrLength = 1; curStrLength <= curVal.Length / 2 && !foundInvalidId; ++curStrLength)
                    {
                        if (curVal.Length % curStrLength != 0)
                        {
                            continue;
                        }

                        List<char> curTestSubstr = curVal.Substring(0, curStrLength).ToCharArray().ToList();

                        bool curIdInvalid = true;

                        curValList.Index().ToList().ForEach(e => curIdInvalid = curIdInvalid && e.Item2 == curTestSubstr[e.Item1 % curStrLength]);

                        counter = curIdInvalid ? counter + i : counter;

                        foundInvalidId = curIdInvalid;
                    }
                }
            }

            return counter;
        }
    }
}
