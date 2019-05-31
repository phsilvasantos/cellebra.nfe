using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;


namespace Nfe.Dominio.Entidades

{
    public class NotaFiscal : Entidade
    {

        public string IDChave { get; set; }//47
        public int? cUF { get; set; }//2
        public string cNF { get; set; }//8
        public string natOp { get; set; }//1-60
        public int? indPag { get; set; }//1 0 avista, 1 a prazo, 2 outros
        public int? mod { get; set; }//2
        public int? serie { get; set; }//1-3
        public string nNF { get; set; }//1-9
        public string dEmi { get; set; }//AAAA-MM-DD
        public string dSaiEnt { get; set; }//AAAA-MM-DD
        public string hSaiEnt { get; set; }//HH:MM:SS
        public int? tpNF { get; set; }// 0 entrada / 1 Saida
        public int? cMunFG { get; set; }//7
        public int? cDV { get; set; }//1
        public int? tpImp { get; set; } // 1 formato retrato, 2 formato paisagem
        public int? tpEmis { get; set; }
        public int? tpAmb { get; set; } // 1 producao 2 - homologacao
        public int? finNFe { get; set; } // 1 normal 2 complementar 3 ajustes
        public int? procEmi { get; set; }
        public string verProc { get; set; }
        public string xJust { get; set; }
        public string lote { get; set; }
        public string recibo { get; set; }
        public string protocolo { get; set; }
        public NFePessoa Cliente { get; set; }
        public Emitente Emitente { get; set; }
        public List<Produtos> Produtos { get; set; }
        public bool PossuiTransportadora { get; set; }
        public Transportadora Transportadora { get; set; }

        
       
        public decimal? vBC { get; set; } // 15-2 dec Base de Cálculo do ICMS
        public decimal? vICMS { get; set; } //15-2 dec Valor Total do ICMS
        public decimal? vBCST { get; set; } //15-2 Base de Cálculo do ICMS ST
        public decimal? vST { get; set; } //15-2 Valor Total do ICMS ST
        public decimal? vProd { get; set; } //15-2 Valor Total dos produtos e serviços
        public decimal? vFrete { get; set; } //15-2 Valor Total do Frete

        public decimal? vSeg { get; set; } //15-2 Valor Total do Seguro
        public decimal? vDesc { get; set; } //15-2 Valor Total do Desconto
        public decimal? vII { get; set; } //15-2 Valor Total do II
        public decimal? vIPI { get; set; } //15-2 Valor Total do IPI
        public decimal? vPIS { get; set; } //15-2 Valor do PIS
        public decimal? vCOFINS { get; set; } //15-2 Valor do COFINS
        public decimal? vOutro { get; set; } //15-2 Outras Despesas acessórias
        public decimal? vNF { get; set; } //15-2 Valor Total da NF-e

        public string esp { get; set; }
        public string marca { get; set; }
        public decimal? volume { get; set; }
        public string pesoLiq { get; set; }
        public string pesoBruto { get; set; }
        public string fatura { get; set; }



        public string Path { get; set; }

