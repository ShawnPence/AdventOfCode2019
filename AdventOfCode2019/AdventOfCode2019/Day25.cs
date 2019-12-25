using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode2019
{
	class Day25
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
			var c = new Computer(fileName);
			bool quit = false;
			while (!quit)
			{
				c.Run();
				while (c.Output.Count > 0)
				{
					char x = (char)c.Output.Dequeue();

					if (x == 10)
					{
						Console.Write("\n");
					}
					else
					{
						Console.Write(x);
					}
				}
				string input = Console.ReadLine();
				if (input == "RESET")
				{
					c = new Computer(fileName);
				}
				else if (input == "QUIT")
				{
					quit = true;
				}
				else
				{
					foreach (char x in input)
						c.Input(x);
					c.Input(10);
				}

			}


		}
	}
}
