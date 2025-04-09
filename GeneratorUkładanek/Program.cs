using System.ComponentModel;
using System.Reflection.PortableExecutable;
using System.Xml;
using System.Security.Cryptography;
using Puzzle;

public class MainClass
{
	static void Main(string[] args)
	{
		int rows = 0; // 0
		int cols = 0; // 1
		int depth = 0; // 2
		Int32.TryParse(args[0], out rows);
		Int32.TryParse(args[1], out cols);
		Int32.TryParse(args[2], out depth);
		Dictionary<string, PuzzleState> saved = new Dictionary<string, PuzzleState>();
		Queue<PuzzleState> queue = new Queue<PuzzleState>();
		Random r = new Random();
		int zRow = rows - 1, zCol = cols - 1;
		int solutionDepth = 0;


		PuzzleState state = new PuzzleState(rows, cols);
		for (int row = 0; row < rows; row++)
		{
			for (int col = 0; col < cols; col++)
			{
				if (row == rows - 1 && col == cols - 1)
				{
					state.SetCell(row, col, 0);
					break;
				}
				state.SetCell(row, col, (row * cols) + col + 1);
			}
		}
		queue.Enqueue(state);

		while (solutionDepth <= depth)
		{
			state = queue.Dequeue();
			solutionDepth = state.Moves.Length;
			if (saved.ContainsKey(state.BoardToKey())) continue;
			foreach (char c in "LDRU")
			{
				PuzzleState nState;
				switch (c)
				{
					case 'U':
						// do góry
						if (!state.IsMoveLegal(c)) break;
						nState = new PuzzleState(state, c);
						if (saved.ContainsKey(nState.BoardToKey())) break;
						queue.Enqueue(nState);
						break;

					case 'D':
						// do dołu
						if (!state.IsMoveLegal(c)) break;
						nState = new PuzzleState(state, c);
						if (saved.ContainsKey(nState.BoardToKey())) break;
						queue.Enqueue(nState);
						break;

					case 'L':
						// w lewo
						if (!state.IsMoveLegal(c)) break;
						nState = new PuzzleState(state, c);
						if (saved.ContainsKey(nState.BoardToKey())) break;
						queue.Enqueue(nState);
						break;

					default:
						// w prawo
						if (!state.IsMoveLegal(c)) break;
						nState = new PuzzleState(state, c);
						if (saved.ContainsKey(nState.BoardToKey())) break;
						queue.Enqueue(nState);
						break;
				}
			}
			if (state.Moves.Length > depth) break;
			saved.Add(state.BoardToKey(), state);
		}

		Dictionary<int, int> depthId = new Dictionary<int, int>();

		foreach (var k in saved.Keys.ToArray<string>())
		{
			if (saved[k].Moves.Length == 0) continue;
			int d = saved[k].Moves.Length;
			if (depthId.ContainsKey(d))
			{
				depthId[d]++;
			}
			else
			{
				depthId.Add(d, 1);
			}
			String file = rows.ToString() + "x" + cols.ToString() + "_" + d.ToString("00") + "_" + depthId[d].ToString("00000") + ".txt";
			String[] lines = new String[rows];
			for (int i = 0; i < rows; i++)
			{
				lines[i] = "";
				for (int j = 0; j < cols; j++)
				{
					lines[i] += saved[k].GetCell(i, j).ToString() + " ";
				}
			}
			SaveToFile(file, rows, cols, lines);
		}
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