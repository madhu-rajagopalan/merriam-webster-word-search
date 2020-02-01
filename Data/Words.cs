using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using WordFinder.Data;
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
           

            if (!IsNullWord(entry))
            {

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
                    //wordInfo.TryAdd(ETYMOLOGY, entry.Etymology.Value);
                    IEnumerable<string> val = Interleave(entry.Etymology.Value, entry.Etymology.It);
                    string[] s = val.Select(p => p).ToArray();
                    wordInfo.TryAdd(ETYMOLOGY, s);
                }
                else
                {
                    wordInfo.Add(ETYMOLOGY, new string[] { SPACE });
                }
            }
            //MW cannot find the word, call google API
            //this not how I want to fallback to google API, this is a hack and a terrible way...
            //I don't have time to rewrite my base classes to fit both models..
            else
            {
                Console.Write("*");

                List<GoogleWord> backupIfMWFailed = GetGoogleWord(entry.Id);

                if (backupIfMWFailed != null)
                {

                    try
                    {
                        if (backupIfMWFailed.Count<GoogleWord>() > 0)
                        {
                            if (backupIfMWFailed[0].phonetic.Count > 0)
                            {
                                wordInfo.TryAdd(PRONUNCIATION, backupIfMWFailed[0].phonetic.ToArray());
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

                            if (backupIfMWFailed[0].meaning != null)
                            {
                                wordInfo.TryAdd(DEFINITION, ConvertMeaningToStrings(backupIfMWFailed[0]));
                            }
                            else
                            {
                                wordInfo.Add(DEFINITION, new string[] { SPACE });
                            }

                            if (backupIfMWFailed[0].origin != null)
                            {

                                wordInfo.TryAdd(ETYMOLOGY, new string[] { backupIfMWFailed[0].origin });
                            }
                            else
                            {
                                wordInfo.Add(ETYMOLOGY, new string[] { SPACE });
                            }
                        }

                    }
                    catch (Exception)
                    {
                        Console.WriteLine("failed to get word from google");
                    }
                }
                else //even google backup failed
                {
                    //write empty values
                    wordInfo.Add(PRONUNCIATION, new string[] { SPACE });
                    wordInfo.Add(POS, new string[] { SPACE });
                    wordInfo.Add(DEFINITION, new string[] { SPACE });
                    wordInfo.Add(ETYMOLOGY, new string[] { SPACE });
                }
            }

            return wordWriter.WriteWord(wordInfo);
        }

        public List<GoogleWord> GetGoogleWord(string word)
        {
            HttpClient client = new HttpClient(string.Empty); //key not needed for google API
            try
            {
                string json = client.GetGoogleWord(word);
                json = json.Replace("\n", string.Empty);
                List<GoogleWord> googleWord = JsonConvert.DeserializeObject<List<GoogleWord>>(json);
                return googleWord;

            }
            catch
            {
                return null;
            }
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
                   new System.IO.StreamWriter(@"/Users/madhurajagopalan/Downloads/outputfile.html"))
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

        public string[] ConvertMeaningToStrings(GoogleWord word)
        {
            List<string> stringValues = new List<string>();


            if(word.meaning.adjective != null && word.meaning.adjective.Count >0)
            {
                foreach (Adjective val in word.meaning.adjective)
                {
                    if (val.definition != null)
                        stringValues.Add(string.Join("Adjective: ", val.definition));
                }
            }

            if (word.meaning.noun != null && word.meaning.noun.Count() >0)
            {
                foreach (Noun val in word.meaning.noun)
                {
                    if (val.definition != null)
                        stringValues.Add(string.Join("Noun: ", val.definition));
                }

            }

            if (word.meaning.verb != null && word.meaning.verb.Count > 0)
            {
                foreach (Verb val in word.meaning.verb)
                {
                    if (val.definition != null)
                        stringValues.Add(string.Join("Verb: ", val.definition));
                }

            }

            return stringValues.ToArray();
        }

        public IEnumerable<string> Interleave(string[] first, List<String> second)
        {
            if (first != null && second != null)
            {
                List<string> list1 = new List<string>(first);

                var enumerator1 = list1.GetEnumerator();
                var enumerator2 = second.GetEnumerator();

                bool firstHasMore;
                bool secondHasMore;

                while ((firstHasMore = enumerator1.MoveNext())
                     | (secondHasMore = enumerator2.MoveNext()))
                {
                    if (firstHasMore)
                        yield return enumerator1.Current;

                    if (secondHasMore)
                        yield return enumerator2.Current;
                }
            }
            
        }

        public bool IsNullWord(Entry entry)
        {
            if (entry.Definition == null)
                return true;
            else
                return false;
        }
    }
}
