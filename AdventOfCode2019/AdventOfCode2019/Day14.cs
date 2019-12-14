using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode2019
{
	class Day14
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

			var lines = new List<string>();
			using (StreamReader sr = new StreamReader(fileName))
			{
				while (sr.Peek() != -1) lines.Add(sr.ReadLine());
			}
			var d = new Dictionary<string, Dictionary<string, long>>();//compound / inputs&quantity to create
			var d2 = new Dictionary<string, long>();//compund / output quantity from inputs in d
			foreach(var line in lines)
			{
				var ing = new Dictionary<string, long>();
				var sides = line.Split("=>");

				var output = sides[1].Trim().Split(" ")[1];
				long q = Convert.ToInt64(sides[1].Trim().Split(" ")[0]);

				var parts = sides[0].Split(",");
				foreach (var part in parts)
				{
					var partName = part.Trim().Split(" ")[1];
					var partAmt = Convert.ToInt64(part.Trim().Split(" ")[0]);
					ing[partName] = partAmt;
				}
				d[output] = ing;
				d2[output] = q;

			}

			var need = new Dictionary<string, long>();
			need["FUEL"] = 1;
			long needed = GetAmount(d, d2, need);
			Console.WriteLine(needed); //part 1

			long max = 2000000000000 / needed;
			long min = 1;
			
			while (max > min)
			{
				long target = (min + max) / 2;
				var needTest = new Dictionary<string, long>();
				needTest["FUEL"] = target;
				if (GetAmount(d, d2, needTest) < 1000000000000)
				{
					min = target + 1;
				}
				else
				{
					max = target;
				}
			
			}
			Console.Write(min - 1); //part 2

		}

		static long MaxDepth(Dictionary<string, Dictionary<string, long>> d, string key)
		{
			if (key == "ORE") return 0;
			long md = -1;
			foreach (var k in d[key].Keys)
			{
				md = Math.Max(md, 1 + MaxDepth(d, k));
			}
			return md;
		}

		static long GetAmount(Dictionary<string, Dictionary<string, long>> d, Dictionary<string,long>d2, Dictionary<string,long> need)
		{
			while (need.Keys.Count > 1 || need.ContainsKey("FUEL"))
			{
				var n2 = new Dictionary<string, long>();
				long md = -1;
				foreach (var k in need.Keys)
				{
					md = Math.Max(md, MaxDepth(d, k));
				}

				foreach (var k in need.Keys)
				{
					if (k == "ORE")
					{
						n2[k] = need[k];
					}
					else
					{
						//only convert the parts that are furthest from ore to their component parts (to avoid creating more of a component than necessary)
						if (MaxDepth(d,k) == md)
						{

							long minMultiple = need[k] % d2[k] == 0 ? need[k] / d2[k] : need[k] / d2[k] + 1;
							if (need[k] < d2[k]) minMultiple = 1;
							foreach (var kin in d[k].Keys)
							{
								if (!n2.ContainsKey(kin)) n2[kin] = 0;
								n2[kin] += minMultiple * d[k][kin];
							}
						}
						else
						{
							if (!n2.ContainsKey(k)) n2[k] = 0;
							n2[k] += need[k];
						}

					}
				}
				need = n2;
			}
			return need["ORE"];
		}


	}
}
