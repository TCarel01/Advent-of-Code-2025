using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2025.Days
{
    public class Day1
    {
        AdventOfCode2025Parser parser = new AdventOfCode2025Parser();



        public int Day1Part1Solver(string filename)
        {
            List<string> input = parser.ParseInputAsSingleArrayOfStrings(filename);

            List<int> rotationVals = input.Select(e => int.Parse(e.Substring(1))).ToList();

            List<string> directions = input.Select(e => e.Substring(0, 1)).ToList();

            int curRotation = 50;
            int runningCount = 0;

            for (int i = 0; i < rotationVals.Count; ++i)
            {
                int parity = directions[i] == "R" ? 1 : -1;

                curRotation = (curRotation + parity * rotationVals[i]) % 100;

                curRotation = curRotation < 0 ? 100 + curRotation : curRotation;

                runningCount += curRotation == 0 ? 1 : 0;
            }

            return runningCount;
        }

        public int Day1Part2Solver(string filename)
        {
            List<string> input = parser.ParseInputAsSingleArrayOfStrings(filename);

            List<int> rotationVals = input.Select(e => int.Parse(e.Substring(1))).ToList();

            List<string> directions = input.Select(e => e.Substring(0, 1)).ToList();

            int curRotation = 50;
            int runningCount = 0;
            bool prevZero = false;

            for (int i = 0; i < rotationVals.Count; ++i)
            {
                int parity = directions[i] == "R" ? 1 : -1;

                curRotation += parity * rotationVals[i];

                if (curRotation <= 0 || curRotation >= 100)
                {
                    int curIncreaseCount = curRotation > 0 || curRotation <= 0 && prevZero? Math.Abs(curRotation / 100) : Math.Abs(curRotation / 100) + 1;

                    runningCount += curIncreaseCount;

                    curRotation = (100 + curRotation % 100) % 100;
                }
               prevZero = curRotation == 0;
            }
            return runningCount;
        }
    }
}
