using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2025.Days
{
    public class Day7
    {

        AdventOfCode2025Parser parser = new AdventOfCode2025Parser();

        public int Day7Part1Solver(string filename)
        {
            List<List<string>> input = parser.ParseInputAsArrayOfStrings(filename);

            (int, int) startCoordinate = (input[0].IndexOf("S"), 0);

            return CountTachyonSplits(input, startCoordinate);
        }

        public long Day7Part2Solver(string filename)
        {
            List<List<string>> input = parser.ParseInputAsArrayOfStrings(filename);

            Dictionary<(int, int), long> timelinesCache = new Dictionary<(int, int), long>();

            (int, int) startCoordinate = (input[0].IndexOf("S"), 0);

            timelinesCache[(startCoordinate.Item1, startCoordinate.Item2)] = 1; 

            HashSet<(int, int)> curDepthNodes = new HashSet<(int, int)> { (startCoordinate.Item1, startCoordinate.Item2) };

            HashSet<(int, int)> nextDepthNodes = new HashSet<(int, int)>();

            for (int yPos = 0; yPos < input.Count; ++yPos)
            {
                foreach (var curNode in curDepthNodes)
                {
                    int curNodeXPos = curNode.Item1;
                    int curNodeYPos = curNode.Item2;

                    string curSymbol = input[curNodeYPos][curNodeXPos];

                    switch (curSymbol)
                    {
                        case ".":
                            input[curNodeYPos][curNodeXPos] = "|";
                            timelinesCache.Add((curNodeXPos, curNodeYPos), timelinesCache[(curNodeXPos, curNodeYPos - 1)]);
                            nextDepthNodes.Add((curNodeXPos, curNodeYPos + 1));
                            break;
                        case "|":
                            timelinesCache[(curNodeXPos, curNodeYPos)] += timelinesCache[(curNodeXPos, curNodeYPos - 1)];
                            break;
                        case "^":
                            if (timelinesCache.ContainsKey((curNodeXPos - 1, curNodeYPos)))
                            {
                                timelinesCache[(curNodeXPos - 1, curNodeYPos)] += timelinesCache[(curNodeXPos, curNodeYPos - 1)];
                            }
                            else
                            {
                                timelinesCache.Add((curNodeXPos - 1, curNodeYPos), timelinesCache[(curNodeXPos, curNodeYPos - 1)]);
                            }

                            if (timelinesCache.ContainsKey((curNodeXPos + 1, curNodeYPos)))
                            {
                                timelinesCache[(curNodeXPos + 1, curNodeYPos)] += timelinesCache[(curNodeXPos, curNodeYPos - 1)];
                            }
                            else
                            {
                                timelinesCache.Add((curNodeXPos + 1, curNodeYPos), timelinesCache[(curNodeXPos, curNodeYPos - 1)]);
                            }

                            input[curNodeYPos][curNodeXPos - 1] = "|";
                            input[curNodeYPos][curNodeXPos + 1] = "|";
                            nextDepthNodes.Add((curNodeXPos - 1, curNodeYPos + 1));
                            nextDepthNodes.Add((curNodeXPos + 1, curNodeYPos + 1));
                            break;
                        case "S":
                            nextDepthNodes.Add((curNodeXPos, curNodeYPos + 1));
                            break;
                        default:
                            return -1;
                    }
                }
                curDepthNodes = nextDepthNodes;
                nextDepthNodes = new();
            }

            long returnVal = 0;

            return timelinesCache.Where(e => e.Key.Item2 == input.Count - 1).Aggregate(returnVal, (acc, e) => acc += e.Value);
        }


        public int CountTachyonSplits(List<List<string>> input, (int, int) startCoordinate)
        {
            int xCoord = startCoordinate.Item1;
            int yCoord = startCoordinate.Item2;

            while (yCoord < input.Count && xCoord >= 0 && xCoord < input[0].Count)
            {
                string nextBlock = input[yCoord][xCoord];

                switch (nextBlock)
                {
                    case ".":
                        input[yCoord][xCoord] = "|";
                        break;
                    case "|":
                        return 0;
                    case "^":
                        return 1 + CountTachyonSplits(input, (xCoord - 1, yCoord)) + CountTachyonSplits(input, (xCoord + 1, yCoord));
                    default:
                        break;
                }

                ++yCoord;
            }

            return 0;
        }
    }
}
