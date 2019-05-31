using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;

namespace Nfe.Dominio.Entidades
{

    public class Emitente : NFePessoa
    {

        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string IM { get; set; }
        public string CNAE { get; set; }
        public string CRT { get; set; }
        public NfeEndereco endereco { get; set; }

        public Emitente(string razao, string fantasia, string ie, string im, string cnae,string crt)
        {
            this.RazaoSocial = razao;
            this.NomeFantasia = fantasia;
            this.IE = ie;
            this.IM = im;
            this.CNAE = cnae;
            this.CRT = crt;
            endereco = new NfeEndereco();
        }

        public Emitente()
        {
        }

        public XmlElement EmitToNfeXml(XmlDocument doc)
        {
            XmlElement CNPJ = doc.CreateElement("CNPJ"); CNPJ.InnerText = this.Cnpj.Trim();
            XmlElement xNome = doc.CreateElement("xNome"); xNome.InnerText = this.RazaoSocial.Trim();
            XmlElement xFant = doc.CreateElement("xFant"); xFant.InnerText = this.NomeFantasia.Trim();

            XmlElement IE = doc.CreateElement("IE"); IE.InnerText = this.IE.Trim();
            XmlElement IM = doc.CreateElement("IM"); IM.InnerText = this.IM.Trim() == "" ? "ISENTO" : this.IM.Trim();
            XmlElement CNAE = doc.CreateElement("CNAE"); CNAE.InnerText = this.CNAE.Trim();
            XmlElement CRT = doc.CreateElement("CRT"); CRT.InnerText = this.CRT.Trim();

            XmlElement emit = doc.CreateElement("emit");
            emit.AppendChild(CNPJ);
            emit.AppendChild(xNome);
            emit.AppendChild(xFant);
            emit.AppendChild(endereco.toNfeXmlEmitente(doc));
            emit.AppendChild(IE);
            emit.AppendChild(IM);
            emit.AppendChild(CNAE);
            emit.AppendChild(CRT);

            return emit;
        }

    }
}  
