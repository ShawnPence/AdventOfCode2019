using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Computer
{
	public class CPU
	{
		Memory memory;

		public int InstructionPointer { get; private set; } = 0;
		

		delegate int Function2Ints(int x, int y);
		delegate bool Bool1Int(int x);

		public CPU(Memory memory)
		{
			this.memory = memory;
		}

		//current instruction;
		Instruction instruction;

		/// <summary>
		/// get the memory location of the value to be used
		/// </summary>
		/// <param name="param"></param>
		/// <param name="write">if the parameter is to be written instead of read, always return the value at the memory location param past the current instruction pointer</param>
		/// <returns></returns>
		/// <remarks>per Advent of Code instructions, 
		/// position mode (0): memory[instructionPointer + parameter number] contains the memory address of the value to be read
		/// immediate mode (1) instructionPointer + paramater number IS the memory address of the value to be read
		/// </remarks>
		int GetAddress(int param, bool write = false) 
		{
			if (write) return memory[InstructionPointer + param]; //per Day 5 instructions, "Parameters that an instruction writes to will never be in immediate mode."
			return instruction.ParamMode[param] == 0 ? memory[InstructionPointer + param] : InstructionPointer + param;
		}

		/// <summary>
		/// verify that the specified address is in range - if not, set instruction pointer to -2
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		bool checkSize(int address)
		{
			if (address >= memory.Size || address < 0) 
			{
				InstructionPointer = -2;
				return false;
			}
			return true;
		}

		/// <summary>
		/// performs the input function on the values from the 1st and 2nd parameters and saves it to the 3rd (see GetAddress for notes about parameter modes and addresses to use)
		/// </summary>
		/// <param name="function">a function that takes 2 ints as inputs and outputs an int</param>
		void WriteThridParam(Function2Ints function)
		{
			if (memory.Size <= InstructionPointer + 3)
			{
				//pointer is too close to end of memory to complete the operation
				InstructionPointer = -2;
				return;
			}
			int x = GetAddress(1);
			int y = GetAddress(2);
			int z = GetAddress(3, true);
			if (x >= memory.Size || y >= memory.Size || z >= memory.Size)
			{
				//x,y,or z pointer is too close to end of memory to complete the operation
				InstructionPointer = -2;
				return;
			}
			memory[z] = function(memory[x], memory[y]);
			InstructionPointer += 4;
		}

		/// <summary>
		/// if function on the first param is true, jump to the address specified by the 2nd parameter, otherwise increment instruction pointer by 2
		/// </summary>
		/// <param name="function"></param>
		void JumpSecondParam(Bool1Int function)
		{
			if (!checkSize(InstructionPointer + 2)) return;
			int testLocation = GetAddress(1);
			int jumpLocation = GetAddress(2);
			if (!checkSize(jumpLocation) || !checkSize(testLocation)) return;
			InstructionPointer = function(memory[testLocation]) ? memory[jumpLocation] : InstructionPointer + 2;
			return;
		}

		private int[] input;
		public int[] Input
		{
			get => input; 
			
			set
			{
				input = value;
				hasInput = true;
				waitingOnInput = false;
				ii = 0;
			}
		}
		public int ii = 0;
		public bool hasInput = true;
		public bool waitingOnInput = false;
		int Input1()
		{

			//Console.ForegroundColor = ConsoleColor.Magenta;
			//Console.WriteLine("Input:");
			//Console.ResetColor();
			//int input = Convert.ToInt32(Console.ReadLine());
			//return input;

			//for day 7 problems
			int inp = Input[ii];
			ii++;
			if (ii >= Input.Length) hasInput = false;
			return inp;


		}

		public int outputX;

		void Output(object output)
		{

			//Console.ForegroundColor = ConsoleColor.Cyan;
			//Console.WriteLine(output.ToString());
			//Console.ResetColor();

			outputX = Convert.ToInt32(output);
			
		}

		/// <summary>
		/// compute per the rules of the Advent of Code challenge
		/// </summary>
		/// <param name="iP">instruction pointer - starting index with the current instruction</param>
		/// <param name="data"></param>
		/// <returns>returns the next starting point, -1 for application completed successfully, or -2 for error</returns>
		internal void Compute()
		{
			if (!checkSize(InstructionPointer)) return;

			instruction = new Instruction(memory[InstructionPointer]);

			switch (instruction.OpCode)
			{
				case 1: //add
					WriteThridParam((int x, int y) => { return x + y; });
					return;
				case 2: //multiply
					WriteThridParam((int x, int y) => { return x * y; });
					return;
				case 3: //input
					if (!checkSize(InstructionPointer + 1)) return;
					int writeAddress = GetAddress(1, true);
					if (!checkSize(writeAddress)) return;
					if(!hasInput)
					{
						waitingOnInput = true;
						return;
					}
					memory[writeAddress] = Input1();
					InstructionPointer += 2;
					return;
				case 4: //output
					if (!checkSize(InstructionPointer + 1)) return;
					int readAddress = GetAddress(1);
					if (!checkSize(readAddress)) return;
					Output(memory[readAddress]);
					InstructionPointer += 2;
					return;
				case 5: //jump if true
					JumpSecondParam((int x) => { return x != 0; });
					return;
				case 6: //jump if false
					JumpSecondParam((int x) => { return x == 0; });
					return;
				case 7: //less than
					WriteThridParam((int x, int y) => { return x < y ? 1 : 0; });
					break;
				case 8: //equals
					WriteThridParam((int x, int y) => { return x == y ? 1 : 0; });
					break;


				case 99:
					//Console.ForegroundColor = ConsoleColor.Green;
					//Console.WriteLine("end of program");
					//Console.ResetColor();
					InstructionPointer = -1;
					return; //end of program
				default:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("unknown error");
					Console.ResetColor();
					InstructionPointer = -2;
					return; //error - per challenge instructions any command other than 1, 2, or 99 is an error
			}

		}

	}
}
