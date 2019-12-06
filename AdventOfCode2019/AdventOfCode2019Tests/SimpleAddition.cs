using NUnit.Framework;

using AdventOfCode2019.Computer;

namespace AdventOfCode2019Tests
{
	public class SimpleAddition
	{
		Computer computer;

		[SetUp]
		public void Setup()
		{
			string testInput = "1,0,0,5,99,0";
			computer = new Computer(testInput.Split(','));
		}

		[Test]
		public void Addition1()
		{
			computer.Run();
			Assert.AreEqual(2, computer.Ram[5]);
		}
	}
}