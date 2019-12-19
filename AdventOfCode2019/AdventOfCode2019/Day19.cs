using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode2019
{
	class Day19
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

			int output = 0;
			for (int x = 0; x < 50; x++)
				for (int y = 0; y < 50; y++)
				{
					Computer.Computer c = new Computer.Computer(fileName);
					c.Input(x);
					c.Input(y);
					c.Run();
					if (c.Output.Dequeue() == 1) output++;
				}


			Console.WriteLine(output);


			//part 2
			bool foundAnswer = false;
			int x1 = 0;
			for (int y = 0; y < 10000 && !foundAnswer; y++)
			{
				bool found1 = false;
				for (int x = x1; x < 10000 && !found1; x++)
				{
					Computer.Computer c = new Computer.Computer(fileName);
					c.Input(x);
					c.Input(y);
					c.Run();
					long val = c.Output.Dequeue();
					if (val == 1)
					{
						found1 = true;
						x1 = x;
						//go down 99 rows, then over until the first 1 is found - mark that as tempX for now
						int tempX = x;
						int tempY = y + 99;
						long val2 = 0;
						while (val2 == 0 && tempX < 10000)
						{
							Computer.Computer c2 = new Computer.Computer(fileName);
							c2.Input(tempX);
							c2.Input(tempY);
							c2.Run();
							val2 = c2.Output.Dequeue();
							if (val2 == 0) tempX++;
						}
						//check if tempX+99 at original y is a 1
						Computer.Computer c3 = new Computer.Computer(fileName);
						c3.Input(tempX + 99);
						c3.Input(y);
						c3.Run();
						long val3 = c3.Output.Dequeue();
						if (val3 == 1)
						{
							foundAnswer = true;
							Console.WriteLine(tempX * 10000 + y);
						}

					}

				}//for loop x

			}//for loop y

		}//Problems()

	}//class

}
