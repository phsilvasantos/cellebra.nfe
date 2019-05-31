using System.Xml;

namespace Nfe.Dominio.Entidades
{
    public class ItemCOFINSST
    {
        public int? COFINSformCalculo { get; set; } // 1 valor, 2 percentual
        public decimal? vBC { get; set; }
        public decimal? pCOFINS { get; set; }
        public decimal? qBCProd { get; set; }
        public decimal? vAliqProd { get; set; }
        public decimal? vCOFINS { get; set; }

        public ItemCOFINSST(int? cofinsformcalc, decimal? vbc, decimal? pcofins, decimal? qbcprod, decimal? valiqprod, decimal? vcofins)
        {
            this.COFINSformCalculo = cofinsformcalc;
            this.vBC = vbc;
            this.pCOFINS = pcofins;
            this.qBCProd = qbcprod;
            this.vAliqProd = valiqprod;
            this.vCOFINS = vcofins;
        }
        public XmlElement toNfeXml(XmlDocument doc)
        {
            XmlElement COFINSST = doc.CreateElement("COFINSST");
            XmlElement vBC = doc.CreateElement("vBC"); vBC.InnerText = Formatador.FormatDecimal(this.vBC);
            XmlElement pCOFINS = doc.CreateElement("pCOFINS"); pCOFINS.InnerText = Formatador.FormatDecimal(this.pCOFINS);
            XmlElement qBCProd = doc.CreateElement("qBCProd"); qBCProd.InnerText = this.qBCProd.Value.ToString("#0");
            XmlElement vAliqProd = doc.CreateElement("vAliqProd"); vAliqProd.InnerText = this.vAliqProd.Value.ToString("#0");
            XmlElement vCOFINS = doc.CreateElement("vCOFINS"); vCOFINS.InnerText = Formatador.FormatDecimal(this.vCOFINS);

                if (this.COFINSformCalculo == 1)
                {
                    COFINSST.AppendChild(vBC);
                    COFINSST.AppendChild(pCOFINS);
                }
                else
                {
                    COFINSST.AppendChild(qBCProd);
                    COFINSST.AppendChild(vAliqProd);
                }
                COFINSST.AppendChild(vCOFINS);
            
            return COFINSST;
        }
    }
}
