using System;
using System.Linq;
using System.Xml.Serialization;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Description;

namespace CodeMagic.Configuration.Xml.Types.Items.Description
{
    [Serializable]
    public class XmlDescriptionConfiguration : IDescriptionConfiguration
    {
        [XmlIgnore]
        public IRarenessDescriptionConfiguration[] RarenessDescription =>
            RarenessDescriptionData.ToArray<IRarenessDescriptionConfiguration>();

        [XmlArray("rareness-description")]
        [XmlArrayItem("rareness")]
        public XmlRarenessDescriptionConfiguration[] RarenessDescriptionData { get; set; }

        [XmlIgnore]
        public IMaterialDescriptionConfiguration[] MaterialDescription =>
            MaterialDescriptionData.ToArray<IMaterialDescriptionConfiguration>();

        [XmlArray("material-description")]
        [XmlArrayItem("material")]
        public XmlMaterialDescriptionConfiguration[] MaterialDescriptionData { get; set; }
    }
}