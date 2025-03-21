public class MainClass
{
	static void Main(string[] args)
	{
		Console.WriteLine(args[0]);
	}

	//  strategia "wszerz" z porządkiem przeszukiwania sąsiedztwa prawo-dół-góra-lewo:
	// program bfs RDUL 4x4_01_0001.txt 4x4_01_0001_bfs_rdul_sol.txt 4x4_01_0001_bfs_rdul_stats.txt
	// strategia "w głąb" z porządkiem przeszukiwania sąsiedztwa lewo-góra-dół-prawo:
	// program dfs LUDR 4x4_01_0001.txt 4x4_01_0001_dfs_ludr_sol.txt 4x4_01_0001_dfs_ludr_stats.txt
	// strategia A* z heurystyką w postaci metryki Manhattan:
	// program astr manh 4x4_01_0001.txt 4x4_01_0001_astr_manh_sol.txt 4x4_01_0001_astr_manh_stats.txt

}