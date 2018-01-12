
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace WordFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            string response = client.GetWord();

            XmlSerializer ser = new XmlSerializer(typeof(Entry_list));
            Entry_list list = response.ParseXML<Entry_list>();

            Console.Read();
        }

    }
}
