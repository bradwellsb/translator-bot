using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TranslatorBot.Models
{
    public class TranslationResult
    {
        public DetectedLanguage detectedLanguage { get; set; }
        public TextResult sourceText { get; set; }
        public Translation[] translations { get; set; }
    }

    public class DetectedLanguage
    {
        public string language { get; set; }
        public float score { get; set; }
    }

    public class TextResult
    {
        public string text { get; set; }
        public string script { get; set; }
    }

    public class Translation
    {
        public string text { get; set; }
        public TextResult transliteration { get; set; }
        public string to { get; set; }
        public Alignment alignment { get; set; }
        public SentenceLength sentLen { get; set; }
    }

    public class Alignment
    {
        public string proj { get; set; }
    }

    public class SentenceLength
    {
        public int[] srcSentLen { get; set; }
        public int[] transSentLen { get; set; }
    }
}
