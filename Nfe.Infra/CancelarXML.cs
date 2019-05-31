using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Nfe.Infra
{
    public class CancelarXML
    {
        private string Versao { get; set; }
        private string Id { get; set; }
        private string TpAmb { get; set; }
        private string NProt { get; set; }
        private string xJust { get; set; }

        public CancelarXML(string versao, string id, string tpamb,string nprot,string xjust)
        {
            this.Versao = versao;
            this.Id = id;
            this.TpAmb = tpamb;
            this.NProt = nprot;
            this.xJust = xjust;
        }
        public XmlDocument GerarXMLCancelamento()
        {
            XmlWriterSettings configXML = new XmlWriterSettings();
            configXML.Indent = true;
            configXML.IndentChars = "";
            configXML.NewLineOnAttributes = false;
            configXML.OmitXmlDeclaration = false;

            Stream xmlSaida = new MemoryStream();

            XmlWriter oXmlGravar = XmlWriter.Create(xmlSaida, configXML);

            oXmlGravar.WriteStartDocument();
            oXmlGravar.WriteStartElement("cancNFe", "http://www.portalfiscal.inf.br/nfe");
            oXmlGravar.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            oXmlGravar.WriteAttributeString("versao", this.Versao);
            oXmlGravar.WriteStartElement("infCanc");
            oXmlGravar.WriteAttributeString("Id", "NFe" + Id);

            oXmlGravar.WriteElementString("tpAmb", this.TpAmb);
            oXmlGravar.WriteElementString("xServ", "CANCELAR");
            oXmlGravar.WriteElementString("chNFe", this.Id);
            oXmlGravar.WriteElementString("nProt", this.NProt);
            oXmlGravar.WriteElementString("xJust", this.xJust);

            oXmlGravar.WriteEndElement(); //Fecha elemento infCanc

            oXmlGravar.WriteEndElement(); //Fecha elemento cancNFe

            oXmlGravar.Flush();
            xmlSaida.Flush();
            xmlSaida.Position = 0;

            XmlDocument documento = new XmlDocument();
            documento.Load(xmlSaida);
            oXmlGravar.Close();

            return documento;
        }
    }
}
