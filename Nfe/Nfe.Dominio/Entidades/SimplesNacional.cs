using System.Xml;

namespace Nfe.Dominio.Entidades
{
    public class ItemSimplesNacional
    {
        private string ICMSSN {get;set;}
        public int? CSOSN { get; set; } // 3 digitos
        public int? orig { get; set; }// 0,1,2
        public decimal? pCredSN { get; set; }
        public decimal? vCredICMSSN { get; set; }
        public int? modBCST { get; set; }// 0, 1, 2, 3, 4, 5
        public decimal? pMVAST { get; set; }
        public decimal? pRedBCST { get; set; }
        public decimal? vBCST { get; set; }
        public decimal? pICMSST { get; set; }
        public decimal? vICMSST { get; set; }
        public decimal? vBCSTRet { get; set; }
        public decimal? vICMSSTRet { get; set; }
        public int? modBC { get; set; }// 1,2,3,4
        public decimal? vBC { get; set; }
        public decimal? pRedBC { get; set; }
        public decimal? pICMS { get; set; }
        public decimal? vICMS { get; set; }
        public int? motDesICMS { get; set; }

        public ItemSimplesNacional(int? csosn, int? Orig, decimal? pcredsn, decimal? vcredicmssn, int? modbcst, decimal? pmvast,decimal? predbcst,
            decimal? vbcst,decimal? picmsst,decimal? vicmsst, decimal? vbcstret,decimal? vicmsstret,int? modbc,decimal? vbc, decimal? predbc, decimal? picms, decimal? vicms, int? motDesICMS)
        {
            this.CSOSN = csosn;
            this.orig = Orig;
            this.pCredSN = pcredsn;
            this.vCredICMSSN = vcredicmssn;
            this.modBCST = modbcst;
            this.pMVAST = pmvast;
            this.pRedBCST = predbcst;
            this.vBCST = vbcst;
            this.pICMSST = picmsst;
            this.vICMSST = vicmsst;
            this.vBCSTRet = vbcstret;
            this.vICMSSTRet = vicmsstret;
            this.modBC = modbc;
            this.vBC = vbc;
            this.pRedBC = predbc;
            this.pICMS = picms;
            this.vICMS = vicms;
            this.motDesICMS = motDesICMS;
            GeraICMSSN(csosn);
        }
        public XmlElement toNfeXml(XmlDocument doc)
        {
            XmlElement ICMS = doc.CreateElement("ICMS");
            XmlElement ICMSSN = doc.CreateElement(this.ICMSSN);
            XmlElement orig = doc.CreateElement("orig"); orig.InnerText = this.orig.ToString();
            
            //Usado se for Simples Nacional(CSOSN)
            XmlElement CSOSN = doc.CreateElement("CSOSN"); CSOSN.InnerText = this.CSOSN.ToString() == "0" ? "00" : this.CSOSN.ToString();
            //Usado se for Tributação Normal(CST)
            XmlElement CST = doc.CreateElement("CST"); CST.InnerText = this.CSOSN.ToString() == "0" ? "00" : this.CSOSN.ToString();
            
            XmlElement pCredSN = doc.CreateElement("pCredSN"); pCredSN.InnerText = Formatador.FormatDecimal(this.pCredSN);
            XmlElement vCredICMSSN = doc.CreateElement("vCredICMSSN"); vCredICMSSN.InnerText = Formatador.FormatDecimal(this.vCredICMSSN);
            XmlElement modBCST = doc.CreateElement("modBCST"); modBCST.InnerText = this.modBCST.ToString();
            XmlElement pMVAST = doc.CreateElement("pMVAST"); pMVAST.InnerText = this.pMVAST.ToString();
            XmlElement pRedBCST = doc.CreateElement("pRedBCST"); pRedBCST.InnerText = this.pRedBCST.ToString();
            XmlElement vBCST = doc.CreateElement("vBCST"); vBCST.InnerText = Formatador.FormatDecimal(this.vBCST);
            XmlElement pICMSST = doc.CreateElement("pICMSST"); pICMSST.InnerText = this.pICMSST.ToString().Replace(",", ".");
            XmlElement vICMSST = doc.CreateElement("vICMSST"); vICMSST.InnerText = Formatador.FormatDecimal(this.vICMSST);
            XmlElement vBCSTRet = doc.CreateElement("vBCSTRet"); vBCSTRet.InnerText = Formatador.FormatDecimal(this.vBCSTRet);
            XmlElement vICMSSTRet = doc.CreateElement("vICMSSTRet"); vICMSSTRet.InnerText = Formatador.FormatDecimal(this.vICMSSTRet);
            XmlElement modBC = doc.CreateElement("modBC"); modBC.InnerText = this.modBC.ToString();
            XmlElement vBC = doc.CreateElement("vBC"); vBC.InnerText = Formatador.FormatDecimal(this.vBC);
            XmlElement pRedBC = doc.CreateElement("pRedBC"); pRedBC.InnerText = this.pRedBC.ToString();
            XmlElement pICMS = doc.CreateElement("pICMS"); pICMS.InnerText = this.pICMS.ToString().Replace(",",".");
            XmlElement vICMS = doc.CreateElement("vICMS"); vICMS.InnerText = Formatador.FormatDecimal(this.vICMS);
            //criar o campo abaixo
            XmlElement motDesICMS = doc.CreateElement("motDesICMS"); motDesICMS.InnerText = this.motDesICMS.ToString();
            ICMS.AppendChild(ICMSSN);

            if (this.ICMSSN == "ICMSSN101")
            {
                ICMSSN.AppendChild(orig);
                ICMSSN.AppendChild(CSOSN);
                ICMSSN.AppendChild(pCredSN);
                ICMSSN.AppendChild(vCredICMSSN);
            }
            else if (this.ICMSSN == "ICMSSN102")
            {
                ICMSSN.AppendChild(orig);
                ICMSSN.AppendChild(CSOSN);
            }
            else if (this.ICMSSN == "ICMSSN201")
            {
                ICMSSN.AppendChild(orig);
                ICMSSN.AppendChild(CSOSN);
                ICMSSN.AppendChild(modBCST);
                ICMSSN.AppendChild(pMVAST);
                ICMSSN.AppendChild(pRedBCST);
                ICMSSN.AppendChild(vBCST);
                ICMSSN.AppendChild(pICMSST);
                ICMSSN.AppendChild(vICMSST);
                ICMSSN.AppendChild(pCredSN);
                ICMSSN.AppendChild(vCredICMSSN);
            }
            else if (this.ICMSSN == "ICMSSN202")
            {
                ICMSSN.AppendChild(orig);
                ICMSSN.AppendChild(CSOSN);
                ICMSSN.AppendChild(modBCST);
                ICMSSN.AppendChild(pMVAST);
                ICMSSN.AppendChild(pRedBCST);
                ICMSSN.AppendChild(vBCST);
                ICMSSN.AppendChild(pICMSST);
                ICMSSN.AppendChild(vICMSST);

            }
            else if (this.ICMSSN == "ICMSSN500")
            {
                ICMSSN.AppendChild(orig);
                ICMSSN.AppendChild(CSOSN);
                ICMSSN.AppendChild(vBCSTRet);
                ICMSSN.AppendChild(vICMSSTRet);
            }
            else if (this.ICMSSN == "ICMSSN900")
            {
                ICMSSN.AppendChild(orig);
                ICMSSN.AppendChild(CSOSN);
                ICMSSN.AppendChild(modBC);
                ICMSSN.AppendChild(vBC);
                ICMSSN.AppendChild(pRedBC);
                ICMSSN.AppendChild(pICMS);
                ICMSSN.AppendChild(vICMS);
                ICMSSN.AppendChild(modBCST);
                ICMSSN.AppendChild(pMVAST);
                ICMSSN.AppendChild(pRedBCST);
                ICMSSN.AppendChild(vBCST);
                ICMSSN.AppendChild(pICMSST);
                ICMSSN.AppendChild(vICMSST);
                ICMSSN.AppendChild(pCredSN);
                ICMSSN.AppendChild(vCredICMSSN);
            }
            else if (this.ICMSSN == "ICMS00")
            {
                ICMSSN.AppendChild(orig);
                ICMSSN.AppendChild(CST);
                ICMSSN.AppendChild(modBC);
                ICMSSN.AppendChild(vBC);
                ICMSSN.AppendChild(pICMS);
                ICMSSN.AppendChild(vICMS);
            }
            else if (this.ICMSSN == "ICMS10")
            {
                ICMSSN.AppendChild(orig);
                ICMSSN.AppendChild(CST);
                ICMSSN.AppendChild(modBC);
                ICMSSN.AppendChild(vBC);
                ICMSSN.AppendChild(pICMS);
                ICMSSN.AppendChild(vICMS);
                ICMSSN.AppendChild(modBCST);
                ICMSSN.AppendChild(pMVAST);
                ICMSSN.AppendChild(pRedBCST);
                ICMSSN.AppendChild(vBCST);
                ICMSSN.AppendChild(pICMSST);
                ICMSSN.AppendChild(vICMSST);
            }
            else if (this.ICMSSN == "ICMS20")
            {
                ICMSSN.AppendChild(orig);
                ICMSSN.AppendChild(CST);
                ICMSSN.AppendChild(modBC);
                ICMSSN.AppendChild(pRedBC);
                ICMSSN.AppendChild(vBC);
                ICMSSN.AppendChild(pICMS);
                ICMSSN.AppendChild(vICMS);
            }
            else if (this.ICMSSN == "ICMS30")
            {
                ICMSSN.AppendChild(orig);
                ICMSSN.AppendChild(CST);
                ICMSSN.AppendChild(modBCST);
                ICMSSN.AppendChild(pMVAST);
                ICMSSN.AppendChild(pRedBCST);
                ICMSSN.AppendChild(vBCST);
                ICMSSN.AppendChild(pICMSST);
                ICMSSN.AppendChild(vICMSST);
            }
            else if (this.ICMSSN == "ICMS40")
            {
                ICMSSN.AppendChild(orig);
                ICMSSN.AppendChild(CST);

                if (vICMS.InnerText != "0.00")
                {
                    ICMSSN.AppendChild(vICMS);
                    ICMSSN.AppendChild(motDesICMS);
                }

            }
            else if (this.ICMSSN == "ICMS51")
            {
                ICMSSN.AppendChild(orig);
                ICMSSN.AppendChild(CST);
                ICMSSN.AppendChild(modBC);
                ICMSSN.AppendChild(pRedBC);
                ICMSSN.AppendChild(vBC);
                ICMSSN.AppendChild(pICMS);
                ICMSSN.AppendChild(vICMS);

            }
            else if (this.ICMSSN == "ICMS60")
            {
                ICMSSN.AppendChild(orig);
                ICMSSN.AppendChild(CST);
                if (vBCSTRet.InnerText != "0.00")
                {
                    ICMSSN.AppendChild(vBCSTRet);
                }
                if (vICMSSTRet.InnerText != "0.00")
                {
                    ICMSSN.AppendChild(vICMSSTRet);
                }

            }
            else if (this.ICMSSN == "ICMS70")
            {
                ICMSSN.AppendChild(orig);
                ICMSSN.AppendChild(CST);
                ICMSSN.AppendChild(modBC);
                ICMSSN.AppendChild(pRedBC);
                ICMSSN.AppendChild(vBC);
                ICMSSN.AppendChild(pICMS);
                ICMSSN.AppendChild(vICMS);
                ICMSSN.AppendChild(modBCST);
                ICMSSN.AppendChild(pMVAST);
                ICMSSN.AppendChild(pRedBCST);
                ICMSSN.AppendChild(vBCST);
                ICMSSN.AppendChild(pICMSST);
                ICMSSN.AppendChild(vICMSST);
            }
            else if (this.ICMSSN == "ICMS90")
            {
                ICMSSN.AppendChild(orig);
                ICMSSN.AppendChild(CST);
                ICMSSN.AppendChild(modBC);
                ICMSSN.AppendChild(vBC);
                ICMSSN.AppendChild(pRedBC);
                ICMSSN.AppendChild(pICMS);
                ICMSSN.AppendChild(vICMS);
                ICMSSN.AppendChild(modBCST);
                ICMSSN.AppendChild(pMVAST);
                ICMSSN.AppendChild(pRedBCST);
                ICMSSN.AppendChild(vBCST);
                ICMSSN.AppendChild(pICMSST);
                ICMSSN.AppendChild(vICMSST);
            }
            return ICMS;
        }
        public void GeraICMSSN(int? csosn)
        {
            if (csosn == 101) { this.ICMSSN = "ICMSSN101"; }
            else if (csosn == 102 || csosn == 103 || csosn == 300 || csosn == 400) { this.ICMSSN = "ICMSSN102"; }
            else if (csosn == 201) { this.ICMSSN = "ICMSSN201"; }
            else if (csosn == 202 || csosn == 203) { this.ICMSSN = "ICMSSN202"; }
            else if (csosn == 500) { this.ICMSSN = "ICMSSN500"; }
            else if (csosn == 900) { this.ICMSSN = "ICMSSN900"; }
            else if (csosn == 0) {this.ICMSSN = "ICMS00";}
            else if (csosn == 10) { this.ICMSSN = "ICMS10"; }
            else if (csosn == 20) { this.ICMSSN = "ICMS20"; }
            else if (csosn == 30) { this.ICMSSN = "ICMS30"; }
            else if (csosn == 40 || csosn == 41 || csosn == 50) { this.ICMSSN = "ICMS40"; }
            else if (csosn == 51) { this.ICMSSN = "ICMS51"; }
            else if (csosn == 60) { this.ICMSSN = "ICMS60"; }
            else if (csosn == 70) { this.ICMSSN = "ICMS70"; }
            else if (csosn == 90) { this.ICMSSN = "ICMS90"; }

        }


    }
}
