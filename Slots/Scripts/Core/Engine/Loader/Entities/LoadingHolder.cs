using System.Xml.Serialization;

namespace UnitySlot {
    public class LoadingHolder {
        [XmlAttribute ("background")]
        public string Background { get; set; }

        [XmlAttribute ("width")]
        public float Width { get; set; }

        [XmlAttribute ("height")]
        public float Height { get; set; }
        //Ниже идут объекты которые нах не нужны по идее
        [XmlAttribute ("lineX")]
        public float LineX { get; set; }

        [XmlAttribute ("lineY")]
        public float LineY { get; set; }

        [XmlElement ("loadingAnimation")]
        public LoadingAnimationHolder animation { get; set; }

        public override string ToString () {
            return string.Format ("[LoadingHolder: Background={0}, Width={1}, Height={2}, LineX={3}, LineY={4}, animation={5}]",
                Background, Width, Height, LineX, LineY, animation.ToString ());
        }
    }
}