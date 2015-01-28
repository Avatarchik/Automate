using System.Xml.Serialization;
using System.Collections.Generic;

namespace UnitySlot {
    public class TexturesHolder {
        [XmlElement ("texture")]
        public List<TextureHolder> Textures { get; set; }

        public override string ToString () {
            return string.Format ("[TexturesHolder: Textures={0}]", Textures.ToString ());
        }
    }
}