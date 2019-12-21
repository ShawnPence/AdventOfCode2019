using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019
{
	public class CPU
	{
		Memory memory;

		/// <summary>memory location of the current instruction</summary>
		public int InstructionPointer { get; private set; } = 0;


		delegate long Function2Long(long x, long y);
		delegate bool Bool1Long(long x);

		public CPU(Memory memory)
		{
			this.memory = memory;
		}

		/// <summary>the current instruction</summary>
		Instruction instruction;


		int relativeBase = 0;

		/// <summary>
		/// get the memory location of the value to be used
		/// </summary>
		/// <param name="param"></param>
		/// <param name="write">if the parameter is to be written instead of read, always return the value at the memory location param past the current instruction pointer</param>
		/// <returns></returns>
		int GetAddress(int param, bool write = false)
		{
			switch (instruction.ParamMode[param])
			{
				case 0:
					// in mode 0, instructionPointer + param contains the memory address of the value
					return Convert.ToInt32(memory[InstructionPointer + param]);
				case 1:
					//in mode 1 (except when writing), instructionPointer + param IS the memory address to read
					//per Day 5 instructions, "Parameters that an instruction writes to will never be in immediate mode."
					return write ? Convert.ToInt32(memory[InstructionPointer + param]) : InstructionPointer + param;
				case 2:
					//mode 2 works like mode 0 but the returned address should be offset by relativeBase
					return Convert.ToInt32(memory[InstructionPointer + param] + relativeBase);
			}
			return Convert.ToInt32(memory[InstructionPointer + param]);
		}


		/// <summary>
		/// performs the input function on the values from the 1st and 2nd parameters and saves it to the 3rd (see GetAddress for notes about parameter modes and addresses to use)
		/// </summary>
		/// <param name="function">a function that takes 2 long as inputs and outputs an long</param>
		void WriteThridParam(Function2Long function)
		{
			int x = GetAddress(1);
			int y = GetAddress(2);
			int z = GetAddress(3, true);
			memory[z] = function(memory[x], memory[y]);
			InstructionPointer += 4;
		}

		/// <summary>
		/// if function on the first param is true, jump to the address specified by the 2nd parameter, otherwise increment instruction pointer by 2
		/// </summary>
		/// <param name="function">a function that takes 1 long as input and returns a boolean</param>
		void JumpSecondParam(Bool1Long function)
		{
			int testLocation = GetAddress(1);
			int jumpLocation = GetAddress(2);
			InstructionPointer = Convert.ToInt32(function(memory[testLocation]) ? memory[jumpLocation] : InstructionPointer + 3);
			return;
		}

		void shiftRelativeBase() {
			int amountAddress = Convert.ToInt32(GetAddress(1));
			relativeBase += Convert.ToInt32(memory[amountAddress]);
			InstructionPointer += 2;
		}


		Queue<long> input = new Queue<long>();
		public bool HasInput { get => input.Count > 0; }
		public bool waitingOnInput = false;
		public void Input(long i) 
		{
			waitingOnInput = false; 
			input.Enqueue(i);
		}
		public void Input(long[] i)
		{
			if (i.Length < 1) return;
			waitingOnInput = false;
			foreach (long l in i) input.Enqueue(l);
		}

		public Queue<long> outputQueue = new Queue<long>();
		void Output(object output)
		{
			outputQueue.Enqueue(Convert.ToInt64(output));
		}


		/// <summary>
		/// compute per the rules of the Advent of Code challenge
		/// </summary>
		internal void Compute()
		{
			instruction = new Instruction(Convert.ToInt32(memory[InstructionPointer]));
			switch (instruction.OpCode)
			{
				case 1: //add
					WriteThridParam((long x, long y) => { return x + y; });
					return;
				case 2: //multiply
					WriteThridParam((long x, long y) => { return x * y; });
					return;
				case 3: //input
					int writeAddress = GetAddress(1, true);
					if(!HasInput)
					{
						waitingOnInput = true;
						return;
					}
					memory[writeAddress] = input.Dequeue();
					InstructionPointer += 2;
					return;
				case 4: //output
					int readAddress = GetAddress(1);
					Output(memory[readAddress]);
					InstructionPointer += 2;
					return;
				case 5: //jump if true
					JumpSecondParam((long x) => { return x != 0; });
					break;
				case 6: //jump if false
					JumpSecondParam((long x) => { return x == 0; });
					break;
				case 7: //less than
					WriteThridParam((long x, long y) => { return x < y ? 1 : 0; });
					break;
				case 8: //equals
					WriteThridParam((long x, long y) => { return x == y ? 1 : 0; });
					break;
				case 9:
					shiftRelativeBase();
					break;
				case 99:
					InstructionPointer = -1;
					return; //end of program
				default:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("unknown error");
					Console.ResetColor();
					InstructionPointer = -2;
					return; //error - per challenge instructions any unrecognized command is an error
			}

		}

	}
}
