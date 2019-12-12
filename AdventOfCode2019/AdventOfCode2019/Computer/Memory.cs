using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Computer
{
	public class Memory
	{
		List<long> ram; //using list to allow dynamic resizing memory if necessary for coding challenges

		public long this[int index]
		{
			get {
				while (ram.Count < index + 1) ram.Add(0);  //per instructions that ram should be significantly larger than input, allow for arbitrarily large ram
				return ram[index];
			}
			set { 
				while (ram.Count < index + 1) ram.Add(0); 
				ram[index] = value;
			}
		}

		public int Size { get => ram.Count; }

		public void ExtendRam(int size)
		{
			if (ram.Count < size)
			{
				while (ram.Count < size) ram.Add(0);
			}
		}

		public Memory(int size = 1024)
		{
			ram = new List<long>(new long[size]);
			
		}

		public Memory(string[] input)
		{
				ram = new List<long>(Array.ConvertAll(input, x => Convert.ToInt64(x)));
		}



	}
}
