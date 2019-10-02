using System;
using System.Xml.Serialization;
using CodeMagic.ItemsGeneration.Configuration.Tool;

namespace CodeMagic.Configuration.Xml.Types.Items.Tool
{
    [Serializable]
    public class XmlToolsConfiguration : IToolsConfiguration
    {
        [XmlIgnore]
        public IToolConfiguration LumberjackAxe => LumberjackAxeData;

        [XmlElement("lumberjack-axe")]
        public XmlToolConfiguration LumberjackAxeData { get; set; }

        [XmlIgnore]
        public IToolConfiguration Pickaxe => PickaxeData;

        [XmlElement("pickaxe")]
        public XmlToolConfiguration PickaxeData { get; set; }
    }
}