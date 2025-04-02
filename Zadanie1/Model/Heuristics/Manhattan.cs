using Puzzle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heuristics
{
	public class Manhattan : Heuristic
	{
		public Manhattan(int rows, int cols) : base(rows, cols)
		{

		}

		public override int CalculatePriority(PuzzleState state)
		{
			int distance = 0;
			for (int x = 0; x < Rows; x++)
			{
				for (int y = 0; y < Cols; y++)
				{
					int num = state.GetCell(x, y);
					int expectedRow = 0;
					int expectedCol = 0;
					if (num == 0)
					{
						expectedRow = Rows - 1;
						expectedCol = Cols - 1;
					}
					else
					{
						expectedRow = (num - 1) / Cols;
						expectedCol = (num - 1) % Cols;
					}
					int xDis = Math.Abs(x - expectedRow);
					int yDis = Math.Abs(y - expectedRow);
					distance += (xDis + yDis);
				}
			}
			return distance;
		}
	}
}
