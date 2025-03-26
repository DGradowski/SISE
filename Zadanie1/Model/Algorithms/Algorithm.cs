using Puzzle;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
	abstract public class Algorithm
	{
		// długość znalezionego rozwiązania;
		private int solutionLength;
		// liczbę stanów odwiedzonych i przetworzonych;
		private int checkedStates;
		// liczbę stanów odwiedzonych i przetworzonych;
		private int processedStates;
		// maksymalną osiągniętą głębokość rekursji;
		private int recursionDepth;
		// czas trwania procesu obliczeniowego.
		private long time;

		public int SolutionLength { get => solutionLength; set => solutionLength = value; }
		public int CheckedStates { get => checkedStates; set => checkedStates = value; }
		public int RecursionDepth { get => recursionDepth; set => recursionDepth = value; }
		public long Time { get => time; set => time = value; }
		public int ProcessedStates { get => processedStates; set => processedStates = value; }

		abstract public PuzzleState FindSolution(PuzzleState state);
	}
}
