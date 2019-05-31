using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Nfe.Dominio.Entidades;
using Nfe.Infra.Configuracao;

namespace Nfe.Infra.AcessoDados
{
    public class RepositorioItemCOFINS
    {

        public List<ItemCOFINS> Lista;

        public RepositorioItemCOFINS()
        {
            Lista = new List<ItemCOFINS>();
        }

        public List<ItemCOFINS> getById(int ID, eTPNotaFiscal TPNotaFiscal, string TipoNota, int IDFechamento = 0)
        {

            ConfiguracaoApp config = new ConfiguracaoApp();
            SqlConnection conn = config.getConexaoBD();

            conn.Open();

            //define adaptador para valores do COFINS
            string instrucao = "";

            //if (TPNotaFiscal == eTPNotaFiscal.eFaturamento)
            //    instrucao = " SELECT * FROM dbo.vw_danfefechamento WHERE id_Fechamento = " + IDFechamento.ToString() + " AND id_ExpedTiposProd = ";
            //else if (TPNotaFiscal == eTPNotaFiscal.eExpedicao)
            //    instrucao = " SELECT * FROM dbo.vw_NFECofinsNFExped WHERE id_ExpedTiposProd = ";
            //else if (TPNotaFiscal == eTPNotaFiscal.eExpedItens)
            //    instrucao = " SELECT * FROM dbo.vw_NFECofinsNFExpedItens WHERE id_Item = ";
            //else if (TPNotaFiscal == eTPNotaFiscal.eSaidaEstoque)
            //    instrucao = " SELECT * FROM dbo.vw_NFECofinsNFSaidaEstoque WHERE id_saidaIteTit = ";

            if (TipoNota.Equals("Saída"))
            {
                instrucao = "SELECT * From dbo.NFECOFINS where id_expedTiposProd = " + ID;
            }
            else
            {
                instrucao = "SELECT " +
                            "CST = codigoSituacaoTributariaCofins," +
                            "vBC = valorBaseCalculoCofins," +
                            "pCOFINS = porcentagemCofins," +
                            "vCOFINS = valorCofins," +
                            "qBCProd = quantidadeVendidaCofins," +
                            "vAliqProd = valorAliquotaCofins " +
                            "From dbo.vw_ImpostosConfiguradosItens where id_item = " + ID;
            }
            SqlDataAdapter daCOFINS = new SqlDataAdapter(instrucao, conn);

            //Define o DataTable para os valores do COFINS
            DataTable dtCOFINS = new DataTable();

            //Preenche o datatable
            daCOFINS.Fill(dtCOFINS);
            conn.Close();

            for (int i = 0; i < dtCOFINS.Rows.Count; i++)
            {
                //Obtem o registro atual conforme o indice i
                DataRow drCOFINS = dtCOFINS.Rows[i];
                Lista.Add(new ItemCOFINS(drCOFINS["CST"].ToString() == "" ? Convert.ToInt32(0) : Convert.ToInt32(drCOFINS["CST"].ToString()),
                                         drCOFINS["vBC"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drCOFINS["vBC"].ToString()),
                                         drCOFINS["pCOFINS"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drCOFINS["pCOFINS"].ToString()),
                                         drCOFINS["vCOFINS"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drCOFINS["vCOFINS"].ToString()),
                                         drCOFINS["qBCProd"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drCOFINS["qBCProd"].ToString()),
                                         drCOFINS["vAliqProd"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drCOFINS["vAliqProd"].ToString())));
            }
            return Lista;
        }
    }
}
