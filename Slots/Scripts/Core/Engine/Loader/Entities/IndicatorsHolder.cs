using System.Xml.Serialization;
using System.Collections.Generic;

namespace UnitySlot {
    public class IndicatorsHolder {
        [XmlAttribute ("width")]
        public float Width { get; set; }

        [XmlAttribute ("height")]
        public float Height { get; set; }

        [XmlAttribute ("format")]
        public string IndicatorFileFormat { get; set; }

        [XmlAttribute ("showBet")]
        public bool ShowBet { get; set; }
        //TODO переменные ниже по идее не нужны, снести потом надо будет
        [XmlAttribute ("x")]
        public float PositionX { get; set; }

        [XmlAttribute ("y")]
        public float PositionY { get; set; }

        [XmlElement ("line")]
        public List<IndicatorHolder> Indicators { get; set; }

        public override string ToString () {
            return string.Format ("[IndicatorsHolder: Width={0}, Height={1}, IndicatorFileFormat={2}, ShowBet={3}, PositionX={4}," +
            "PositionY={5}, Indicators={6}]", Width, Height, IndicatorFileFormat, ShowBet, PositionX, PositionY, Indicators.ToString ());
        }
    }
}