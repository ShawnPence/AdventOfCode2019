using System;
using System.Collections.Generic;
using System.Text;
using AdventOfCode2019;
using NUnit.Framework;

namespace AdventOfCode2019Tests
{
	class Day12Examples
	{
		[Test]
		public void Example1()
		{
			string[] moonStrings = new string[] { "<x=-1, y=0, z=2>", "<x=2, y=-10, z=-7>", "<x=4, y=-8, z=8>", "<x=3, y=5, z=-1>" };
			Day12.Problem(10, moonStrings, out int energyAfterIterations, out long returnToOrigin);
			Assert.AreEqual(179, energyAfterIterations);
			Assert.AreEqual(2772, returnToOrigin);

		}

		[Test]
		public void Example2()
		{
			string[] moonStrings = new string[] { "<x=-8, y=-10, z=0>","<x=5, y=5, z=10>","<x=2, y=-7, z=3>","<x=9, y=-8, z=-3>" };
			Day12.Problem(100, moonStrings, out int energyAfterIterations, out long returnToOrigin);
			Assert.AreEqual(1940, energyAfterIterations);
			Assert.AreEqual(4686774924, returnToOrigin);
		}

	}
}