        public NotaFiscal()
        {
            Produtos = new List<Produtos>();
        }
        
      
        private string getDateNow()
        {
            var dt = DateTime.Now;
            return dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString();
        }
        public void gerarChave(Emitente emit, int operacao)
        {
            int[] pesos = { 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            StringBuilder sb = new StringBuilder();
            string ano = DateTime.Now.Year.ToString().Remove(0,2);
            string mes = DateTime.Now.Month.ToString();
            if (mes.Length <2)
            {
                mes = "0" + mes;
            }
            string serie2 = null;
            if (serie.ToString().Length < 3)
            {
                serie2 = serie.ToString().PadLeft(3, '0');
            }
            else
            {
                serie2 = serie.ToString();
            }
            string numNota = null;
            if (nNF.ToString().Length < 9)
            {
                numNota = nNF.ToString().PadLeft(9, '0');
            }
            else
            {
                numNota = nNF.ToString();
            }
            string codNUm = null;
            if (cNF.ToString().Length < 8)
            {
                codNUm = cNF.ToString().PadLeft(8, '0');
            }
            else
            {
                codNUm = cNF.ToString();
            }
            sb.Append(cUF);
            sb.Append(ano);
            sb.Append(mes);
            sb.Append(emit.Cnpj.Trim());
            sb.Append(mod);
            sb.Append(serie2);
            sb.Append(numNota);
            sb.Append(tpEmis);
            sb.Append(codNUm);
            

            if (sb.Length == 43)
            {
                List<int> chaves = new List<int>();

                string numeros = sb.ToString();
                for (int i = 0; i < 43; i++)
                {
                    chaves.Add(Convert.ToInt32(numeros.Substring(i, 1)));
                }
                List<int> ponderacao = new List<int>();
                for (int i = 0; i < 43; i++)
                {
                    ponderacao.Add(pesos[i] * chaves[i]);
                }
                int total = 0;
                foreach (int item in ponderacao)
                {
                    total += item;
                }
                int rest = total % 11;
                if (rest == 0 || rest ==1)
                {
                    cDV = 0;
                }
                else
                {
                    cDV = 11 - rest;
                }                
                sb.Append(cDV);
            }
            else
            {
                throw new Exception("Erro na composição dos caracteres da chave");
            }

            if ((this.IDChave == null) || (this.IDChave == "") || (operacao == 0))
            {
                this.IDChave = sb.ToString();
            }
        }
        public XmlElement toNfeXml(XmlDocument doc)

        {
            XmlElement xcUF = doc.CreateElement("cUF");
            xcUF.InnerText = cUF.ToString().Trim();
            XmlElement xcNF = doc.CreateElement("cNF");
            xcNF.InnerText = cNF.ToString().Trim();
            XmlElement xnatOp = doc.CreateElement("natOp");
            xnatOp.InnerText = natOp.ToString().Trim();
            XmlElement xindPag = doc.CreateElement("indPag");
            xindPag.InnerText = indPag.ToString().Trim();
            XmlElement xmod = doc.CreateElement("mod");
            xmod.InnerText = mod.ToString().Trim();
            XmlElement xserie = doc.CreateElement("serie");
            xserie.InnerText = serie.ToString().Trim();
            XmlElement xnNF = doc.CreateElement("nNF");
            xnNF.InnerText = nNF.ToString().Trim();
            XmlElement xdEmi = doc.CreateElement("dEmi");
            xdEmi.InnerText = dEmi.ToString().Trim();
            XmlElement xtpNF = doc.CreateElement("tpNF");
            xtpNF.InnerText = tpNF.ToString().Trim();
            XmlElement xcMunFG = doc.CreateElement("cMunFG");
            xcMunFG.InnerText = cMunFG.ToString().Trim();
            XmlElement xtpImp = doc.CreateElement("tpImp");
            xtpImp.InnerText = tpImp.ToString().Trim();
            XmlElement xtpEmis = doc.CreateElement("tpEmis");
            xtpEmis.InnerText = tpEmis.ToString().Trim();
            XmlElement xcDV = doc.CreateElement("cDV");
            xcDV.InnerText = cDV.ToString().Trim();
            XmlElement xtpAmb = doc.CreateElement("tpAmb"); // 1 producao 2 homologacao
            xtpAmb.InnerText = tpAmb.ToString().Trim();
            XmlElement xfinNFe = doc.CreateElement("finNFe");
            xfinNFe.InnerText = finNFe.ToString().Trim();
            XmlElement xprocEmi = doc.CreateElement("procEmi");
            xprocEmi.InnerText = procEmi.ToString().Trim();
            XmlElement xverProc = doc.CreateElement("verProc");
            xverProc.InnerText = this.verProc.Trim();
           

            XmlElement ide = doc.CreateElement("ide");
            ide.AppendChild(xcUF);
            ide.AppendChild(xcNF);
            ide.AppendChild(xnatOp);
            ide.AppendChild(xindPag);
            ide.AppendChild(xmod);
            ide.AppendChild(xserie);
            ide.AppendChild(xnNF);
            ide.AppendChild(xdEmi);
            ide.AppendChild(xtpNF);
            ide.AppendChild(xcMunFG);
            ide.AppendChild(xtpImp);
            ide.AppendChild(xtpEmis);
            ide.AppendChild(xcDV);
            ide.AppendChild(xtpAmb);
            ide.AppendChild(xfinNFe);
            ide.AppendChild(xprocEmi);
            ide.AppendChild(xverProc);
            if (tpEmis==2)
            {
                XmlElement xdhCont = doc.CreateElement("dhCont");
                xdhCont.InnerText = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                XmlElement xxJust = doc.CreateElement("xJust");
                xxJust.InnerText = xJust.Trim();
                ide.AppendChild(xdhCont);
                ide.AppendChild(xxJust);
            }
            return ide;
        }


    }
}
