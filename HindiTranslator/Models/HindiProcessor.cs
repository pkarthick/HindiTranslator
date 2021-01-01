using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Models
{
    
    static class HindiProcessor
    {

        private static Dictionary<string, string[]> vowels = new Dictionary<string, string[]>
        {

            ["अ"] = new string[] { "a" }, //a
            ["आ"] = new string[] { "aa", "A" }, //aa
            ["इ"] = new string[] { "i" }, //i	
            ["ई"] = new string[] { "ee", "I" }, //ee
            ["उ"] = new string[] { "u" }, //u
            ["ऊ"] = new string[] { "oo", "U" }, //oo
            ["ऋ"] = new string[] { "R" }, //ri
            ["ए"] = new string[] { "e" }, //ae
            ["ऐ"] = new string[] { "ai" }, //ai
            ["ओ"] = new string[] { "o" }, //o
            ["औ"] = new string[] { "au" }, //au
            ["अं"] = new string[] { "aM" }, //am
            ["आः"] = new string[] { "aha" }, //aha
            ["अँ"] = new string[] { "a~M" }

        };

        private static Dictionary<string, string[]> baseConsanants = new Dictionary<string, string[]>
        {

            ["क"] = new string[] { "k" },
            ["ख"] = new string[] { "kh" },
            ["ग"] = new string[] { "g" },
            ["घ"] = new string[] { "gh" },
            ["ङ"] = new string[] { "NG" },
            ["च"] = new string[] { "ch" },
            ["छ"] = new string[] { "Ch" },
            ["ज"] = new string[] { "j" },
            ["झ"] = new string[] { "z" },
            ["ञ"] = new string[] { "NY" },
            ["ट"] = new string[] { "T" },
            ["ठ"] = new string[] { "Th" },
            ["ड"] = new string[] { "D" },
            ["ढ"] = new string[] { "Dh" },
            ["ण"] = new string[] { "N" },
            ["त"] = new string[] { "t" },
            ["थ"] = new string[] { "th" },
            ["द"] = new string[] { "d" },
            ["ध"] = new string[] { "dh" },
            ["न"] = new string[] { "n" },
            ["प"] = new string[] { "p" },
            ["फ"] = new string[] { "f", "ph" },
            ["ब"] = new string[] { "b" },
            ["भ"] = new string[] { "bh" },
            ["म"] = new string[] { "m" },
            ["य"] = new string[] { "y" },
            ["र"] = new string[] { "r" },
            ["ल"] = new string[] { "l" },
            ["ळ"] = new string[] { "L" },
            ["ऴ"] = new string[] { "LL" },
            ["व"] = new string[] { "v", "w" },
            ["श"] = new string[] { "sh" },
            ["ष"] = new string[] { "Sh" },
            ["स"] = new string[] { "s" },
            ["ह"] = new string[] { "h" },
            ["क्ष"] = new string[] { "x" },
            ["त्र"] = new string[] { "tr" },
            ["ज्ञ"] = new string[] { "jNY", "Jh" },
            ["द्"] = new string[] { "ddh" },
            ["क़"] = new string[] { "K" },
            ["ख़"] = new string[] { "Kh" },
            ["ग़"] = new string[] { "G" },
            ["ज़"] = new string[] { "Z" },
            ["ड़"] = new string[] { "DD" },
            ["ढ़"] = new string[] { "DH" },
            ["फ़"] = new string[] { "F" },
            ["य़"] = new string[] { "Y" },
            ["ऩ"] = new string[] { "NN" }
        };

        private static Dictionary<string, string[]> extensibleLetters = new Dictionary<string, string[]>
        {
            [""] = new string[] { "a" },
            ["्"] = new string[] { "" }, //virama
            ["ा"] = new string[] { "aa", "A" }, //a
            ["ि"] = new string[] { "i" }, //i
            ["ी"] = new string[] { "ee", "I" }, //ee
            ["ु"] = new string[] { "u" }, //u
            ["ू"] = new string[] { "oo", "U" }, //oo
            ["े"] = new string[] { "e" }, //ae
            ["ै"] = new string[] { "ai" }, //ai
            ["ो"] = new string[] { "o" }, //o
            ["ौ"] = new string[] { "au" }, //au

            ["ृ"] = new string[] { "ru" }, //ru

            ["ः"] = new string[] { "aha" }, //visarga aha
        };

        private static Dictionary<string, string[]> miscLetters = new Dictionary<string, string[]>
        {
            ["़"] = new string[] { "" }, //nukta dot below
            ["ॄ"] = new string[] { "" },


            ["ं"] = new string[] { "M" }, //anusvara am
            ["ँ"] = new string[] { "~M" }, //candra bindu

            ["॑"] = new string[] { "'" }, //udatta
            ["॒"] = new string[] { "_" }, //anudatta
            ["॓"] = new string[] { "`" }, //accent grave
            ["॔"] = new string[] { "!`" }, //accent aigu 
            ["ॠ"] = new string[] { "rī" }, //rī
            ["ऌ"] = new string[] { "li" }, //li
            ["ॡ"] = new string[] { "lī" }, //lī

            ["।"] = new string[] { "|" }, //danda
            ["॥"] = new string[] { "||" }, //double danda
            ["ऽ"] = new string[] { "" }, //avagraha
            ["॰"] = new string[] { "" }, //
            ["ॐ"] = new string[] { "om" } //om		
        };


        private static Dictionary<string, string> readableToHindiMap = new Dictionary<string, string>
        {
        };

        private static Dictionary<string, string> hindiToReadableMap = new Dictionary<string, string>
        {
        };

        private static Dictionary<string, IHindiLetter[]> consanantsArray = new Dictionary<string, IHindiLetter[]>
        {
        };

        static HindiProcessor()
        {
            populateVowels();
            populateMiscallenous();
            populateConsanants();

        }


        private static void populateReadableMap(string readable, string letter)
        {
            readableToHindiMap[readable] = letter;
        }

        private static void populateHindiMap(string readable, string letter)
        {
            hindiToReadableMap[letter] = readable;
        }

        private static void populateVowels()
        {

            foreach (var vowel in vowels)
            {
                foreach (var readable in vowel.Value)
                {
                    populateHindiMap(readable, vowel.Key);
                    populateReadableMap(readable, vowel.Key);
                }
            }

        }

        private static void populateConsanants()
        {

            foreach (var baseConsanant in baseConsanants)
            {
                var derivedConsanants = new List<IHindiLetter> { };

                var baseReadable = baseConsanant.Value[0];

                foreach (var baseConsanantReadable in baseConsanant.Value)
                {
                    foreach (var extensibleLetter in extensibleLetters)
                    {
                        foreach (var extensibleLetterReadable in extensibleLetter.Value)
                        {
                            var letter = baseConsanant.Key + extensibleLetter.Key;
                            var readable = baseReadable + extensibleLetterReadable;

                            populateHindiMap(readable, letter);
                            populateReadableMap(readable, letter);

                            derivedConsanants.Add(new HindiLetter { readable = readable, letter = letter });

                        }
                    }

                }

                consanantsArray[baseConsanant.Key] = derivedConsanants.ToArray();

            }

        }

        private static void populateMiscallenous()
        {
            foreach (var letter in miscLetters)
            {

                foreach (var readable in letter.Value)
                {

                    populateHindiMap(readable, letter.Key);
                    populateReadableMap(readable, letter.Key);

                }

            }
        }

        private static Match getLastMatch(string word)
        {

            var match = new Match();

            if (word.Length > 1)
            {
                for (var i = 0; i < word.Length; i++)
                {
                    var sub = word.Substring(0, i + 1);

                    string letter = null;

                    if (readableToHindiMap.ContainsKey(sub))
                    {
                        letter = readableToHindiMap[sub];

                        if (extensibleLetters.ContainsKey(sub) && i > 0)
                        {
                            letter = readableToHindiMap[sub.Substring(0, sub.Length - 1)] + extensibleLetters[word.Substring(sub.Length - 1)];
                        }
                    }

                    if (letter != null)
                    {
                        match.letter = letter;
                        match.sub = sub;
                        match.found = true;
                        match.index = i + 1;
                        continue;
                    }

                }
            }
            else
            {
                
                if (readableToHindiMap.ContainsKey(word))
                {
                    match.letter = readableToHindiMap[word];
                    match.sub = word;
                    match.found = true;
                    match.index = 1;
                }
            }

            return match;

        }


        public static string GetHindiWord(string englishWord)
        {

            englishWord = englishWord.Trim();

            string hindiWord = "";

            var match = getLastMatch(englishWord);

            while (match.found)
            {
                hindiWord += match.letter;

                if (match.index < englishWord.Length)
                {
                    englishWord = englishWord.Substring(match.index);

                }
                else if (match.sub == englishWord)
                {
                    englishWord = "";
                    break;
                }

                match = getLastMatch(englishWord);

            }

            if (englishWord.Length > 0)
            {
                return hindiWord + englishWord;
            }

            var c = hindiWord[hindiWord.Length - 1];

            if (c == '्')
                hindiWord = hindiWord.Substring(0, hindiWord.Length - 1);

            return hindiWord;

        }


        public static string GetReadableWord(string hindiWord, bool skipPostfixProcessing = true)
        {

            string readable = "";

            for (var i = 0; i < hindiWord.Length; i++)
            {
                var c = hindiWord.Substring(i, 1);

                if (c == "।" || c == "्" || c == "ा" || c == "ि" || c == "ी" || c == "ु" || c == "ू" || c == "ृ" || c == "े" || c == "ै" || c == "ो" || c == "ौ")
                {
                    if (readable.Length > 0)
                    {
                        readable = readable.Substring(0, readable.Length - 1);

                        if (c != "्")
                            readable += extensibleLetters[c][0];
                    }
                }
                else
                {
                    readable += hindiToReadableMap[c];
                }

            }

            if (!skipPostfixProcessing)
            {
                if (readable.Length >= 2 && readable[readable.Length - 1] == 'a' && readable[readable.Length - 2] != 'a')
                {
                    readable = readable.Substring(0, readable.Length - 1);
                }
            }

            return readable;

        }
        
        public static bool IsHindiWord(string word)
        {
            if (word.Any(c => Char.IsLetter(c) && c < 256))
            {
                return false;
            }

            return true;
        }

    }

}
