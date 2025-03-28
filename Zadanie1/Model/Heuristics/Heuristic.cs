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
			this.orderPriority = priority;
		}

		public PuzzleState SelectPath(PuzzleState[] paths)
		{
			List<PuzzleState> states = new List<PuzzleState>();
			states.Add(paths[0]);
			double currPriority = CalculatePriority(states[0]);
			foreach (PuzzleState path in paths)
			{
				double newPrioriy = CalculatePriority(path);
				if (newPrioriy > currPriority)
				{
					currPriority = newPrioriy;
					states.Clear();
					states.Add(path);
				}
			}
			Random rand = new Random();
			if (orderPriority.Length == 0) return states[rand.Next(states.Count)];
			foreach (char move in orderPriority)
			{
				foreach (PuzzleState state in states)
				{
					if (state.Moves.Last() == move) return state;
				}
			}
			return states[0];
		}

		abstract public double CalculatePriority(PuzzleState state);
	}
}
