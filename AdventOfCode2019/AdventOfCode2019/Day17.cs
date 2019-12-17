using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode2019

{
	class Day17
	{
		public static void Problems()
		{

			bool visualize = false;

			Console.WriteLine("Input file:");
			string fileName = Console.ReadLine();
			while (!File.Exists(fileName))
			{
				Console.WriteLine("file not found - try again:");
				fileName = Console.ReadLine();
			}

			Computer.Computer comp = new Computer.Computer(fileName);
			comp.Input(76);
			comp.Run();
			List<string> lines = new List<string>();

			HashSet<int> sc = new HashSet<int>();

			StringBuilder s = new StringBuilder();
			while (comp.Output.Count > 0)
			{
				long next = comp.Output.Dequeue();
				if (next == 10)
				{
					if(s.Length > 0) lines.Add(s.ToString());
					s = new StringBuilder();
				}
				else
				{
					s.Append((char)next);


				}

			}
			if(s.Length > 0) lines.Add(s.ToString());


			long AP = 0;
			for (int r = 1; r < lines.Count - 1; r++)
			{
				for (int c = 1; c < lines[r].Length - 1; c++)
				{ 
					//check this and left/right/top/bottom for #^v<>
					if(IsScaffold(lines[r][c]) && IsScaffold(lines[r-1][c]) && IsScaffold(lines[r+1][c]) && IsScaffold(lines[r][c-1]) && IsScaffold(lines[r][c+1]))
					{

						AP += r * c;
					}
				}
			}



			/*Visualization*/
			if(visualize) foreach (string line in lines) Console.WriteLine(line);

			//part 2... 

			//(find robot)
			int x = 0;
			int y = 0;
			for (int r = 0; r < lines.Count; r++)
			{
				for (int c = 0; c < lines[r].Length; c++)
				{
					if (IsScaffold(lines[r][c])) sc.Add(r * 100 + c);
					if (IsRobot(lines[r][c]))
					{
						x = c; 
						y = r;
					}
				}
			}


			//find full walk path string
			//first attempt - don't turn until necessary 
			// (visualization of provided input says this is possible with only one way to turn at each endpoint)
			StringBuilder path = new StringBuilder();
			HashSet<int> found = new HashSet<int>();
			found.Add(x * 100 + y);
			int forward = 0;
			char direction = lines[y][x];
			while (found.Count < sc.Count)
			{
				switch (direction)
				{
					case '^':
						if (y != 0 && IsScaffold(lines[y - 1][x]))
						{
							forward++;
							y--;
							found.Add(x * 100 + y);
						}
						else if (x != 0 && IsScaffold(lines[y][x - 1]))
						{
							path.Append(forward.ToString());
							forward = 0;
							path.Append("L");
							direction = '<';
						}
						else if (x != (lines[y].Length - 1) && IsScaffold(lines[y][x + 1]))
						{
							path.Append(forward.ToString());
							forward = 0;
							path.Append("R");
							direction = '>';
						}
						else
						{
							throw new Exception("no where to go");
						}

						break;
					case '>':
						if (x != (lines[y].Length - 1) && IsScaffold(lines[y][x + 1]))
						{
							forward++;
							x++;
							found.Add(x * 100 + y);
						}
						else if (y != 0 && IsScaffold(lines[y - 1][x]))
						{
							path.Append(forward.ToString());
							forward = 0;
							path.Append("L");
							direction = '^';
						}
						else if (y != (lines.Count - 1) && IsScaffold(lines[y + 1][x]))
						{
							path.Append(forward.ToString());
							forward = 0;
							path.Append("R");
							direction = 'v';
						}
						else
						{
							throw new Exception("no where to go");
						}

						break;
					case '<':

						if (x != 0 && IsScaffold(lines[y][x - 1]))
						{
							forward++;
							x--;
							found.Add(x * 100 + y);
						}
						else if (y != 0 && IsScaffold(lines[y - 1][x]))
						{
							path.Append(forward.ToString());
							forward = 0;
							path.Append("R");
							direction = '^';
						}
						else if (y != (lines.Count - 1) && IsScaffold(lines[y + 1][x]))
						{
							path.Append(forward.ToString());
							forward = 0;
							path.Append("L");
							direction = 'v';
						}
						else
						{
							throw new Exception("no where to go");
						}
						break;
					case 'v':
						if (y != (lines.Count - 1) && IsScaffold(lines[y + 1][x]))
						{
							forward++;
							y++;
							found.Add(x * 100 + y);
						}
						else if (x != 0 && IsScaffold(lines[y][x - 1]))
						{
							path.Append(forward.ToString());
							forward = 0;
							path.Append("R");
							direction = '<';
						}
						else if (x != (lines[y].Length - 1) && IsScaffold(lines[y][x + 1]))
						{
							path.Append(forward.ToString());
							forward = 0;
							path.Append("L");
							direction = '>';
						}
						else
						{
							throw new Exception("no where to go");
						}

						break;
				}
			}

			path.Append(forward);
			


			//break into combinations of 3 substrings not greater than 20 characters (including commas between letters and numbers )
			//done visually - may rewrite with algorithm later to work with other possible input paths...
			/*
			 * L4L4L6R10L6 L4L4L6R10L6 L12L6R10L6 R8R10L6 R8R10L6 L4L4L6R10L6 R8R10L6 L12L6R10L6 R8R10L6 L12L6R10L6
			   AAAAAAAAAAA AAAAAAAAAAA BBBBBBBBBB CCCCCCC CCCCCCC AAAAAAAAAAA CCCCCCC BBBBBBBBBB CCCCCCC BBBBBBBBBB
			 * */

			String MainInstructions = "A,A,B,C,C,A,C,B,C,B";
			String A = "L,4,L,4,L,6,R,10,L,6";
			String B = "L,12,L,6,R,10,L,6";
			String C = "R,8,R,10,L,6";




			Computer.Computer c2 = new Computer.Computer(fileName);
			c2.Ram[0] = 2;
			foreach (var ins in MainInstructions) c2.Input(ins);
			c2.Input(10);
			foreach (var ins in A) c2.Input(ins);
			c2.Input(10);
			foreach (var ins in B) c2.Input(ins);
			c2.Input(10);
			foreach (var ins in C) c2.Input(ins);
			c2.Input(10);
			c2.Input(visualize ? 'y' : 'n');
			c2.Input(10);
			c2.Run();

			//Console.WriteLine();
			StringBuilder output = new StringBuilder();
			while (c2.Output.Count > 1)
			{
				long val = c2.Output.Dequeue();
				if (visualize)
				{

					if (val == 10)
					{

						Console.WriteLine(output.ToString());
						if (output.Length == 0)
						{
							//for visualization of map
							System.Threading.Thread.Sleep(45);
							Console.Clear();
						}
						output = new StringBuilder();
					}
					else
					{
						output.Append((char)val);
					}
				}
			}
			//if (output.Length > 0) Console.WriteLine(output.ToString());
			long result = c2.Output.Dequeue();

			
			//print expected directions
			if (visualize) Console.WriteLine(path.ToString().TrimStart('0')); 
			//result with provided input was L4L4L6R10L6L4L4L6R10L6L12L6R10L6R8R10L6R8R10L6L4L4L6R10L6R8R10L6L12L6R10L6R8R10L6L12L6R10L6

			Console.WriteLine($"Part 1: {AP}");
			Console.WriteLine($"Part 2: {result}");
		}

		public static bool IsScaffold(char x)
		{
			if (x == '#' || x == '^' || x == 'v' || x == '<' || x == '>') return true;
			return false;
		}

		public static bool IsRobot(char x)
		{
			if (x == '^' || x == 'v' || x == '<' || x == '>') return true;
			return false;
		}

	}
}
