using System;
using System.Collections.Generic;
using System.Text;

namespace Nfe.Dominio.Entidades
{

    public class NatOperacao : Entidade
    {
        public string Descricao { get; set; }
        public string CodSPOperacao { get; set; }
        public string CodForaSPOperacao { get; set; }
        public Int32 IDNaturezaOperacao { get; set; }

        public NatOperacao(string Descricao, string codSPOperacao, string codForaSPOperacao, Int32 IDNaturezaOperacao)
        {
            this.Descricao = Descricao;
            this.CodSPOperacao = codSPOperacao;
            this.CodForaSPOperacao = codForaSPOperacao;
            this.IDNaturezaOperacao = IDNaturezaOperacao;
        }
        public NatOperacao()
        {}
    }
}
