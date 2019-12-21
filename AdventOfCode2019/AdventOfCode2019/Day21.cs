using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AdventOfCode2019
{
	
	class Day21
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


			var comp = new Computer.Computer(fileName);
			string[] instructions = new string[] {
				"NOT A J\n",
				"NOT B T\n",
				"OR J T\n",
				"NOT C J\n",
				"OR J T\n",
				"OR T J\n",
				"AND D J\n",//at this point, will jump if D is present unless ABCD
				"WALK\n" 
			};
			foreach (string s in instructions)
				foreach (char c in s)
					comp.Input(c);
			comp.Run();
			StringBuilder sb = new StringBuilder();
			while (comp.Output.Count > 0)
			{
				long next = comp.Output.Dequeue();
				if (next > 128)
				{
					sb.Append(next.ToString());
					sb.Append(Environment.NewLine);
				}
				else
				{
					sb.Append((char)next);
				}
			}
			Console.Write(sb);
			Console.WriteLine("");
			Console.WriteLine("Part 2:");

			instructions = new string[] {
				"NOT A J\n",
				"NOT B T\n",
				"OR J T\n",
				"NOT C J\n",
				"OR J T\n",
				"OR T J\n",
				"AND D J\n",//at this point, will jump if D is present unless ABCD
				"AND E T\n",
				"OR H T\n",
				"AND T J\n",//same as step 1 execpt don't jump if !(E|H) -- don't jump if can neither jump nor walk after this jump
				"RUN\n"
			};
			comp = new Computer.Computer(fileName);
			foreach (string s in instructions)
				foreach (char c in s)
					comp.Input(c);
			comp.Run();
			while (comp.Output.Count > 0)
			{
				long next = comp.Output.Dequeue();
				if (next > 128)
				{
					sb.Append(next.ToString());
					sb.Append(Environment.NewLine);
				}
				else
				{
					sb.Append((char)next);
				}
			}
			Console.Write(sb);

			Console.WriteLine("");
			stopwatch.Stop();
			Console.WriteLine($"Time to complete (ms): {stopwatch.ElapsedMilliseconds}");
		}
	}
}
