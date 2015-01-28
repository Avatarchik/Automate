using System.Xml.Serialization;

namespace UnitySlot {
    [XmlRoot ("game")]
    public class GameHolder {
        [XmlAttribute ("descVersion")]
        public int DescVersion { get; set; }

        [XmlAttribute ("id")]
        public string Id { get; set; }

        [XmlAttribute ("name")]
        public string Name { get; set; }

        [XmlAttribute ("version")]
        public int Version { get; set; }

        [XmlAttribute ("minAppVersion")]
        public int MinAppVersion { get; set; }

        [XmlAttribute ("minLevel")]
        public int MinLevel { get; set; }

        [XmlAttribute ("gameType")]
        public string GameType { get; set; }

        [XmlElement ("icon")]
        public IconHolder Icon;
        [XmlElement ("fun-icon")]
        public IconHolder FunIcon;
        [XmlElement ("loading")]
        public LoadingHolder LoadingSceneConfig;
        [XmlElement ("field")]
        public AreaHolder Area;
        [XmlElement ("datasource")]
        public DataSource DataSource;
        [XmlElement ("info")]
        public InfoHolder SlotInfo;
        [XmlElement ("bonus")]
        public BonusGame BonusGame;
        [XmlElement ("double")]
        public DoubleGame DoubleGame;

        public override string ToString () {
            return string.Format ("[GameHolder: Icon={0}, FunIcon={1}, LoadingSceneConfig={2}, area={3}, DataSource={4}, " +
            "SlotInfo={5}, BonusGame={6}, DoubleGame={7}, DescVersion={8}, Id={9}, Name={10}, Version={11}, MinAppVersion={12}, " +
            "MinLevel={13}, GameType={14}]", Icon.ToString (), FunIcon.ToString (), LoadingSceneConfig.ToString (),
                Area.ToString (), DataSource.ToString (), SlotInfo.ToString (), BonusGame.ToString (),
                DoubleGame.ToString (), DescVersion, Id, Name, Version, MinAppVersion, MinLevel, GameType);
        }
    }
}