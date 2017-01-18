using System;
using System.Collections.Generic;
using System.Text;
using Cypher.Properties;

namespace Cypher {
    internal class Program {
        private static string[] _threeLetters;
        private static readonly Random Rand = new Random();

        private static void Main() {
            Console.WriteLine("\t\tBorne Cypher, v0.1" + Environment.NewLine);
            Console.WriteLine("Please enter a string to encrypt:");

            var input = Console.ReadLine();

            input = CypherText(input);

            Console.WriteLine($"Full Cypher: \"{input}\".");

            var reconstruct = CypherText(input);

            Console.WriteLine($"Decyphered: \"{reconstruct}\"");

            Console.ReadLine();
        }

        private static string CypherText(string p) {
            PadString(ref p);

#if DEBUG

			Console.WriteLine(string.Format("Padded input: \"{0}\".", p));
			Console.WriteLine(string.Format("Chars: {0}, \tHextets: {1}." + Environment.NewLine, p.Length, p.Length / 16));

#endif

            var hextets = new List<string>();
            var decypheredHextets = new List<string>();
            var b = new StringBuilder(p);

            while (b.Length > 0) {
                var hex = b.ToString().Substring(0, 16);

                b.Remove(0, 16);

                hextets.Add(hex);
            }

            foreach (var t in hextets) {
#if DEBUG
				Console.WriteLine(string.Format("Hextet {0}: \"{1}\"", i + 1, Hextets[i]));
#endif
                var cyphered = string.Empty;

                for (var j = 0; j < 4; j++)
                for (var k = j; k < t.Length; k += 4)
                    cyphered += t[k];

                decypheredHextets.Add(cyphered);
#if DEBUG
				Console.WriteLine(string.Format("\tCyphered: \"{0}\"", cyphered));
#endif
            }

            b.Clear();

            foreach (var str in decypheredHextets)
                b.Append(str);


            return b.ToString();
        }

        private static void PadString(ref string input) {
            var paddingNeeded = input.Length % 16;
            if (paddingNeeded != 0)
                paddingNeeded = 16 - paddingNeeded;

            var b = new StringBuilder(input);

            while (paddingNeeded > 0) {
                switch (paddingNeeded) {
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

        private static void AddThreeLetterWord(StringBuilder b) {
            if (_threeLetters == null)
                _threeLetters = Resources.threeletters.Split(new[] {Environment.NewLine},
                    StringSplitOptions.RemoveEmptyEntries);

            var add = _threeLetters[Rand.Next(0, _threeLetters.Length - 1)].ToLower();

            b.Append(" " + add);
        }
    }
}