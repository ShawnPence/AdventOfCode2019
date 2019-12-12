using NUnit.Framework;

using AdventOfCode2019.Computer;

namespace AoC19ComputerTests
{
	public class SimpleAddition
	{
		Computer computer;

		[Test]
		public void Addition1()
		{
			string testInput = "1,0,0,5,99,0";
			computer = new Computer(testInput.Split(','));
			computer.Run();
			Assert.AreEqual(2, computer.Ram[5]);
		}
	}
}