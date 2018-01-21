
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using WordFinder.Utils;
using WordFinder.Writer;

namespace WordFinder
{
    class Program
    {
        static HttpClient client;

        static void Main(string[] args)
        {
            //each word is separated by a new line
            //string inputFilePath = args[0];

            string inputFilePath = "/Users/mrajagopalan/Downloads/inputfile.txt";

            string apiKey = GetApiKey();

            client = new HttpClient(apiKey);
            IWriter writer = new HtmlWriter();
            Words list = new Words(writer);

            // Read each line of the file into a string array. Each element
            // of the array is one line of the file.
            string[] lines = System.IO.File.ReadAllLines(inputFilePath);
            List<Entry> result;

            foreach (string line in lines)
            {
                try
                {
                    result = GetWord(line.Trim());
                    if (result != null)
                    {
                        list.AddWords(line, result);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.Write(list.Write());

            Console.Read();
        }

        private static string GetApiKey()
        {
            string apikeypath = Path.Combine(Directory.GetCurrentDirectory(), "api-key");
            string key="";

            try
            {
                string[] lines = File.ReadAllLines(apikeypath);
                if(lines.Length > 0)
                {
                    return lines[0];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return key;

        }

        static List<Entry> GetWord(string word)
        {
            string response = client.GetWordResponse(word);

            XDocument doc = XDocument.Parse(response);
            IEnumerable<XElement> items = doc.Element("entry_list").Elements("entry");

            List<Entry> words = new List<Entry>();

            foreach(XElement element in items)
            {
                Console.Write(element.ToString());

                XmlSerializer ser = new XmlSerializer(typeof(Entry));
                Entry entry = element.ToString().ParseXML<Entry>();

                words.Add(entry);

            }

            return words;
        }
    }
}
