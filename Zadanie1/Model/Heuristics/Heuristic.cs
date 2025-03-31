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

		public string OrderPriority { get => orderPriority; }

		protected Heuristic(string priority = "")
		{
			orderPriority = priority;
		}

		public PuzzleState SelectPath(PuzzleState state)
		{
			List<PuzzleState> legalStates = new List<PuzzleState>();
			foreach (char d in "LDRU")
			{
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

		abstract public double CalculatePriority(PuzzleState state);

		abstract public PuzzleState ResolveTie(List<PuzzleState> paths);
	}
}
