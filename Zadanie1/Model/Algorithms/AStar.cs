using Heuristics;
using Puzzle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
	public class AStar : Algorithm
	{
		private Heuristic heuristic;
		String order = "LDRU";
		int moveScale;
		public AStar(Heuristic heuristic, int moveScale = 1)
		{
			this.heuristic = heuristic;
			this.moveScale = moveScale;
		}

		public override PuzzleState FindSolution(PuzzleState state)
		{
			RecursionDepth = 0;
			var watch = System.Diagnostics.Stopwatch.StartNew();
			Dictionary<string, (PuzzleState state, int value)> visited = new Dictionary<string, (PuzzleState, int)>();
			Dictionary<string, (PuzzleState state, int value)> paths = new Dictionary<string, (PuzzleState, int)>();
			List<PuzzleState> bestStates = new List<PuzzleState>();
			Random rng = new Random();

			while (!state.IsSolved())
			{
				String key = state.BoardToKey();
				visited.Add(key, (state, state.Moves.Length * moveScale + heuristic.CalculatePriority(state)));
				if (paths.ContainsKey(key)) paths.Remove(key);

				foreach (char d in order)
				{
					if (!state.IsMoveLegal(d)) continue;
					ProcessedStates++;
					PuzzleState newState = new PuzzleState(state, d);
					if (visited.ContainsKey(newState.BoardToKey())) continue;
					if (paths.ContainsKey(newState.BoardToKey()))
					{
						PuzzleState s = paths[newState.BoardToKey()].state;
						if (s.Moves.Length > newState.Moves.Length)
						{
							paths[newState.BoardToKey()] = (newState, newState.Moves.Length * moveScale + heuristic.CalculatePriority(newState));
						}
					}
					else
					{
						int v = newState.Moves.Length * moveScale + heuristic.CalculatePriority(newState);
						paths.Add(newState.BoardToKey(), (newState, v));
					}
				}
				
				bestStates.Clear();
				string[] keys = paths.Keys.ToArray();
				int bestValue = paths[keys[0]].value;
				foreach (string k in keys)
				{
					if (paths[k].value == bestValue)
					{
						bestStates.Add(paths[k].state);
					}
					else if (paths[k].value < bestValue)
					{
						bestStates.Clear();
						bestValue = paths[k].value;
						bestStates.Add(paths[k].state);
					}
				}
				state = bestStates[rng.Next(bestStates.Count)];
				if (RecursionDepth < state.Moves.Length) RecursionDepth = state.Moves.Length;
			}
			watch.Stop();
			Time = watch.Elapsed.TotalMilliseconds;
			CheckedStates = visited.Count;
			SolutionLength = state.Moves.Length;
			return state;
		}
	}
}
