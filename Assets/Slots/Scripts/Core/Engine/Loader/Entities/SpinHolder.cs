using System.Xml.Serialization;

namespace UnitySlot {
    public class SpinHolder {
        [XmlAttribute ("blur")]
        public bool Blur { get; set; }

        [XmlAttribute ("knockback")]
        public bool KnockBack { get; set; }

        [XmlAttribute ("speed")]
        public float Speed { get; set; }

        [XmlAttribute ("timerRate")]
        public float TimerRate { get; set; }

        [XmlAttribute ("delay")]
        public int Delay{ get; set; }

        public override string ToString () {
            return string.Format ("[SpinHolder: Blur={0}, KnockBack={1}, Speed={2}, TimerRate={3}, Delay={4}]",
                Blur, KnockBack, Speed, TimerRate, Delay);
        }
    }
}