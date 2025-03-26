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
			RecursionDepth = 0;
			var watch = System.Diagnostics.Stopwatch.StartNew();
			Queue<PuzzleState> states = new Queue<PuzzleState>();
			states.Enqueue(state);
			PuzzleState current;

			while (states.Count > 0)
			{
				current = states.Dequeue();
				if (current.Moves.Length > RecursionDepth) RecursionDepth = current.Moves.Length;
				CheckedStates++;
				if (!current.IsSolved())
				{
					foreach(char move in order)
					{
						if (!current.IsMoveLegal(move)) continue;
						states.Enqueue(new PuzzleState(current, move));
						ProcessedStates++;
					}
				} else
				{
					SolutionLength = current.Moves.Length;
					watch.Stop();
					Time = watch.ElapsedMilliseconds;
					return current;
				}
			}
			throw new Exception("Couldn't find solution");
		}
	}
}
