using System.Xml.Serialization;

namespace UnitySlot {
    public class LineAnimationHolder {
        [XmlAttribute ("name")]
        public string Name { get; set; }

        [XmlAttribute ("event")]
        public string Event { get; set; }

        public override string ToString () {
            return string.Format ("[LineAnimationHolder: Name={0}, Event={1}]", Name, Event);
        }
    }
}