using System.ComponentModel;
using System.Reflection.PortableExecutable;
using System.Xml;
using System.Security.Cryptography;
using System.Net;
using System.Numerics;
public class MainClass
{
	static void Main(string[] args)
	{
		string[] lines;
		using (var sr = new StreamReader(args[0]))
		{
			lines = sr.ReadToEnd().Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
		}
		Dictionary <int, List<string>> depthValue = new Dictionary<int, List<string>>();
		foreach (string line in lines)
		{
			string[] data = line.Split(' ');
			if (!depthValue.ContainsKey(Int32.Parse(data[0])))
			{
				depthValue[Int32.Parse(data[0])] = new List<string>();
				depthValue[Int32.Parse(data[0])].Add(line);
			}
			depthValue[Int32.Parse(data[0])].Add(line);
		}
		foreach (int i in depthValue.Keys)
		{
			Console.WriteLine("");
			Console.WriteLine("DEPTH: " + i);
			Dictionary<string, int> number = new Dictionary<string, int>(); ;
			Dictionary<string, double> length = new Dictionary<string, double>();
			Dictionary<string, double> visited = new Dictionary<string, double>();
			Dictionary<string, double> processed = new Dictionary<string, double>();
			Dictionary<string, double> depth = new Dictionary<string, double>();
			Dictionary<string, double> time = new Dictionary<string, double>();
			foreach (string line in depthValue[i])
			{
				string[] data = line.Split(" ");
				string n = data[2];
				if (n == "astr") n = data[3];
				if (number.ContainsKey(n))
				{
					number[n]++;
					length[n] += Double.Parse(data[4]);
					visited[n] += Double.Parse(data[5]);
					processed[n] += Double.Parse(data[6]);
					depth[n] += Double.Parse(data[7]);
					time[n] += Double.Parse(data[8]);
				}
				else
				{
					number[n] = 1;
					length[n] = Double.Parse(data[4]);
					visited[n] = Double.Parse(data[5]);
					processed[n] = Double.Parse(data[6]);
					depth[n] = Double.Parse(data[7]);
					time[n] = Double.Parse(data[8]);
				}
			}
			foreach (string x in number.Keys)
			{
				length[x] /= number[x];
				visited[x] /= number[x];
				processed[x] /= number[x];
				depth[x] /= number[x];
				time[x] /= number[x];
			}
			Console.WriteLine("Śr. dł. rozwiązania & {0} & {1} & {2} & {3} \\\\ \\hline", length["bfs"].ToString("F0"), length["dfs"].ToString("F0"), length["hamm"].ToString("F0"), length["manh"].ToString("F0"));
			Console.WriteLine("Śr. odwiedzonych & {0} & {1} & {2} & {3} \\\\ \\hline", visited["bfs"].ToString("F0"), visited["dfs"].ToString("F0"), visited["hamm"].ToString("F0"), visited["manh"].ToString("F0"));
			Console.WriteLine("Śr. przetworzonych & {0} & {1} & {2} & {3} \\\\ \\hline", processed["bfs"].ToString("F0"), processed["dfs"].ToString("F0"), processed["hamm"].ToString("F0"), processed["manh"].ToString("F0"));
			Console.WriteLine("Śr. maks. głębokość & {0} & {1} & {2} & {3} \\\\ \\hline", depth["bfs"].ToString("F0"), depth["dfs"].ToString("F0"), depth["hamm"].ToString("F0"), depth["manh"].ToString("F0"));
			Console.WriteLine("Śr. czas & {0} & {1} & {2} & {3} \\\\ \\hline", time["bfs"].ToString("F3"), time["dfs"].ToString("F3"), time["hamm"].ToString("F3"), time["manh"].ToString("F3"));
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
}