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
		public Hamming(int rows, int cols, string priority = "") : base(rows, cols, priority)
		{

		}

		public override int CalculatePriority(PuzzleState state)
		{
			int fields = Rows * Cols;
			for (int x = 0; x < Rows; x++)
			{
				for (int y = 0; y < Cols; y++)
				{
					if (state.GetCell(x, y) == (x * Cols) + y + 1) fields--;
					else if (x == Rows - 1 && y == Cols && state.GetCell(x, y) == 0) fields--;
				}
			}
			return fields;
		}

		public override PuzzleState ResolveTie(List<PuzzleState> paths)
		{
			Random rng = new Random();
			if (OrderPriority == "") return paths[rng.Next(paths.Count)];
			foreach (char d in OrderPriority)
			{
				foreach (PuzzleState path in paths)
				{
					if (path.Moves[path.Moves.Length - 1] == d) return path;
				}
			}
			return paths[0];
		}
	}
}
