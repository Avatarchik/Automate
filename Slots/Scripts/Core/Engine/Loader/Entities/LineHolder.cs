using System.Xml.Serialization;
using System.Collections.Generic;
using System;

namespace UnitySlot {
    public class LineHolder {
        [XmlAttribute ("index")]
        public int Index { get; set; }

        [XmlAttribute ("color")]
        public string Color { get; set; }

        [XmlAttribute ("rolls")]
        public string StringRolls { get; set; }

        List<int> _rolls;

        [XmlIgnore]
        public List<int> Rolls {
            get {
                if (_rolls == null || _rolls.Count < 1) {
                    _rolls = new List<int> ();
                    if (string.IsNullOrEmpty (StringRolls)) {
                        string[] tokens = StringRolls.Split (',');
                        var rolls = Array.ConvertAll<string, int> (tokens, int.Parse);
                        foreach (int item in rolls) {
                            _rolls.Add (item);
                        }
                    }
                }
                return _rolls;
            }
        }

        public override string ToString () {
            return string.Format ("[LineHolder: Index={0}, Color={1}, StringRolls={2}]", Index, Color, StringRolls);
        }
    }
}