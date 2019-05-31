using System;
using System.Collections.Generic;
using System.Text;

namespace Nfe.Dominio.Entidades
{
    public class ConsultaNota
    {
        public string conSitNfe { get; set; }
        public string versao { get; set; }
        public int tpAmb { get; set; }
        public string xServ;
        public string chNFe { get; set; }
        public ConsultaNota(string versao, int tpAmb, string chave)
        {
            this.versao = versao;
            this.tpAmb = tpAmb;
            this.chNFe = chave;
            this.xServ = "CONSULTAR";
            this.conSitNfe = "consSitNFe";
        }
    }
}
