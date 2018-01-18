
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
            client = new HttpClient();
            IWriter writer = new HtmlWriter();
            Words list = new Words(writer);

            List<Entry> result = GetWord("potato");
            if(result != null)
            {
                list.AddWords(result);
            }
            Console.Write(list.Write());

            //Console.Write(HtmlWriterUtil.WriteHtmlTemplate());

            Console.Read();
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
