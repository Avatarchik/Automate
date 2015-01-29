using System.Xml.Serialization;

namespace UnitySlot {
    public class IndicatorHolder {
        [XmlAttribute ("Index")]
        public int Index { get; set; }

        public override string ToString () {
            return string.Format ("[IndicatorHolder: Index={0}]", Index);
        }
    }
}