
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace WordFinder
{
    [XmlRoot(ElementName = "sound")]
    public class Sound
    {
        [XmlElement(ElementName = "wav")]
        public string Wav { get; set; }
        [XmlElement(ElementName = "wpr")]
        public string Wpr { get; set; }
    }

    [XmlRoot(ElementName = "in")]
    public class Inflection
    {
        [XmlElement(ElementName = "if")]
        public string If { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "et")]
    public class Etymology
    {
        [XmlText]
        public string[] Value { get; set; }

        [XmlElement(ElementName = "it")]
        public List<string> It { get; set; }


    }

    [XmlRoot(ElementName = "dt")]
    public class DefiningText
    {
        [XmlText]
        public string[] Value { get; set; }

        [XmlElement(ElementName = "sx")]
        public Sx Sx { get; set; }

    }

    [XmlRoot(ElementName = "sx")]
    public class Sx
    {
        [XmlElement(ElementName = "sxn")]
        public string Sxn { get; set; }
    }

    [XmlRoot(ElementName = "def")]
    public class Definition
    {
        [XmlElement(ElementName = "vt")]
        public string Vt { get; set; }
        [XmlElement(ElementName = "date")]
        public string Date { get; set; }
        //[XmlElement(ElementName = "sn")]
        //public List<string> Sn { get; set; }
        [XmlElement(ElementName = "dt")]
        public List<DefiningText> Definingtext { get; set; }
        [XmlElement(ElementName = "ssl")]
        public string Ssl { get; set; }
        [XmlElement(ElementName = "ss")]
        public string Ss { get; set; }
    }

    [XmlRoot(ElementName = "pr")]
    public class Pronunciation
    {
        [XmlText]
        public string[] Value { get; set; }

        [XmlElement(ElementName = "it")]
        public string It { get; set; }
    }

    [XmlRoot(ElementName = "uro")]
    public class Uro
    {
        [XmlElement(ElementName = "ure")]
        public string Ure { get; set; }
        [XmlElement(ElementName = "sound")]
        public Sound Sound { get; set; }
        [XmlElement(ElementName = "pr")]
        public Pronunciation Pronunciation { get; set; }
        [XmlElement(ElementName = "fl")]
        public string Fl { get; set; }
    }

    [XmlRoot(ElementName = "entry")]
    public class Entry
    {
        public int SerialNumber { get; set; }
        [XmlElement(ElementName = "ew")]
        public string Ew { get; set; }
        [XmlElement(ElementName = "hw")]
        public string Hw { get; set; }
        [XmlElement(ElementName = "sound")]
        public Sound Sound { get; set; }
        [XmlElement(ElementName = "pr")]
        public Pronunciation Pronunciation { get; set; }
        [XmlElement(ElementName = "fl")]
        public string Fl { get; set; }
        [XmlElement(ElementName = "in")]
        public List<Inflection> Inflection { get; set; }
        [XmlElement(ElementName = "et")]
        public Etymology Etymology { get; set; }
        [XmlElement(ElementName = "def")]
        public Definition Definition { get; set; }
        [XmlElement(ElementName = "uro")]
        public List<Uro> Uro { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "entry_list")]
    public class Entries
    {
        [XmlElement(ElementName = "entry")]
        public List<Entry> Entry { get; set; }
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
    }

}
