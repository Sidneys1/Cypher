using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cypher
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("\t\tBorne Cypher, v0.1" + Environment.NewLine);
			Console.WriteLine("Please enter a string to encrypt:");

			string input = Console.ReadLine();

			input = CypherText(input);

			Console.WriteLine(string.Format("Full Cypher: \"{0}\".", input));

			string reconstruct = CypherText(input);

			Console.WriteLine(string.Format("Decyphered: \"{0}\"", reconstruct));

			Console.ReadLine();
		}

		private static string CypherText(string p)
		{
			PadString(ref p);

#if DEBUG

			Console.WriteLine(string.Format("Padded input: \"{0}\".", p));
			Console.WriteLine(string.Format("Chars: {0}, \tHextets: {1}." + Environment.NewLine, p.Length, p.Length / 16));

#endif

			List<string> Hextets = new List<string>();
			List<string> DecypheredHextets = new List<string>();
			StringBuilder b = new StringBuilder(p);

			while (b.Length > 0)
			{
				string hex;

				hex = b.ToString().Substring(0, 16);

				b.Remove(0, 16);

				Hextets.Add(hex);
			}

			for (int i = 0; i < Hextets.Count; i++)
			{
#if DEBUG
				Console.WriteLine(string.Format("Hextet {0}: \"{1}\"", i + 1, Hextets[i]));
#endif
				string cyphered = string.Empty;

				for (int j = 0; j < 4; j++)
				{
					for (int k = j; k < Hextets[i].Length; k += 4)
					{
						cyphered += Hextets[i][k];
					}
				}

				DecypheredHextets.Add(cyphered);
#if DEBUG
				Console.WriteLine(string.Format("\tCyphered: \"{0}\"", cyphered));
#endif
			}

			b.Clear();

			foreach (string str in DecypheredHextets)
			{
				b.Append(str);
			}


			return b.ToString();
		}

		private static void PadString(ref string input)
		{
			int paddingNeeded = input.Length % 16;
			if (paddingNeeded != 0)
				paddingNeeded = 16 - paddingNeeded;

			StringBuilder b = new StringBuilder(input);

			while (paddingNeeded > 0)
			{

				switch (paddingNeeded)
				{
					case 1:
						b.Append('.');
						break;

					case 2:
						b.Append("..");
						break;

					case 3:
						b.Append("...");
						break;

					default:
						AddThreeLetterWord(b);
						break;
				}

				paddingNeeded = b.Length % 16;
				if (paddingNeeded != 0)
					paddingNeeded = 16 - paddingNeeded;
			}

			input = b.ToString();
		}

		private static string[] threeLetters;
		private static Random r = new Random();

		private static void AddThreeLetterWord(StringBuilder b)
		{
			if (threeLetters == null)
				threeLetters = Cypher.Properties.Resources.threeletters.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

			string add = threeLetters[r.Next(0, threeLetters.Length - 1)].ToLower();

			b.Append(" " + add);
		}
	}
}
