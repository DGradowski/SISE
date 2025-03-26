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
			Queue<PuzzleState> states = new Queue<PuzzleState>();
			states.Enqueue(state);
			PuzzleState current;

			while (states.Count > 0)
			{
				current = states.Dequeue();
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
					return current;
				}
			}
			throw new Exception("Couldn't find solution");
		}
	}
}
