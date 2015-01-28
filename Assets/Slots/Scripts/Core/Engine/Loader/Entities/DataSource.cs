using System.Xml.Serialization;

namespace UnitySlot {
    public class DataSource {
        [XmlAttribute ("class")]
        public string DataSourceClassName { get; set; }

        public override string ToString () {
            return string.Format ("[DataSource: DataSourceClassName={0}]", DataSourceClassName);
        }
    }
}