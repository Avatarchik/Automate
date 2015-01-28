using System;
using System.Xml.Serialization;

namespace UnitySlot {
    public class BonusGame {
        [XmlAttribute ("elements")]
        public int Elements{ get; set; }

        [XmlAttribute ("keyword")]
        public string Keyword{ get; set; }

        [XmlAttribute ("settings")]
        public string SettingsFileName{ get; set; }

        [XmlAttribute ("class")]
        public string BonusClassName{ get; set; }

        public override string ToString () {
            return string.Format ("[BonusGame: Elements={0}, Keyword={1}, SettingsFileName={2}, BonusClassName={3}]",
                Elements, Keyword, SettingsFileName, BonusClassName);
        }
    }
}