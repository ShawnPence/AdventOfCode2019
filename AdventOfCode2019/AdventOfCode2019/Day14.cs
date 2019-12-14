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
			var inputs = new Dictionary<string, Dictionary<string, long>>();//item / inputs&quantity to create
			var outputs = new Dictionary<string, long>();//item / output quantity from items in inputs[item]
			foreach(var line in lines)
			{
				var inputParts = new Dictionary<string, long>();
				var sides = line.Split("=>");

				var output = sides[1].Trim().Split(" ")[1];
				long quantity = Convert.ToInt64(sides[1].Trim().Split(" ")[0]);

				var parts = sides[0].Split(",");
				foreach (var part in parts)
				{
					var partName = part.Trim().Split(" ")[1];
					var partAmt = Convert.ToInt64(part.Trim().Split(" ")[0]);
					inputParts[partName] = partAmt;
				}
				inputs[output] = inputParts;
				outputs[output] = quantity;

			}

			var need = new Dictionary<string, long>();
			need["FUEL"] = 1;
			long needed = GetAmount(inputs, outputs, need);
			Console.WriteLine(needed); //part 1

			long max = 2000000000000 / needed;
			long min = 1;
			
			while (max > min)
			{
				long testValue = (min + max) / 2;
				var needTest = new Dictionary<string, long>();
				needTest["FUEL"] = testValue;
				if (GetAmount(inputs, outputs, needTest) < 1000000000000)
				{
					min = testValue + 1;
				}
				else
				{
					max = testValue;
				}
			
			}
			Console.Write(min - 1); //part 2

		}

		static long MaxDepth(Dictionary<string, Dictionary<string, long>> inputs, string key)
		{
			if (key == "ORE") return 0;
			long md = -1;
			foreach (var k in inputs[key].Keys)
			{
				md = Math.Max(md, 1 + MaxDepth(inputs, k));
			}
			return md;
		}

		static long GetAmount(Dictionary<string, Dictionary<string, long>> inputs, Dictionary<string,long>outputs, Dictionary<string,long> need)
		{
			while (need.Keys.Count > 1 || need.ContainsKey("FUEL"))
			{
				var needNew = new Dictionary<string, long>();
				long maxDepth = -1;
				foreach (var k in need.Keys)
				{
					maxDepth = Math.Max(maxDepth, MaxDepth(inputs, k));
				}

				foreach (var k in need.Keys)
				{
					if (k == "ORE")
					{
						needNew[k] = need[k];
					}
					else
					{
						//only convert the parts that are furthest from ore to their component parts (to avoid creating more of a component than necessary)
						if (MaxDepth(inputs,k) == maxDepth)
						{

							long minMultiple = need[k] % outputs[k] == 0 ? need[k] / outputs[k] : need[k] / outputs[k] + 1;
							foreach (var kInput in inputs[k].Keys)
							{
								if (!needNew.ContainsKey(kInput)) needNew[kInput] = 0;
								needNew[kInput] += minMultiple * inputs[k][kInput];
							}
						}
						else
						{
							if (!needNew.ContainsKey(k)) needNew[k] = 0;
							needNew[k] += need[k];
						}

					}
				}
				need = needNew;
			}
			return need["ORE"];
		}


	}
}
