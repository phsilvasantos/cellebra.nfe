using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Nfe.Dominio.Entidades;
using Nfe.Infra.Configuracao;

namespace Nfe.Infra.AcessoDados
{
    public class RepositorioItemPISST
    {
        List<ItemPISST> Lista;
        public RepositorioItemPISST()
        {
            Lista = new List<ItemPISST>();
        }

        public List<ItemPISST> getById(int ID, eTPNotaFiscal TpNotaFiscal, string TipoNota, int IDFechamento = 0)
        {
            ConfiguracaoApp config = new ConfiguracaoApp();
            SqlConnection conn = config.getConexaoBD();

            conn.Open();

            string instrucao = "";

            //if (TpNotaFiscal == eTPNotaFiscal.eFaturamento)
            //    instrucao = " SELECT * FROM vw_NFEPISSTNFFat WHERE id_Fechamento = " + IDFechamento.ToString() + " AND id_ExpedTiposProd = ";
            //else if (TpNotaFiscal == eTPNotaFiscal.eExpedicao)
            //    instrucao = " SELECT * FROM vw_NFEPISSTNFExped WHERE id_ExpedTiposProd = ";
            //else if (TpNotaFiscal == eTPNotaFiscal.eExpedItens)
            //    instrucao = " SELECT * FROM vw_NFEPISSTNFExpedItens WHERE id_ExpedTiposProd = ";
            //else if (TpNotaFiscal == eTPNotaFiscal.eSaidaEstoque)
            //    instrucao = " SELECT * FROM vw_NFEPISSTNFSaidaEstoque WHERE id_saidaIteTit = ";

            if(TipoNota.Equals("Saída"))
            {
                instrucao = "SELECT * From dbo.NFEPISST where id_expedTiposProd = " + ID;
            }
            else
            {
                instrucao = "SELECT " +
                            "CST = CodigoSituacaoTributariaPis," +
                            "Tipo = tipoPISST," +
                            "vBC = valorBaseCalculoPisST," +
                            "pPIS = porcentagemPisST," +
                            "vPIS = valorPisST," +
                            "qBCProd = quantidadeVendidaPisST," +
                            "vAliqProd = valorAliquotaPisST " +
                            "From dbo.vw_ImpostosConfiguradosItens where id_item = " + ID;
            }

            SqlDataAdapter daPISST = new SqlDataAdapter(instrucao, conn);
            DataTable dtPISST = new DataTable();
            daPISST.Fill(dtPISST);
            conn.Close();
            
            for(int i=0;i < dtPISST.Rows.Count;i++)
            {
                DataRow drPISST = dtPISST.Rows[i];

                Lista.Add(new ItemPISST(drPISST["tipo"].ToString() == "" ? 0 :Convert.ToInt32(drPISST["tipo"].ToString()),
                                        drPISST["vBC"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drPISST["vBC"].ToString()),
                                        drPISST["pPIS"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drPISST["pPIS"].ToString()),
                                        drPISST["vPIS"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drPISST["vPIS"].ToString()),
                                        drPISST["qBCProd"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drPISST["qBCProd"].ToString()),
                                        drPISST["vAliqProd"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drPISST["vAliqProd"].ToString())));

            }
            return Lista;
        }
    }
}
