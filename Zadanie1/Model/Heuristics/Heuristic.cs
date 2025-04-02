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
		private int rows;
		private int cols;

		public int Rows { get => rows; }
		public int Cols { get => cols; }

		protected Heuristic(int rows, int cols)
		{
			this.rows = rows;
			this.cols = cols;
		}

		abstract public int CalculatePriority(PuzzleState state);
	}
}
