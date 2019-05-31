using System;
using System.Threading;
using System.IO;
using System.Timers;
using System.Xml;
//using System.Xml.Linq;
//using System.Linq;
using System.Collections.Generic;
using System.Text;
using Nfe.Infra.Configuracao;
namespace Nfe.Infra
{
    public class StatusServico
    {
        public string consStatServ { get; set; }
        public string nomearquivo { get; set; }
        public string nomearquivoRet {get;set;}
        public string versao { get; set; }
        public int tpAmb { get; set; }
        public int cUF { get; set; }
        public string xServ { get; set; }
        private XmlDocument doc { get; set; }
        public string PathDestinoXML;
        public string PathRetornoXML;
        public string PathRetornoXMLErr;
        public string resposta { get; set; }

        public StatusServico(string versao, string serv)
        {
            ConfiguracaoApp config = new ConfiguracaoApp();
            this.versao = versao;
            this.tpAmb = config.tpAmb;
            this.cUF = config.Uf;
            this.xServ = serv;
            this.doc = new XmlDocument();
            string data = DateTime.Now.ToString("yyyyMMdd");
            string hora = DateTime.Now.ToString("hhmmss");
            
            this.nomearquivo =  data+"T"+hora;
            this.PathDestinoXML = config.PastaXmlEnvio + "\\" + nomearquivo + "-ped-sta.xml";
            this.PathRetornoXML = config.PastaXmlRetorno + "\\"+ nomearquivo + "-sta.xml";
            this.PathRetornoXMLErr = config.PastaXmlRetorno + "\\" + nomearquivo + "-sta.err";
        }
        public void SalvaXML()
        {
            XmlElement conSitNfe = doc.CreateElement("consStatServ");
            conSitNfe.SetAttribute("xmlns", "http://www.portalfiscal.inf.br/nfe");
            conSitNfe.SetAttribute("versao", this.versao);
            XmlElement tpAmb = doc.CreateElement("tpAmb"); tpAmb.InnerText = this.tpAmb.ToString();
            XmlElement cUF = doc.CreateElement("cUF"); cUF.InnerText = this.cUF.ToString(); ;
            XmlElement xServ = doc.CreateElement("xServ"); xServ.InnerText = this.xServ;

            conSitNfe.AppendChild(tpAmb);
            conSitNfe.AppendChild(cUF);
            conSitNfe.AppendChild(xServ);

            XmlElement root = doc.DocumentElement;
            XmlDeclaration xmlDec = doc.CreateXmlDeclaration("1.0", "UTF-8", "no");
            doc.InsertBefore(xmlDec, root);
            doc.AppendChild(conSitNfe);
            doc.Save(this.PathDestinoXML);
        }
        public void GetResposta()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(3000);
                if (verResposta())
                {
                    break;
                }
                else
                {
                    continue;
                }
            }
        }
        public bool verResposta()
        {
            bool retorno = false;
            FileInfo arq = new FileInfo(this.PathRetornoXML);
            FileInfo arqErr = new FileInfo(this.PathRetornoXMLErr);
            if (arq.Exists)
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(this.PathRetornoXML);
                XmlNodeList retConsStatServ = null;
                retConsStatServ = xml.GetElementsByTagName("retConsSitNFe");
                foreach (XmlNode consStatServNode in retConsStatServ)
                {
                    XmlElement consStatServNodeElement = (XmlElement)consStatServNode;
                    this.resposta = consStatServNodeElement.GetElementsByTagName("xMotivo")[0].InnerText;
                    retorno = true;
                }
            }
            else if (arqErr.Exists)
            {
                using (StreamReader sr  = arqErr.OpenText())
                {
                    string s = "";
                    string resp = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        resp += s;
                        retorno = true;
                    }
                    UTF8Encoding utf88 = new UTF8Encoding();
                    byte[] byteArray = Encoding.ASCII.GetBytes(resp); 
                    byte[] utf8Array = Encoding.Convert(Encoding.ASCII, Encoding.UTF8, byteArray);
                    string finalString = utf88.GetString(utf8Array);
                    this.resposta = finalString;
                }
            }
            else
            {
                retorno = false;
                this.resposta = "Não foi possivel verificar sistema sefaz, tente novamente mais tarde ou entre em contato com o suporte";
            }
            return retorno;            
        }
    }
}
