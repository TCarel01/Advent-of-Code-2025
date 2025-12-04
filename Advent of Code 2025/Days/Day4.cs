using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2025.Days
{
    public class Day4
    {

        AdventOfCode2025Parser parser = new AdventOfCode2025Parser();


        public int Day4Part1Solver(string filename)
        {
            List<List<string>> input = parser.ParseInputAsArrayOfStrings(filename);

            return GetRemovableRolls(input).Count;
        }

        public int Day4Part2Solver(string filename)
        {
            List<List<string>> input = parser.ParseInputAsArrayOfStrings(filename);

            int count = 0;

            bool checkNext = true;

            while (checkNext)
            {
                List<(int, int)> curCoords = GetRemovableRolls(input);

                checkNext = curCoords.Count > 0;

                curCoords.ForEach(e => input[e.Item2][e.Item1] = ".");

                count += curCoords.Count;
            }

            return count;
        }


        public List<(int, int)> GetRemovableRolls(List<List<string>> input)
        {
            List<(int, int)> coords = new List<(int, int)>();

            for (int y = 0; y < input.Count; ++y)
            {
                for (int x = 0; x < input[0].Count; ++x)
                {
                    if (input[y][x] != "@")
                    {
                        continue;
                    }

                    int curChar = 0;

                    int minY = Math.Max(0, y - 1);
                    int maxY = Math.Min(input.Count - 1, y + 1);

                    int minX = Math.Max(0, x - 1);
                    int maxX = Math.Min(input[0].Count - 1, x + 1);

                    for (int curY = minY; curY <= maxY; ++curY)
                    {
                        for (int curX = minX; curX <= maxX; ++curX)
                        {
                            curChar = input[curY][curX] == "@" ? curChar + 1 : curChar;
                        }
                    }

                    if (curChar < 5)
                    {
                        coords.Add((x, y));
                    }
                }
            }

            return coords;
        }
    }
}
