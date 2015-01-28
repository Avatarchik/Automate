using System.Xml.Serialization;

namespace UnitySlot {
    public class ExampleHolder {
        [XmlAttribute ("Index")]
        public int Index { get; set; }

        public override string ToString () {
            return string.Format ("[ExampleHolder: Index={0}]", Index);
        }
    }
}