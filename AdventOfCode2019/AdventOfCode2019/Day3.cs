using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode2019
{
	class Day3
	{
		public static void Problem1()
		{
			Console.WriteLine("Input file:");
			string fileName = Console.ReadLine();
			while (!File.Exists(fileName))
			{
				Console.WriteLine("file not found - try again:");
				fileName = Console.ReadLine();
			}

			Dictionary<int, HashSet<int>> coords = new Dictionary<int, HashSet<int>>();
			int closest = Int32.MaxValue;
			int currentY = 0;
			int currentX = 0;
			coords[0] = new HashSet<int>(new int[]{ 0});
			using (StreamReader sr = new StreamReader(fileName, true))
			{
				foreach (string s in sr.ReadLine().Split(','))
				{
					switch (s[0])
					{
						case 'U':
							int moveU = Convert.ToInt32(s.Substring(1));
							for (int i = 0; i <= moveU; i++)
							{
								coords[currentX].Add(currentY + i);
							}
							currentY += moveU;
							break;
						case 'D':
							int moveD = Convert.ToInt32(s.Substring(1));
							for (int i = 0; i <= moveD; i++)
							{
								coords[currentX].Add(currentY - i);
							}
							currentY -= moveD;
							break;
						case 'L':
							int moveL = Convert.ToInt32(s.Substring(1));
							for (int i = 0; i <= moveL; i++)
							{
								if (!coords.ContainsKey(currentX - i)) coords[currentX - i] = new HashSet<int>();
								coords[currentX - i].Add(currentY);
							}
							currentX -= moveL;
							break;
						case 'R':
							int moveR = Convert.ToInt32(s.Substring(1));
							for (int i = 0; i <= moveR; i++)
							{
								if (!coords.ContainsKey(currentX + i)) coords[currentX + i] = new HashSet<int>();
								coords[currentX + i].Add(currentY);
							}
							currentX += moveR;
							break;

					}



				}

				//second line
				currentX = 0;
				currentY = 0;
				foreach (string s in sr.ReadLine().Split(','))
				{
					switch (s[0])
					{
						case 'U':
							int moveU = Convert.ToInt32(s.Substring(1));
							for (int i = 0; i <= moveU; i++)
							{
								if ((currentX != 0 || currentY != 0) && coords.ContainsKey(currentX) && coords[currentX].Contains(currentY + i))
								{
									int tempX = Math.Abs(currentX);
									int tempY = Math.Abs(currentY + i);
									closest = Math.Min(closest, tempX + tempY);
								}

							}
							currentY += moveU;
							break;
						case 'D':
							int moveD = Convert.ToInt32(s.Substring(1));
							for (int i = 0; i <= moveD; i++)
							{
								if ((currentX != 0 || currentY != 0) && coords.ContainsKey(currentX) && coords[currentX].Contains(currentY - i))
								{
									int tempX = Math.Abs(currentX);
									int tempY = Math.Abs(currentY - i);
									closest = Math.Min(closest, tempX + tempY);
								}

							}
							currentY -= moveD;
							break;
						case 'L':
							int moveL = Convert.ToInt32(s.Substring(1));
							for (int i = 0; i <= moveL; i++)
							{
								if ((currentX != 0 || currentY != 0) && coords.ContainsKey(currentX - i) && coords[currentX - i].Contains(currentY))
								{
									int tempX = Math.Abs(currentX - i);
									int tempY = Math.Abs(currentY);
									closest = Math.Min(closest, tempX + tempY);
								}

							}
							currentX -= moveL;
							break;
						case 'R':
							int moveR = Convert.ToInt32(s.Substring(1));
							for (int i = 0; i <= moveR; i++)
							{
								if ((currentX != 0 || currentY != 0) && coords.ContainsKey(currentX + i) && coords[currentX + i].Contains(currentY))
								{
									int tempX = Math.Abs(currentX + i);
									int tempY = Math.Abs(currentY);
									closest = Math.Min(closest, tempX + tempY);
								}

							}
							currentX += moveR;
							break;

					}



				}

			}

			Console.WriteLine(closest.ToString());

		}

		public static void Problem2()
		{
			Console.WriteLine("Input file:");
			string fileName = Console.ReadLine();
			while (!File.Exists(fileName))
			{
				Console.WriteLine("file not found - try again:");
				fileName = Console.ReadLine();
			}

			Dictionary<int, Dictionary<int,int>> coords = new Dictionary<int, Dictionary<int,int>>();
			int minSteps = Int32.MaxValue;
			int currentY = 0;
			int currentX = 0;
			coords[0] = new Dictionary<int, int>();
			coords[0][0] = 0;
			int steps = 0;
			using (StreamReader sr = new StreamReader(fileName, true))
			{
				foreach (string s in sr.ReadLine().Split(','))
				{
					switch (s[0])
					{
						case 'U':
							int moveU = Convert.ToInt32(s.Substring(1));
							for (int i = 1; i <= moveU; i++)
							{
								steps++;
								if (!coords[currentX].ContainsKey(currentY + i)) coords[currentX][currentY + i] = steps;

							}
							currentY += moveU;
							break;
						case 'D':
							int moveD = Convert.ToInt32(s.Substring(1));
							for (int i = 1; i <= moveD; i++)
							{
								steps++;
								if (!coords[currentX].ContainsKey(currentY - i)) coords[currentX][currentY - i] = steps;

							}
							currentY -= moveD;
							break;
						case 'L':
							int moveL = Convert.ToInt32(s.Substring(1));
							for (int i = 1; i <= moveL; i++)
							{
								steps++;
								if (!coords.ContainsKey(currentX - i)) coords[currentX - i] = new Dictionary<int,int>();
								if (!coords[currentX - i].ContainsKey(currentY)) coords[currentX - i][currentY] = steps;

							}
							currentX -= moveL;
							break;
						case 'R':
							int moveR = Convert.ToInt32(s.Substring(1));
							for (int i = 1; i <= moveR; i++)
							{
								steps++;
								if (!coords.ContainsKey(currentX + i)) coords[currentX + i] = new Dictionary<int, int>();
								if (!coords[currentX + i].ContainsKey(currentY)) coords[currentX + i][currentY] = steps;

							}
							currentX += moveR;
							break;

					}



				}

				//second line
				currentX = 0;
				currentY = 0;
				steps = 0;
				foreach (string s in sr.ReadLine().Split(','))
				{
					switch (s[0])
					{
						case 'U':
							int moveU = Convert.ToInt32(s.Substring(1));
							for (int i = 1; i <= moveU; i++)
							{
								steps++;
								if ((currentX != 0 || currentY != 0) && coords.ContainsKey(currentX) && coords[currentX].ContainsKey(currentY + i))
								{
									minSteps = Math.Min(minSteps, steps + coords[currentX][currentY + i]);
								}

							}
							currentY += moveU;
							break;
						case 'D':
							int moveD = Convert.ToInt32(s.Substring(1));
							for (int i = 1; i <= moveD; i++)
							{
								steps++;
								if ((currentX != 0 || currentY != 0) && coords.ContainsKey(currentX) && coords[currentX].ContainsKey(currentY - i))
								{
									minSteps = Math.Min(minSteps, steps + coords[currentX][currentY - i]);

								}


							}
							currentY -= moveD;
							break;
						case 'L':
							int moveL = Convert.ToInt32(s.Substring(1));
							for (int i = 1; i <= moveL; i++)
							{
								steps++;
								if ((currentX != 0 || currentY != 0) && coords.ContainsKey(currentX - i) && coords[currentX - i].ContainsKey(currentY))
								{
									minSteps = Math.Min(minSteps, steps + coords[currentX - i][currentY]);

								}


							}
							currentX -= moveL;
							break;
						case 'R':
							int moveR = Convert.ToInt32(s.Substring(1));
							for (int i = 1; i <= moveR; i++)
							{
								steps++;
								if ((currentX != 0 || currentY != 0) && coords.ContainsKey(currentX + i) && coords[currentX + i].ContainsKey(currentY))
								{
									minSteps = Math.Min(minSteps, steps + coords[currentX + i][currentY]);

								}
							

							}
							currentX += moveR;
							break;

					}



				}

			}

			Console.WriteLine(minSteps.ToString());

		}
	}
}
