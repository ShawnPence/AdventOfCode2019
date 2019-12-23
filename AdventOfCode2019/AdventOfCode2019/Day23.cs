using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode2019
{
	class Day23
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
			List<Computer> computers = new List<Computer>();
			for (int i = 0; i < 50; i++)
			{
				var computer = new Computer(fileName);
				computer.Input(i);
				computer.Run();
				computers.Add(computer);
			}

			bool part1Complete = false;
			bool part2Complete = false;

			long natX = 0;
			long natY = 0;
			HashSet<int> waiting = new HashSet<int>();
			long natYDelivered = Int64.MinValue;

			while (!part1Complete || !part2Complete)
			{
				for (int i = 0; i < 50; i++)
				{
					if(i == 0 && waiting.Count == 50)
					{
						computers[i].Input(natX);
						computers[i].Input(natY);
						if (natY == natYDelivered)
						{
							part2Complete = true;
							Console.WriteLine($"problem 2: {natYDelivered}");
						}
						else
						{
							natYDelivered = natY;

						}
						waiting.Remove(0);
					}

					//check for messages on the output queue
					while (computers[i].Output.Count > 0)
					{
						int destination = Convert.ToInt32(computers[i].Output.Dequeue());
						long x = computers[i].Output.Dequeue();
						long y = computers[i].Output.Dequeue();

						if (destination == 255 )
						{
							natX = x;
							natY = y;
							if(!part1Complete)
							{
								Console.WriteLine($"part 1: {y}");
								part1Complete = true;
							}
						}
						else
						{
							//deliver messages to the appropriate computer
							computers[destination].Input(x);
							computers[destination].Input(y);
							if (waiting.Contains(destination)) waiting.Remove(destination);
						}
					}

					//if this computer's input queue is empty, queue -1 per Advent of Code day 23 instructions
					if (!computers[i].Processor.HasInput)
					{
						waiting.Add(i);
						computers[i].Input(-1);
					}
					else
					{
						if (waiting.Contains(i)) waiting.Remove(i);
					}

					computers[i].Run();

				}
			}
		}
	}
}
