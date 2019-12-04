using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode2019
{
	class Day4
	{
		public static void Problem()
		{
			List<int> nums = new List<int>();
			for (int i = 387638; i <= 919123; i++)
			{
				char last = ' ';
				bool hasDouble = false;
				bool increasing = true;
				foreach(char x in i.ToString())
				{
					if (x == last) 
					{ 
						hasDouble = true; 
					}
					else if (x < last)
					{
						increasing = false;
						break;
					}
					last = x;
				}
				if (increasing && hasDouble) nums.Add(i);
			}
			Console.WriteLine(nums.Count.ToString());
		}

		public static void Problem2()
		{
			List<int> nums = new List<int>();
			for (int i = 387638; i <= 919123; i++)
			{
				char last = ' ';
				int repeatCount = 1;
				bool increasing = true;
				bool hasDouble = false;
				foreach (char x in i.ToString())
				{
					if (x == last) 
					{
						repeatCount++; 
					}
					else if (x < last) 
					{
						increasing = false; 
						break; 
					}
					else 
					{
						if (repeatCount == 2) hasDouble = true;
						repeatCount = 1;
					}
					last = x;
				}
				if (repeatCount == 2) hasDouble = true; //catch doubles at the end of the number e.g. 444455

				if (increasing && hasDouble) nums.Add(i);
			}
			Console.WriteLine(nums.Count.ToString());
		}
	}
}
