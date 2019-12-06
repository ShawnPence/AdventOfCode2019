using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Numerics;
using AdventOfCode2019.Computer;

namespace AdventOfCode2019
{
	class Day6
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
			Dictionary<string, string> orbits = new Dictionary<string, string>();

			using (StreamReader sr = new StreamReader(fileName))
			{
				while (sr.Peek() != -1)
				{
					string[] o = sr.ReadLine().Split(')');
					orbits[o[1]] = o[0];

				}
			}

			int result = 0;
			foreach (string s in orbits.Keys) result += countOrbits(orbits, s);

			Console.WriteLine(result);
			Console.WriteLine(uncommonAncestorCount(orbits, "SAN", "YOU"));
		}

		static int countOrbits(Dictionary<string, string> orbits, string start)
		{
			int outval = 1;
			while(orbits[start] != "COM")
			{
				outval++;
				start = orbits[start];
			}
			return outval;
		}

		static int uncommonAncestorCount(Dictionary<string, string> orbits, string start, string start2) 
		{
			HashSet<string> o1 = new HashSet<string>();
			start = orbits[start];
			while (orbits[start] != "COM")
			{
				o1.Add(start);
				start = orbits[start];
			}
			HashSet<string> o2 = new HashSet<string>();
			start2 = orbits[start2]; //start at the object "YOU" are orbiting
			while (orbits[start2] != "COM")
			{
				o2.Add(start2);
				start2 = orbits[start2];
			}
			HashSet<string> all = new HashSet<string>();
			all.UnionWith(o1);
			all.UnionWith(o2);
			int commoncount = (o2.Count + o1.Count) - all.Count;
			int result = (o2.Count + o1.Count - 2 * commoncount);

			return result;

			}
	}
}
