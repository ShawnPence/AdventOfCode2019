using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Numerics;
using AdventOfCode2019.Computer;

namespace AdventOfCode2019
{
	class Day7
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

			long best = 0;
			for (int a = 0; a < 5; a++)
			{
				long outA = getOutput(fileName, a, 0);

				for (int b = 0; b < 5; b++)
				{


					if (b == a) continue;

					long outB = getOutput(fileName, b, outA);

					for (int c = 0; c < 5; c++)
					{
						if (c == a || c == b) continue;
						long outC = getOutput(fileName, c, outB);

						for (int d = 0; d < 5; d++)
						{
							if (d == a || d == b || d == c) continue;
							int e = 10 - a - b - c - d;

							long outD = getOutput(fileName, d, outC);
							
							best = Math.Max(best, getOutput(fileName, e, outD));
						}
					}
				}
			}
			Console.WriteLine(best.ToString());
			


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

			long best = 0;
			for (int a = 0; a < 5; a++)
			{

				for (int b = 0; b < 5; b++)
				{


					if (b == a) continue;


					for (int c = 0; c < 5; c++)
					{
						if (c == a || c == b) continue;

						for (int d = 0; d < 5; d++)
						{
							if (d == a || d == b || d == c) continue;
							int e = 10 - a - b - c - d;
							Computer.Computer[] comp = new Computer.Computer[5];
							long[] signal = new long[5];
							int[] vals = new int[] { a + 5, b + 5, c + 5, d + 5, e + 5 };
							for (int i = 0; i < 5; i++)
							{ comp[i] = new Computer.Computer(fileName);
								comp[i].processor.Input(vals[i]);
								comp[i].processor.Input(signal[(i + 4) % 5]);
								comp[i].Run();
								signal[i] = comp[i].processor.outputQueue.Dequeue();
							}
							int l = 0;
							while (comp[4].processor.InstructionPointer > 0)
							{
								//run until E's program terminates
								comp[l].processor.Input(signal[(l+4) % 5]);
								comp[l].Run();
								signal[l] = comp[l].processor.outputQueue.Dequeue();
								l++;
								l = l % 5;
							}
							best = Math.Max(best, l == 0 ? signal[4] : comp[4].processor.outputQueue.Dequeue());

						}
					}
				}
			}
			Console.WriteLine(best);



		}

		public static long getOutput(string fileName, int phase, long input)
		{
			Computer.Computer comp = new Computer.Computer(fileName);
			comp.processor.Input(new long[] { phase, input });
			comp.Run();
			return comp.processor.outputQueue.Dequeue();
		}
	}
}
