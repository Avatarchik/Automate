using System.Xml.Serialization;
using System.Collections.Generic;

namespace UnitySlot {
    public class ElementsHolder {
        [XmlAttribute ("count")]
        public int Count { get; set; }

        [XmlAttribute ("format")]
        public string LineFileFormat { get; set; }

        [XmlAttribute ("startIndex")]
        public int StartIndex { get; set; }

        [XmlAttribute ("width")]
        public float Width { get; set; }

        [XmlAttribute ("height")]
        public float Height { get; set; }

        [XmlAttribute ("specialBorder")]
        public string SpecialBorderFile { get; set; }

        [XmlElement ("element")]
        public List<ElementHolder> Elements;

        public override string ToString () {
            return string.Format ("[ElementsHolder: Count={0}, LineFileFormat={1}, StartIndex={2}, Width={3}, Height={4}, " +
            "SpecialBorderFile={5}, Elements={6}]", Count, LineFileFormat, StartIndex, Width, Height, SpecialBorderFile, Elements.ToString ());
        }
    }
}