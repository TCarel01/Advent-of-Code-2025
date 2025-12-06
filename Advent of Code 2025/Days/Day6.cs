using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2025.Days
{
    public class Day6
    {

        AdventOfCode2025Parser parser = new AdventOfCode2025Parser();

        public long Day6Part1Solver(string filename)
        {
            List<string> input = parser.ParseInputAsSingleArrayOfStrings(filename);

            List<List<long>> inputNums = OrganizeInput(input);

            List<string> symbols = input[input.Count - 1].ToCharArray().Where(e => e == '+' || e == '*').Select(f => f.ToString()).ToList();

            long totalSum = 0;

            for (int i = 0; i < symbols.Count; ++i)
            {
                string curSymbol = symbols[i];

                long startingVal = curSymbol == "+" ? 0 : 1;

                for (int j = 0; j < inputNums.Count; ++j)
                {
                    startingVal = curSymbol == "+" ? startingVal + inputNums[j][i] : startingVal * inputNums[j][i];
                }
                totalSum += startingVal;
            }

            return totalSum;
        }

        public long Day6Part2Solver(string filename)
        {
            List<List<string>> input = parser.ParseInputAsArrayOfStrings(filename);

            List<string> symbols = input[input.Count - 1].Where(e => e == "+" || e == "*").Select(f => f.ToString()).ToList();

            symbols.Reverse();

            long runningTotal = 0;

            int curIdx = input[0].Count() - 1;

            foreach (var symbol in symbols)
            {
                List<long> parsedLongs = new List<long>();

                while (true)
                {
                    string curLongToAdd = "";

                    for (int i = 0; i < input.Count - 1; ++i)
                    {
                        curLongToAdd = int.TryParse(input[i][curIdx], out int result) ? $"{curLongToAdd}{input[i][curIdx]}" : curLongToAdd;
                    }

                    parsedLongs.Add(long.Parse(curLongToAdd));

                    if (input[input.Count - 1][--curIdx + 1] == symbol)
                    {
                        --curIdx;
                        break;
                    }
                }

                long startingAccumulatorValue = symbol == "+" ? 0 : 1;

                runningTotal += parsedLongs.Aggregate(startingAccumulatorValue, (e, acc) => symbol == "+" ? acc + e : acc * e);
            }

            return runningTotal;
        }


        public List<List<long>> OrganizeInput(List<string> input)
        {
            List<List<long>> convertedInput = new List<List<long>>();

            for (int i = 0; i < input.Count - 1; ++i)
            {
                List<long> curLine = new List<long>();

                List<string> curEntry = input[i].Split(' ').ToList();

                long result;

                curLine = curEntry.Where(e => long.TryParse(e, out result)).Select(e => long.Parse(e)).ToList();

                convertedInput.Add(curLine);
            }

            return convertedInput;
        }
    }
}
