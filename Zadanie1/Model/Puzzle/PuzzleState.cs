using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle
{
	public class PuzzleState
	{
		private String moves;
		private int[,] board;
		private int zeroRow = 0;
		private int zeroCol = 0;

		public PuzzleState(int rows, int columns, String prevMoves = "")
		{
			board = new int[rows, columns];
			moves = "";
		}

		public PuzzleState(PuzzleState state, char? move = null)
		{
			board = new int[state.Rows, state.Columns];
			for (int i = 0; i < state.Rows; i++)
			{
				for (int j = 0; j < state.Columns; j++)
				{
					board[i,j] = state.GetCell(i, j);
				}
			}
			moves = state.Moves + move;
			if (move != null) MakeMove((char) move);
		}

		public int Rows => board.GetLength(0);
		public int Columns => board.GetLength(1);
		public string Moves => moves;

		public int[,] Board => board;
		public int GetCell(int row, int column) { return board[row,column]; }
		public void SetCell(int row, int column, int value) { board[row,column] = value; }


		public bool IsSolved()
		{
			for (int i = 0; i < Rows; i++)
			{
				for(int j = 0; j < Columns; j++)
				{
					if (i == Rows - 1 && j == Columns - 1)
					{
						if (board[i,j] != 0) return false;
					}
					else
					{
						if (board[i, j] != i * Columns + j + 1) return false;
					}
				}
			}
			return true;
		}
		public bool IsMoveLegal(char move)
		{
			FindZero();
			if (zeroCol == 0 && move == 'L') return false;
			if (zeroCol == Columns - 1 && move == 'R') return false;
			if (zeroRow == 0 && move == 'U') return false;
			if (zeroRow == Rows - 1 && move == 'D') return false;
			return true;
		}

		public void MakeMove(char move)
		{
			if (!IsMoveLegal(move)) throw new Exception("Trying to make illegal move");
			FindZero();
			switch (move)
			{
				case 'L':
					(board[zeroRow, zeroCol], board[zeroRow, zeroCol - 1]) = (board[zeroRow, zeroCol - 1], board[zeroRow, zeroCol]);
					break;
				case 'R':
					(board[zeroRow, zeroCol], board[zeroRow, zeroCol + 1]) = (board[zeroRow, zeroCol + 1], board[zeroRow, zeroCol]);
					break;
				case 'U':
					(board[zeroRow, zeroCol], board[zeroRow - 1, zeroCol]) = (board[zeroRow - 1, zeroCol], board[zeroRow, zeroCol]);
					break;
				case 'D':
					(board[zeroRow, zeroCol], board[zeroRow + 1, zeroCol]) = (board[zeroRow + 1, zeroCol], board[zeroRow, zeroCol]);
					break;
			}
		}

		public void FindZero()
		{
			if (board[zeroRow, zeroCol] == 0) return;
			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{
					if (board[i, j] == 0)
					{
						zeroRow = i;
						zeroCol = j;
						return;
					}
				}
			}
		}

		public String BoardToKey()
		{
			String key = "";
			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{
					key += board[i, j].ToString() + " ";
				}
			}
			return key;
		}

		public bool CompareBoard(PuzzleState s)
		{
			if (s.Rows != Rows) return false;
			if (s.Columns != Columns) return false;
			for (int i = 0;i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{
					if (s.GetCell(i, j) !=  GetCell(i, j)) return false;
				}
			}
			return true;
		}
	}
}
