using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TranslatorBot.Models
{
    public class DetectLanguageResult
    {
        public string language { get; set; }
        public float score { get; set; }
        public bool isTranslationSupported { get; set; }
        public bool isTransliterationSupported { get; set; }
        public AltTranslations[] alternatives { get; set; }
    }
    public class AltTranslations
    {
        public string language { get; set; }
        public float score { get; set; }
        public bool isTranslationSupported { get; set; }
        public bool isTransliterationSupported { get; set; }
    }
}
