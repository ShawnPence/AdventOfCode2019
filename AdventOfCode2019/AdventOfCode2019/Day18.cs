using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace AdventOfCode2019
{
	class Day18
	{
		public static void Problems()
		{


			Console.WriteLine("Input file:");
			string fileName = Console.ReadLine();
			while (!File.Exists(fileName))
			{
				Console.WriteLine("file not found - try again:");
				fileName = Console.ReadLine();
			}

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			List<string> maze = new List<string>();
			using (StreamReader sr = new StreamReader(fileName))
			{
				while (sr.Peek() != -1)
					maze.Add(sr.ReadLine());
			}
			Dictionary<char, int> positions = new Dictionary<char, int>();
			HashSet<char> lowers = new HashSet<char>();
			//find start and all letters
			for (int y = 0; y < maze.Count; y++)
				for (int x = 0; x < maze[y].Length; x++)
				{
					char c = maze[y][x];
					if (c != '#' && c != '.')
					{
						positions[c] = x * 100 + y;
						if (c - 'a' >= 0) lowers.Add(c);
					}
				}

			RemoveDeadEnds(maze, positions);

			//SaveMapCopy(fileName, "_a", maze); //save copy of map with dead ends blocked off


			//build graph
			var graph = new Dictionary<char, Dictionary<char, StepsToPoint>>();
			BuildGraph(maze, positions, graph);

			int minSteps = Int32.MaxValue;

			minSteps = FindMinSteps(lowers, graph, minSteps);

			stopwatch.Stop();
			Console.WriteLine($"Part 1 answer: {minSteps}");
			Console.WriteLine($"part 1 time (ms): {stopwatch.ElapsedMilliseconds}");

			//part 2
			stopwatch.Restart();


			ModifyMazePart2(maze, positions);
			//SaveMapCopy(fileName, "_b", maze);


			//rebuild graph
			graph.Clear();
			BuildGraph(maze, positions, graph);
			minSteps = Int32.MaxValue;
			minSteps = FindMinSteps2(lowers, graph, minSteps);

			stopwatch.Stop();
			Console.WriteLine($"Part 2 answer: {minSteps}");
			Console.WriteLine($"part 2 time (ms): {stopwatch.ElapsedMilliseconds}");


		}

		/// <summary>
		/// separate the maze into 4 quadrants per part 2 instructions
		/// </summary>
		/// <param name="maze"></param>
		/// <param name="positions"></param>
		private static void ModifyMazePart2(List<string> maze, Dictionary<char, int> positions)
		{
			// add 4 @ to map at current @ +- 1 x and y
			int atX = positions['@'] / 100;
			int atY = positions['@'] % 100;
			StringBuilder above = new StringBuilder(maze[atY - 1]);
			StringBuilder center = new StringBuilder(maze[atY]);
			StringBuilder below = new StringBuilder(maze[atY + 1]);
			above.Remove(atX - 1, 3);
			above.Insert(atX - 1, "@#@");
			center.Remove(atX - 1, 3);
			center.Insert(atX - 1, "###");
			below.Remove(atX - 1, 3);
			below.Insert(atX - 1, "@#@");
			maze[atY - 1] = above.ToString();
			maze[atY] = center.ToString();
			maze[atY + 1] = below.ToString();
		}

		private static int FindMinSteps(HashSet<char> lowers, Dictionary<char, Dictionary<char, StepsToPoint>> graph, int minSteps)
		{
			Stack<ToHere> attempts = new Stack<ToHere>();
			Dictionary<long, int> bestToHere = new Dictionary<long, int>();// bitmask of keys found a = 1, b = 2, c = 4, etc., plus current key point as (key << 32)
			ToHere start = new ToHere() { current = '@' };
			attempts.Push(start);
			while (attempts.Count > 0)
			{
				var currentPos = attempts.Pop();
				List<NextStepCandidate> possible = new List<NextStepCandidate>();
				foreach (char destination in lowers)
				{
					//if not already visited destination and can get from source to destination with current set of keys
					if (((currentPos.keys & (1 << (destination - 'a'))) == 0) && (graph[currentPos.current][destination].doors & currentPos.keys) == graph[currentPos.current][destination].doors)
					{
						NextStepCandidate nextStep = new NextStepCandidate();
						nextStep.val = destination;
						nextStep.steps = currentPos.steps + graph[currentPos.current][destination].steps;
						long state = currentPos.keys + (1L << (destination - 'a')) + (1L << ((destination - 'a') + 32));

						//only add this as a candidate next step if the combination of collected keys and ending position take less steps to reach than previously found for that combination
						if (nextStep.steps < minSteps && (!bestToHere.ContainsKey(state) || bestToHere[state] > nextStep.steps)) possible.Add(nextStep);
					}

				}
				possible.Sort((NextStepCandidate x, NextStepCandidate y) => { return x.steps.CompareTo(y.steps); });
				possible.Reverse(); //put the longest options on the stack first so that shortest possible are tried first;
				foreach (var nextStep in possible)
				{
					var destination = nextStep.val;
					var next = new ToHere();
					next.current = destination;
					next.keys = currentPos.keys;
					next.keys += 1 << (destination - 'a');
					next.steps = currentPos.steps;
					next.steps += graph[currentPos.current][destination].steps;
					if (next.keys == 67108863) //lowest 26 bits all 1
					{
						minSteps = Math.Min(minSteps, next.steps);
					}
					else if (next.steps < minSteps)
					{
						long state = currentPos.keys + (1L << (destination - 'a')) + (1L << ((destination - 'a') + 32));
						bestToHere[state] = next.steps;
						attempts.Push(next);
					}
				}

			}

			return minSteps;
		}

		/// <summary>
		/// Modified version of FindMinSteps to deal with 4 separate quadrants
		/// </summary>
		/// <returns></returns>
		private static int FindMinSteps2(HashSet<char> lowers, Dictionary<char, Dictionary<char, StepsToPoint>> graph, int minSteps)
		{
			Stack<ToHere2> attempts = new Stack<ToHere2>();
			Dictionary<long, int> bestToHere = new Dictionary<long, int>();// bitmask of keys found a = 1, b = 2, c = 4, etc., plus current key points as (key << 32)
			ToHere2 start = new ToHere2();
			attempts.Push(start);
			while (attempts.Count > 0)
			{
				var currentPos = attempts.Pop();
				

				List<NextStepCandidate> possible = new List<NextStepCandidate>();
				foreach (char destination in lowers)
				{
					if((currentPos.keys & (1 << (destination - 'a'))) == 0) //if not already visited destination
					{
						//find same quadrant character - if there's a non '@' character, assume that is the correct choice
						char quadChar = ' ';
						foreach (char possibleQ in currentPos.current)
						{
							if ((possibleQ != '@' || quadChar == ' ') && graph[possibleQ].ContainsKey(destination)) quadChar = possibleQ;
						}


						//if can get from source to destination with current set of keys
						if ((graph[quadChar][destination].doors & currentPos.keys) == graph[quadChar][destination].doors)
						{
							NextStepCandidate nextStep = new NextStepCandidate();
							nextStep.val = destination;
							nextStep.steps = currentPos.steps + graph[quadChar][destination].steps;
							long state = currentPos.keys + (1L << (destination - 'a')) + (1L << ((destination - 'a') + 32));

							//only add this as a candidate next step if the combination of collected keys and ending position take less steps to reach than previously found for that combination
							if (nextStep.steps < minSteps && (!bestToHere.ContainsKey(state) || bestToHere[state] > nextStep.steps)) possible.Add(nextStep);
						} 
					}

				}
				possible.Sort((NextStepCandidate x, NextStepCandidate y) => { return x.steps.CompareTo(y.steps); });
				possible.Reverse(); //put the longest options on the stack first so that shortest possible are tried first;
				foreach (var nextStep in possible)
				{
					var destination = nextStep.val;
					char quadChar = ' ';
					foreach (char possibleQ in currentPos.current)
					{
						if ((possibleQ != '@' || quadChar == ' ') && graph[possibleQ].ContainsKey(destination)) quadChar = possibleQ;
					}
					var next = new ToHere2();
					next.current = new List<char>(currentPos.current);
					if (quadChar == '@')
					{
						if (next.current.Count == 4) next.current.Remove(quadChar); //only remove @ once all 4 quadrants have been accessed
						next.current.Add(destination);
					}
					else
					{
						next.current.Remove(quadChar);
						next.current.Add(destination);
					}

					next.keys = currentPos.keys;
					next.keys += 1 << (destination - 'a');
					next.steps = currentPos.steps;
					next.steps += graph[quadChar][destination].steps;
					if (next.keys == 67108863) //lowest 26 bits all 1
					{
						minSteps = Math.Min(minSteps, next.steps);
					}
					else if (next.steps < minSteps)
					{
						long state = currentPos.keys + (1L << (destination - 'a')) + (1L << ((destination - 'a') + 32));
						bestToHere[state] = next.steps;
						attempts.Push(next);
					}
				}

			}

			return minSteps;
		}



		private static void SaveMapCopy(string fileName, string appendString, List<string> maze)
		{

			//save temp map with all dead ends blocked
			string tempFile = fileName.Replace(".txt", $"{appendString}.txt", StringComparison.OrdinalIgnoreCase);
			using StreamWriter sw = new StreamWriter(tempFile, false);
			foreach (string line in maze) sw.WriteLine(line);
			sw.Flush();
		}

		private static void RemoveDeadEnds(List<string> lines, Dictionary<char, int> positions)
		{
			//walk maze and find all dead ends
			Queue<int> locations = new Queue<int>();
			HashSet<int> visited = new HashSet<int>();
			Queue<int> deadEnds = new Queue<int>();
			locations.Enqueue(positions['@']);
			while (locations.Count > 0)
			{
				var location = locations.Dequeue();
				int ways = 0;
				int x = location / 100;
				int y = location % 100;

				for (int i = 0; i < 4; i++)
				{
					int nextX = x;
					int nextY = y;
					switch (i)
					{
						case 0: //up
							nextY--;
							break;
						case 1: //down
							nextY++;
							break;
						case 2: //left
							nextX--;
							break;
						case 3: //right
							nextX++;
							break;

					}
					bool canMove = (nextY >= 0 && nextY < lines.Count && nextX >= 0 && nextX < lines[nextY].Length);

					if (canMove && !visited.Contains(nextX * 100 + nextY) && lines[nextY][nextX] != '#')
					{
						visited.Add(nextX * 100 + nextY);
						locations.Enqueue(nextX * 100 + nextY);
					}
					if (lines[nextY][nextX] != '#') ways++;
				}
				if (ways == 1 && lines[y][x] == '.') deadEnds.Enqueue(location); //can only move in one direction from this spot - this is a dead end

			}//while next.count > 0;
			while (deadEnds.Count > 0)
			{
				var deadEnd = deadEnds.Dequeue();
				int x = deadEnd / 100;
				int y = deadEnd % 100;
				StringBuilder l1 = new StringBuilder();
				l1.Append(lines[y]);
				l1.Remove(x, 1);
				l1.Insert(x, '#');
				lines[y] = l1.ToString();
				//find one direction that can move
				for (int i = 0; i < 4; i++)
				{
					int nextX = x;
					int nextY = y;
					switch (i)
					{
						case 0: //up
							nextY--;
							break;
						case 1: //down
							nextY++;
							break;
						case 2: //left
							nextX--;
							break;
						case 3: //right
							nextX++;
							break;
					}
					bool canMove = (nextY >= 0 && nextY < lines.Count && nextX >= 0 && nextX < lines[nextY].Length);

					if (canMove)
					{
						//check if that is now a dead end
						int open = 0;
						if (nextY - 1 >= 0 && lines[nextY - 1][nextX] != '#') open++;
						if (nextY + 1 < lines.Count && lines[nextY + 1][nextX] != '#') open++;
						if (nextX - 1 >= 0 && lines[nextY][nextX - 1] != '#') open++;
						if (nextX + 1 < lines[nextY].Length && lines[nextY][nextX + 1] != '#') open++;
						if (open < 2 && lines[nextY][nextX] == '.') deadEnds.Enqueue(nextX * 100 + nextY);
					}
				}//move dir for loop
			}//while deadend count
		}

		private static void BuildGraph(List<string> maze, Dictionary<char, int> pos, Dictionary<char, Dictionary<char, StepsToPoint>> graph)
		{
			foreach (char source in pos.Keys)
			{
				if (source >= 'a' || source == '@')
				{
					foreach (char destination in pos.Keys)
					{
						if (destination != source && (destination >= 'a' || destination == '@') && (!graph.ContainsKey(source) || !graph[source].ContainsKey(destination)))
						{
							StepsToPoint best = ShortestPath(pos[source], pos[destination], maze);
							if (best.steps != 0)
							{
								if (!graph.ContainsKey(source)) graph[source] = new Dictionary<char, StepsToPoint>();
								if (!graph.ContainsKey(destination)) graph[destination] = new Dictionary<char, StepsToPoint>();
								graph[source][destination] = best;
								graph[destination][source] = best;
							}
						}
					}
				}
			}//build graph
		}

		static StepsToPoint ShortestPath(int start, int destination, List<string> maze)
		{

			//search maze for best path
			Queue<StepsToPoint> paths = new Queue<StepsToPoint>();
			StepsToPoint point = new StepsToPoint{ x = start / 100, y = start % 100 };
			if (maze[point.y][point.x] != '#')
			{
				paths.Enqueue(point);
			}
			else
			{
				//if the starting point is a wall, enqueue the the 4 '@' in part 2:
				paths.Enqueue(new StepsToPoint() { x = point.x - 1, y = point.y - 1 });
				paths.Enqueue(new StepsToPoint() { x = point.x + 1, y = point.y - 1 });
				paths.Enqueue(new StepsToPoint() { x = point.x - 1, y = point.y + 1 });
				paths.Enqueue(new StepsToPoint() { x = point.x + 1, y = point.y + 1 });
			}

			HashSet<int> found = new HashSet<int> { start };

			while (paths.Count > 0)
			{

				var path = paths.Dequeue();
				for (int i = 0; i < 4; i++)
				{
					int nextX = path.x;
					int nextY = path.y;
					switch (i)
					{
						case 0:	//up
							nextY--;
							break;
						case 1:	//down
							nextY++;
							break;
						case 2:	//left
							nextX--;
							break;
						case 3:	//right
							nextX++;
							break;
					}

					bool canMove = (nextY >= 0 && nextY < maze.Count && nextX >= 0 && nextX < maze[nextY].Length);
					if (canMove && maze[nextY][nextX] != '#' && (!found.Contains(nextX * 100 + nextY) || (maze[path.y][path.x] - 'a' >= 0)))
					{
						StepsToPoint next = new StepsToPoint();
						next.steps = path.steps + 1;
						next.x = nextX;
						next.y = nextY;
						next.doors = path.doors;

						if ((next.x * 100 + next.y) == destination)
						{
							return next;
						}
						found.Add(nextX * 100 + nextY);
						if (maze[nextY][nextX] >= 'A' && maze[nextY][nextX] <= 'Z')
						{
							next.doors += 1 << (maze[nextY][nextX] - 'A');
						}
						paths.Enqueue(next);
					}
				}
			}//while
			return new StepsToPoint();
		}

		/// <summary>
		/// steps and doors up to the current point
		/// </summary>
		class StepsToPoint
		{
			/// <summary>
			/// doors passed between points - stored as bitmask a=1, b=2, c=4, etc.
			/// </summary>
			public int doors = 0;

			public int steps;
			
			public int x;
			
			public int y;
		}


		/// <summary>
		/// current point, steps taken, and all keys collected
		/// </summary>
		class ToHere
		{
			public char current;
			public int keys = 0;
			public int steps = 0;
		}


		/// <summary>
		/// current points in all 4 quadrants, total steps taken, and all keys collected
		/// </summary>
		class ToHere2
		{
			public List<char> current = new List<char>(){'@'};
			public int keys = 0;
			public int steps = 0;
		}

		/// <summary>
		/// possible next step key and step count
		/// </summary>
		class NextStepCandidate
		{
			public char val;
			public int steps = 0;
		}


	}
}
