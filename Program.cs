
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

            int serial = 1;

            foreach (string line in lines)
            {
                try
                {
                    result = GetWord(line.Trim());
                    if (result != null)
                    {
                        list.AddWords(serial, line, result);
                        serial++;

                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Did not get word details for: {0}", line ));
                    Console.WriteLine(ex.Message);
                }

            }

            list.Write();

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
                //Console.WriteLine(ex.Message);
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
                try
                {
                    string xml = element.ToString();
                    XmlSerializer ser = new XmlSerializer(typeof(Entry));
                    Entry entry = xml.ParseXML<Entry>();

                    words.Add(entry);
                }
                catch(Exception ex){
                    Console.WriteLine("Failed to parse word result");
                    Console.WriteLine(ex.Message);
                }

            }

            return words;
        }
    }
}
