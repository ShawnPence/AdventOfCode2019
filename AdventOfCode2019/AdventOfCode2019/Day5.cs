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
		public static int Multiply(int iP, int c, int b, int a, List<long> data, int pC = 0, int pB = 0, int pA = 0)
		{
			pA = 0; //per day 5 instructions "parameters that an instruction writes to will never be in immediate mode"
			c = pC == 1 ? c : Convert.ToInt32(data[c]);
			b = pB == 1 ? b : Convert.ToInt32(data[b]);
			a = pA == 1 ? a : Convert.ToInt32(data[a]);

			if (data.Count <= c || data.Count <= b || data.Count <= a) return -2;
			data[a] = data[c] * data[b];
			return iP == a ? iP : iP + 4;

		}

		/// <summary>
		/// add data[a] + data[b] and store at data[c]
		/// </summary>
		/// <returns>true if successful, false if a,b,or c are out of range</returns>
		public static int Add(int iP, int c, int b, int a, List<long> data, int pC = 0, int pB = 0, int pA = 0)
		{
			pA = 0; //per day 5 instructions "parameters that an instruction writes to will never be in immediate mode"
			c = pC == 1 ? c : Convert.ToInt32(data[c]);
			b = pB == 1 ? b : Convert.ToInt32(data[b]);
			a = pA == 1 ? a : Convert.ToInt32(data[a]);
			if (data.Count <= c || data.Count <= b || data.Count <= a) return -2;
			data[a] = data[c] + data[b];
			return iP == a ? iP : iP + 4;

		}

		public static int JumpIfTrue(int c, int b, int iP, List<long> data, int pC = 0, int pB = 0)
		{
			c = pC == 1 ? c : Convert.ToInt32(data[c]);
			b = pB == 1 ? b : Convert.ToInt32(data[b]);
			if (data.Count <= c || data.Count <= b) return -2; //error
			return data[c] != 0 ? Convert.ToInt32(data[b]) : iP + 3;

		}

		public static int JumpIfFalse(int c, int b, int iP, List<long> data, int pC = 0, int pB = 0)
		{
			c = pC == 1 ? c : Convert.ToInt32(data[c]);
			b = pB == 1 ? b : Convert.ToInt32(data[b]);
			if (data.Count <= c || data.Count <= b) return -2; //error
			return data[c] == 0 ? Convert.ToInt32(data[b]) : iP + 3;

		}

		public static int LessThan(int iP, int c, int b, int a, List<long> data, int pC = 0, int pB = 0, int pA = 0)
		{
			pA = 0; //per day 5 instructions "parameters that an instruction writes to will never be in immediate mode"
			c = pC == 1 ? c : Convert.ToInt32(data[c]);
			b = pB == 1 ? b : Convert.ToInt32(data[b]);
			a = pA == 1 ? a : Convert.ToInt32(data[a]);
			if (data.Count <= c || data.Count <= b || data.Count <= a) return -2;
			data[a] = data[c] < data[b] ? 1 : 0;
			return iP == a ? iP : iP + 4;

		}

		public static int Equals(int iP, int c, int b, int a, List<long> data, int pC = 0, int pB = 0, int pA = 0)
		{
			pA = 0; //per day 5 instructions "parameters that an instruction writes to will never be in immediate mode"
			c = pC == 1 ? c : Convert.ToInt32(data[c]);
			b = pB == 1 ? b : Convert.ToInt32(data[b]);
			a = pA == 1 ? a : Convert.ToInt32(data[a]);
			if (data.Count <= c || data.Count <= b || data.Count <= a) return -2;
			data[a] = data[c] == data[b] ? 1 : 0;
			return iP == a ? iP : iP + 4;

		}

		/// <summary>
		/// compute per the rules of the Advent of Code challenge
		/// </summary>
		/// <param name="iP">instruction pointer - starting index with the current instruction</param>
		/// <param name="data"></param>
		/// <returns>returns the next starting point, -1 for completed successfully, or -2 for error</returns>
		public static int Compute(int iP, List<long> data)
		{
			int instruction = Convert.ToInt32(data[iP]);
			int opcode = instruction % 100;
			int pC = instruction / 100 % 10;
			int pB = instruction / 1000 % 10;
			int pA = instruction / 10000;


			if (iP >= data.Count || iP < 0) return -2;
			switch (opcode)
			{
				case 1:
					if (iP + 3 >= data.Count) return -2; //can't retrieve next values
					return Add(iP, iP + 1, iP + 2, iP + 3, data, pC, pB, pA);

				case 2:
					if (iP + 3 >= data.Count) return -2; //can't retrieve next values
					return Multiply(iP, iP + 1, iP + 2, iP + 3, data, pC, pB, pA);
				case 3:
					if (iP + 1 >= data.Count) return -2;
					Console.ForegroundColor = ConsoleColor.Magenta;
					Console.WriteLine("Input:");
					Console.ResetColor();
					long input = Convert.ToInt64(Console.ReadLine());
					int a3 = pC == 1 ? iP + 1 : Convert.ToInt32(data[iP + 1]);
					if (data.Count <= a3) return -2;
					data[a3] = input;
					return iP + 2;
				case 4:
					if (iP + 1 >= data.Count) return -2;
					int a4 = pC == 1 ? iP + 1 : Convert.ToInt32(data[iP + 1]);
					if (data.Count <= a4) return -2;
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.WriteLine(data[a4].ToString());
					Console.ResetColor();
					return iP + 2;
				case 5:
					if (iP + 2 >= data.Count) return -2;
					return JumpIfTrue(iP + 1, iP + 2, iP, data, pC, pB);
				case 6:
					if (iP + 2 >= data.Count) return -2;
					return JumpIfFalse(iP + 1, iP + 2, iP, data, pC, pB);
				case 7:
					if (iP + 3 >= data.Count) return -2;
					return LessThan(iP, iP + 1, iP + 2, iP + 3, data, pC, pB, pA);
				case 8:
					if (iP + 3 >= data.Count) return -2;
					return Equals(iP, iP + 1, iP + 2, iP + 3, data, pC, pB, pA);


				case 99:
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("end of program");
					Console.ResetColor();
					return -1; //end of program
				default:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("unknown error");
					Console.ResetColor();
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

				
	}
}
