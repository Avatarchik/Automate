using System.Xml.Serialization;

namespace UnitySlot {
    [System.Serializable] 
    [XmlRoot ("slotVariables")]
    public class SlotVariables {
        string _defaultReferrer = "";
    
        [XmlElement ("defaultReferrer")]
        public string DefaultReferrer {
            get { return _defaultReferrer;}
            set{ _defaultReferrer = value;}
        }
    
        public override string ToString () {
            return string.Format ("[SlotVariables: DefaultReferrer={0}]", DefaultReferrer);
        }
    }
}