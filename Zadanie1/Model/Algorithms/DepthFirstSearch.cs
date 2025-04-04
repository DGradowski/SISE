﻿using Puzzle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
	internal class DepthFirstSearch : Algorithm
	{
		private String order;
		private int maxDepth;

		public DepthFirstSearch(string order, int maxDepth)
		{
			this.order = order;
			this.maxDepth = maxDepth;
		}

		public override PuzzleState FindSolution(PuzzleState state)
		{
			RecursionDepth = 0;
			var watch = System.Diagnostics.Stopwatch.StartNew();
			Stack<PuzzleState> states = new Stack<PuzzleState>();
			states.Push(state);
			PuzzleState current;
			char[] reversedOrder = order.ToCharArray();
			Array.Reverse(reversedOrder);

			while (states.Count > 0)
			{
				current = states.Pop();
				if (current.Moves.Length > RecursionDepth) RecursionDepth = current.Moves.Length;
				CheckedStates++;
				if (!current.IsSolved())
				{
					if (maxDepth == current.Moves.Length) continue;
					foreach (char move in reversedOrder)
					{
						if (!current.IsMoveLegal(move)) continue;
						states.Push(new PuzzleState(current, move));
						ProcessedStates++;
					}
				}
				else
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

