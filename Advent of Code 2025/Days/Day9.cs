using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2025.Days
{
    public class Day9
    {

        AdventOfCode2025Parser parser = new AdventOfCode2025Parser();

        public long Day9Part1Solver(string filename)
        {
            List<string> input = parser.ParseInputAsSingleArrayOfStrings(filename);

            List<List<int>> coords = input.Select(e => e.Split(',').Select(f => int.Parse(f)).ToList()).ToList();

            long maxArea = 0;

            foreach (var coord1 in coords)
            {
                foreach (var coord2 in coords)
                {
                    int width = Math.Abs(coord1[1] - coord2[1]) + 1;
                    int height = Math.Abs(coord1[0] - coord2[0]) + 1;

                    maxArea = Math.Max((long)width * height, maxArea);
                }
            }
            return maxArea;
        }

        public long Day9Part2Solver(string filename)
        {
            List<string> input = parser.ParseInputAsSingleArrayOfStrings(filename);

            List<List<int>> coords = input.Select(e => e.Split(',').Select(f => int.Parse(f)).ToList()).ToList();

            foreach (var coord in coords)
            {
                coord.Reverse();
            }

            var perimeterDict = GetPerimeter(coords);

            var perimeterCache = new Dictionary<(int, int, string), int>();

            long maxArea = 0;

            for (int i = 0; i < coords.Count; ++i)
            {
                var coord1 = coords[i];
                for (int j = i; j < coords.Count; ++j)
                {
                    var coord2 = coords[j];
                    int width = Math.Abs(coord1[1] - coord2[1]) + 1;
                    int height = Math.Abs(coord1[0] - coord2[0]) + 1;

                    maxArea = CheckRectangleIsValid(coord1, coord2, perimeterDict, perimeterCache) ? Math.Max((long)width * (long)height, maxArea) : maxArea;
                }

            }

            return maxArea;
        }

        public bool CheckRectangleIsValid(List<int> point1, List<int> point2, Dictionary<(int, int), string> perimeter, Dictionary<(int, int, string), int> perimeterCache)
        {
            // for now i'm disregarding rectangles of width 1 as I think it's unlikely this will have the maximum width, come back and fix this later
            if (point1[0] == point2[0] || point1[1] == point2[1])
            {
                return false;
            }

            List<int> topLeftCorner = null;
            List<int> topRightCorner = null;
            List<int> bottomLeftCorner = null;
            List<int> bottomRightCorner = null;

            var perimeterList = perimeter.ToList();

            if (point1[0] < point2[0] && point1[1] < point2[1])
            {
                topLeftCorner = point1;
                bottomRightCorner = point2;

                //topRightCorner = new List<int> { bottomRightCorner[0], topLeftCorner[1] };
                //bottomLeftCorner = new List<int> { topLeftCorner[0], bottomRightCorner[1] };

            }
            else if (point1[0] > point2[0] && point1[1] < point2[1])
            {
                topRightCorner = point1;
                bottomLeftCorner = point2;

                //topLeftCorner = new List<int> { bottomLeftCorner[0], topRightCorner[1] };
                //bottomRightCorner = new List<int> { topRightCorner[0], bottomLeftCorner[1] };
            }
            else if (point1[0] < point2[0] && point1[1] > point2[1])
            {
                topRightCorner = point2;
                bottomLeftCorner = point1;

                //topLeftCorner = new List<int> { bottomLeftCorner[0], topRightCorner[1] };
                //bottomRightCorner = new List<int> { topRightCorner[0], bottomLeftCorner[1] };
            }
            else
            {
                topLeftCorner = point2;
                bottomRightCorner = point1;

                //topRightCorner = new List<int> { bottomRightCorner[0], topLeftCorner[1] };
                //bottomLeftCorner = new List<int> { topLeftCorner[0], bottomRightCorner[1] };
            }

            if (topLeftCorner != null)
            {
                int topLeftXValidInt = -1;
                int topLeftYValidInt = -1;
                int bottomRightXValidInt = -1;
                int bottomRightYValidInt = -1;

                if (perimeterCache.ContainsKey((topLeftCorner[0], topLeftCorner[1], "TLX")))
                {
                    topLeftXValidInt = perimeterCache[(topLeftCorner[0], topLeftCorner[1], "TLX")];
                }
                else
                {
                    var xCoordListTL = perimeterList.Where(e => e.Key.Item2 == topLeftCorner[1] && e.Key.Item1 > topLeftCorner[0] && (e.Value == "DL" || e.Value == "UD"));
                    topLeftXValidInt = xCoordListTL.Any() ? xCoordListTL.Select(e => e.Key.Item1).Min() : -1;
                    perimeterCache.Add((topLeftCorner[0], topLeftCorner[1], "TLX"), topLeftXValidInt);

                }

                if (perimeterCache.ContainsKey((topLeftCorner[0], topLeftCorner[1], "TLY")))
                {
                    topLeftYValidInt = perimeterCache[(topLeftCorner[0], topLeftCorner[1], "TLY")];
                }
                else
                {
                    var yCoordListTL = perimeterList.Where(e => e.Key.Item1 == topLeftCorner[0] && e.Key.Item2 > topLeftCorner[1] && (e.Value == "UR" || e.Value == "LR"));
                    topLeftYValidInt = yCoordListTL.Any() ? yCoordListTL.Select(e => e.Key.Item2).Min() : -1;
                    perimeterCache.Add((topLeftCorner[0], topLeftCorner[1], "TLY"), topLeftYValidInt);
                }  
                
                if (perimeterCache.ContainsKey((bottomRightCorner[0], bottomRightCorner[1], "BRX")))
                {
                    bottomRightXValidInt = perimeterCache[(bottomRightCorner[0], bottomRightCorner[1], "BRX")];
                }
                else
                {
                    var xCoordListBR = perimeterList.Where(e => e.Key.Item2 == bottomRightCorner[1] && e.Key.Item1 < bottomRightCorner[0] && (e.Value == "UD" || e.Value == "UR"));
                    bottomRightXValidInt = xCoordListBR.Any() ? xCoordListBR.Select(e => e.Key.Item1).Max() : -1;
                    perimeterCache.Add((bottomRightCorner[0], bottomRightCorner[1], "BRX"), bottomRightXValidInt);
                }

                
                if (perimeterCache.ContainsKey((bottomRightCorner[0], bottomRightCorner[1], "BRY")))
                {
                    bottomRightYValidInt = perimeterCache[(bottomRightCorner[0], bottomRightCorner[1], "BRY")];
                }
                else
                {
                    var yCoordListBR = perimeterList.Where(e => e.Key.Item1 == bottomRightCorner[0] && e.Key.Item2 < bottomRightCorner[1] && (e.Value == "LR" || e.Value == "DL"));
                    bottomRightYValidInt = yCoordListBR.Any() ? yCoordListBR.Select(e => e.Key.Item2).Max() : -1;
                    perimeterCache.Add((bottomRightCorner[0], bottomRightCorner[1], "BRY"), bottomRightYValidInt);
                }


                bool topLeftXValid = topLeftXValidInt != -1 && topLeftXValidInt >= bottomRightCorner[0];

                bool topLeftYValid = topLeftYValidInt != -1 && topLeftYValidInt >= bottomRightCorner[1];

                bool bottomRightXValid = bottomRightXValidInt != -1 && bottomRightXValidInt <= topLeftCorner[0];

                bool bottomRightYValid = bottomRightYValidInt != -1 && bottomRightYValidInt <= topLeftCorner[1];

                return topLeftXValid && topLeftYValid && bottomRightXValid && bottomRightYValid;
            }
            else
            {
                int topRightXValidInt = -1;
                int topRightYValidInt = -1;
                int bottomLeftXValidInt = -1;
                int bottomLeftYValidInt = -1;

                if (perimeterCache.ContainsKey((topRightCorner[0], topRightCorner[1], "TRX")))
                {
                    topRightXValidInt = perimeterCache[(topRightCorner[0], topRightCorner[1], "TRX")];
                }
                else
                {
                    var xCoordListTR = perimeterList.Where(e => e.Key.Item2 == topRightCorner[1] && e.Key.Item1 < topRightCorner[0] && (e.Value == "DR" || e.Value == "UD"));
                    topRightXValidInt = xCoordListTR.Any() ? xCoordListTR.Select(e => e.Key.Item1).Max() : -1;
                    perimeterCache.Add((topRightCorner[0], topRightCorner[1], "TRX"), topRightXValidInt);
                }

                if (perimeterCache.ContainsKey((topRightCorner[0], topRightCorner[1], "TRY")))
                {
                    topRightYValidInt = perimeterCache[(topRightCorner[0], topRightCorner[1], "TRY")];
                }
                else
                {
                    var yCoordListTR = perimeterList.Where(e => e.Key.Item1 == topRightCorner[0] && e.Key.Item2 > topRightCorner[1] && (e.Value == "LR" || e.Value == "UL"));
                    topRightYValidInt = yCoordListTR.Any() ? yCoordListTR.Select(e => e.Key.Item2).Min() : -1;
                    perimeterCache.Add((topRightCorner[0], topRightCorner[1], "TRY"), topRightYValidInt);
                }

                if (perimeterCache.ContainsKey((bottomLeftCorner[0], bottomLeftCorner[1], "BLX")))
                {
                    bottomLeftXValidInt = perimeterCache[(bottomLeftCorner[0], bottomLeftCorner[1], "BLX")];
                }
                else
                {
                    var xCoordListBL = perimeterList.Where(e => e.Key.Item2 == bottomLeftCorner[1] && e.Key.Item1 > bottomLeftCorner[0] && (e.Value == "UD" || e.Value == "UL"));
                    bottomLeftXValidInt = xCoordListBL.Any() ? xCoordListBL.Select(e => e.Key.Item1).Min() : -1;
                    perimeterCache.Add((bottomLeftCorner[0], bottomLeftCorner[1], "BLX"), bottomLeftXValidInt);
                }

                if (perimeterCache.ContainsKey((bottomLeftCorner[0], bottomLeftCorner[1], "BLY")))
                {
                    bottomLeftYValidInt = perimeterCache[(bottomLeftCorner[0], bottomLeftCorner[1], "BLY")];
                }
                else
                {
                    var yCoordListBL = perimeterList.Where(e => e.Key.Item1 == bottomLeftCorner[0] && e.Key.Item2 < bottomLeftCorner[1] && (e.Value == "DR" || e.Value == "LR"));
                    bottomLeftYValidInt = yCoordListBL.Any() ? yCoordListBL.Select(e => e.Key.Item2).Max() : -1;
                }

                bool topRightXValid = topRightXValidInt != -1 && topRightXValidInt <= bottomLeftCorner[0];
                bool topRightYValid = topRightYValidInt != -1 && topRightYValidInt >= bottomLeftCorner[1];
                bool bottomLeftXValid = bottomLeftXValidInt != -1 && bottomLeftXValidInt >= topRightCorner[0];
                bool bottomLeftYValid = bottomLeftYValidInt != -1 && bottomLeftYValidInt <= topRightCorner[1];

                return topRightXValid && topRightYValid && bottomLeftXValid && bottomLeftYValid;
            }

                return false;
        }

        Dictionary<(int, int), string> GetPerimeter(List<List<int>> coords)
        {

            Dictionary<(int, int), string> perimeter = new();

            bool continueLoop = true;

            List<int> lastCoord = coords[coords.Count - 1];

            List<int> firstCoord = coords[0];

            string prevDir = firstCoord[0] - lastCoord[0] < 0 ? "R" : firstCoord[0] - lastCoord[0] > 0 ? "L" : firstCoord[1] - lastCoord[1] > 0 ? "U" : "D";

            for (int i = 1; i < coords.Count + 1; ++i)
            {
                int firstPointIdx = i - 1;
                int secondPointIdx = i;

                if (secondPointIdx == coords.Count)
                {
                    secondPointIdx = 0;
                }

                if (i == coords.Count)
                {
                    continueLoop = false;
                }

                int point1x = coords[firstPointIdx][0];
                int point1y = coords[firstPointIdx][1];

                int point2x = coords[secondPointIdx][0];
                int point2y = coords[secondPointIdx][1];

                int xDifference = point1x - point2x;
                int yDifference = point1y - point2y;

                int minX = Math.Min(point1x, point2x);
                int maxX = Math.Max(point1x, point2x);

                int minY = Math.Min(point1y, point2y);
                int maxY = Math.Max(point1y, point2y);

                string curDir = xDifference < 0 ? "R" : xDifference > 0 ? "L" : yDifference < 0 ? "D" : "U";

                if (minX != maxX)
                {
                    for (int x = minX; x <= maxX; ++x)
                    {
                        if (!perimeter.ContainsKey((x, point1y)))
                        {
                            if (x != minX && x != maxX)
                            {
                                perimeter.Add((x, point1y), "LR");
                            }
                            else if ((curDir == "R" && x == minX) || (curDir == "L" && x == maxX))
                            {
                                perimeter.Add((x, point1y), $"{prevDir}{curDir}");
                            }
                        }
                    }
                }
                else if (minY != maxY)
                {
                    for (int y = minY; y <= maxY; ++y)
                    {
                        if (!perimeter.ContainsKey((point1x, y)))
                        {
                            if (y != minY && y != maxY)
                            {
                                perimeter.Add((point1x, y), "UD");
                            }
                            else if ((curDir == "D" && y == minY) || (curDir == "U" && y == maxY))
                            {
                                perimeter.Add((point1x, y), $"{curDir}{prevDir}");
                            }
                        }
                    }
                }

                prevDir = curDir == "R" ? "L" : curDir == "L" ? "R" : curDir == "U" ? "D" : "U";
            }

            return perimeter;
        }
    }
}
