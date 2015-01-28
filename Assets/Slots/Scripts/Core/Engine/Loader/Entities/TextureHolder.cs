using System.Xml.Serialization;

namespace UnitySlot {
    public class TextureHolder {
        [XmlAttribute ("file")]
        public string TextureFile { get; set; }

        [XmlAttribute ("width")]
        public float Width { get; set; }

        [XmlAttribute ("height")]
        public float Height { get; set; }
        //TODO переменные ниже по идее не нужны, снести потом надо будет
        [XmlAttribute ("x")]
        public float PositionX { get; set; }

        [XmlAttribute ("y")]
        public float PositionY { get; set; }

        public override string ToString () {
            return string.Format ("[TextureHolder: TextureFile={0}, Width={1}, Height={2}, PositionX={3}, PositionY={4}]",
                TextureFile, Width, Height, PositionX, PositionY);
        }
    }
}