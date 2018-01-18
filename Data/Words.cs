using System;
using System.Collections.Generic;
using System.Text;
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
            Dictionary<string, string> wordInfo = new Dictionary<string, string>();
            wordInfo.TryAdd(WORD, entry.Id);
            //wordInfo.TryAdd(PRONUNCIATION, entry.Pronunciation); //write one for now
            wordInfo.TryAdd(DEFINITION, entry.Definition.Ss);//write one definition for now
            wordInfo.TryAdd(ETYMOLOGY, entry.Etymology.It[0].ToString());//write one for now
            return wordWriter.WriteWord(wordInfo);
        }

        public string Write()
        {
            string wrapper = wordWriter.WriteTemaplte();
            foreach (Entry word in listOfWords)
            {
                lines.Append(AddWordToOutput(word));
            }

            return string.Format(wrapper,lines.ToString());
        }
    }
}
