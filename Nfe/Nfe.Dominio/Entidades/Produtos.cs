using System;
using System.Collections.Generic;
using System.Text;

namespace Nfe.Dominio.Entidades
{
    public class Produtos : Entidade
    {
        public int IdNota { get; set; }
        public int cProd { get; set; } // 1-60 codigo de produto
        public string xProd { get; set; } //1-120
        public string NCM { get; set; }// 2 ou 8 digitos
        public int CFOP { get; set; } //4digitos  OBRIGATORIO
        public string uCom { get; set; } // 1-6 dig unidade comercial
        public decimal qCom { get; set; }// 15 e 0-4 quantidade comercial
        public decimal vUnCom { get; set; } //21 dig e 0-10 deci
        public decimal vProd { get; set; }// 15dig 2 dec valor total bruto dos produtos
        public string uTrib { get; set; } //1-6dig
        public decimal qTrib { get; set; }//15 dig 0-4 decimal
        public decimal vUnTrib { get; set; }//21 dig 0-10dec
        public int indTot { get; set; } // valor 0 - valor do vProd nao compoe o val total da nfe, 1 - valor do vProd  compoe o val total da nfe
        public decimal vFrete { get; set; } // 15 dig 2 decimal - valor do vFrete nao compoe o val total da nfe


        public List<ItemPIS> PIS { get; set; }
        public List<ItemPISST> PISST { get; set; }
        public List<ItemCOFINS> COFINS { get; set; }
        public List<ItemCOFINSST> COFINSST { get; set; }
        public List<ItemSimplesNacional> SimpNac { get; set; }

        public string ICMSSN101 { get; set; } // Tributação do ICMS pelo SIMPLES NACIONAL eCSOSN=101 (v.2.0)  tamanho 3digitos exatos
        public int orig { get; set; }// 1 digg  0 – Nacional; 1 – Estrangeira – Importação direta;  – 2 Estrangeira – Adquirida no mercado interno.
        public string CSOSN { get; set; } // 101- Tributada pelo Simples Nacional com permissão de crédito. (v.2.0)
        public decimal pCredSN { get; set; } // 5 2dec
        public decimal vCredICMSSN { get; set; }// 15 e 2dec



        public bool TemIpi { get; set; } // possui ou nao ipi

        public Produtos()
        {
        }
        public Produtos(int id, int idnota,int cprod,string xprod,string ncm, int cfop,string ucom,decimal qcom,decimal vuncom,
            decimal vprod,string utrib, decimal qtrib,decimal vuntrib, int indtot, string icmssn101, int orig, string csosn, decimal pcredsn, decimal vcredicmssn, decimal vFrete)
        {
            SetId(id);
            this.IdNota = idnota;
            this.cProd = cprod;
            this.xProd = xprod;
            this.NCM = ncm;
            this.CFOP = cfop;
            this.uCom = ucom;
            this.qCom = qcom;
            this.vUnCom = vuncom;
            this.vProd = vprod;
            this.uTrib = utrib;
            this.qTrib = qtrib;
            this.vUnTrib = vuntrib;
            this.indTot = indTot;
            this.vFrete = vFrete;
        }
    }
}
