using System;
using System.Collections.Generic;
using System.Text;

namespace Nfe.Dominio.Entidades
{

    public class FormaDePagamento : Entidade
    {
        public string Descricao { get; set; }
        public string codFormaPagtoNFE { get; set; }
        public string codMeioPagamento { get; set; }
        public Int32 IDFormaPagamento { get; set; }

        public FormaDePagamento(string Descricao, string codFormaPagtoNFE, Int32 IDFormaPagamento, string codMeioPagtoNFE)
        {
            this.Descricao = Descricao;
            this.codFormaPagtoNFE = codFormaPagtoNFE;
            this.codMeioPagamento = codMeioPagtoNFE;
            this.IDFormaPagamento = IDFormaPagamento;
        }
        public FormaDePagamento()
        { }
    }
}
