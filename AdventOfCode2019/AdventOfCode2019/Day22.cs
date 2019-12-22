using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace AdventOfCode2019
{
	class Day22
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
			List<string> inputs = new List<string>();
			using (StreamReader sr = new StreamReader(fileName))
			{
				while (sr.Peek() != -1)
					inputs.Add(sr.ReadLine());
			}

			NewCardStack(out int[] cards, 10007);
			ProcessInstructions(inputs, ref cards);
			Console.WriteLine(Array.IndexOf(cards, 2019));

			//part 2 - mod math
			long size = 119315717514047;
			long repeats = 101741582076661;
			long endPosition = 2020;

			ReverseParts(inputs, size, out BigInteger a, out BigInteger b);
			
			//(card position * (a^repeats) + b * (1 + a + a^2...a^(repeats-1)) ) % size
			BigInteger result = (BigInteger.ModPow(a, repeats, size) * endPosition + b * (BigInteger.ModPow(a, repeats, size) + size - 1) * BigInteger.ModPow(a - 1, size - 2, size)) % size;

			Console.WriteLine(result);

		}


		private static void ReverseParts(List<string> inputs, long size, out BigInteger a, out BigInteger b)
		{
			//previous position for card# when applying each operation in reverse can be reduced to a and b such that the position = (a * card# + b) % decksize
			//apply all input operations in reverse order to find a and b for the above forumla

			a = 1;
			b = 0;
			for (int i = inputs.Count - 1; i >= 0; i--)
			{
				string line = inputs[i];
				switch (line.Split(' ')[1])
				{
					case "into":
						//new stack
						a *= -1;
						b += 1;
						b *= -1;
						break;
					case "with":
						BigInteger p = BigInteger.ModPow(Convert.ToInt64(line.Split(' ')[3]), size - 2, size); //inverse modulus of increment,cardsize
						a = (a * p);
						b = (b * p);
						break;
					default:
						//cut by increment
						b +=  Convert.ToInt64(line.Split(' ')[1]);
						break;
				}
				a = (a + size) % size;
				b = (b + size) % size;
			}

		}


		private static void NewCardStack(out int[] cards, int count)
		{
			cards = new int[count];
			for (int i = 0; i < count; i++)
			{
				cards[i] = i;
			}
		}

		private static void ProcessInstructions(List<string> inputs,ref int[] cards)
		{
			foreach (string line in inputs)
			{
				switch (line.Split(' ')[1])
				{
					case "into":
						NewStack(ref cards);
						break;
					case "with":
						DealWithIncrement(ref cards, Convert.ToInt32(line.Split(' ')[3]));
						break;
					default:
						Cut(ref cards, Convert.ToInt32(line.Split(' ')[1]));
						break;

				}

			}
		}

		public static void DealWithIncrement(ref int[] cards, int increment)
		{
			int[] output = new int[cards.Length];
			for (int i = 0; i < cards.Length; i++)
			{
				output[(i * increment) % cards.Length] = cards[i];
			}
			cards = output;
		}

		public static void NewStack(ref int[] cards)
		{
			Array.Reverse(cards);
		}

		public static void Cut(ref int[] cards, int n)
		{
			int[] output = new int[cards.Length];
			if (n >= 0)
			{
				for (int i = 0; i < cards.Length; i++)
				{
					output[i] = cards[(i + n) % cards.Length];
				}
			}
			else
			{
				for (int i = 0; i < cards.Length; i++)
				{
					output[i] = cards[(cards.Length + i + n) % cards.Length];
				}
			}
			cards = output;
		}

	}


}
