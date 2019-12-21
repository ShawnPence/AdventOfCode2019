using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019
{ 
	class Instruction
	{
		readonly int instruction;
		public int[] ParamMode { get; }
		public int OpCode { get; }


		public Instruction(int input)
		{
			instruction = input;
			OpCode = instruction % 100;
			ParamMode = new int[4]; //longer than needed to use 1-based numbering 1st / 2nd / 3rd to match challenge description text
			ParamMode[1] = instruction / 100 % 10;
			ParamMode[2] = instruction / 1000 % 10;
			ParamMode[3] = instruction / 10000;
		}
	}
}
