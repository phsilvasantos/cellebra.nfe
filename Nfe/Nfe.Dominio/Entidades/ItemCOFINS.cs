using System.Xml;
using Nfe.Dominio;
namespace Nfe.Dominio.Entidades
{
    public class ItemCOFINS
    {
        public string TipoCofins { get; set; }
        public int? CST { get; set; }
        public decimal? vBC { get; set; }
        public decimal? pCOFINS { get; set; }
        public decimal? vCOFINS { get; set; }
        public decimal? qBCProd { get; set; }
        public decimal? vAliqProd { get; set; }
        public ItemCOFINS(int? cst, decimal? vbc, decimal? pcofins, decimal? vcofins, decimal? qbcprod, decimal? valiqprod)
        {
            this.CST = cst;
            this.vBC = vbc;
            this.pCOFINS = pcofins;
            this.vCOFINS = vcofins;
            this.qBCProd = qbcprod;
            this.vAliqProd = valiqprod;
            GeraTipoCofins(cst);
        }
        public XmlElement toNfeXml(XmlDocument doc)
        {
            XmlElement COFINS = doc.CreateElement("COFINS");
            XmlElement COFINSTipo = doc.CreateElement(this.TipoCofins);
            XmlElement CST = doc.CreateElement("CST");  CST.InnerText = this.CST.ToString().PadLeft(2,'0');
            XmlElement vBC = doc.CreateElement("vBC"); vBC.InnerText = Formatador.FormatDecimal(this.vBC);
            XmlElement pCOFINS = doc.CreateElement("pCOFINS"); pCOFINS.InnerText = Formatador.FormatDecimal(this.pCOFINS);
            XmlElement vCOFINS = doc.CreateElement("vCOFINS"); vCOFINS.InnerText = Formatador.FormatDecimal(this.vCOFINS);
            XmlElement qBCProd = doc.CreateElement("qBCProd"); qBCProd.InnerText = this.qBCProd.Value.ToString("#0");
            XmlElement vAliqProd = doc.CreateElement("vAliqProd"); vAliqProd.InnerText = Formatador.FormatDecimal(this.vAliqProd);

            COFINS.AppendChild(COFINSTipo);
            if (this.TipoCofins == "COFINSAliq")
            {
                if (!string.IsNullOrWhiteSpace(CST.InnerText)) { COFINSTipo.AppendChild(CST); }
                if (!string.IsNullOrWhiteSpace(vBC.InnerText)) { COFINSTipo.AppendChild(vBC); }
                if (!string.IsNullOrWhiteSpace(pCOFINS.InnerText)) { COFINSTipo.AppendChild(pCOFINS); }
            
            }
            else if (this.TipoCofins == "COFINSQtde")
            {
                if (!string.IsNullOrWhiteSpace(CST.InnerText)) { COFINSTipo.AppendChild(CST); }
                if (!string.IsNullOrWhiteSpace(qBCProd.InnerText)) { COFINSTipo.AppendChild(qBCProd); }
                if (!string.IsNullOrWhiteSpace(vAliqProd.InnerText)) { COFINSTipo.AppendChild(vAliqProd); }
            }
            else if (this.TipoCofins == "COFINSNT")
            {
                if (!string.IsNullOrWhiteSpace(CST.InnerText)) { COFINSTipo.AppendChild(CST); }
            }
            else if (this.TipoCofins == "COFINSOutr")
            {
                if (!string.IsNullOrWhiteSpace(CST.InnerText)) { COFINSTipo.AppendChild(CST); }
                if (!string.IsNullOrWhiteSpace(qBCProd.InnerText)) { COFINSTipo.AppendChild(qBCProd); }
                if (!string.IsNullOrWhiteSpace(vAliqProd.InnerText)) { COFINSTipo.AppendChild(vAliqProd); }
                if (!string.IsNullOrWhiteSpace(vBC.InnerText)) { COFINSTipo.AppendChild(vBC); }
                if (!string.IsNullOrWhiteSpace(pCOFINS.InnerText)) { COFINSTipo.AppendChild(pCOFINS);
                }
            }

            //Alterado a pedido de Hatori no dia 27/07/2011
            //if (!string.IsNullOrWhiteSpace(vCOFINS.InnerText)) { COFINSTipo.AppendChild(vCOFINS); }
            if (!string.IsNullOrWhiteSpace(vCOFINS.InnerText) && vCOFINS.InnerText != "0.00") { COFINSTipo.AppendChild(vCOFINS); }


            return COFINS;
            }
        public void GeraTipoCofins(int? cst)
        {
            if (cst == 1 || cst == 2 || cst == 43)
            {
                this.TipoCofins = "COFINSAliq";
            }
            if (cst == 3)
            {
                this.TipoCofins = "COFINSQtde";
            }
            if (cst == 4 || cst == 6 || cst == 7 || cst == 8 || cst == 9)
            {
                this.TipoCofins = "COFINSNT";
            }
            if (cst == 99)
            {
                this.TipoCofins = "COFINSOutr";
            }
        }
    }
}
