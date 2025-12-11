using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2025.Days
{
    public class Day11
    {

        AdventOfCode2025Parser parser = new AdventOfCode2025Parser();

        public long Day11Part1Solver(string filename)
        {
            List<string> input = parser.ParseInputAsSingleArrayOfStrings(filename);

            List<string> nodes = input.Select(e => e.Split(':').First()).ToList();

            Dictionary<string, List<string>> graph = ConstructGraph(input);

            string startNode = "you";

            return CountPathsWithDFS(graph, startNode, true, true, false, new());
        }

        public long Day11Part2Solver(string filename)
        {
            List<string> input = parser.ParseInputAsSingleArrayOfStrings(filename);

            List<string> nodes = input.Select(e => e.Split(':').First()).ToList();

            Dictionary<string, List<string>> graph = ConstructGraph(input);

            string startNode = "svr";

            return CountPathsWithDFS(graph, startNode, false, false, true, new());
        }


        Dictionary<string, List<string>> ConstructGraph(List<string> input)
        {
            Dictionary<string, List<string>> graph = new Dictionary<string, List<string>>();

            input.ForEach(e =>
            {
                string inputNode = e.Split(":").First();

                List<string> remainingNodes = e.Split(':').Last().Split(' ').Where(f => f != "").ToList();

                graph.Add(inputNode, remainingNodes);
            });

            return graph;
        }

        long CountPathsWithDFS(Dictionary<string, List<string>> graph, string startNode, bool dacVisited, bool fftVisited, bool part2, Dictionary<(string, bool, bool), long> pathCache)
        {
            if (startNode == "out")
            {
                return dacVisited && fftVisited ? 1 : 0;
            }

            if (pathCache.ContainsKey((startNode, dacVisited, fftVisited)))
            {
                return pathCache[(startNode, dacVisited, fftVisited)];
            }

            List<string> allNextNodes = graph[startNode];

            long returnCount = 0;

            dacVisited = dacVisited || startNode == "dac";

            fftVisited = fftVisited || startNode == "fft";

            allNextNodes.ForEach(e =>
            {
                returnCount += CountPathsWithDFS(graph, e, dacVisited, fftVisited, part2, pathCache);
            });

            if (!pathCache.ContainsKey((startNode, dacVisited, fftVisited)))
            {
                pathCache.Add((startNode, dacVisited, fftVisited), returnCount);
            }

            return returnCount;
        }
    }
}
