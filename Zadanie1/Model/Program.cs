using Puzzle;
using System.ComponentModel;
using System.Reflection.PortableExecutable;

public class MainClass
{
	
	static void Main(string[] args)
	{
		PuzzleState state;
		state = LoadBoard(args[0]);
		DrawBoard(state);
	}

	//  strategia "wszerz" z porządkiem przeszukiwania sąsiedztwa prawo-dół-góra-lewo:
	// program bfs RDUL 4x4_01_0001.txt 4x4_01_0001_bfs_rdul_sol.txt 4x4_01_0001_bfs_rdul_stats.txt
	// strategia "w głąb" z porządkiem przeszukiwania sąsiedztwa lewo-góra-dół-prawo:
	// program dfs LUDR 4x4_01_0001.txt 4x4_01_0001_dfs_ludr_sol.txt 4x4_01_0001_dfs_ludr_stats.txt
	// strategia A* z heurystyką w postaci metryki Manhattan:
	// program astr manh 4x4_01_0001.txt 4x4_01_0001_astr_manh_sol.txt 4x4_01_0001_astr_manh_stats.txt

	static PuzzleState LoadBoard(String file)
	{
		using (StreamReader reader = new StreamReader(file))
		{
			string[] words = reader.ReadToEnd().Split(' ', '\n');

			int rows = Int32.Parse(words[0]);
			int columns = Int32.Parse(words[0]);

			PuzzleState state = new PuzzleState(rows, columns);

			for (int row = 0; row < rows; row++)
			{
				for (int col = 0; col < columns; col++)
				{
					int value = Int32.Parse(words[(row * columns) + col + 2]);
					state.SetCell(row, col, value);
				}
			}
			return state;
		}
	}

	static void DrawBoard(PuzzleState state)
	{
		int rows = state.Rows;
		int columns = state.Columns;
		String line = "";
		for (int row = 0; row < rows; row++)
		{
			line = "";
			for (int col = 0; col < columns; col++)
			{
				line += state.GetCell(row, col).ToString();
				line += " ";
			}
			Console.WriteLine(line);
		}
	}
}