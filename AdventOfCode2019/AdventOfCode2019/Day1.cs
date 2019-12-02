using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode2019
{
	static class Day1
	{
		public static void Problem1()
		{
			Console.WriteLine("Input file:");
			string fileName = Console.ReadLine();
			long outVal = 0;
			using (StreamReader sr = new StreamReader(fileName, true))
			{
				while (sr.Peek() != -1)
				{
					outVal += Convert.ToInt64(sr.ReadLine()) / 3 - 2;
				}

			}
			Console.WriteLine(outVal.ToString());
		}
		public static void Problem2()
		{
			Console.WriteLine("Input file:");
			string fileName = Console.ReadLine();
			long outVal = 0;
			using (StreamReader sr = new StreamReader(fileName, true))
			{
				while (sr.Peek() != -1)
				{
					outVal += RecurseFuel(Convert.ToInt64(sr.ReadLine()));
				}

			}
			Console.WriteLine(outVal.ToString());
		}
		public static long RecurseFuel(long x)
		{
			if (x <= 6) return 0;
			long y = x / 3 - 2;
			return y + RecurseFuel(y);
		}

	}
}
