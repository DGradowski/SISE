using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
	abstract public class Algorithm
	{
		// długość znalezionego rozwiązania;
		private int solutionLength;
		// liczbę stanów odwiedzonych;
		private int checkedStates;
		// liczbę stanów przetworzonych;
		private int idkStates;
		// maksymalną osiągniętą głębokość rekursji;
		private int rekursionDepth;
		// czas trwania procesu obliczeniowego.
		private double time;
	}
}
