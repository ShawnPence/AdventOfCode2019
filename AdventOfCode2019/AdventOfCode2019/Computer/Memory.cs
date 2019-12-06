using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Computer
{
	public class Memory
	{
		List<int> ram; //using list to allow dynamic resizing memory if necessary for coding challenges

		public int this[int index]
		{
			get { return ram[index]; }
			set { ram[index] = value; }
		}

		public int Size { get => ram.Count; }

		public Memory(int size = 1024)
		{
			ram = new List<int>(new int[size]);
			
		}

		public Memory(string[] input)
		{
				ram = new List<int>(Array.ConvertAll(input, x => Convert.ToInt32(x)));
		}



	}
}
