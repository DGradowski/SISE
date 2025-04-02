using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Puzzle;

namespace Heuristics
{
	abstract public class Heuristic
	{
		private String orderPriority;
		private int rows;
		private int cols;

		public string OrderPriority { get => orderPriority; }
		public int Rows { get => rows; }
		public int Cols { get => cols; }

		protected Heuristic(int rows, int cols, string priority = "")
		{
			orderPriority = priority;
			this.rows = rows;
			this.cols = cols;
		}

		public PuzzleState SelectPath(PuzzleState state)
		{
			List<PuzzleState> legalStates = new List<PuzzleState>();
			char lastMove;
			if (state.Moves.Length > 0) lastMove = state.Moves[state.Moves.Length - 1];
			else lastMove = 'N';

			foreach (char d in "LDRU")
			{
				if (d == 'L' && lastMove == 'R') continue;
				if (d == 'R' && lastMove == 'L') continue;
				if (d == 'D' && lastMove == 'U') continue;
				if (d == 'U' && lastMove == 'D') continue;
				if (state.IsMoveLegal(d)) legalStates.Add(new PuzzleState(state, d));
			}

			List<PuzzleState> states = new List<PuzzleState>();

			states.Add(legalStates[0]);
			double currPriority = CalculatePriority(states[0]);
			foreach (PuzzleState path in legalStates)
			{
				double newPrioriy = CalculatePriority(path);
				if (newPrioriy > currPriority)
				{
					currPriority = newPrioriy;
					states.Clear();
					states.Add(path);
				}
				else if (newPrioriy == currPriority)
				{
					states.Add(path);
				}
			}

			if (states.Count == 1) return states[0];
			else return ResolveTie(states);
		}

		abstract public int CalculatePriority(PuzzleState state);

		abstract public PuzzleState ResolveTie(List<PuzzleState> paths);
	}
}
