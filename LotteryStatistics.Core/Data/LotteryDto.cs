using System.Xml.Serialization;

namespace LotteryStatistics.Core.Data
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "lottery", Namespace = "", IsNullable = false)]
    public class LotteryDto
    {
        /// <remarks/>
        [XmlArray(ElementName = "drawings")]
        [System.Xml.Serialization.XmlArrayItemAttribute("drawing", IsNullable = false)]
        public string[] Drawings { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("name")]
        public string Name { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("highestNumber")]
        public byte HighestNumber { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("updateUrl")]
        public string UpdateUrl { get; set; }

    }
}
