using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode2019
{
	class Day11
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
			int x = 0;
			int y = 0;
			var colors = new Dictionary<int, Dictionary<int, long>>();
			int direction = 0;

			Computer c = new Computer(fileName);
			c.Run();
			while (c.processor.InstructionPointer >= 0)
			{
				//read output queue, move as instructed

				while (c.processor.outputQueue.Count > 0)
				{
					long colorToPaint = c.processor.outputQueue.Dequeue();
					if (!colors.ContainsKey(x)) colors[x] = new Dictionary<int, long>();
					colors[x][y] = colorToPaint;
					direction = Turn(direction, c.processor.outputQueue.Dequeue());
					switch (direction)
					{
						case 0:
							y--;
							break;
						case 90:
							x++;
							break;
						case 180:
							y++;
							break;
						case 270:
							x--;
							break;

					}


				}

				if (!colors.ContainsKey(x) || !colors[x].ContainsKey(y))
				{
					c.processor.Input(0);
				}
				else
				{
					c.processor.Input(colors[x][y]);
				}

				c.Run();

			}

			//read any remaining queue once processor is complete
			while (c.processor.outputQueue.Count > 0)
			{
				long colorToPaint = c.processor.outputQueue.Dequeue();
				if (!colors.ContainsKey(x)) colors[x] = new Dictionary<int, long>();
				colors[x][y] = colorToPaint;
				direction = Turn(direction, c.processor.outputQueue.Dequeue());
				switch (direction)
				{
					case 0:
						y--;
						break;
					case 90:
						x++;
						break;
					case 180:
						y++;
						break;
					case 270:
						x--;
						break;

				}


			}

			//count painted cells

			int painted = 0;
			foreach (int keyX in colors.Keys)
			{
				foreach (int keyY in colors[keyX].Keys) painted++;
			}

			Console.WriteLine(painted);

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
			int x = 0;
			int y = 0;
			var colors = new Dictionary<int, Dictionary<int, long>>();
			int direction = 0;

			Computer c = new Computer(fileName);
			//tell computer first input is painted;
			c.processor.Input(1);
			c.Run();
			while (c.processor.InstructionPointer >= 0)
			{
				//read output queue, move as instructed

				while (c.processor.outputQueue.Count > 0)
				{
					long colorToPaint = c.processor.outputQueue.Dequeue();
					if (!colors.ContainsKey(x)) colors[x] = new Dictionary<int, long>();
					colors[x][y] = colorToPaint;
					direction = Turn(direction, c.processor.outputQueue.Dequeue());
					switch (direction)
					{
						case 0:
							y--;
							break;
						case 90:
							x++;
							break;
						case 180:
							y++;
							break;
						case 270:
							x--;
							break;

					}


				}

				if (!colors.ContainsKey(x) || !colors[x].ContainsKey(y))
				{
					c.processor.Input( 0);
				}
				else
				{
					c.processor.Input(colors[x][y]);
				}

				c.Run();

			}

			//read any remaining queue once processor is complete
			while (c.processor.outputQueue.Count > 0)
			{
				long colorToPaint = c.processor.outputQueue.Dequeue();
				if (!colors.ContainsKey(x)) colors[x] = new Dictionary<int, long>();
				colors[x][y] = colorToPaint;
				direction = Turn(direction, c.processor.outputQueue.Dequeue());
				switch (direction)
				{
					case 0:
						y--;
						break;
					case 90:
						x++;
						break;
					case 180:
						y++;
						break;
					case 270:
						x--;
						break;

				}


			}

			int minX = 0;
			int maxX = 0;
			int minY = 0;
			int maxY = 0;

			foreach (int xKey in colors.Keys)
			{
				minX = Math.Min(xKey, minX);
				maxX = Math.Max(xKey, maxX);
				foreach (int yKey in colors[xKey].Keys)
				{
					minY = Math.Min(yKey, minY);
					maxY = Math.Max(yKey, maxY);
				}
			}

			for (int y1 = minY; y1 <= maxY; y1++)
			{
				StringBuilder s = new StringBuilder();
				for (int x1 = minX; x1 <= maxX; x1++)
				{
					if (!colors.ContainsKey(x1) || !colors[x1].ContainsKey(y1))
					{
						s.Append(" ");
					}
					else
					{
						s.Append(colors[x1][y1] == 0 ? " " : "#");
					}
				}
				Console.WriteLine(s.ToString());
			}
			

		}


		public static int Turn(int startDir, long turn)
		{
			startDir += turn == 0 ? -90 : 90;
			return startDir < 0 ? startDir + 360 : startDir > 270 ? startDir - 360 : startDir;
		}
	}
}
