using NUnit.Framework;
using AdventOfCode2019;

namespace AoC19ComputerTests
{
	public class Day2Examples
	{
		Computer computer;

		[SetUp]
		public void Setup()
		{

		}

		[Test]
		public void Example1()
		{
			string testInput = "1,9,10,3,2,3,11,0,99,30,40,50";
			computer = new Computer(testInput.Split(','));
			computer.Run();
			Assert.AreEqual(3500, computer.Ram[0]);
		}

		[Test]
		public void Example2()
		{
			string testInput = "1,0,0,0,99";
			computer = new Computer(testInput.Split(','));
			computer.Run();
			Assert.AreEqual(2, computer.Ram[0]);
		}

		[Test]
		public void Example3()
		{
			string testInput = "2,3,0,3,99";
			computer = new Computer(testInput.Split(','));
			computer.Run();
			Assert.AreEqual(6, computer.Ram[3]);
		}

		[Test]
		public void Example4()
		{
			string testInput = "2,4,4,5,99,0";
			computer = new Computer(testInput.Split(','));
			computer.Run();
			Assert.AreEqual(9801, computer.Ram[5]);
		}

		[Test]
		public void Example5()
		{
			string testInput = "1,1,1,4,99,5,6,0,99";
			computer = new Computer(testInput.Split(','));
			computer.Run();
			Assert.AreEqual(30, computer.Ram[0]);
			Assert.AreEqual(2, computer.Ram[4]);
		}




	}
}