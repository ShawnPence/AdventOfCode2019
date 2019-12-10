using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Numerics;


namespace AdventOfCode2019
{
	class Day10
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
			var lines = new List<string>();
			var canSee = new Dictionary<int, List<int>>(); //row*100 + col;
			using (StreamReader sr = new StreamReader(fileName))
			{
				while(sr.Peek() != -1) lines.Add(sr.ReadLine());
			}
			for (int r = 0; r < lines.Count; r++)
			{
				int lastThisLine = -1; //find last # to left that could be seen on same line
				for (int c = 0; c < lines[r].Length; c++)
				{
					if (lines[r][c] == '#')
					{
						if (!canSee.ContainsKey(r * 100 + c)) canSee[r * 100 + c] = new List<int>();
						if (lastThisLine != -1)
						{
							canSee[r * 100 + lastThisLine].Add(r * 100 + c);
							canSee[r * 100 + c].Add(r * 100 + lastThisLine);
						}
						lastThisLine = c;
						var block = new HashSet<decimal>();  //slope of all found objects
						bool inColumn = false;
						for (int r2 = r + 1; r2 < lines.Count; r2++)
						{
							for (int c2 = 0; c2 < lines[r2].Length; c2++)
							{
								if (lines[r2][c2] == '#')
								{
									if (!canSee.ContainsKey(r2 * 100 + c2)) canSee[r2 * 100 + c2] = new List<int>();
									
									int rO = r2 - r;
									int cO = c2 - c;
									if (cO == 0 && !inColumn)
									{
										canSee[r * 100 + c].Add(r2 * 100 + c2);
										canSee[r2 * 100 + c2].Add(r * 100 + c);
										inColumn = true;
									}
									else if (cO != 0)
									{
										decimal slope = Convert.ToDecimal(rO) / Convert.ToDecimal(cO);
										if (!block.Contains(slope))
										{
											canSee[r * 100 + c].Add(r2 * 100 + c2);
											canSee[r2 * 100 + c2].Add(r * 100 + c);
											block.Add(slope);
										}
									}
								}
							}
						}
					}
				}
			}

			int maxSee = 0;
			int bestK = -1;
			foreach (int k in canSee.Keys)
			{
				if (canSee[k].Count > maxSee)
				{
					maxSee = canSee[k].Count;
					bestK = k;
				}
			}


			Console.WriteLine(maxSee.ToString());

			//answer to problem 1 indicates more than 200 on first rotation, so no need to check for objects behind

			Dictionary<double, int> objectAtAngle = new Dictionary<double, int>();
			List<int> vals = canSee[bestK];
			List<double> angles = new List<double>();
			foreach (int val in vals)
			{
				//calculate offset from bestK (using K as 0,0)
				int colOffset = (val % 100) - (bestK % 100);
				int rowOffset = (val / 100) - (bestK / 100);
				double angle = Math.Atan2(colOffset, -rowOffset);
				if (angle < 0) angle = 2 * Math.PI + angle;
				objectAtAngle[angle] = val;
				angles.Add(angle);

			}
			angles.Sort();


			//reverse the row*100 + col to x*100+y (col*100 + row)
			int th = objectAtAngle[angles[199]];
			int thX = th % 100; 
			int thY = th / 100;

			Console.WriteLine((thX * 100 + thY).ToString());	   			 		  		  		 
		}
	}
}
