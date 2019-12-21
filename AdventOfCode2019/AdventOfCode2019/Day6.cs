using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Numerics;

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
			start = orbits[start]; //begin counting at the object being orbited by start
			while (orbits[start] != "COM")
			{
				o1.Add(start);
				start = orbits[start];
			}
			HashSet<string> o2 = new HashSet<string>();
			start2 = orbits[start2]; //begin counting at the object being orbited by start2
			while (orbits[start2] != "COM")
			{
				o2.Add(start2);
				start2 = orbits[start2];
			}
			o1.SymmetricExceptWith(o2);
			return o1.Count;

		}
	}
}
