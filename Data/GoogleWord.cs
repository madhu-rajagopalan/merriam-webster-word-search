using System;
namespace WordFinder.Data
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;


    public class Exclamation
    {
        public string definition { get; set; }
        public string example { get; set; }
    }

    public class Noun
    {
        public string definition { get; set; }
        public string example { get; set; }
    }

    public class Verb
    {
        public string definition { get; set; }
        public string example { get; set; }
    }

    public class Adjective
    {
        public string definition { get; set; }
        public string example { get; set; }
        public List<string> synonyms { get; set; }
    }

    public class Meaning
    {
        public List<Exclamation> exclamation { get; set; }
        public List<Noun> noun { get; set; }
        public List<Verb> verb { get; set; }
        public List<Adjective> adjective { get; set; }
    }

    public class GoogleWord
    {
        public string word { get; set; }
        [JsonConverter(typeof(SingleValueArrayConverter<string>))]
        public List<string> phonetic { get; set; }
        public string origin { get; set; }
        public Meaning meaning { get; set; }

    }

    public class SingleValueArrayConverter<T> : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object retVal = new Object();
            if (reader.TokenType == JsonToken.StartObject)
            {
                T instance = (T)serializer.Deserialize(reader, typeof(T));
                retVal = new List<T>() { instance };
            }
            else if(reader.TokenType == JsonToken.String)
            {
                T instance = (T)serializer.Deserialize(reader, typeof(T));
                retVal = new List<T>() { instance };
            }
            else if (reader.TokenType == JsonToken.StartArray)
            {
                retVal = serializer.Deserialize(reader, objectType);
            }
            return retVal;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }


}
