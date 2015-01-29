using System.Xml.Serialization;
using System.Collections.Generic;

namespace UnitySlot {
    public class LinesHolder {
        [XmlAttribute ("count")]
        public int Count { get; set; }

        [XmlAttribute ("width")]
        public float Width { get; set; }

        [XmlAttribute ("height")]
        public float Height { get; set; }

        [XmlAttribute ("format")]
        public string LineFileFormat { get; set; }

        [XmlAttribute ("step")]
        public int Step { get; set; }

        [XmlAttribute ("showTogether")]
        public bool ShowTogether { get; set; }

        [XmlAttribute ("showWinningBorder")]
        public bool ShowWinningBorder { get; set; }

        [XmlAttribute ("timerRate")]
        public float TimerRate { get; set; }

        [XmlAttribute ("blinkRate")]
        public float BlinkRate { get; set; }

        [XmlElement ("line")]
        public List<LineHolder> Lines { get; set; }

        [XmlElement ("indicators")]
        public IndicatorsHolder Indicators { get; set; }

        [XmlElement ("examples")]
        public ExamplesHolder Examples { get; set; }

        [XmlElement ("borders")]
        public BordersHolder Borders { get; set; }
        //TODO переменные ниже по идее не нужны, снести потом надо будет
        [XmlAttribute ("x")]
        public float PositionX { get; set; }

        [XmlAttribute ("y")]
        public float PositionY { get; set; }

        public override string ToString () {
            return string.Format ("[LinesHolder: Count={0}, Width={1}, Height={2}, LineFileFormat={3}, Step={4}, ShowTogether={5}," +
            "ShowWinningBorder={6}, TimerRate={7}, BlinkRate={8}, Lines={9}, Indicators={10}, Examples={11}, Borders={12}, " +
            "PositionX={13}, PositionY={14}]", Count, Width, Height, LineFileFormat, Step, ShowTogether, ShowWinningBorder,
                TimerRate, BlinkRate, Lines.ToString (), Indicators.ToString (), Examples.ToString (), Borders.ToString (),
                PositionX, PositionY);
        }
    }
}