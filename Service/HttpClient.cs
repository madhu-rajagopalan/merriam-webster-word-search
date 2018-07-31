using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace WordFinder
{
    public class HttpClient
    {
        string baseUrl = @"https://www.dictionaryapi.com/api/v1/references/collegiate/xml/{0}?key={1}";
        string apiKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:WordFinder.HttpClient"/> class.
        /// </summary>
        /// <param name="key">Key.</param>
        public HttpClient(string key)
        {
            apiKey = key;
        }

        /// <summary>
        /// Gets the word response.
        /// </summary>
        /// <returns>The word response.</returns>
        /// <param name="word">Word.</param>
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

            //Console.WriteLine(html);
            return html;
        }
    }
}
