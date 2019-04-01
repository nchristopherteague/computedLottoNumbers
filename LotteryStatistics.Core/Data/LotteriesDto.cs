namespace LotteryStatistics.Core.Data
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "lotteries", Namespace = "", IsNullable = false)]
    public class LotteriesDto
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("lottery")]
        public LotteryDto[] Lottery { get; set; }
    }
}
