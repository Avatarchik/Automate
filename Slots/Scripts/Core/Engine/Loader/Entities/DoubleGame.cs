using System.Xml.Serialization;

namespace UnitySlot {
    public class DoubleGame {
        [XmlAttribute ("settings")]
        public string SettingsFileName{ get; set; }

        [XmlAttribute ("class")]
        public string DoubleClassName{ get; set; }

        public override string ToString () {
            return string.Format ("[DoubleGame: SettingsFileName={0}, DoubleClassName={1}]", SettingsFileName, DoubleClassName);
        }
    }
}