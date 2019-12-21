using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Numerics;


namespace AdventOfCode2019
{
	class Day9
	{
		public static void Problem()
		{
			Console.WriteLine("Input file:");
			string fileName = Console.ReadLine();
			while (!File.Exists(fileName))
			{
				Console.WriteLine("file not found - try again:");
				fileName = Console.ReadLine();
			}
			string input;
			using (StreamReader sr = new StreamReader(fileName))
			{
				input = sr.ReadLine();
			}
			//problem 1
			Computer c = new Computer(input.Split(','));
			c.processor.Input(1);
			c.Run();
			Console.WriteLine(c.processor.outputQueue.Dequeue().ToString());
			//problem 2
			c = new Computer(input.Split(','));
			c.processor.Input(2);
			c.Run();
			Console.WriteLine(c.processor.outputQueue.Dequeue().ToString());
		}
	}
}
