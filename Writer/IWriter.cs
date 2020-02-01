using System;
using System.Collections.Generic;

namespace WordFinder.Writer
{
    public interface IWriter
    {
        string WriteTemaplte();
        string WriteWord(Dictionary<string, string[]> wordInfo);
        string WriteNewLine();
        void SetHeader(string[] headerColumns);
    }
}
