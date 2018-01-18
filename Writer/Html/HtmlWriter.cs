using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using WordFinder.Writer;

namespace WordFinder.Utils
{
    public class HtmlWriter: IWriter
    {
        internal XmlWriterSettings settings;
        private XElement table = new XElement("table");
        public HtmlWriter()
        {
            settings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
                Indent = true,
                IndentChars = "\t"
            };
        }

        public string WriteTemaplte()
        {
            XDocument xDocument = new XDocument(
                new XDocumentType("html", null, null, null),
                new XElement("html",
                    new XElement("head"),
                    new XElement("body",
                                 new XElement("div", "{0}")
                        ))
            );

            return xDocument.ToString();
        }

        public string WriteWord(Dictionary<string, string> wordInfo)
        {
            XElement row = new XElement("tr");

            foreach (KeyValuePair<string, string> entry in wordInfo)
            {
                XElement cell = new XElement("td", entry.Value);
                row.Add(cell);
            }
            return row.ToString();
        }   
    }
}
