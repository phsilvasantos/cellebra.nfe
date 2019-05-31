using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Nfe.Dominio.Entidades
{
    
    public class NFePessoa : Entidade
    {
        public TipoPesssoa TipoPessoa { get; set; }
        public string Cnpj { get; set; }
        public string Cpf { get; set; }
        public string NomeOURazao { get; set; }

        public int IdEndereco { get; set; }
        public NfeEndereco Endereco { get; set; }
        
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string IE { get; set; }

        public string Rg { get; set; }
        public string Contato { get; set; }
        public string Fax { get; set; }
        
        

        public NFePessoa(TipoPesssoa tipo, string cnpjOuCpf, string nome, NfeEndereco endereco, string tel, string email,string ie)
        {
            this.TipoPessoa = tipo;
            if (TipoPessoa == TipoPesssoa.Juridico)
            {
                this.Cnpj = cnpjOuCpf;
            }
            else
            {
                this.Cpf = cnpjOuCpf;
            }
            this.NomeOURazao = nome;
            this.Endereco = endereco;
            this.Telefone = tel;
            this.Email = email;
            this.IE = ie;            
        }
        public NFePessoa()
        {
        }
        public  XmlElement toNfeXml(XmlDocument doc,int? tpAmb )
        {
            XmlElement dest = doc.CreateElement("dest");

            if (this.TipoPessoa == TipoPesssoa.Fisico)
            {
                XmlElement pessoaDest = doc.CreateElement("CPF"); pessoaDest.InnerText = this.Cpf.Trim();
                dest.AppendChild(pessoaDest);
            }
            else
            {
                XmlElement pessoaDest = doc.CreateElement("CNPJ");

                if (tpAmb ==1) //producao
                {
                    pessoaDest.InnerText = this.Cnpj.Trim();
                }
                else //homologacao
                {
                    pessoaDest.InnerText = "00000000000191";
                }
                
                dest.AppendChild(pessoaDest);
            }

            XmlElement xNomeDest = doc.CreateElement("xNome");
            if (tpAmb == 1)
            {
                xNomeDest.InnerText = this.NomeOURazao.Trim();
            }
            else
            {
                xNomeDest.InnerText = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";
            }
            XmlElement IExml = doc.CreateElement("IE");
            if (tpAmb ==1)
            {
                if (this.IE == null)
                {
                    IExml.InnerText = "";
                }
                else
                {
                    IExml.InnerText = this.IE.Trim();
                }
            }
            
            
           
            dest.AppendChild(xNomeDest);
            dest.AppendChild(Endereco.toNfeXmlDestinatario(doc));
            dest.AppendChild(IExml);
            return dest;
        }
        public void LoadTesteCliente()
        {
            this.TipoPessoa = TipoPesssoa.Fisico;
            this.Cpf = "31730705855";
            this.NomeOURazao = "Fernando Mikio Hatori";
        }
    }
}
