using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace WordFinder
{
    public class HttpClient
    {
        public string GetWord()
        {
            string html = string.Empty;
            string url = @"https://www.dictionaryapi.com/api/v1/references/collegiate/xml/chastisement?key=08b06987-1ad8-473e-a766-ba4cf8968b62";

            System.Net.HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            Console.WriteLine(html);
            return html;
        }
    }
}
