using System.Xml.Serialization;

namespace UnitySlot {
    public class BordersHolder {
        [XmlAttribute ("count")]
        public int Count { get; set; }

        [XmlAttribute ("width")]
        public float Width { get; set; }

        [XmlAttribute ("height")]
        public float Height { get; set; }

        [XmlAttribute ("format")]
        public string BorderFileFormat { get; set; }

        [XmlAttribute ("startIndex")]
        public int StartIndex { get; set; }
        //TODO переменные ниже по идее не нужны, снести потом надо будет
        [XmlAttribute ("x")]
        public float PositionX { get; set; }

        [XmlAttribute ("y")]
        public float PositionY { get; set; }

        public override string ToString () {
            return string.Format ("[BordersHolder: Count={0}, Width={1}, Height={2}, BorderFileFormat={3}, StartIndex={4}, " +
            "PositionX={5}, PositionY={6}]", Count, Width, Height, BorderFileFormat, StartIndex, PositionX, PositionY);
        }
    }
}