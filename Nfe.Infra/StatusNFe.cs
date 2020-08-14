using System;
using System.Threading;
using System.IO;
using System.Timers;
using System.Xml;
//using System.Xml.Linq;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Nfe.Infra.Configuracao;

namespace Nfe.Infra
{
    public class StatusNFe
    {
        public string chave { get; set; }
        public string nomearquivo { get; set; }
        public string nomearquivoRet {get;set;}
        public string versao { get; set; }
        public int tpAmb { get; set; }
        public string xServ { get; set; }
        private XmlDocument doc { get; set; }
        public string PathDestinoXML;
        public string PathAutorizadoXML;
        public string PathEmProcessamentoXML;
        public string PathRetornoXML;
        public string PathRetornoXMLErr { get; set; }
        public string PathErroXML { get; set; }
        public string resposta { get; set; }
        public string protocolo { get; set; }
        public string DataCancelamentoOuEnvioNFE { get; set; }

        public StatusNFe(string chave,string vers,string serv)
        {
            ConfiguracaoApp config = new ConfiguracaoApp();
            this.versao = vers;
            this.chave = chave;
            this.tpAmb = config.tpAmb;
            this.xServ = serv;
            this.doc = new XmlDocument();

            SqlConnection conn = config.getConexaoBD();
            conn.Open();

            SqlDataAdapter daData = new SqlDataAdapter("select CONVERT(VARCHAR(4),datepart(yyyy,getdate())) + CONVERT(char(2),datepart(mm,getdate())) as dataEnvio", conn);
            DataTable dtData = new DataTable();
            

            //preenche o datatable
            daData.Fill(dtData);
            DataRow drData = dtData.Rows[0];
            this.nomearquivo = chave;
            this.PathAutorizadoXML = config.PastaXmlEnviado + "\\" + "Autorizados\\" + drData["dataEnvio"].ToString() + "\\" + nomearquivo + "-procNFE.xml";
            this.PathEmProcessamentoXML = config.PastaXmlEmProcessamento + "\\" + nomearquivo + "-nfe.xml";
            this.PathDestinoXML = config.PastaXmlEnvio + "\\" + nomearquivo + "-ped-sit.xml";
            this.PathRetornoXML = config.PastaXmlRetorno + "\\"+ nomearquivo + "-sit.xml";
            this.PathRetornoXMLErr = config.PastaXmlRetorno + "\\" + nomearquivo + "-sit.err";
            this.PathErroXML = config.PastaXmlErro + "\\" + nomearquivo + "-nfe.xml";
            conn.Close();
        }
        public void SalvaXML()
        {
            XmlElement conSitNfe = doc.CreateElement("consSitNFe");
            conSitNfe.SetAttribute("xmlns", "http://www.portalfiscal.inf.br/nfe");
            conSitNfe.SetAttribute("versao", this.versao);
            XmlElement tpAmb = doc.CreateElement("tpAmb"); tpAmb.InnerText = this.tpAmb.ToString();
            XmlElement chNFE = doc.CreateElement("chNFe"); chNFE.InnerText = this.chave.ToString(); ;
            XmlElement xServ = doc.CreateElement("xServ"); xServ.InnerText = this.xServ;

            conSitNfe.AppendChild(tpAmb);            
            conSitNfe.AppendChild(xServ);
            conSitNfe.AppendChild(chNFE);

            XmlElement root = doc.DocumentElement;
            XmlDeclaration xmlDec = doc.CreateXmlDeclaration("1.0", "UTF-8", "no");
            doc.InsertBefore(xmlDec, root);
            doc.AppendChild(conSitNfe);

            doc.Save(this.PathDestinoXML);
        }
        public void SalvaXML(string StatusNotaFiscal)
        {
            XmlElement conSitNfe = doc.CreateElement("consSitNFe");
            conSitNfe.SetAttribute("xmlns", "http://www.portalfiscal.inf.br/nfe");
            conSitNfe.SetAttribute("versao", this.versao);
            XmlElement tpAmb = doc.CreateElement("tpAmb"); tpAmb.InnerText = this.tpAmb.ToString();
            XmlElement chNFE = doc.CreateElement("chNFe"); chNFE.InnerText = this.chave.ToString(); ;
            XmlElement xServ = doc.CreateElement("xServ"); xServ.InnerText = this.xServ;

            conSitNfe.AppendChild(tpAmb);            
            conSitNfe.AppendChild(xServ);
            conSitNfe.AppendChild(chNFE);

            XmlElement root = doc.DocumentElement;
            XmlDeclaration xmlDec = doc.CreateXmlDeclaration("1.0", "UTF-8", "no");
            doc.InsertBefore(xmlDec, root);
            doc.AppendChild(conSitNfe);
            if (!File.Exists(PathAutorizadoXML) && StatusNotaFiscal.Equals("Autorizado o uso da NF-e"))
            {
                if (File.Exists(PathErroXML))
                 {
                    if(File.Exists(PathEmProcessamentoXML))
                    {
                        File.Delete(PathEmProcessamentoXML);
                    }
                     File.Copy(PathErroXML, PathEmProcessamentoXML);
                 }
            }
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
                try
                {

                    XmlDocument xml = new XmlDocument();
                    xml.Load(this.PathRetornoXML);
                    XmlNodeList retConsStatServ = null;
                    retConsStatServ = xml.GetElementsByTagName("retConsSitNFe");
                    foreach (XmlNode consStatServNode in retConsStatServ)
                    {
                        XmlElement consStatServNodeElement = (XmlElement)consStatServNode;

                        XmlNodeList protocolo = doc.GetElementsByTagName("nProt");
                        if (protocolo.Count > 0)
                            this.protocolo = consStatServNodeElement.GetElementsByTagName("nProt")[0].InnerText;

                        this.resposta = consStatServNodeElement.GetElementsByTagName("xMotivo")[0].InnerText;

                        this.DataCancelamentoOuEnvioNFE = consStatServNodeElement.GetElementsByTagName("dhRecbto")[0].InnerText.ToString().Substring(0, 10);
                        retorno = true;
                    }
                }
                catch (Exception ex)
                { 
                }

            }
            else if (arqErr.Exists)
            {
                using (StreamReader sr = arqErr.OpenText())
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
