using System;

namespace Nfe.Dominio.Entidades
{
    public class NfeInformacoesEmpresa
    {
        public string CNPJ { get; set; }
        public string NomeEmpresa { get; set; }
        public string NomeFantasia { get; set; }
        public string InscEstadual { get; set; }
        public string InscMunicipal { get; set; }
        public string CNAE { get; set; }
        public string CRT { get; set; }
        public NfeEndereco Endereco { get; set;}
        public int tpAmb { get; set; }
        public int tpEmis { get; set; }

        public NfeInformacoesEmpresa()
        {
        }

        public NfeInformacoesEmpresa(string CNPJ, 
                                     string NomeEmpresa, 
                                     string NomeFantasia, 
                                     string InscEstadual, 
                                     string InscMunicipal, 
                                     string CNAE, 
                                     string CRT,
                                     NfeEndereco Endereco,
                                     int tpAmb,
                                     int tpEmis)
        {
            this.CNPJ = CNPJ;
            this.NomeEmpresa = NomeEmpresa;
            this.NomeFantasia = NomeFantasia;
            this.InscEstadual = InscEstadual;
            this.InscMunicipal = InscMunicipal;
            this.CNAE = CNAE;
            this.CRT = CRT;
            this.Endereco = Endereco;
            this.tpAmb = tpAmb;
            this.tpEmis= tpEmis;
        }
    }
}
