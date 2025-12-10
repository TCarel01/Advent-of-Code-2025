using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Z3;

namespace Advent_of_Code_2025.Days
{
    public class Day10
    {
        AdventOfCode2025Parser parser = new AdventOfCode2025Parser();

        public long Day10Part1Solver(string filename)
        {
            List<string> input = parser.ParseInputAsSingleArrayOfStrings(filename);

            List<List<string>> splitInput = input.Select(e => e.Split(" ").Where(f => f != "").ToList()).ToList();

            List<string> nodes = splitInput.Select(e => e[0]).ToList();

            List<List<List<int>>> buttonLists = GetButtonListsFromSplitInput(splitInput);

            long runningCounter = 0;

            for (int i = 0; i < nodes.Count; ++i)
            {
                runningCounter += GetCurrentNodeCount(nodes[i], buttonLists[i]);
            }

            return runningCounter;
        }

        public long Day10Part2Solver(string filename) 
        {
            // Part 2 uses Z3 as a solver. Examples of people using the various solvers were used to help me construct my solution

            List<string> input = parser.ParseInputAsSingleArrayOfStrings(filename);

            List<List<string>> splitInput = input.Select(e => e.Split(" ").Where(f => f != "").ToList()).ToList();

            List<string> nodes = splitInput.Select(e => e[e.Count - 1]).ToList();

            List<List<List<int>>> buttonLists = GetButtonListsFromSplitInput(splitInput);

            long runningCounter = 0;

            for (int i = 0; i < nodes.Count; ++i)
            {
                runningCounter += SolveEquationSystem(nodes[i], buttonLists[i]);
            }

            return runningCounter; 
        }

        public List<List<List<int>>> GetButtonListsFromSplitInput(List<List<string>> splitInput)
        {
            List<List<string>> buttons = splitInput.Select((e, idx1) => e.Where((f, idx) => idx > 0 && idx < splitInput[idx1].Count - 1).ToList()).ToList();

            buttons = buttons.Select(e => e.Select(f => f.Substring(1, f.Length - 2)).ToList()).ToList();

            List<List<List<int>>> buttonLists = buttons.Select(e => e.Select(f => f.Split(',').Select(g => int.Parse(g)).ToList()).ToList()).ToList();

            return buttonLists;
        }


        public string GetNextNode(string curNode, List<int> transitionFunction)
        {
            List<string> splitString = curNode.ToCharArray().Select(e => e.ToString()).ToList();

            transitionFunction.ForEach(e => splitString[e + 1] = splitString[e + 1] == "#" ? "." : "#");

            return string.Join("", splitString);
        }

        public long SolveEquationSystem(string targetNode, List<List<int>> transitionFunctions)
        {
            using (Context ctx = new Context())
            {
                string tempCurNode = targetNode.Substring(1, targetNode.Length - 2);

                List<int> curNodeCounts = tempCurNode.Split(',').Select(e => int.Parse(e)).ToList();

                Expr zero = ctx.MkNumeral(0, ctx.MkIntSort());

                List<Expr> totals = curNodeCounts.Select((e, idx) => ctx.MkNumeral(e, ctx.MkIntSort())).ToList();

                List<IntExpr> variables = transitionFunctions.Select((e, idx) => ctx.MkIntConst($"x{idx}")).ToList();

                List<BoolExpr> constraints = transitionFunctions.Select((e, idx) => ctx.MkGe(variables[idx], (ArithExpr)zero)).ToList();

                Optimize optimize = ctx.MkOptimize();

                for (int i = 0; i < totals.Count; ++i)
                {
                    List<IntExpr> curTotalsEq = new List<IntExpr>();

                    for (int j = 0; j < transitionFunctions.Count; ++j)
                    {
                        var curTransitionFunction = transitionFunctions[j];

                        if (curTransitionFunction.Contains(i))
                        {
                            curTotalsEq.Add(variables[j]);
                        }
                    }
                    var array = curTotalsEq.ToArray();

                    constraints.Add(ctx.MkEq(ctx.MkAdd(curTotalsEq), totals[i]));
                }

                optimize.Add(ctx.MkAnd(constraints.ToArray()));

                optimize.MkMinimize(ctx.MkAdd(variables));

                optimize.Check();

                Model model = optimize.Model;

                return model.Decls.Aggregate((long)0, (acc, e) => ((IntNum)(model.ConstInterp(e))).Int);
            };
        }

        public long GetCurrentNodeCount(string targetNode, List<List<int>> stateTransitions)
        {
            HashSet<string> seenNodes = new HashSet<string>();

            HashSet<string> curNodes = new HashSet<string>();

            HashSet<string> nextNodes = new HashSet<string>();

            string startNode = string.Join("", targetNode.ToCharArray().Select(e => e == '#' ? '.' : e));

            curNodes.Add(startNode);

            long stepCount = 0;

            while (!curNodes.Contains(targetNode))
            {
                foreach (string node in curNodes)
                {
                    seenNodes.Add(node);

                    foreach (List<int> transition in stateTransitions)
                    {
                        string nextNode = GetNextNode(node, transition);

                        if (!seenNodes.Contains(nextNode) && !nextNodes.Contains(nextNode) && !curNodes.Contains(nextNode))
                        {
                            nextNodes.Add(nextNode);
                        }
                    }
                }

                curNodes = nextNodes;
                nextNodes = new HashSet<string>();

                stepCount += 1;
            }

            return stepCount;
        }
    }
}
