using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode2019
{
	class Day15
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

			//inputs
			//north (1), south (2), west (3), and east (4)

			//outputs
			/*
			 *  0: The repair droid hit a wall. Its position has not changed.
			 *  1: The repair droid has moved one step in the requested direction.
			 *  2: The repair droid has moved one step in the requested direction; its new position is the location of the oxygen system.
			 * 
			 */

			bool done = false;
			
			Dictionary<int, HashSet<int>> walls = new Dictionary<int, HashSet<int>>();
			Dictionary<int, HashSet<int>> found = new Dictionary<int, HashSet<int>>();
			Queue<DroidPath> next = new Queue<DroidPath>(); 
			DroidPath start = new DroidPath();
			found[0] = new HashSet<int>();
			found[0].Add(0);
			AddNextPaths(next, start, walls, found);

			DroidPath oxygenLocation = new DroidPath();

			while (!done && next.Count > 0)
			{
				//dequeue next item
				DroidPath d = next.Dequeue();
				//run computer to end of inputs
				Computer.Computer c = new Computer.Computer(fileName);
				foreach (int m in d.stepOrder) c.processor.Input(m);
				c.Run();

				//read last output queue item
				long[] output = c.processor.outputQueue.ToArray();
				switch (output[output.Length - 1])
				{
					case 0:
						//wall - add to walls list - don't attempt to move
						if (!walls.ContainsKey(d.X)) walls[d.X] = new HashSet<int>();
						walls[d.X].Add(d.Y);
						break;
					case 1:
						//enqueue next steps
						AddNextPaths(next, d, walls, found);
						break;
					case 2:
						done = true;
						oxygenLocation = d;
						Console.WriteLine(d.StepCount);// part one
						break;
				}
			}

			Dictionary<int, Dictionary<int,int>> stepsTo = new Dictionary<int, Dictionary<int, int>>();
			found.Clear();
			found[oxygenLocation.X] = new HashSet<int>();
			found[oxygenLocation.X].Add(oxygenLocation.Y);
			oxygenLocation.StepCount = 0;
			next.Clear();
			AddNextPaths(next, oxygenLocation, walls, found);
			while (next.Count > 0)
			{
				//dequeue next item
				DroidPath d = next.Dequeue();

				//run computer to end of inputs
				Computer.Computer c = new Computer.Computer(fileName);
				foreach (int m in d.stepOrder) c.processor.Input(m);
				c.Run();

				//read last output queue item
				long[] output = c.processor.outputQueue.ToArray();
				switch (output[output.Length - 1])
				{
					case 0:
						//wall - add to walls list - don't attempt to move
						if (!walls.ContainsKey(d.X)) walls[d.X] = new HashSet<int>();
						walls[d.X].Add(d.Y);
						break;
					case 1:
						//enqueue next steps
						AddNextPaths(next, d, walls, found);
						if (!stepsTo.ContainsKey(d.X) || !stepsTo[d.X].ContainsKey(d.Y))
						{
							if (!stepsTo.ContainsKey(d.X)) stepsTo[d.X] = new Dictionary<int, int>();
							stepsTo[d.X][d.Y] = d.StepCount;
						}
						break;
					case 2:
						//already been here - no need to keep searching past this
						break;
				}
			}//end while loop 2

			int steps = 0;
			foreach (int x in stepsTo.Keys)
			{
				foreach (int y in stepsTo[x].Keys)
				{
					steps = Math.Max(steps, stepsTo[x][y]);
				}
			}

			Console.WriteLine(steps);
		}

		static void AddNextPaths(Queue<DroidPath> next, DroidPath current, Dictionary<int, HashSet<int>> walls, Dictionary<int, HashSet<int>> found)
		{
			//inputs
			//north (1), south (2), west (3), and east (4)
			for (int i = 1; i <= 4; i++)
			{
				DroidPath d = new DroidPath();
				d.stepOrder.AddRange(current.stepOrder);
				d.StepCount = current.StepCount + 1;
				d.X = current.X;
				d.Y = current.Y;
				d.stepOrder.Add(i);
				switch (i)
				{
					case 1:
						d.Y--;
						break;
					case 2:
						d.Y++;
						break;
					case 3:
						d.X--;
						break;
					case 4:
						d.X++;
						break;		
				}
				bool add = true;
				if (found.ContainsKey(d.X) && found[d.X].Contains(d.Y)) add = false;  //already visited this location - don't add to queue
				if (walls.ContainsKey(d.X) && walls[d.X].Contains(d.Y)) add = false; //don't walk into wall
				if (!found.ContainsKey(d.X)) found[d.X] = new HashSet<int>();
				found[d.X].Add(d.Y);
				if(add) next.Enqueue(d);
			}
		}

		class DroidPath
		{
			public int X = 0;
			public int Y = 0;
			public int StepCount = 0;
			public List<int> stepOrder = new List<int>();
		}

	}
}
