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
			Random rng = new Random();

			while (!state.IsSolved())
			{
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
				List<PuzzleState> bestStates = new List<PuzzleState>();
				string[] keys = paths.Keys.ToArray();
				int bestValue = paths[keys[0]].value;
				foreach (string key in keys)
				{
					if (paths[key].value == bestValue)
					{
						bestStates.Add(paths[key].state);
					}
					else if (paths[key].value < bestValue)
					{
						bestStates.Clear();
						bestValue = paths[key].value;
						bestStates.Add(paths[key].state);
					}
				}
				if (!visited.ContainsKey(state.BoardToKey()))
				{
					visited.Add(state.BoardToKey(), (state, state.Moves.Length * moveScale + heuristic.CalculatePriority(state)));
				}
				if (paths.ContainsKey(state.BoardToKey())) paths.Remove(state.BoardToKey());
				state = bestStates[rng.Next(bestStates.Count)];
				if (RecursionDepth < state.Moves.Length) RecursionDepth = state.Moves.Length;
			}
			Time = watch.ElapsedMilliseconds;
			CheckedStates = visited.Count;
			SolutionLength = state.Moves.Length;
			return state;
		}
	}
}
