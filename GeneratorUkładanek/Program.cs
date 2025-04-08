using System.ComponentModel;
using System.Reflection.PortableExecutable;
using System.Xml;
using System.Security.Cryptography;

public class MainClass
{
	static void Main(string[] args)
	{
		int rows = 0; // 0
		int cols = 0; // 1
		int depth = 0; // 2
		string file = args[3]; // 3
		Int32.TryParse(args[0], out rows);
		Int32.TryParse(args[1], out cols);
		Int32.TryParse(args[2], out depth);
		HashSet<string> visited = new HashSet<string>();
		Random r = new Random();
		int zRow = rows - 1, zCol = cols - 1;


		int[,] board = new int[rows, cols];
		for (int row = 0; row < rows; row++)
		{
			for (int col = 0; col < cols; col++)
			{
				if (row == rows - 1 && col == cols - 1)
				{
					board[row, col] = 0;
					break;
				}
				board[row, col] = (row * cols) + col + 1;
			}
		}
		for (int i = 0; i < depth; i++)
		{
			visited.Add(BoardToKey(board));
			bool boardChanged = false;
			while (!boardChanged)
			{
				int x = r.Next(4);
				switch (x)
				{
					case 0:
						// do góry
						if (zRow == 0) break;
						(board[zRow, zCol], board[zRow - 1, zCol]) = (board[zRow - 1, zCol], board[zRow, zCol]);
						if (visited.Contains(BoardToKey(board)))
						{
							(board[zRow, zCol], board[zRow - 1, zCol]) = (board[zRow - 1, zCol], board[zRow, zCol]);
						}
						else
						{
							boardChanged = true;
							zRow--;
						}
						break;

					case 1:
						// do dołu
						if (zRow == rows - 1) break;
						(board[zRow, zCol], board[zRow + 1, zCol]) = (board[zRow + 1, zCol], board[zRow, zCol]);
						if (visited.Contains(BoardToKey(board)))
						{
							(board[zRow, zCol], board[zRow + 1, zCol]) = (board[zRow + 1, zCol], board[zRow, zCol]);
						}
						else
						{
							boardChanged = true;
							zRow++;
						}
						break;

					case 2:
						// w lewo
						if (zCol == 0) break;
						(board[zRow, zCol], board[zRow, zCol - 1]) = (board[zRow, zCol - 1], board[zRow, zCol]);
						if (visited.Contains(BoardToKey(board)))
						{
							(board[zRow, zCol], board[zRow, zCol - 1]) = (board[zRow, zCol - 1], board[zRow, zCol]);
						}
						else
						{
							boardChanged = true;
							zCol--;
						}
						break;

					default:
						// w prawo
						if (zCol == cols - 1) break;
						(board[zRow, zCol], board[zRow, zCol + 1]) = (board[zRow, zCol + 1], board[zRow, zCol]);
						if (visited.Contains(BoardToKey(board)))
						{
							(board[zRow, zCol], board[zRow, zCol + 1]) = (board[zRow, zCol + 1], board[zRow, zCol]);
						}
						else
						{
							boardChanged = true;
							zCol++;
						}
						break;
				}
			}
		}

		String[] lines = new String[rows];
		for (int i = 0; i < board.GetLength(0); i++)
		{
			lines[i] = "";
			for (int j = 0; j < board.GetLength(1); j++)
			{
				lines[i] += board[i, j].ToString() + " ";
			}
		}

		SaveToFile(file, rows, cols, lines);
	}


	static void SaveToFile(String file, int row, int col, String[] s)
	{
		using (StreamWriter sw = new StreamWriter(file))
		{
			String line = row.ToString() + " " + col.ToString();
			sw.WriteLine(line);
			foreach (String s2 in s)
			{
				sw.WriteLine(s2);
			}
		}
	}

	static public String BoardToKey(int[,] board)
	{
		String key = "";
		for (int i = 0; i < board.GetLength(0); i++)
		{
			for (int j = 0; j < board.GetLength(1); j++)
			{
				key += board[i, j].ToString() + " ";
			}
		}
		return key;
	}
}