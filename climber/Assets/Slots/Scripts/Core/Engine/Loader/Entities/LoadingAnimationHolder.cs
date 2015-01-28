using System.Xml.Serialization;

namespace UnitySlot {
    public class LoadingAnimationHolder {
        [XmlAttribute ("file")]
        public string AnimationFile { get; set; }

        [XmlAttribute ("countElements")]
        public int CountElements { get; set; }

        [XmlAttribute ("delay")]
        public float Delay { get; set; }

        public override string ToString () {
            return string.Format ("[LoadingAnimationHolder: AnimationFile={0}, CountElements={1}, Delay={2}]",
                AnimationFile, CountElements, Delay);
        }
    }
}