using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace AdventOfCode2019
{
	class Day16
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


			string line;
			using (StreamReader sr = new StreamReader(fileName))
			{
				line = sr.ReadLine();
			}

			int[] pattern = new int[] { 1, 0, -1, 0 };


			//build line for q2 and store offset before FFT on first problem
			int offset = Convert.ToInt32(line.Substring(0, 7));

			int[] line2 = new int[line.Length * 10000];
			for (int i = 0; i < line.Length; i++)
			{
				int a = Convert.ToInt16(line[i] - '0');
				for (int j = 0; j < 10000; j++)
				{
					line2[j * line.Length + i] = a;
				}
			}


			//problem 1
			for (int loop = 0; loop < 100; loop++)
			{
				StringBuilder newLine = new StringBuilder(line.Length);
				for (int i = 0; i < line.Length; i++)
				{
					int sum = 0;
					for (int j = 0; (j + i) < line.Length; j++)
					{
						sum += pattern[(j/(i+1)) % 4] * Convert.ToInt32(line[j + i] - '0');
					}
					if (sum < 0) sum *= -1;
					sum %= 10;
					newLine.Append(sum.ToString());
				}
				line = newLine.ToString();
			}
			Console.WriteLine(line.Substring(0, 8)); //part 1


			//problem 2

			//pattern:
			/*
			 * this only works because the required offset is over 1/2 way through the larger line
			 * 
			 * once at least 1/2 way through list, all numbers from line[i] (where i is 1/2 way or more) to the
			 * end will be added to find the value of nextLine[i] 
			 * 
			 * (this is because if i > length/2, at least length/2 1's will be at the start of the pattern...)
			 * 
			 * keep track of the sum from offset to end, use that to calculate new line at that point
			 * then work across that line, subtracting the old values before incrementing i
			 * 
			 * repeat for 100 loops
			 * 
			 */
			for (int loop = 0; loop < 100; loop++)
			{
				int[] newLine2 = new int[line2.Length];
				int digitSum = 0;
				for (int i = offset; i < line2.Length; i++)
				{
					digitSum += line2[i]; //calculate digit sum
				}
				for (int i = offset; i < line2.Length; i++)
				{
					//digitSum at next line of code will be sum(line2[i]... line2[line2.Length - 1]) 
					newLine2[i] = digitSum % 10; //take last digit of current sum - save that as new value at this position
										
					digitSum -= line2[i]; //subtract old value at this position before moving on to next position
				}
				line2 = newLine2;
			}

			StringBuilder ans2 = new StringBuilder();
			for (int i = offset; i < offset + 8; i++)
			{
				ans2.Append(line2[i].ToString()); //get 8 digits starting at offset
			}
			Console.WriteLine(ans2.ToString());
		}
	}
}
