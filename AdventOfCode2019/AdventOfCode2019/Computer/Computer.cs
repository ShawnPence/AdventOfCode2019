using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode2019.Computer
{
	public class Computer
	{
		Memory ram;
		public Memory Ram { get => ram; }


		CPU processor;

		public Computer(string[] input)
		{
			ram = new Memory(input);
			processor = new CPU(ram);
		}

		public Computer(string filename)
		{
			using (StreamReader sr = new StreamReader(filename, true))
			{
				ram = new Memory(sr.ReadLine().Split(','));
			}
			processor = new CPU(ram);
		}

	
		public void Run()
		{
			while (processor.InstructionPointer >= 0) processor.Compute();
		}

	}
}
