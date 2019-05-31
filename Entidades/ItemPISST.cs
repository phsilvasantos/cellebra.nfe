using System.Xml;


namespace Nfe.Dominio.Entidades
{
    public class ItemPISST
    {
        public int? PISformCalculo { get; set; } // 1 valor, 2 percentual
        private string PISST { get; set; }
        public decimal? vBC { get; set; }
        public decimal? pPIS { get; set; }
        public decimal? vPIS { get; set; }
        public decimal? qBCProd { get; set; }
        public decimal? vAliqProd { get; set; }
        public ItemPISST(int? pisformcalc,decimal? vbc, decimal? ppis, decimal? vpis, decimal? qbcprod, decimal? valiqprod)
        {
            this.PISformCalculo = pisformcalc;
            this.vBC = vbc;
            this.pPIS = ppis;
            this.vPIS = vpis;
            this.qBCProd = qbcprod;
            this.vAliqProd = valiqprod;
        }
        public XmlElement toNfeXml(XmlDocument doc)
        {
            XmlElement PISST = doc.CreateElement("PISST");
            XmlElement vBC = doc.CreateElement("vBC"); vBC.InnerText = Formatador.FormatDecimal(this.vBC);
            XmlElement pPIS = doc.CreateElement("pPIS"); pPIS.InnerText = Formatador.FormatDecimal(this.pPIS);
            XmlElement vPIS = doc.CreateElement("vPIS"); vPIS.InnerText = Formatador.FormatDecimal(this.vPIS);
            XmlElement qBCProd = doc.CreateElement("qBCProd"); qBCProd.InnerText = this.qBCProd.Value.ToString("#0");
            XmlElement vAliqProd = doc.CreateElement("vAliqProd"); vAliqProd.InnerText = this.vAliqProd.Value.ToString("#0");

            if (this.PISformCalculo == 1)
            {
                PISST.AppendChild(vBC);
                PISST.AppendChild(pPIS);
            }
            else
            {
                PISST.AppendChild(qBCProd);
                PISST.AppendChild(vAliqProd);
            }
            PISST.AppendChild(vPIS);
            return PISST;
        }
    }
}
