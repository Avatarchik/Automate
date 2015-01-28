using System.Xml.Serialization;

namespace UnitySlot {
    public class RollHolder {
        [XmlAttribute ("index")]
        public int Index { get; set; }

        [XmlAttribute ("scatterSound")]
        public string ScatterSound { get; set; }

        public override string ToString () {
            return string.Format ("[RollHolder: Index={0}, ScatterSound={1}]", Index, ScatterSound);
        }
    }
}