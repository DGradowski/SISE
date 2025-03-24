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
		// maksymalną osiągniętą głębokość rekursji;
		private int rekursionDepth;
		// czas trwania procesu obliczeniowego.
		private double time;

		protected Algorithm()
		{
		}

		abstract public PuzzleState FindSolution(PuzzleState state);
	}
}
