using System;
using Nfe.Dominio.Entidades;
using Nfe.Infra.AcessoDados;
using System.Data;
using System.Data.SqlClient;
using Nfe.Infra.Configuracao;

namespace Nfe.Infra.AcessoDados
{
    public class NaturezaOperacao
    {
        public NatOperacao GetById(int id)
        {
            try
            {
                ConfiguracaoApp config = new ConfiguracaoApp(false);
                SqlConnection conn = config.getConexaoBD();
                NatOperacao natoper = new NatOperacao();
                conn.Open();

                //define adaptador para a Natureza de Operação
                SqlDataAdapter daNatOperacao = new SqlDataAdapter("Select * from Gerenciamento_Comum.dbo.NaturOperacoes WHERE id_NaturOper = " + id.ToString(), conn);
                DataTable dtNatOperacao = new DataTable();

                //preenche o datatable
                daNatOperacao.Fill(dtNatOperacao);
                conn.Close();

                DataRow drNatOperacao = dtNatOperacao.Rows[0];

                natoper.Descricao = drNatOperacao["DescOperacao"].ToString();
                natoper.CodSPOperacao = drNatOperacao["CodSpOperacao"].ToString();
                natoper.CodForaSPOperacao = drNatOperacao["CodForaSPOperacao"].ToString();
                natoper.IDNaturezaOperacao = Convert.ToInt32(drNatOperacao["id_NaturOper"].ToString());

                return natoper;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro no momento de capturar as informações da natureza de operação: " + ex.Message);
            }
        }
    }
}
