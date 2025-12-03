using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2025.Days
{
    public class Day3
    {

        AdventOfCode2025Parser parser = new AdventOfCode2025Parser();


        public int Day3Part1Solver(string filename)
        {
            List<List<int>> input = parser.ParseInputAs2DArrayOfInts(filename);

            int finalSum = 0;

            foreach (var bank in input)
            {
                List<int> currentBank = bank.Where((e, idx) => idx != bank.Count - 1).ToList();

                int max = currentBank.Max();

                int maxIdx = currentBank.IndexOf(max);

                List<int> remainingDigits = bank.Where((e, idx) => idx > maxIdx).ToList();

                int remainingMax = remainingDigits.Max();

                finalSum += int.Parse($"{max}{remainingMax}");
            }

            return finalSum;
        }

        public long Day3Part2Solver(string filename)
        {
            List<List<int>> input = parser.ParseInputAs2DArrayOfInts(filename);

            long finalSum = 0;

            foreach (var bank in input)
            {
                finalSum += FindCurMaxJoltage(12, bank, "");
            }

            return finalSum;
        }


        private long FindCurMaxJoltage(int curIteration, List<int> curBank, string curJoltageString)
        {
            if (curIteration == 0)
            {
                return long.Parse(curJoltageString);
            }

            List<int> curConsiderationBank = curBank.Where((e, idx) => idx <= curBank.Count - curIteration).ToList();

            int curMax = curConsiderationBank.Max();

            int curMaxIdx = curConsiderationBank.IndexOf(curMax);

            List<int> remainingInts = curBank.Where((e, idx) => idx > curMaxIdx).ToList();

            curJoltageString = $"{curJoltageString}{curMax}";

            return FindCurMaxJoltage(curIteration - 1, remainingInts, curJoltageString);
        }
    }
}
