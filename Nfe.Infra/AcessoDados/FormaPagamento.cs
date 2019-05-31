using System;
using Nfe.Dominio.Entidades;
using Nfe.Infra.AcessoDados;
using System.Data;
using System.Data.SqlClient;
using Nfe.Infra.Configuracao;

namespace Nfe.Infra.AcessoDados
{
    public class FormaPagamento : Entidade
    {
        public FormaDePagamento GetById(int id)
        {
            try
            {
                ConfiguracaoApp config = new ConfiguracaoApp(false);
                SqlConnection conn = config.getConexaoBD();
                FormaDePagamento FormaPagto = new FormaDePagamento();
                conn.Open();

                //define adaptador para a Natureza de Operação
                SqlDataAdapter daFormaPagto = new SqlDataAdapter("Select * from Gerenciamento_Comum.dbo.FormasPagamento WHERE id_FormaPagamento = " + id.ToString(), conn);
                DataTable dtFormaPagto = new DataTable();

                //preenche o datatable
                daFormaPagto.Fill(dtFormaPagto);
                conn.Close();

                DataRow drFormaPagto = dtFormaPagto.Rows[0];

                FormaPagto.Descricao = drFormaPagto["Descricao"].ToString();
                FormaPagto.codFormaPagtoNFE = drFormaPagto["cod_formapagtoNFE"].ToString();
                FormaPagto.codMeioPagamento = drFormaPagto["codMeioPagtoNFE"].ToString();
                FormaPagto.IDFormaPagamento = Convert.ToInt32(drFormaPagto["id_formaPagamento"].ToString());

                return FormaPagto;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro no momento de capturar as informações da natureza de operação: " + ex.Message);
            }
        }
    }
}
