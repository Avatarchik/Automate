using System.Xml.Serialization;

namespace UnitySlot {
    public class InfoHolder {
        [XmlAttribute ("texture")]
        public string BackgroundFileName { get; set; }

        [XmlAttribute ("width")]
        public float Width { get; set; }

        [XmlAttribute ("height")]
        public float Height { get; set; }

        [XmlElement ("pages")]
        public PagesHolder InfoPages;

        public override string ToString () {
            return string.Format ("[InfoHolder: InfoPages={0}, BackgroundFileName={1}, Width={2}, Height={3}]",
                InfoPages.ToString (), BackgroundFileName, Width, Height);
        }
    }
}