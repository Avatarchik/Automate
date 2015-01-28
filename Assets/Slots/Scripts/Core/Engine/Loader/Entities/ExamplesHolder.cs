using System.Xml.Serialization;
using System.Collections.Generic;

namespace UnitySlot {
    public class ExamplesHolder {
        [XmlAttribute ("width")]
        public float Width { get; set; }

        [XmlAttribute ("height")]
        public float Height { get; set; }

        [XmlAttribute ("format")]
        public string ExampleFileFormat { get; set; }

        [XmlAttribute ("useSpecific")]
        public bool UseSpecific { get; set; }

        [XmlAttribute ("startIndex")]
        public int StartIndex { get; set; }
        //TODO переменные ниже по идее не нужны, снести потом надо будет
        [XmlAttribute ("x")]
        public float PositionX { get; set; }

        [XmlAttribute ("y")]
        public float PositionY { get; set; }

        [XmlElement ("example")]
        public List<ExampleHolder> Examples { get; set; }

        public override string ToString () {
            return string.Format ("[ExamplesHolder: Width={0}, Height={1}, ExampleFileFormat={2}, UseSpecific={3}, StartIndex={4}," +
            "PositionX={5}, PositionY={6}, Examples={7}]", Width, Height, ExampleFileFormat, UseSpecific, StartIndex, PositionX,
                PositionY, Examples.ToString ());
        }
    }
}