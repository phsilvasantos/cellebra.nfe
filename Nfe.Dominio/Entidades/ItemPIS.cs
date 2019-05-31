using System.Xml;
using System.Text;
using Nfe.Dominio;

namespace Nfe.Dominio.Entidades
{
    public class ItemPIS
    {
        public string tipoPIS { get; set; }
        public int? CST { get; set; }
        public decimal? vBC { get; set; }
        public decimal? pPIS { get; set; }
        public decimal? vPIS { get; set; }
        public decimal? qBCProd { get; set; }
        public decimal? vAliqProd { get; set; }

        public ItemPIS(int? cst, decimal? vbc, decimal? ppis, decimal? vpis, decimal? qbcprod, decimal? valiqprod)
        {
            this.CST = cst;
            this.vBC = vbc;
            this.pPIS = ppis;
            this.vPIS = vpis;
            this.qBCProd = qbcprod;
            this.vAliqProd = valiqprod;
            GeraTipoXlml(cst);

        }
        public XmlElement toNfeXml(XmlDocument doc)
        {
            XmlElement PIS = doc.CreateElement("PIS");
            XmlElement PISXml = doc.CreateElement(this.tipoPIS);

            XmlElement CST = doc.CreateElement("CST"); CST.InnerText = this.CST.ToString().PadLeft(2, '0');
            XmlElement vBC = doc.CreateElement("vBC"); vBC.InnerText = Formatador.FormatDecimal(this.vBC);
            XmlElement pPIS = doc.CreateElement("pPIS"); pPIS.InnerText = Formatador.FormatDecimal(this.pPIS);
            XmlElement vPIS = doc.CreateElement("vPIS"); vPIS.InnerText = Formatador.FormatDecimal(this.vPIS);
            XmlElement qBCProd = doc.CreateElement("qBCProd"); qBCProd.InnerText = Formatador.FormatDecimal(this.qBCProd);
            XmlElement vAliqProd = doc.CreateElement("vAliqProd"); vAliqProd.InnerText = Formatador.FormatDecimal(this.vAliqProd);

            PIS.AppendChild(PISXml);
            if (this.tipoPIS == "PISAliq")
            {
                PISXml.AppendChild(CST);
                PISXml.AppendChild(vBC);
                PISXml.AppendChild(pPIS);
                PISXml.AppendChild(vPIS);
            }
            else if(this.tipoPIS == "PISQtde")
            {
                PISXml.AppendChild(CST);
                PISXml.AppendChild(qBCProd);
                PISXml.AppendChild(vAliqProd);
                PISXml.AppendChild(vPIS);
            }
            else if (this.tipoPIS == "PISNT")
            {
                PISXml.AppendChild(CST);
            }
            else if (this.tipoPIS == "PISOutr")
            {
                PISXml.AppendChild(CST);
                PISXml.AppendChild(vBC);
                PISXml.AppendChild(pPIS);
                PISXml.AppendChild(qBCProd);
                PISXml.AppendChild(vAliqProd);
                PISXml.AppendChild(vPIS);
            }            
            return PIS;
        }
        public void GeraTipoXlml(int? cst)
        {
            if (cst == 1 || cst == 2 || cst == 35) { this.tipoPIS = "PISAliq"; }
            else if (cst == 3) { this.tipoPIS = "PISQtde"; }
            else if (cst == 4 || cst == 6 || cst == 7 || cst == 8 || cst == 9) { this.tipoPIS = "PISNT"; }
            else if (cst == 99) { this.tipoPIS = "PISOutr"; }
        }
        
    }
    
}
