using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using WordFinder.Writer;

namespace WordFinder
{
    public class Words
    {
        IWriter wordWriter;
        private static string SERIAL = "#";
        private static string WORD = "Word";
        private static string DEFINITION = "Definition";
        private static string ETYMOLOGY = "Etymology";
        private static string POS = "POS";
        private static string PRONUNCIATION = "Pronunciation";
        private static string URL = "Link";
        private static string SPACE = " ";
        private static string mwLinkPrefix = "https://www.merriam-webster.com/dictionary/{0}";


        private static string RegexToRemove = @"\[[0-9*]\]";

        private List<Entry> listOfWords = new List<Entry>();


        private StringBuilder lines = new StringBuilder();
        public Words(IWriter writer){
            wordWriter = writer;
        }

        public void AddWord(Entry word){
            listOfWords.Add(word);
        }

        public void AddWords(int serialNumber, string id, List<Entry> words)
        {
            Console.Write(".");

            bool alreadyAddedSerailNumber = false;
            bool wordAdded = false;
            if (words != null)
            {
                if(words.Count == 0)
                {
                    Entry entry = new Entry()
                    {
                        SerialNumber = serialNumber,
                        Id = id

                    };  

                    listOfWords.Add(entry);
                    wordAdded = true;
                    return;
                }

                foreach(Entry word in words)
                {
                    if (!alreadyAddedSerailNumber)
                    {
                        word.SerialNumber = serialNumber;
                        alreadyAddedSerailNumber = true;
                    }
                    else{
                        word.SerialNumber = 0;
                    }
                    string distinctWord = Regex.Replace(word.Id, RegexToRemove, "");
                    if(id == distinctWord)
                    {
                        listOfWords.Add(word);
                        wordAdded = true;
                    }
                }

                if(words.Count > 0 && !wordAdded)
                {
                    Entry entry = new Entry()
                    {
                        SerialNumber = serialNumber,
                        Id = id

                    };

                    listOfWords.Add(entry);
                    wordAdded = true;
                }
            }
        }

        public Words() : this(null)
        {}


        private string AddWordToOutput(Entry entry)
        {
            Dictionary<string, string[]> wordInfo = new Dictionary<string, string[]>();

            if (entry.SerialNumber > 0)
            {
                wordInfo.TryAdd(SERIAL, new string[] { entry.SerialNumber.ToString() });
                wordInfo.Add(URL, new string[] { string.Format(mwLinkPrefix, entry.Id) });
            }
            else
            {
                wordInfo.Add(SERIAL, new string[] { SPACE });
                wordInfo.Add(URL, new string[] { SPACE });
            }

            wordInfo.TryAdd(WORD, new string[] { entry.Id });

            if (entry.Pronunciation != null)
            {
                wordInfo.TryAdd(PRONUNCIATION, entry.Pronunciation.Value);
            }
            else
            {
                wordInfo.Add(PRONUNCIATION, new string[] { SPACE });
            }

            //part of speech
            if (entry.Fl != null)
            {
                wordInfo.TryAdd(POS, new string[] { entry.Fl });
            }
            else
            {
                wordInfo.Add(POS, new string[] { SPACE });
            }

            if (entry.Definition != null)
            {
                wordInfo.TryAdd(DEFINITION, ConvertDefinitionsToStrings(entry.Definition.Definingtext));
            }
            else
            {
                wordInfo.Add(DEFINITION, new string[] { SPACE });
            }

            if (entry.Etymology != null)
            {
                wordInfo.TryAdd(ETYMOLOGY, entry.Etymology.Value);
            }
            else
            {
                wordInfo.Add(ETYMOLOGY, new string[] { SPACE });
            }
            return wordWriter.WriteWord(wordInfo);
        }

        public string Write()
        {
            wordWriter.SetHeader(new string[] { SERIAL, URL, WORD, PRONUNCIATION, POS, DEFINITION, ETYMOLOGY });

            string wrapper = wordWriter.WriteTemaplte();

            foreach (Entry word in listOfWords)
            {
                lines.Append(AddWordToOutput(word));
            }

            string output = string.Format(wrapper, lines.ToString());

            using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter(@"/Users/mrajagopalan/Downloads/outputfile.html"))
            {
                file.WriteLine(output);

            }

            return output;
        }

       

        public string[] ConvertDefinitionsToStrings(List<DefiningText> values)
        {
            List<string> stringValues = new List<string>();

            if (values != null)
            {
                foreach (DefiningText val in values)
                {
                    if(val.Value != null)
                        stringValues.Add(string.Join("", val.Value));
                }
            }
            return stringValues.ToArray();
        }
    }
}
