using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AdventOfCode2019
{
	class Day20
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

			Dictionary<int, int> warps = new Dictionary<int, int>();
			ProcessMap(maze, warps,out int start,out int finish);
			Console.WriteLine(ShortestPath(start, finish, maze, warps));
			Console.WriteLine(ShortestPath(start, finish, maze, warps, false));
			stopwatch.Stop();
			Console.WriteLine($"time to complete (ms): {stopwatch.ElapsedMilliseconds}");
		}

		/// <summary>
		/// find all warp points and the start/end points
		/// </summary>
		static void ProcessMap(List<string> maze, Dictionary<int, int> warps, out int Start, out int Finish)
		{
			Start = 0;
			Finish = 0;
			Dictionary<string, List<WarpLocation>> warpCodes = new Dictionary<string, List<WarpLocation>>();
			HashSet<int> warpPoints = new HashSet<int>();
			for (int y = 0; y < maze.Count; y++)
			{
				for (int x = 0; x < maze[y].Length; x++)
				{

					//if this is a letter, check right or down and add it to warp letters.
					if ((maze[y][x] - 'A') >= 0 && !warpPoints.Contains(x * 1000 + y))
					{
						int nearExit;
						int enter;
						string warpCode;
						//check right(if possible)
						if (x + 1 < maze[y].Length && maze[y][x + 1] - 'A' >= 0)
						{
							warpCode = maze[y].Substring(x, 2);
							//find . destination

							if (x > 0 && maze[y][x - 1] == '.')
							{
								nearExit = (x - 1) * 1000 + y;
								enter = x * 1000 + y;
							}
							else
							{
								nearExit = (x + 2) * 1000 + y;
								enter = (x + 1) * 1000 + y;
							}
							warpPoints.Add(x * 1000 + y);
							warpPoints.Add((x + 1) * 1000 + y);
							x++;
						}
						else
						{
							//vertical
							warpCode = maze[y].Substring(x,1) + maze[y+1].Substring(x,1);
							//find . destination

							if (y > 0 && maze[y - 1][x] == '.')
							{
								nearExit = x * 1000 + y - 1;
								enter = x * 1000 + y;
							}
							else
							{
								nearExit = x * 1000 + y + 2;
								enter = x * 1000 + y + 1;
							}
							warpPoints.Add(x * 1000 + y);
							warpPoints.Add(x * 1000 + y + 1);
						}
						if (warpCode == "AA") Start = nearExit;
						if (warpCode == "ZZ") Finish = nearExit;
						if (!warpCodes.ContainsKey(warpCode)) warpCodes.Add(warpCode, new List<WarpLocation>());
						warpCodes[warpCode].Add(new WarpLocation() { nearExit = nearExit, enter = enter });
					}
				}
			}
			foreach (string key in warpCodes.Keys)
			{
				if (key != "AA" && key != "ZZ")
				{
					warps[warpCodes[key][0].enter] = warpCodes[key][1].nearExit;
					warps[warpCodes[key][1].enter] = warpCodes[key][0].nearExit;
				}
			}
		}

		//determine if a warp point is on the inside of the maze
		static bool IsInnerWarp(int x, int y, List<string> maze)
		{
			if (x < 2) return false;
			if (y < 2) return false;
			if (x > maze[0].Length - 3) return false;
			if (y > maze.Count - 3) return false;
			return true;
		}

		static int ShortestPath(int start, int destination, List<string> maze, Dictionary<int, int> warps, bool ignoreZ = true)
		{

			//search maze for best path

			Queue<StepsToPoint> paths = new Queue<StepsToPoint>();
			
			StepsToPoint point = new StepsToPoint { x = start / 1000, y = start % 1000 };
			
			paths.Enqueue(point);

			HashSet<int> found = new HashSet<int> { start }; //z*1000000 + x*1000 + y

			while (paths.Count > 0)
			{

				var path = paths.Dequeue();
				for (int i = 0; i < 4; i++)
				{
					int nextX = path.x;
					int nextY = path.y;
					int nextZ = path.z;
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
					//if next is a warp, replace it with its warp endpoint
					int nextIntXY = nextX * 1000 + nextY;
					if (warps.ContainsKey(nextIntXY) && (ignoreZ || nextZ != 0 || IsInnerWarp(nextX,nextY, maze))) //for part 2 (ignoreZ==false), only use warp if it is an inner warp point or z != 0
					{
						if(!ignoreZ) nextZ += IsInnerWarp(nextX, nextY, maze) ? 1 : -1;
						nextIntXY = warps[nextIntXY];
						nextX = nextIntXY / 1000;
						nextY = nextIntXY % 1000;
					}

					bool canMove = (nextY >= 0 && nextY < maze.Count && nextX >= 0 && nextX < maze[nextY].Length);
					if (canMove && maze[nextY][nextX] == '.' && !found.Contains(nextZ * 1000000 + nextX * 1000 + nextY))
					{
						StepsToPoint next = new StepsToPoint
						{
							steps = path.steps + 1,
							x = nextX,
							y = nextY,
							z = nextZ
						};

						if ((next.x * 1000 + next.y) == destination && next.z == 0)
						{
							return next.steps;
						}
						found.Add(nextZ * 1000000 + nextX * 1000 + nextY);
						paths.Enqueue(next);
					}
				}
			}//while
			return 0;
		}



		/// <summary>
		/// steps up to the current point
		/// </summary>
		class StepsToPoint
		{

			public int steps = 0;

			public int x;

			public int y;

			public int z = 0;
		}

		class WarpLocation
		{
			public int enter; //letter that would enter the warp
			public int nearExit; //exit '.' next to letter at enter point (not the destination of the warp)
		}
	}
}
