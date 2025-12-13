using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2025.Days
{
    public class Day12
    {

        AdventOfCode2025Parser parser = new AdventOfCode2025Parser();

        public int Day12Solver(string filename)
        {
            List<string> input = parser.ParseInputAsSingleArrayOfStrings(filename);

            List<int> areaCounter = new List<int>();

            int curAreaCounter = 0;

            for (int i = 0; i < 31; ++i)
            {
                curAreaCounter += input[i].ToCharArray().Select(f => f.ToString()).Aggregate(0, (acc, e) => e == "#" ? acc + 1 : acc);
                if (i % 5 == 0 && i != 0)
                {
                    areaCounter.Add(curAreaCounter);
                    curAreaCounter = 0;
                }
            }

            int counter = 0;

            for (int i = 30; i < input.Count; ++i)
            {
                bool checkLine = CheckLine(input[i], areaCounter);

                counter = checkLine ? counter + 1 : counter;
            }

            return counter;
        }

        public bool CheckLine(string line, List<int> areaCounter)
        {
            int firstDim = int.Parse(line.Substring(0, line.IndexOf("x")));

            int secondDim = int.Parse(line.Substring(line.IndexOf("x") + 1, line.IndexOf(":") - 3));

            string test = line.Substring(line.IndexOf(' ') + 1);

            List<int> numInputs = test.Split(' ').Select(e => int.Parse(e)).ToList();

            int totalArea = firstDim * secondDim;

            int maxTotal = 0;
            int totalMinBound = 0;

            for (int i = 0; i < numInputs.Count; ++i)
            {
                maxTotal += 9 * numInputs[i];

                totalMinBound += numInputs[i] * areaCounter[i];
            }

            return totalMinBound < totalArea;
        }
    }
}
