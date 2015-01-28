using System.Xml.Serialization;

namespace UnitySlot {
    public class PagesHolder {
        [XmlAttribute ("count")]
        public int PageCount{ get; set; }

        [XmlAttribute ("format")]
        public string PageFileFormat{ get; set; }

        public override string ToString () {
            return string.Format ("[PagesHolder: PageCount={0}, PageFileFormat={1}]", PageCount, PageFileFormat);
        }
    }
}