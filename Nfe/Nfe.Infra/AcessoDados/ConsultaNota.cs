using System;
using System.Xml;
using System.Collections.Generic;
using Nfe.Dominio.Entidades;
using Nfe.Infra.Configuracao;

namespace Nfe.Infra.AcessoDados
{
    public class RepositorioConsultaNota
    {
        public string PathDestinoXML;
        public ConsultaNota dados { get; set; }
        private XmlDocument doc;
        private NotaFiscal nota;

        public RepositorioConsultaNota(string chave)
        {
            ConfiguracaoApp Config = new ConfiguracaoApp();

            dados = new ConsultaNota("2.00", 2, chave);
            this.PathDestinoXML = Config.PastaXmlEnvio + chave + "-ped-sit.xml";
            doc = new XmlDocument();
        }
        public void SalvaXML()
        {
            XmlElement conSitNfe = doc.CreateElement(dados.conSitNfe);
            conSitNfe.SetAttribute("xmlns", "http://www.portalfiscal.inf.br/nfe");
            conSitNfe.SetAttribute("versao", dados.versao);
            XmlElement tpAmb = doc.CreateElement("tpAmb"); tpAmb.InnerText = dados.tpAmb.ToString();
            XmlElement xServ = doc.CreateElement("xServ"); xServ.InnerText = dados.xServ;
            XmlElement chNFe = doc.CreateElement("chNFe"); chNFe.InnerText = dados.chNFe;

            conSitNfe.AppendChild(tpAmb);
            conSitNfe.AppendChild(xServ);
            conSitNfe.AppendChild(chNFe);

            XmlElement root = doc.DocumentElement;
            XmlDeclaration xmlDec = doc.CreateXmlDeclaration("1.0", "UTF-8", "no");
            doc.InsertBefore(xmlDec, root);
            doc.AppendChild(conSitNfe);
            doc.Save(this.PathDestinoXML);
        }
    }
}
