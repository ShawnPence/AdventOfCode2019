using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace AdventOfCode2019
{
	class Day8
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

			int minZero = Int32.MaxValue;
			int output = 0;
			int i = 0;
			int offset = 25 * 6;
			int[] nums = new int[offset];
			string[] output2 = new string[offset];
			for (int j = 0; j < offset; j++) { nums[j] = -1; output2[j] = " "; }

			while (i * offset < input.Length)
			{
				int z = 0;
				int o = 0;
				int t = 0;
				for (int j = 0; j < offset; j++)
				{
					int k = j + i * offset;
					if (k >= input.Length) { i = input.Length; break; }
					switch (input[k])
					{
						case '0':
							z++;
							if (nums[j] == -1) { nums[j] = 2; }
							break;
						case '1':
							o++;
							if (nums[j] == -1) { nums[j] = 1; output2[j] = "*"; }
							break;
						case '2':
							t++;
							break;
					}
				}
				if (z < minZero)
				{
					minZero = z;
					output = o * t;
				}
				i++;
			}
			Console.WriteLine(output.ToString());
			Console.WriteLine();
			for (int j = 0; j < 6; j++)
			{
				StringBuilder s = new StringBuilder(25);
				for (int k = 0; k < 25; k++)
				{
					s.Append(output2[j * 25 + k]);
				}
				Console.WriteLine(s);
			}
		}
	}
}
