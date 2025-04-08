using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Puzzle;

namespace Algorithms
{
	public class BreadthFirstSearch : Algorithm
	{
		private String order;

		public BreadthFirstSearch(string order)
		{
			this.order = order;
		}

		public override PuzzleState FindSolution(PuzzleState state)
		{
			HashSet<String> visited = new HashSet<String>();
			RecursionDepth = 0;
			var watch = System.Diagnostics.Stopwatch.StartNew();
			Queue<PuzzleState> states = new Queue<PuzzleState>();
			states.Enqueue(state);
			PuzzleState current;

			while (states.Count > 0)
			{
				current = states.Dequeue();
				visited.Add(current.BoardToKey());
				if (current.Moves.Length > RecursionDepth) RecursionDepth = current.Moves.Length;
				CheckedStates++;
				if (!current.IsSolved())
				{
					foreach(char move in order)
					{
						if (!current.IsMoveLegal(move)) continue;
						PuzzleState newState = new PuzzleState(current, move);
						if (visited.Contains(newState.BoardToKey())) continue;
						states.Enqueue(newState);
						ProcessedStates++;
					}
				} else
				{
					SolutionLength = current.Moves.Length;
					watch.Stop();
					Time = watch.Elapsed.TotalMilliseconds;
					return current;
				}
			}
			throw new Exception("Couldn't find solution");
		}
	}
}
