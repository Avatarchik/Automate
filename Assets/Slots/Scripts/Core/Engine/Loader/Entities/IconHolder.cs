using System.Xml.Serialization;
using System.ComponentModel;

namespace UnitySlot {
    public class IconHolder {
        [XmlAttribute ("name")]
        public string Name { get; set; }

        [DefaultValue (0.0f), XmlAttribute ("width")]
        public float Width{ get; set; }

        [DefaultValue (0.0f), XmlAttribute ("height")]
        public float Height{ get; set; }

        [XmlAttribute ("rows")]
        public int Rows { get; set; }

        [XmlAttribute ("columns")]
        public int Columns { get; set; }

        public override string ToString () {
            return string.Format ("[IconHolder: Name={0}, Width={1}, Height={2}, Rows={3}, Columns={4}]",
                Name, Width, Height, Rows, Columns);
        }
    }
}