using System.Xml.Serialization;

namespace UnitySlot {
    public class SlotInfoPreview {
        [XmlAttribute ("columns")]
        public int Columns { get; set; }

        [XmlAttribute ("rows")]
        public int Rows { get; set; }

        [XmlAttribute ("height")]
        public float Height { get; set; }

        [XmlAttribute ("width")]
        public float Width { get; set; }

        public override string ToString () {
            return string.Format ("[SlotInfoPreview: Columns={0}, Rows={1}, Height={2}, Width={3}]", Columns, Rows, Height, Width);
        }
    }
}