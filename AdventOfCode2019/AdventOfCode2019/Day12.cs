using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode2019
{
	public class Day12
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

			Problem(1000, lines.ToArray(), out int energyAfterIterations, out long returnToOrigin);
			Console.WriteLine(energyAfterIterations.ToString());
			Console.WriteLine(returnToOrigin.ToString());

		}

		public static void Problem(int iterations, string[] moonStrings, out int energyAfterIterations, out long returnToOrigin)
		{
			Moon[] moons = new Moon[moonStrings.Length];
			Moon[] originalMoons = new Moon[moonStrings.Length];
			for (int i = 0; i < moonStrings.Length; i++)
			{
				moons[i] = new Moon(moonStrings[i]);
				originalMoons[i] = new Moon(moonStrings[i]);
			}

			/*
			 * The x, y, and z values are independent - for part 2, find the repeat periods of 
			 * each individual part and then find the least common multiple of the 3 values
			 */
			long xPeriod = Int64.MaxValue;
			long yPeriod = Int64.MaxValue;
			long zPeriod = Int64.MaxValue;

			//fixed number of iterations for part 1
			for (int i = 1; i <= iterations; i++)
			{
				ApplyGravityAndMove(moons);

				//check if x's, y's, or z's are in initial state - if so, set period for that axis
				if (xPeriod == Int64.MaxValue && CompareX(originalMoons, moons)) xPeriod = i;
				if (yPeriod == Int64.MaxValue && CompareY(originalMoons, moons)) yPeriod = i;
				if (zPeriod == Int64.MaxValue && CompareZ(originalMoons, moons)) zPeriod = i;

				////debugging use only
				//Console.WriteLine($"after {(i + 1).ToString()} steps:");
				//for (int j = 0; j < 4; j++)
				//{
				//	Console.WriteLine(moons[j].ToString());
				//}
			}

			int energy = 0;
			for (int i = 0; i < 4; i++) energy += moons[i].Energy;
			energyAfterIterations = energy;


			//loop until repeat periods for X, Y, and Z found for part 2
			long loops = iterations;
			while (xPeriod == Int64.MaxValue || yPeriod == Int64.MaxValue || zPeriod == Int64.MaxValue)
			{
				ApplyGravityAndMove(moons);
				loops++;
				//check if x's, y's, or z's are in initial state - if so, set period for that axis
				if (xPeriod == Int64.MaxValue && CompareX(originalMoons, moons)) xPeriod = loops;
				if (yPeriod == Int64.MaxValue && CompareY(originalMoons, moons)) yPeriod = loops;
				if (zPeriod == Int64.MaxValue && CompareZ(originalMoons, moons)) zPeriod = loops;
			}

			returnToOrigin = LCM(xPeriod, LCM(yPeriod, zPeriod));

			//debugging:
			//Console.WriteLine($"xPeriod:{xPeriod.ToString()},yPeriod:{yPeriod.ToString()},zPeriod:{zPeriod.ToString()}");
		}

		static void ApplyGravityAndMove(Moon[] moons)
		{
			for (int j = 0; j < 3; j++)
			{
				for (int k = j + 1; k < 4; k++)
				{
					moons[j].ApplyGravity(moons[k]);
					moons[k].ApplyGravity(moons[j]);
				}
			}
			for (int j = 0; j < 4; j++) moons[j].Move();
		}

		public static long GCD(long a, long b)
		{
			if (b == 0) return a;
			return GCD(b, a % b);
		}

		public static long LCM(long a, long b)
		{
			return a * b / GCD(a, b);
		}


		public static bool CompareX(Moon[] original, Moon[] current)
		{
			for (int i = 0; i < original.Length; i++)
			{
				if (current[i].X != original[i].X || current[i].VX != 0) return false;
			}
			return true;
		}

		public static bool CompareY(Moon[] original, Moon[] current)
		{
			for (int i = 0; i < original.Length; i++)
			{
				if (current[i].Y != original[i].Y || current[i].VY != 0) return false;
			}
			return true;
		}

		public static bool CompareZ(Moon[] original, Moon[] current)
		{
			for (int i = 0; i < original.Length; i++)
			{
				if (current[i].Z != original[i].Z || current[i].VZ != 0) return false;
			}
			return true;
		}

		public class Moon
		{
			public int X;
			public int Y;
			public int Z;

			public int VX = 0;
			public int VY = 0;
			public int VZ = 0;

			public Moon(int x, int y, int z)
			{
				X = x;
				Y = y;
				Z = z;
			}

			public Moon(string moonVal)
			{
				string[] replaceVals = new string[] { "<", "=", "x", "y", "z", ">" };
				foreach (string x in replaceVals) moonVal = moonVal.Replace(x, "");
				var vals = moonVal.Split(',');
				X = Convert.ToInt32(vals[0]);
				Y = Convert.ToInt32(vals[1]);
				Z = Convert.ToInt32(vals[2]);
			}

			public override string ToString()
			{
				return $"<x={X.ToString()},y={Y.ToString()},z={Z.ToString()},vX={VX.ToString()},vY={VY.ToString()},vZ={VZ.ToString()}>";
			}

			public void ApplyGravity(Moon m)
			{
				VX += m.X.CompareTo(X);
				VY += m.Y.CompareTo(Y);
				VZ += m.Z.CompareTo(Z);
			}

			public void Move()
			{
				X += VX;
				Y += VY;
				Z += VZ;
			}

			public int Energy
			{
				get
				{
					return (Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z)) * (Math.Abs(VX) + Math.Abs(VY) + Math.Abs(VZ));
				}

			}
		}



	}
}
