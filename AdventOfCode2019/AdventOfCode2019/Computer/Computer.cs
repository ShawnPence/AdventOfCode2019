using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode2019
{
	public class Computer
	{
		Memory ram;
		public Memory Ram { get => ram; set => ram = value; }


		public CPU Processor { get; private set; }

		public Computer(string[] input)
		{
			ram = new Memory(input);
			Processor = new CPU(ram);
		}

		public Computer(string filename)
		{
			using (StreamReader sr = new StreamReader(filename, true))
			{
				ram = new Memory(sr.ReadLine().Split(','));
			}
			Processor = new CPU(ram);
		}

		public void Input(long val) => Processor.Input(val);
		public void Input(long[] val) => Processor.Input(val);
		public Queue<long> Output => Processor.OutputQueue;
	
		public void Run()
		{
			while (Processor.InstructionPointer >= 0 && !Processor.WaitingOnInput) Processor.Compute();
		}

	}
}
