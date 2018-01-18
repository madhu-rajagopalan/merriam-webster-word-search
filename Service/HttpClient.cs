using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace WordFinder
{
    public class HttpClient
    {
        string apiKey = "08b06987-1ad8-473e-a766-ba4cf8968b62";
        string baseUrl = @"https://www.dictionaryapi.com/api/v1/references/collegiate/xml/{0}?key={1}";

        public string GetWordResponse(string word)
        {
            string html = string.Empty;
            string absUrl = string.Format(baseUrl, word, apiKey);

            System.Net.HttpWebRequest request = (HttpWebRequest)WebRequest.Create(absUrl);
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
