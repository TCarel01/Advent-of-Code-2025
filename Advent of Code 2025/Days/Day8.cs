using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2025.Days
{
    public class Day8
    {

        AdventOfCode2025Parser parser = new AdventOfCode2025Parser();

        public long Day8Part1Solver(string filename, int numPoints)
        {
            List<string> input = parser.ParseInputAsSingleArrayOfStrings(filename);

            List<List<int>> coords = input.Select(e => e.Split(',').Select(f => int.Parse(f)).ToList()).ToList();

            Dictionary<(int, int), double> distances = new Dictionary<(int, int), double>();

            for (int i = 0; i < coords.Count; ++i)
            {
                List<int> point1 = coords[i];
                for (int j = i + 1; j < coords.Count; ++j)
                {
                    List<int> point2 = coords[j];

                    double distance = PointDistance(point1, point2);
                    distances.Add((i, j), distance);
                }
            }

            var sortedDistances = distances.ToList();
            sortedDistances.Sort((i, j) => i.Value - j.Value < 0 ? -1 : i.Value - j.Value == 0 ? 0 : 1);

            List<List<int>> circuits = new List<List<int>>();

            for (int i = 0; i < input.Count; ++i)
            {
                circuits.Add(new List<int>  { i });
            }


            int junctionBox1 = int.MaxValue;
            int junctionBox2 = int.MaxValue;
            for (int i = 0; i < numPoints || numPoints == -1; ++i)
            {
                var shortestDistance = sortedDistances.First();
                sortedDistances.RemoveAt(0);

                junctionBox1 = shortestDistance.Key.Item1;
                junctionBox2 = shortestDistance.Key.Item2;

                List<int> circuit1 = circuits.ElementAt(circuits.IndexOf(circuits.Where(e => e.Contains(shortestDistance.Key.Item1)).FirstOrDefault()));
                List<int> circuit2 = circuits.ElementAt(circuits.IndexOf(circuits.Where(e => e.Contains(shortestDistance.Key.Item2)).FirstOrDefault()));

                if (circuit1 == circuit2)
                {
                    continue;
                }

                CombineCircuits(circuits, circuit1, circuit2);

                if (circuits.Count == 1)
                {
                    break;
                }
            }

            circuits.Sort((i, j) => j.Count - i.Count);

            return numPoints == -1 ? coords[junctionBox1][0] * coords[junctionBox2][0] : circuits[0].Count * circuits[1].Count * circuits[2].Count;
        }

        public long Day8Part2Solver(string filename)
        {
            return Day8Part1Solver(filename, -1);
        }

        private double PointDistance(List<int> point1, List<int> point2)
        {
            return Math.Sqrt(Math.Pow(point1[0] - point2[0], 2) +  Math.Pow(point1[1] - point2[1], 2) + Math.Pow(point1[2] - point2[2], 2));
        }


        private void CombineCircuits(List<List<int>> circuits, List<int> circuit1, List<int> circuit2)
        {
            circuits.Remove(circuit1);
            circuits.Remove(circuit2);
            circuit1 = circuit1.Concat(circuit2).ToList();
            circuit1.Sort();
            circuits.Add(circuit1);
        }

    }
}
