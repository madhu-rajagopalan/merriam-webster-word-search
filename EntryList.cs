
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
    }

    [XmlRoot(ElementName = "et")]
    public class Etymology
    {
        [XmlElement(ElementName = "it")]
        public List<string> It { get; set; }
    }

    [XmlRoot(ElementName = "dt")]
    public class DefiningText
    {
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
        [XmlElement(ElementName = "sn")]
        public List<string> Sn { get; set; }
        [XmlElement(ElementName = "dt")]
        public List<Dt> Dt { get; set; }
        [XmlElement(ElementName = "ssl")]
        public string Ssl { get; set; }
        [XmlElement(ElementName = "ss")]
        public string Ss { get; set; }
    }

    [XmlRoot(ElementName = "pr")]
    public class Pronunciation
    {
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
        public Pr Pr { get; set; }
        [XmlElement(ElementName = "fl")]
        public string Fl { get; set; }
    }

    [XmlRoot(ElementName = "entry")]
    public class Entry
    {
        [XmlElement(ElementName = "ew")]
        public string Ew { get; set; }
        [XmlElement(ElementName = "hw")]
        public string Hw { get; set; }
        [XmlElement(ElementName = "sound")]
        public Sound Sound { get; set; }
        [XmlElement(ElementName = "pr")]
        public string Pr { get; set; }
        [XmlElement(ElementName = "fl")]
        public string Fl { get; set; }
        [XmlElement(ElementName = "in")]
        public List<In> In { get; set; }
        [XmlElement(ElementName = "et")]
        public Et Et { get; set; }
        [XmlElement(ElementName = "def")]
        public Def Def { get; set; }
        [XmlElement(ElementName = "uro")]
        public List<Uro> Uro { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "entry_list")]
    public class EntryList
    {
        [XmlElement(ElementName = "entry")]
        public Entry Entry { get; set; }
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
    }

}
