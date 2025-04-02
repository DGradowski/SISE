using Heuristics;
using Puzzle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
	public class AStar : Algorithm
	{
		private Heuristic heuristic;
		public AStar(Heuristic heuristic)
		{
			this.heuristic = heuristic;
		}

		public override PuzzleState FindSolution(PuzzleState state)
		{
			// To nie działa i nie ma sensu ale będzie miało
			PuzzleState currentState = state;
			while (!currentState.IsSolved())
			{
				currentState = heuristic.SelectPath(state);
			}
			return currentState;
		}
	}
}
