using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using WordFinder.Writer;

namespace WordFinder.Utils
{
    public class HtmlWriter: IWriter
    {
        /// <summary>
        /// The settings.
        /// </summary>
        internal XmlWriterSettings settings;

        internal string styleUrl = "https://unpkg.com/purecss@1.0.0/build/pure-min.css";
        internal string styleUrlIntegrity = "sha384-nn4HPE8lTHyVtfCBi5yW9d20FjT8BJwUXyWZT9InLYax14RDjBj46LmSztkmNP9w";
        internal string styleUrlCrossorigin = "anonymous";

        private XElement tableHead = new XElement("thead");

        /// <summary>he
        /// Initializes a new instance of the <see cref="T:WordFinder.Utils.HtmlWriter"/> class.
        /// </summary>
        public HtmlWriter()
        {
            settings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
                Indent = true,
                IndentChars = "\t"
            };
        }

        /// <summary>
        /// Writes the temaplte.
        /// </summary>
        /// <returns>The temaplte.</returns>
        public string WriteTemaplte()
        {
            XDocument xDocument = new XDocument(
                new XDocumentType("html", null, null, null),
                    new XElement("html",
                        new XElement("head", new XElement("link", 
                                     new XAttribute("rel","stylesheet"), 
                                     new XAttribute("href", styleUrl), 
                                     new XAttribute("integrity", styleUrlIntegrity),
                                     new XAttribute("crossorigin", styleUrlCrossorigin))),
                            new XElement("body",
                                 new XElement("div", 
                                              new XElement("table", new XAttribute("class", "pure-table pure-table-bordered"),tableHead, "{0}"))
                        ))
            );

            return xDocument.ToString();
        }

        /// <summary>
        /// Writes the word.
        /// </summary>
        /// <returns>The word.</returns>
        /// <param name="wordInfo">Word info.</param>
        public string WriteWord(Dictionary<string, string[]> wordInfo)
        {
            XElement row = new XElement("tr");

            foreach (KeyValuePair<string, string[]> entry in wordInfo)
            {
                string[] content = entry.Value;

                XElement cell = new XElement("td", Normalize(content));
                row.Add(cell);
            }
            return row.ToString();
        }



        /// <summary>
        /// Writes the new line.
        /// </summary>
        /// <returns>The new line.</returns>
        public string WriteNewLine(){
            return "<br/>";
        }

        /// <summary>
        /// Normalize the specified values.
        /// </summary>
        /// <returns>The normalize.</returns>
        /// <param name="values">Values.</param>
        public XElement Normalize(string[] values)
        {
            XElement div = new XElement("div");

            if (values != null)
            {
                foreach (string val in values)
                {
                    div.Add(new XElement("span", val), new XElement("br"));
                }
            }
            return div;
        }

        /// <summary>
        /// Sets the header.
        /// </summary>
        /// <param name="headerColumns">Header columns.</param>
        public void SetHeader(string[] headerColumns)
        {
            XElement row = new XElement("tr");

            if (headerColumns != null)
            {
                foreach (string val in headerColumns)
                {
                    row.Add(new XElement("th", val));
                }
            }

            tableHead.Add(row);
        }
    }
}
