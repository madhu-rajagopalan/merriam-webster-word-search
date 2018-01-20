using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using WordFinder.Writer;

namespace WordFinder
{
    public class Words
    {
        IWriter wordWriter;
        private static string WORD = "Word";
        private static string DEFINITION = "Definition";
        private static string ETYMOLOGY = "Etymology";
        //private static string POS = "POS";
        private static string PRONUNCIATION = "Pronunciation";
        private static string SPACE = " ";

        private List<Entry> listOfWords = new List<Entry>();


        private StringBuilder lines = new StringBuilder();
        public Words(IWriter writer){
            wordWriter = writer;
        }

        public void AddWord(Entry word){
            listOfWords.Add(word);
        }

        public void AddWords(List<Entry> words)
        {
            listOfWords.AddRange(words);
        }

        public Words() : this(null)
        {}


        private string AddWordToOutput(Entry entry)
        {
            Dictionary<string, string[]> wordInfo = new Dictionary<string, string[]>();
            wordInfo.TryAdd(WORD, new string[] { entry.Id });
            if (entry.Pronunciation != null)
            {
                wordInfo.TryAdd(PRONUNCIATION, entry.Pronunciation.Value);
            }
            else
            {
                wordInfo.Add(PRONUNCIATION, new string[] { SPACE });
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
            wordWriter.SetHeader(new string[] { WORD, PRONUNCIATION, DEFINITION, ETYMOLOGY });

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
                    stringValues.Add(val.Value);
                }
            }
            return stringValues.ToArray();
        }
    }
}
