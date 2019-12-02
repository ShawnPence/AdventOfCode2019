using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode2019
{
	static class Day2
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
			int next = 0;
			List<long> data = new List<long>();
			using (StreamReader sr = new StreamReader(fileName, true))
			{
				foreach (string s in sr.ReadLine().Split(','))
				{
					data.Add(Convert.ToInt64(s));
				}

			}
			data[1] = 12;
			data[2] = 2;
			while (next >= 0)
			{
				next = Compute(next, data);
			}

			Console.WriteLine(data[0].ToString());

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
			int next = 0;
			List<long> data = new List<long>();
			using (StreamReader sr = new StreamReader(fileName, true))
			{
				foreach (string s in sr.ReadLine().Split(','))
				{
					data.Add(Convert.ToInt64(s));
				}

			}
			bool solved = false;
			for (long n = 0; n < 100 && !solved; n++)
			{
				for (long v = 0; v < 100 && !solved; v++)
				{
					next = 0;
					List<long> dataCopy = new List<long>(data);
					dataCopy[1] = n;
					dataCopy[2] = v;
					while (next >= 0)
					{
						next = Compute(next, dataCopy);
					}
					
					if (dataCopy[0] == 19690720)
					{
						Console.WriteLine((100 * n + v).ToString());
						solved = true;
					}
				}
			}


			

		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="startAt"></param>
		/// <param name="data"></param>
		/// <returns>returns the next starting point, -1 for completed, or -2 for error</returns>
		public static int Compute(int startAt, List<long> data)
		{
			if (startAt >= data.Count || startAt < 0) return -2;
			switch (data[startAt])
			{
				case 1:
					if (startAt + 3 >= data.Count) return -2; //can't retrieve next values
					if (!Add(Convert.ToInt32(data[startAt + 1]), Convert.ToInt32(data[startAt + 2]), Convert.ToInt32(data[startAt + 3]), data)) return -2;
					return startAt + 4;
				case 2:
					if (startAt + 3 >= data.Count) return -2; //can't retrieve next values
					if (!Multiply(Convert.ToInt32(data[startAt + 1]), Convert.ToInt32(data[startAt + 2]), Convert.ToInt32(data[startAt + 3]), data)) return -2;
					return startAt + 4;
				case 99:
					return -1; //end program
				default:
					return -2; //error
			}

		}

		public static bool Multiply(int a, int b, int c, List<long> data)
		{
			if (data.Count <= a || data.Count <= b || data.Count <= c) return false;
			data[c] = data[a] * data[b];
			return true;

		}
		public static bool Add(int a, int b, int c, List<long> data)
		{
			if (data.Count <= a || data.Count <= b || data.Count <= c) return false;
			data[c] = data[a] + data[b];
			return true;

		}


	}
}
