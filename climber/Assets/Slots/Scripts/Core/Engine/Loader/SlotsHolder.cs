using System.Xml.Serialization;
using System.Collections.Generic;

namespace UnitySlot {
    [XmlRoot ("slots")]
    public class SlotHolder {
        [XmlAttribute ("version")]
        public int Version { get; set; }

        [XmlElement ("slot")]
        public List<SlotInfo> Slots;

        public override string ToString () {
            return string.Format ("[SlotsHolder: Slots={0}, Version={1}]", Slots.Count, Version);
        }
    }
}