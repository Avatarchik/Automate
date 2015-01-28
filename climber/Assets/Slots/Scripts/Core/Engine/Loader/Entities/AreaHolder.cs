using System.Xml.Serialization;

namespace UnitySlot {
    public class AreaHolder {
        [XmlAttribute ("width")]
        public float Width { get; set; }

        [XmlAttribute ("height")]
        public float Height { get; set; }

        [XmlElement ("spin")]
        public SpinHolder Spin;
        [XmlElement ("rollsarea")]
        public RollsAreaHolder RollsArea;
        [XmlElement ("elements")]
        public ElementsHolder Elements;
        [XmlElement ("lines")]
        public LinesHolder Lines;
        [XmlElement ("textures")]
        public TexturesHolder Textures;

        public override string ToString () {
            return string.Format ("[AreaHolder: Width={0}, Height={1}, Spin={2}, RollsArea={3}, " +
            "Elements={4}, Lines={5}, Textures={6}]", Width, Height, Spin.ToString (),
                RollsArea.ToString (), Elements.ToString (), Lines.ToString (), Textures.ToString ());
        }
    }
}