using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode2019
{
	static class Day5
	{
		//todo: this needs a refactor/cleanup (this "computer" has been used for two different problems now, so might as well make it more robust for future use)
		//	    will tackle the refactor in the morning, but saving progress with a working Day5 solution for now.

		/// <summary>
		/// multiply data[a] * data[b] and store at data[c]
		/// </summary>
		/// <returns>true if successful, false if a,b,or c are out of range</returns>
		public static int Multiply(int startAt, int a, int b, int c, List<long> data, int pA = 0, int pB = 0, int pC = 0)
		{
			a = pA == 1 ? a : Convert.ToInt32(data[a]);
			b = pB == 1 ? b : Convert.ToInt32(data[b]);
			c = pC == 1 ? c : Convert.ToInt32(data[c]);

			if (data.Count <= a || data.Count <= b || data.Count <= c) return -2;
			data[c] = data[a] * data[b];
			return startAt == c ? startAt : startAt + 4;

		}

		/// <summary>
		/// add data[a] + data[b] and store at data[c]
		/// </summary>
		/// <returns>true if successful, false if a,b,or c are out of range</returns>
		public static int Add(int startAt, int a, int b, int c, List<long> data, int pA = 0, int pB = 0, int pC = 0)
		{
			a = pA == 1 ? a : Convert.ToInt32(data[a]);
			b = pB == 1 ? b : Convert.ToInt32(data[b]);
			c = pC == 1 ? c : Convert.ToInt32(data[c]);
			if (data.Count <= a || data.Count <= b || data.Count <= c) return -2;
			data[c] = data[a] + data[b];
			return startAt == c ? startAt : startAt + 4;

		}

		public static int JumpIfTrue(int a, int b, int startAt, List<long> data, int pA = 0, int pB = 0)
		{
			a = pA == 1 ? a : Convert.ToInt32(data[a]);
			b = pB == 1 ? b : Convert.ToInt32(data[b]);
			if (data.Count <= a || data.Count <= b) return -2; //error
			return data[a] != 0 ? Convert.ToInt32(data[b]) : startAt + 3;

		}

		public static int JumpIfFalse(int a, int b, int startAt, List<long> data, int pA = 0, int pB = 0)
		{
			a = pA == 1 ? a : Convert.ToInt32(data[a]);
			b = pB == 1 ? b : Convert.ToInt32(data[b]);
			if (data.Count <= a || data.Count <= b) return -2; //error
			return data[a] == 0 ? Convert.ToInt32(data[b]) : startAt + 3;

		}

		public static int LessThan(int startAt, int a, int b, int c, List<long> data, int pA = 0, int pB = 0, int pC = 0)
		{
			a = pA == 1 ? a : Convert.ToInt32(data[a]);
			b = pB == 1 ? b : Convert.ToInt32(data[b]);
			c = pC == 1 ? c : Convert.ToInt32(data[c]);
			if (data.Count <= a || data.Count <= b || data.Count <= c) return -2;
			data[c] = data[a] < data[b] ? 1 : 0;
			return startAt == c ? startAt : startAt + 4;

		}

		public static int Equals(int startAt, int a, int b, int c, List<long> data, int pA = 0, int pB = 0, int pC = 0)
		{
			a = pA == 1 ? a : Convert.ToInt32(data[a]);
			b = pB == 1 ? b : Convert.ToInt32(data[b]);
			c = pC == 1 ? c : Convert.ToInt32(data[c]);
			if (data.Count <= a || data.Count <= b || data.Count <= c) return -2;
			data[c] = data[a] == data[b] ? 1 : 0;
			return startAt == c ? startAt : startAt + 4;

		}

		/// <summary>
		/// compute per the rules of the Advent of Code challenge
		/// </summary>
		/// <param name="startAt">starting index with the current instruction</param>
		/// <param name="data"></param>
		/// <returns>returns the next starting point, -1 for completed successfully, or -2 for error</returns>
		public static int Compute(int startAt, List<long> data)
		{
			int codes = Convert.ToInt32(data[startAt]);
			int ins = codes % 100;
			int pA = codes / 100 % 10;
			int pB = codes / 1000 % 10;
			int pC = codes / 10000;


			if (startAt >= data.Count || startAt < 0) return -2;
			switch (ins)
			{
				case 1:
					if (startAt + 3 >= data.Count) return -2; //can't retrieve next values
					return Add(startAt, startAt + 1, startAt + 2, startAt + 3, data, pA, pB, pC);

				case 2:
					if (startAt + 3 >= data.Count) return -2; //can't retrieve next values
					return Multiply(startAt, startAt + 1, startAt + 2, startAt + 3, data, pA, pB, pC);
				case 3:
					if (startAt + 1 >= data.Count) return -2;

					Console.WriteLine("Input:");
					long input = Convert.ToInt64(Console.ReadLine());
					int a3 = pA == 1 ? startAt + 1 : Convert.ToInt32(data[startAt + 1]);
					if (data.Count <= a3) return -2;
					data[a3] = input;
					return startAt + 2;
				case 4:
					if (startAt + 1 >= data.Count) return -2;
					int a4 = pA == 1 ? startAt + 1 : Convert.ToInt32(data[startAt + 1]);
					if (data.Count <= a4) return -2;

					Console.WriteLine(data[a4].ToString());
					return startAt + 2;
				case 5:
					if (startAt + 2 >= data.Count) return -2;
					return JumpIfTrue(startAt + 1, startAt + 2, startAt, data, pA, pB);
				case 6:
					if (startAt + 2 >= data.Count) return -2;
					return JumpIfFalse(startAt + 1, startAt + 2, startAt, data, pA, pB);
				case 7:
					if (startAt + 3 >= data.Count) return -2;
					return LessThan(startAt, startAt + 1, startAt + 2, startAt + 3, data, pA, pB, pC);
				case 8:
					if (startAt + 3 >= data.Count) return -2;
					return Equals(startAt, startAt + 1, startAt + 2, startAt + 3, data, pA, pB, pC);


				case 99:
					Console.WriteLine("end of program");
					return -1; //end of program
				default:
					Console.WriteLine("unknown error");
					return -2; //error - per challenge instructions any command other than 1, 2, or 99 is an error
			}

		}

		/// <summary>
		/// compute per the rules of the Advent of Code challenge
		/// </summary>
		/// <returns>returns -1 for completed successfully, or -2 for error</returns>
		public static int Compute(List<long> data)
		{
			int next = 0;
			while (next >= 0)
			{
				next = Compute(next, data);
			}
			return next;
		}

		public static void Problem1()
		{
			Console.WriteLine("Input file:");
			string fileName = Console.ReadLine();
			while (!File.Exists(fileName))
			{
				Console.WriteLine("file not found - try again:");
				fileName = Console.ReadLine();
			}
			List<long> data = new List<long>();
			using (StreamReader sr = new StreamReader(fileName, true))
			{
				foreach (string s in sr.ReadLine().Split(','))
				{
					data.Add(Convert.ToInt64(s));
				}

			}
			Compute(data);

		}//Problem1


		public static void Problem2()
		{
			Console.WriteLine("Input file:");
			string fileName = Console.ReadLine();
			while (!File.Exists(fileName))
			{
				Console.WriteLine("file not found - try again:");
				fileName = Console.ReadLine();
			}

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

					List<long> dataCopy = new List<long>(data);
					dataCopy[1] = n;
					dataCopy[2] = v;
					Compute(dataCopy);

					if (dataCopy[0] == 19690720)
					{
						Console.WriteLine((100 * n + v).ToString());
						solved = true;
					}
				}
			}

		}//Problem2
	}
}
