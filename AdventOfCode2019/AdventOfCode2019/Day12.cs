using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode2019
{
	class Day12
	{
		public static void Problem()
		{
			///* example 1 (test with 10 steps) */
			///*<x=-1, y=0, z=2>
			//<x=2, y=-10, z=-7>
			//<x=4, y=-8, z=8>
			//<x=3, y=5, z=-1>
			//*/
			//moon[] moons = new moon[4];
			//moons[0] = new moon(-1, 0, 2);
			//moons[1] = new moon(2, -10, -7);
			//moons[2] = new moon(4, -8, 8);
			//moons[3] = new moon(3, 5, -1);


			/*//Example 2 (test with 100 steps)
			 * <x=-8, y=-10, z=0>
				<x=5, y=5, z=10>
				<x=2, y=-7, z=3>
				<x=9, y=-8, z=-3>
			 * */
			//moon[] moons = new moon[4];
			//moons[0] = new moon(-8, -10, 0);
			//moons[1] = new moon(5, 5, 10);
			//moons[2] = new moon(2, -7, 3);
			//moons[3] = new moon(9, -8, -3);



			//actual problem
			/*
			<x=-1, y=-4, z=0>
			<x=4, y=7, z=-1>
			<x=-14, y=-10, z=9>
			<x=1, y=2, z=17>
			 */
			moon[] moons = new moon[4];
			moons[0] = new moon(-1, -4, 0);
			moons[1] = new moon(4, 7, -1);
			moons[2] = new moon(-14, -10, 9);
			moons[3] = new moon(1, 2, 17);


			moon[] originalMoons = new moon[4];
			originalMoons[0] = new moon(-1, -4, 0);
			originalMoons[1] = new moon(4, 7, -1);
			originalMoons[2] = new moon(-14, -10, 9);
			originalMoons[3] = new moon(1, 2, 17);


			/*
			 * The x, y, and z values are independent - for part 2, find the repeat periods of 
			 * each individual part and then find the least common multiple of the 3 values
			 */
			long xPeriod = Int64.MaxValue;
			long yPeriod = Int64.MaxValue;
			long zPeriod = Int64.MaxValue;

			for (int i = 0; i < 1000; i++)
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

				//check if x's, y's, or z's are in initial state - if so, set period for that axis
				if (xPeriod == Int64.MaxValue && compareX(originalMoons, moons)) xPeriod = i + 1;
				if (yPeriod == Int64.MaxValue && compareY(originalMoons, moons)) yPeriod = i + 1;
				if (zPeriod == Int64.MaxValue && compareZ(originalMoons, moons)) zPeriod = i + 1;



				////debugging use only
				//Console.WriteLine($"after {(i + 1).ToString()} steps:");
				//for (int j = 0; j < 4; j++)
				//{
				//	Console.WriteLine($"<x={moons[j].X},y={moons[j].Y},z={moons[j].Z},vx={moons[j].VX},vy={moons[j].VY},vz={moons[j].VZ}>");
				//}
			}

			int energy = 0;
			for (int i = 0; i < 4; i++) energy += moons[i].Energy;
			Console.WriteLine(energy);

			long loops = 1000;
			while (xPeriod == Int64.MaxValue || yPeriod == Int64.MaxValue || zPeriod == Int64.MaxValue)
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
				loops++;


				//check if x's, y's, or z's are in initial state - if so, set period for that axis
				if (xPeriod == Int64.MaxValue && compareX(originalMoons, moons)) xPeriod = loops;
				if (yPeriod == Int64.MaxValue && compareY(originalMoons, moons)) yPeriod = loops;
				if (zPeriod == Int64.MaxValue && compareZ(originalMoons, moons)) zPeriod = loops;
			}
			

			Console.WriteLine(LCM(xPeriod, LCM(yPeriod, zPeriod)).ToString());

			//debugging:
			//Console.WriteLine($"xPeriod:{xPeriod.ToString()},yPeriod:{yPeriod.ToString()},zPeriod:{zPeriod.ToString()}");


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


		public static bool compareX(moon[] original, moon[] current)
		{
			for (int i = 0; i < original.Length; i++)
			{
				if (current[i].X != original[i].X || current[i].VX != 0) return false;
			}
			return true;
		}

		public static bool compareY(moon[] original, moon[] current)
		{
			for (int i = 0; i < original.Length; i++)
			{
				if (current[i].Y != original[i].Y || current[i].VY != 0) return false;
			}
			return true;
		}

		public static bool compareZ(moon[] original, moon[] current)
		{
			for (int i = 0; i < original.Length; i++)
			{
				if (current[i].Z != original[i].Z || current[i].VZ != 0) return false;
			}
			return true;
		}

		public class moon
		{
			public int X;
			public int Y;
			public int Z;

			public int VX = 0;
			public int VY = 0;
			public int VZ = 0;

			public moon(int x, int y, int z)
			{
				X = x;
				Y = y;
				Z = z;
			}

			public moon(string moonVal)
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

			public void ApplyGravity(moon m)
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
