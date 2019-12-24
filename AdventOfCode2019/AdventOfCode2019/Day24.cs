using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode2019
{
	class Day24
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
			List<string> lines = new List<string>();
			using (StreamReader sr = new StreamReader(fileName))
			{
				while (sr.Peek() != -1) lines.Add(sr.ReadLine());
			}
			List<string> initialState = new List<string>(lines);

			bool part1comptete = false;
			HashSet<int> found = new HashSet<int> { GetRating(lines) };
			while (!part1comptete)
			{
				Process2D(ref lines);
				int rating = GetRating(lines);
				if (found.Contains(rating))
				{
					part1comptete = true;
					Console.WriteLine($"Part 1: {rating}");
				}
				found.Add(rating);
			}

			string empty = ".....";
			List<string> emptyPattern = new List<string>();
			for (int i = 0; i < 5; i++) emptyPattern.Add(empty);

			Dictionary<int, List<string>> patterns = new Dictionary<int, List<string>>();
			for (int z = -200; z < 201; z++)
			{
				patterns[z] = new List<string>(emptyPattern);
			}
			patterns[0] = new List<string>(initialState);

			for (int i = 0; i < 200; i++)
				Process3D(ref patterns);

			Console.WriteLine($"Part 2: {Count(patterns)}");
			
		}

		static int GetRating(List<string> lines)
		{
			int shift = 0;
			int total = 0;
			foreach (var line in lines)
				foreach (var x in line)
				{
					if (x == '#') total += (1 << shift);
					shift++;
				}
			return total;
		}


		static int Count(Dictionary<int, List<string>> patterns)
		{
			int output = 0;
			for (int z = -200; z < 201; z++)
				for (int r = 0; r < 5; r++)
					for (int c = 0; c < 5; c++)
						if (patterns[z][r][c] == '#') output++;
			return output;
		}

		static void Process2D(ref List<string> lines)
		{
			var output = new List<string>();
			for (int r = 0; r < lines.Count; r++)
			{
				StringBuilder s = new StringBuilder();
				for (int c = 0; c < lines.Count; c++)
				{
					int count = 0;
					if (r > 0) count += lines[r - 1][c] == '#' ? 1 : 0;
					if (r < lines.Count - 1) count += lines[r + 1][c] == '#' ? 1 : 0;
					if (c > 0) count += lines[r][c-1] == '#' ? 1 : 0;
					if (c < lines[r].Length - 1) count += lines[r][c + 1] == '#' ? 1 : 0;

					if (count == 1 || ((lines[r][c] == '.') && count == 2))
					{
						s.Append('#');
					}
					else
					{
						s.Append('.');
					}

				}
				output.Add(s.ToString());
			}
			lines = output;
		}

		static void Process3D(ref Dictionary<int, List<string>> patterns)
		{
			Dictionary<int, List<string>> output = new Dictionary<int, List<string>>();
			for (int z = -200; z < 201; z++)
			{
				var outLine = new List<string>();
				for (int r = 0; r < patterns[z].Count; r++)
				{
					StringBuilder s = new StringBuilder();
					for (int c = 0; c < patterns[z].Count; c++)
					{
						
						int count = 0;
						//above
						if (r > 0 && (r != 3 || c != 2))
						{
							count += patterns[z][r - 1][c] == '#' ? 1 : 0;
						}
						else if (r == 3 && c == 2 && z < 200)
						{
							//check all 5 tiles on bottom of z+1
							for (int c1 = 0; c1 < 5; c1++)
							{
								if (patterns[z + 1][4][c1] == '#') count++;
							}

						}
						else if (r == 0 && z > -200)
						{
							//check the tile on 2nd row middle of z - 1
							if (patterns[z - 1][1][2] == '#') count++;
						}

						//below
						if ((r != 1 || c != 2) && r < patterns[z].Count - 1)
						{
							count += patterns[z][r + 1][c] == '#' ? 1 : 0;
						}
						else if (r == 1 && c == 2 && z < 200)
						{
							//check all 5 tiles on top of z+1
							for (int c1 = 0; c1 < 5; c1++)
							{
								if (patterns[z + 1][0][c1] == '#') count++;
							}

						}
						else if (r == patterns[z].Count - 1 && z > -200)
						{
							//check the tile on 4th row middle of z - 1
							if (patterns[z - 1][3][2] == '#') count++;
						}

						//left
						if (c > 0 && (c != 3 || r != 2))
						{
							count += patterns[z][r][c - 1] == '#' ? 1 : 0;
						}
						else if (r == 2 && c == 3 && z < 200)
						{
							//check all 5 tiles on right of z+1
							for (int r1 = 0; r1 < 5; r1++)
							{
								if (patterns[z + 1][r1][4] == '#') count++;
							}
						}
						else if (c == 0 && z > -200)
						{
							//check the tile on 2nd column middle of z - 1
							if (patterns[z - 1][2][1] == '#') count++;

						}

						//right
						if ((c < patterns[z][r].Length - 1) && (c != 1 || r != 2))
						{
							count += patterns[z][r][c + 1] == '#' ? 1 : 0;
						}
						else if (r == 2 && c == 1 && z < 200)
						{
							//check all 5 tiles on left of z+1
							for (int r1 = 0; r1 < 5; r1++)
							{
								if (patterns[z + 1][r1][0] == '#') count++;
							}
						}
						else if (c == 4 && z > -200)
						{
							//check the tile on 4th column middle of z - 1
							if (patterns[z - 1][2][3] == '#') count++;

						}

						if ((r!= 2 || c != 2) && (count == 1 || ((patterns[z][r][c] == '.') && count == 2)))
						{
							s.Append('#');
						}
						else
						{
							s.Append('.');
						}

					}
					outLine.Add(s.ToString());
				}
				output[z] = outLine;
			}
			patterns = output;
		}
	}
}
