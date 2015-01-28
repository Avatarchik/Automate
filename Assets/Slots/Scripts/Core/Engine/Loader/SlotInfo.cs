using System.Xml.Serialization;
using System;

namespace UnitySlot {
    public class SlotInfo {
        [XmlAttribute("newSlot")]
        public bool NewSlot { get; set; }
        [XmlAttribute ("comingSoon")]
        public bool ComingSoon { get; set; }

        [XmlAttribute ("descVersion")]
        public int DescVersion { get; set; }

        [XmlAttribute ("fileSize")]
        public int SlotSize { get; set; }

        [XmlAttribute ("gameType")]
        public string GameType { get; set; }

        [XmlAttribute ("id")]
        public string Id { get; set; }

        [XmlAttribute ("minAppVersion")]
        public int MinAppVersion { get; set; }

        [XmlAttribute ("minLevel")]
        public int MinLevel { get; set; }

        [XmlAttribute ("name")]
        public string Name { get; set; }

        [XmlAttribute ("version")]
        public int Version { get; set; }

        [XmlElement ("url")]
        public string ProdactionUrl;
        [XmlElement ("test-url")]
        public string TestUrl;

        public Uri URL 
        {
            get 
            {
                #if !UNITY_EDITOR
                return new Uri(ProdactionUrl);
                #elif UNITY_EDITOR
                return new Uri(TestUrl);
                #endif
            }
        }

        [XmlElement ("preview")]
        public SlotInfoPreview Preview;
        [XmlElement ("fun-preview")]
        public SlotInfoPreview FunPreview;

        public override string ToString () {
            return string.Format ("[SlotInfo: ProdactionUrl={0}, TestUrl={1}, Preview={2}, FunPreview={3}, ComingSoon={4}, DescVersion={5}, " +
            "SlotSize={6}, GameType={7}, Id={8}, MinAppVersion={9}, MinLevel={10}, Name={11}, Version={12}]",
                ProdactionUrl, TestUrl, Preview.ToString (), FunPreview.ToString (), ComingSoon, DescVersion, SlotSize, GameType, Id,
                MinAppVersion, MinLevel, Name, Version);
        }
    }
}
