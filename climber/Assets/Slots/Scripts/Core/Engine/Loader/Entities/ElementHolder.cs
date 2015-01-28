using System.Xml.Serialization;
using System.Collections.Generic;
using System;

namespace UnitySlot {
    public class ElementHolder {
        [XmlAttribute ("index")]
        public int Index { get; set; }

        [XmlAttribute ("winSound")]
        public string WinSound { get; set; }

        [XmlAttribute ("allowedRolls")]
        public string StringAllowedRolls { get; set; }

        [XmlAttribute ("scatter")]
        public bool Scatter{ get; set; }

        [XmlAttribute ("wild")]
        public bool Wild { get; set; }

        [XmlAttribute ("canRepeat")]
        public bool CanRepeat { get; set; }

        [XmlAttribute ("trigger")]
        public bool Trigger { get; set; }

        [XmlElement ("animation")]
        public List<LineAnimationHolder> Animations;
        HashSet<int> _allowedRolls;

        public HashSet<int> AllowedRolls {
            get {
                if (_allowedRolls == null || _allowedRolls.Count < 1) {
                    _allowedRolls = new HashSet<int> ();
                    if (string.IsNullOrEmpty (StringAllowedRolls)) {
                        string[] tokens = StringAllowedRolls.Split (',');
                        var rolls = Array.ConvertAll<string, int> (tokens, int.Parse);
                        foreach (int item in rolls) {
                            _allowedRolls.Add (item);
                        }
                    }
                }
                return _allowedRolls;
            }
        }

        public override string ToString () {
            return string.Format ("[ElementHolder: Index={0}, WinSound={1}, StringAllowedRolls={2}, Scatter={3}, Wild={4}, " +
            "CanRepeat={5}, Trigger={6}, Animations={7}]", Index, WinSound, StringAllowedRolls, Scatter, Wild, CanRepeat,
                Trigger, Animations.ToString ());
        }
    }
}