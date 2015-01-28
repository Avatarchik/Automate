using System.Xml.Serialization;
using System.Collections.Generic;

namespace UnitySlot {
    public class RollsAreaHolder {
        [XmlAttribute ("elementWidth")]
        public float ElementWidth { get; set; }

        [XmlAttribute ("elementHeight")]
        public float ElementHeight { get; set; }

        [XmlAttribute ("rollsCount")]
        public int RollsCount { get; set; }

        [XmlAttribute ("elementsPerRoll")]
        public int ElementsPerRoll { get; set; }

        [XmlAttribute ("soundCount")]
        public int SoundCount { get; set; }
        //TODO переменные ниже по идее не нужны, снести потом надо будет
        [XmlAttribute ("x")]
        public float PositionX { get; set; }

        [XmlAttribute ("y")]
        public float PositionY { get; set; }

        [XmlAttribute ("xMargin")]
        public float MarginX { get; set; }

        [XmlAttribute ("yMargin")]
        public float MarginY { get; set; }

        [XmlElement ("roll")]
        public List<RollHolder> Rolls { get; set; }

        public override string ToString () {
            return string.Format ("[RollsAreaHolder: ElementWidth={0}, ElementHeight={1}, RollsCount={2}, ElementsPerRoll={3}, " +
            "SoundCount={4}, PositionX={5}, PositionY={6}, MarginX={7}, MarginY={8}, Rolls={9}]",
                ElementWidth, ElementHeight, RollsCount, ElementsPerRoll, SoundCount, PositionX, PositionY, MarginX, MarginY,
                Rolls.ToString ());
        }
    }
}