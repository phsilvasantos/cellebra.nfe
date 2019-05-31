using System;
using System.Collections.Generic;
using Nfe.Dominio.Entidades;
using Nfe.Infra.Configuracao;
using System.Data;
using System.Data.SqlClient;

namespace Nfe.Infra.AcessoDados
{
    public class RepositorioItemCOFINSST
    {
        List<ItemCOFINSST> Lista;
        public RepositorioItemCOFINSST()
        {
            Lista = new List<ItemCOFINSST>();
        }

        public List<ItemCOFINSST> getById(int ID, eTPNotaFiscal TpNotaFiscal, string TipoNota, int IDFechamento = 0)
        {

            ConfiguracaoApp config = new ConfiguracaoApp();
            SqlConnection conn = config.getConexaoBD();

            conn.Open();

            //define adaptador para valores do COFINS
            string instrucao = "";

            //if (TpNotaFiscal == eTPNotaFiscal.eFaturamento)
            //    instrucao = " SELECT * FROM dbo.vw_NFECofinsSTNFFat WHERE id_Fechamento = " + IDFechamento.ToString() + " AND id_ExpedTiposProd = ";
            //else if (TpNotaFiscal == eTPNotaFiscal.eExpedicao)
            //    instrucao = " SELECT * FROM dbo.vw_NFECofinsSTNFExped WHERE id_ExpedTiposProd = ";
            //else if (TpNotaFiscal == eTPNotaFiscal.eExpedItens)
            //    instrucao = " SELECT * FROM dbo.vw_NFECofinsSTNFExpedItens WHERE id_Item = ";
            //else if (TpNotaFiscal == eTPNotaFiscal.eSaidaEstoque)
            //    instrucao = " SELECT * FROM dbo.vw_NFECofinsSTNFSaidaEstoque WHERE id_saidaIteTit = ";

            if (TipoNota.Equals("Saída"))
            {
                instrucao = "SELECT * From dbo.NFECOFINSST where id_expedTiposProd = " + ID;
            }
            else
            {
                instrucao = "SELECT "+
                            "CST = codigoSituacaoTributariaCofins," +
                            "Tipo = tipoCOFINSST," +
                            "vBC = valorBaseCalculoCofinsST," +
                            "pCOFINS = porcentagemCofinsST," +
                            "vCOFINS = valorCofinsST," +
                            "qBCProd = quantidadeVendidaCofinsST," +
                            "vAliqProd = valorAliquotaCofinsST " +
                            "From dbo.vw_ImpostosConfiguradosItens where id_item = " + ID;
            }

            SqlDataAdapter daCOFINSST = new SqlDataAdapter(instrucao, conn);

            //Define o DataTable para os valores do COFINS
            DataTable dtCOFINSST = new DataTable();

            //Preenche o datatable
            daCOFINSST.Fill(dtCOFINSST);
            conn.Close();

            for (int i = 0; i < dtCOFINSST.Rows.Count; i++)
            {
                //Obtem o registro atual conforme o indice i
                DataRow drCOFINSST = dtCOFINSST.Rows[i];

                Lista.Add(new ItemCOFINSST(drCOFINSST["Tipo"].ToString() == "" ? 0 : Convert.ToInt32(drCOFINSST["Tipo"].ToString()),
                                           drCOFINSST["vBC"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drCOFINSST["vBC"].ToString()),
                                           drCOFINSST["pCOFINS"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drCOFINSST["pCOFINS"].ToString()),
                                           drCOFINSST["qBCProd"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drCOFINSST["qBCProd"].ToString()),
                                           drCOFINSST["vAliqProd"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drCOFINSST["vAliqProd"].ToString()),
                                           drCOFINSST["vCOFINS"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drCOFINSST["vCOFINS"].ToString())
                                           ));
            }
            return Lista;
        }

    }
}
