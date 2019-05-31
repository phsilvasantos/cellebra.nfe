using System;
using System.Xml;

namespace Nfe.Dominio.Entidades
{
    public class NfeEndereco
    {
        /// <summary>
        /// Obrigatorio 2-60
        /// </summary>
        public string Logradouro { get; set; }
        /// <summary>
        /// Obrigatorio 1-60
        /// </summary>
        public string Numero { get; set; }
        /// <summary>
        /// Obrigatorio 1-60
        /// </summary>
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Pais { get; set; }
        public int CodPais { get; set; }
        public string Municipio { get; set; }
        public System.Nullable<Int32> CodMunicipio  { get; set; }
        public string UF { get; set; }

        public NfeEndereco()
        {
        }
        
        public NfeEndereco(string logradouro, string numero, string complemento, string bairro, string cep, string pais, int codPais,string municipio, int? codMunicipio,string uf)
        {
            this.Logradouro = logradouro;
            this.Numero = numero;
            this.Complemento = complemento;
            this.Bairro = bairro;
            this.Cep = cep;
            this.Pais = pais;
            this.CodPais = codPais;
            this.Municipio = municipio;
            this.CodMunicipio = codMunicipio;
            this.UF = uf;
        }
        public void SearchUFCodigo()
        {
        }
        public void SearchMunicipioCodigo()
        {
        }
        public XmlElement toNfeXmlDestinatario(XmlDocument doc)
        {
            XmlElement enderDest = doc.CreateElement("enderDest");
            XmlElement xLgrDest = doc.CreateElement("xLgr"); xLgrDest.InnerText = this.Logradouro;
            XmlElement nroDest = doc.CreateElement("nro"); nroDest.InnerText = this.Numero;
            
            XmlElement xCplDest = doc.CreateElement("xCpl"); xCplDest.InnerText = this.Complemento;
            XmlElement xBairroDest = doc.CreateElement("xBairro"); xBairroDest.InnerText = this.Bairro;
            XmlElement cMunDest = doc.CreateElement("cMun"); cMunDest.InnerText = this.CodMunicipio.ToString();
            XmlElement xMunDest = doc.CreateElement("xMun"); xMunDest.InnerText = this.Municipio;
            XmlElement UFDest = doc.CreateElement("UF"); UFDest.InnerText = this.UF;
            XmlElement CEPDest = doc.CreateElement("CEP"); CEPDest.InnerText = this.Cep.ToString();
            XmlElement cPaisDest = doc.CreateElement("cPais"); cPaisDest.InnerText = this.CodPais.ToString();
            XmlElement xPaisDest = doc.CreateElement("xPais"); xPaisDest.InnerText = this.Pais;

            enderDest.AppendChild(xLgrDest);
            enderDest.AppendChild(nroDest);
            if (!string.IsNullOrWhiteSpace(this.Complemento))
            {
                enderDest.AppendChild(xCplDest);
            }           
            enderDest.AppendChild(xBairroDest);
            if (!string.IsNullOrWhiteSpace(this.CodMunicipio.ToString()))
            {
                enderDest.AppendChild(cMunDest);
            }
            else
            {
                throw new Exception("Codigo do municipio do destinatario inválido");
            }
            
            enderDest.AppendChild(xMunDest);
            enderDest.AppendChild(UFDest);
            enderDest.AppendChild(CEPDest);
            enderDest.AppendChild(cPaisDest);
            enderDest.AppendChild(xPaisDest);
            return enderDest;
        }
        public XmlElement toNfeXmlEmitente(XmlDocument doc)
        {
            XmlElement xLgr = doc.CreateElement("xLgr"); xLgr.InnerText = this.Logradouro;
            XmlElement nro = doc.CreateElement("nro"); nro.InnerText = this.Numero;
            XmlElement xBairro = doc.CreateElement("xBairro"); xBairro.InnerText = this.Bairro;
            XmlElement cMun = doc.CreateElement("cMun"); cMun.InnerText = this.CodMunicipio.ToString();
            XmlElement xMun = doc.CreateElement("xMun"); xMun.InnerText = this.Municipio;
            XmlElement UF = doc.CreateElement("UF"); UF.InnerText = this.UF;
            XmlElement CEP = doc.CreateElement("CEP"); CEP.InnerText = this.Cep.ToString();
            XmlElement cPais = doc.CreateElement("cPais"); cPais.InnerText = this.CodPais.ToString();
            XmlElement xPais = doc.CreateElement("xPais"); xPais.InnerText = this.Pais;

            XmlElement enderEmit = doc.CreateElement("enderEmit");
            enderEmit.AppendChild(xLgr);
            enderEmit.AppendChild(nro);
            enderEmit.AppendChild(xBairro);
            enderEmit.AppendChild(cMun);
            enderEmit.AppendChild(xMun);
            enderEmit.AppendChild(UF);
            enderEmit.AppendChild(CEP);
            enderEmit.AppendChild(cPais);
            enderEmit.AppendChild(xPais);
            return enderEmit;
        }
        
    }
}
