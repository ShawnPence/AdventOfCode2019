using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2019
{
	class Day13
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
			Computer.Computer c = new Computer.Computer(fileName);
			c.Run();
			long[] output = c.processor.outputQueue.ToArray();
			int blocks = 0;
			for (int i = 2; i < output.Length; i += 3)
			{
				if (output[i] == 2) blocks++;
			}
			Console.WriteLine(blocks);

			//added to determine screen size for part 2
			long maxX = 0;
			long maxY = 0;
			for (int i = 0; i < output.Length; i += 3)
			{
				maxX = Math.Max(maxX, output[i]);
				maxY = Math.Max(maxY, output[i + 1]);
			}

			Console.WriteLine($"maxX:{maxX} maxY:{maxY}");

		}

		public static void Problem2()
		{
			Console.WriteLine("Input file:");
			string fileName = Console.ReadLine();
			while (!File.Exists(fileName))
			{
				Console.WriteLine("file not found - try again:");
				fileName = Console.ReadLine();
			}


			Computer.Computer c = new Computer.Computer(fileName);
			c.Ram[0] = 2;
			while (c.processor.InstructionPointer >= 0)
			{
				c.Run();
				long[] output = c.processor.outputQueue.ToArray();
				c.processor.outputQueue.Clear();
				int eX = 0;
				int oX = 0;
				for (int i = 0; i < output.Length - 2; i += 3)
				{
					int x = Convert.ToInt32(output[i]);
					int y = Convert.ToInt32(output[i + 1]);

					if (x == -1 && y == 0)
					{
						Console.SetCursorPosition(0, 0);
						Console.Write($"Score: {output[i + 2].ToString().PadLeft(6, '0')}");
					}
					else
					{
						Console.SetCursorPosition(x, y + 2);
						switch (output[i + 2])
						{
							case 0:
								Console.Write(" ");
								break;
							case 1:
								Console.Write('|');
								break;
							case 2:
								Console.Write("#");
								break;
							case 3:
								Console.Write("=");
								eX = x;
								break;
							case 4:
								Console.Write("o");
								oX = x;
								break;

						}
					}


				}//draw screen for loop

				//move paddle to track the ball
				c.processor.Input(oX.CompareTo(eX));

			};

			{
				//draw any remaining output after game ends
				long[] output = c.processor.outputQueue.ToArray();
				for (int i = 0; i < output.Length - 2; i += 3)
				{
					int x = Convert.ToInt32(output[i]);
					int y = Convert.ToInt32(output[i + 1]);

					if (x == -1 && y == 0)
					{
						Console.SetCursorPosition(0, 0);
						Console.Write($"Score: {output[i + 2].ToString().PadLeft(6, '0')}");
					}
					else
					{
						Console.SetCursorPosition(x, y + 2);
						switch (output[i + 2])
						{
							case 1:
								Console.Write('|');
								break;
							case 2:
								Console.Write("#");
								break;
							case 3:
								Console.Write("=");
								break;
							case 4:
								Console.Write("o");
								break;
						}
					}


				}//draw screen for loop

			}
			Console.SetCursorPosition(0, 22);
			Console.Write("Press any key to end");
			Console.ReadKey();


		}
	}
}
