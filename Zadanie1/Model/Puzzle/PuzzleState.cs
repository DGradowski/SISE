using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle
{
	internal class PuzzleState
	{
		private String moves;
		private int[][] board;
		private int zeroRow = 0;
		private int zeroCol = 0;

		public PuzzleState(int rows, int columns, String prevMove = "")
		{
			board = new int[rows][];
			for (int i = 0; i < rows; i++)
			{
				board[i] = new int[columns];
			}
			moves = prevMove;
		}

		public PuzzleState(PuzzleState state, String move)
		{
			board = new int[state.Rows][];
			for (int i = 0; i < state.Rows; i++)
			{
				board[i] = new int[state.Columns];
			}

			for (int i = 0; i < state.Rows; i++)
			{
				for (int j = 0; j < state.Columns; j++)
				{
					board[i][j] = state.GetCell(i, j);
				}
			}
			moves = state.Moves + move;
			MakeMove(move);
		}

		public int Rows => board.Length;
		public int Columns => board[0].Length;
		public string Moves => moves;
		public int GetCell(int row, int column) { return board[row][column]; }
		public void SetCell(int row, int column, int value) { board[row][column] = value; }


		public bool IsSolved()
		{
			for (int i = 0; i < board.Length; i++)
			{
				for(int j = 0; j < board[i].Length; j++)
				{
					if (i == board.Length && j == board[i].Length)
					{
						if (board[i][j] != 0) return false;
					}
					else
					{
						if (board[i][j] != i * board[i].Length + j + 1) return false;
					}
				}
			}
			return true;
		}
		public bool IsMoveLegal(String move)
		{
			FindZero();
			if (zeroCol == 0 && move == "L") return false;
			if (zeroCol == Columns - 1 && move == "R") return false;
			if (zeroRow == 0 && move == "U") return false;
			if (zeroRow == Rows - 1 && move == "D") return false;
			return true;
		}

		public void MakeMove(String move)
		{
			if (!IsMoveLegal(move)) throw new Exception("Trying to make illegal move");
			switch (move)
			{
				case "L":
					(board[zeroRow][zeroCol], board[zeroRow][zeroCol - 1]) = (board[zeroRow][zeroCol - 1], board[zeroRow][zeroCol]);
					break;
				case "R":
					(board[zeroRow][zeroCol], board[zeroRow][zeroCol + 1]) = (board[zeroRow][zeroCol + 1], board[zeroRow][zeroCol]);
					break;
				case "U":
					(board[zeroRow][zeroCol], board[zeroRow + 1][zeroCol]) = (board[zeroRow + 1][zeroCol], board[zeroRow][zeroCol]);
					break;
				case "D":
					(board[zeroRow][zeroCol], board[zeroRow - 1][zeroCol]) = (board[zeroRow - 1][zeroCol], board[zeroRow][zeroCol]);
					break;
			}
		}

		public void FindZero()
		{
			if (board[zeroRow][zeroCol] == 0) return;
			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{
					if (board[i][j] == 0)
					{
						zeroRow = i;
						zeroCol = j;
						return;
					}
				}
			}
		}
	}
}
