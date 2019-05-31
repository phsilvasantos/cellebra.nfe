using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Nfe.Dominio.Entidades;
using Nfe.Infra.Configuracao;

namespace Nfe.Infra.AcessoDados
{
    public class RepositorioItemPIS
    {
        List<ItemPIS> Lista;

        public RepositorioItemPIS()
        {
            Lista = new List<ItemPIS>();
        }

        public List<ItemPIS> getById(int ID, eTPNotaFiscal TPNotaFiscal, string TipoNota, int IDFechamento = 0)
        {
            ConfiguracaoApp config = new ConfiguracaoApp();
            SqlConnection conn = config.getConexaoBD();

            conn.Open();

            string instrucao = "";

            //if(TPNotaFiscal == eTPNotaFiscal.eFaturamento)
            //    instrucao = " SELECT * FROM vw_NFEPISNFFat WHERE id_Fechamento = " + IDFechamento.ToString() + " AND id_ExpedTiposProd = ";
            //else if(TPNotaFiscal == eTPNotaFiscal.eExpedicao)
            //    instrucao = " SELECT * FROM vw_NFEPISNFExped WHERE id_ExpedTiposProd = ";
            //else if(TPNotaFiscal == eTPNotaFiscal.eExpedItens)
            //    instrucao = " SELECT * FROM vw_NFEPISNFExpedItens WHERE id_ItensExpedicao = ";
            //else if(TPNotaFiscal == eTPNotaFiscal.eSaidaEstoque)
            //    instrucao = " SELECT * FROM vw_NFEPISNFSaidaEstoque WHERE id_saidaIteTit = ";

            if (TipoNota.Equals("Saída"))
            {
                instrucao = "SELECT * From dbo.NFEPIS where id_expedTiposProd = " + ID;
            }
            else
            {
                instrucao = "SELECT " +
                            "CST = CodigoSituacaoTributariaPis," +
                            "vBC = valorBaseCalculoPis," +
                            "pPIS = porcentagemPis," +
                            "vPIS = valorPis," +
                            "qBCProd = quantidadeVendidaPis," +
                            "vAliqProd = valorAliquotaPis " +
                            "From dbo.vw_ImpostosConfiguradosItens where id_item = " + ID;
            }

            SqlDataAdapter daPIS = new SqlDataAdapter(instrucao, conn);

            DataTable dtPIS = new DataTable();
            daPIS.Fill(dtPIS);
            conn.Close();
            for (int i = 0; i < dtPIS.Rows.Count; i++)
            {
                DataRow drPIS = dtPIS.Rows[i];
                Lista.Add(new ItemPIS(drPIS["CST"].ToString() == "" ? Convert.ToInt32(0) :Convert.ToInt32(drPIS["CST"].ToString()),
                                      drPIS["vBC"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drPIS["vBC"].ToString()),
                                      drPIS["pPIS"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drPIS["pPIS"].ToString()),
                                      drPIS["vPIS"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drPIS["vPIS"].ToString()),
                                      drPIS["qBCProd"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drPIS["qBCProd"].ToString()),
                                      drPIS["vAliqProd"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drPIS["vAliqProd"].ToString())));
            }
            return Lista;
        }
    }
}
