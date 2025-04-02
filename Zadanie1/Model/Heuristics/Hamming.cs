using Puzzle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heuristics
{
	public class Hamming : Heuristic
	{
		public Hamming(int rows, int cols) : base(rows, cols)
		{

		}

		public override int CalculatePriority(PuzzleState state)
		{
			int fields = 0;
			for (int x = 0; x < Rows; x++)
			{
				for (int y = 0; y < Cols; y++)
				{
					if (state.GetCell(x, y) != (x * Cols) + y + 1) fields++;
					else if (x == Rows - 1 && y == Cols && state.GetCell(x, y) != 0) fields++;
				}
			}
			return fields;
		}
	}
}
