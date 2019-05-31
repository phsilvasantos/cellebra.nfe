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

        public List<ItemCOFINS> getById(int ID, eTPNotaFiscal TPNotaFiscal, string TipoNota, int IDFechamento = 0, string OrigemNFE = "", double valorproduto = 0.00)
        {

            ConfiguracaoApp config = new ConfiguracaoApp(false);
            SqlConnection conn = config.getConexaoBD();

            conn.Open();

            //define adaptador para valores do COFINS
            string instrucao = "";

            if (TipoNota.Equals("Saída"))
                instrucao = "SELECT * From dbo.NFECOFINS where id_expedTiposProd = " + ID;
            else
                if (TPNotaFiscal == eTPNotaFiscal.eFaturamento || TPNotaFiscal == eTPNotaFiscal.eExpedicao || TPNotaFiscal == eTPNotaFiscal.eExpedItens)
                    if (OrigemNFE == "PRODUTOS")

                        instrucao = "SELECT " +
                                    "  CST = codigoSituacaoTributariaCofins," +
                                    "  vBC = valorBaseCalculoCofins," +
                                    "  pCOFINS = porcentagemCofins," +
                                    "  vCOFINS = valorCofins," +
                                    "  qBCProd = quantidadeVendidaCofins," +
                                    "  vAliqProd = valorAliquotaCofins " +
                                    " FROM dbo.vw_ImpostosConfiguradosProdutos where id_produto = " + ID;
                    else

                        instrucao = " SELECT " +
                                    "  CST = codigoSituacaoTributariaCofins," +
                                    "  vBC = valorBaseCalculoCofins," +
                                    "  pCOFINS = porcentagemCofins," +
                                    "  vCOFINS = valorCofins," +
                                    "  qBCProd = quantidadeVendidaCofins," +
                                    "  vAliqProd = valorAliquotaCofins " +
                                    " FROM dbo.vw_ImpostosConfiguradosItens where id_item = " + ID;
                else
                    instrucao = "SELECT " +
                                "CST = codigoSituacaoTributariaCofins," +
                                "vBC = valorBaseCalculoCofins," +
                                "pCOFINS = porcentagemCofins," +
                                "vCOFINS = valorCofins," +
                                "qBCProd = quantidadeVendidaCofins," +
                                "vAliqProd = valorAliquotaCofins " +
                                "From dbo.vw_ImpostosConfiguradosItens where id_item = " + ID;

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

                if (valorproduto != 0)
                    drCOFINS["vBC"] = valorproduto.ToString();

                if (drCOFINS["pCOFINS"].ToString() == "")
                    drCOFINS["pCOFINS"] = "0.00";

                if (Convert.ToDouble(drCOFINS["pCOFINS"].ToString()) != 0 && valorproduto != 0.00)
                    drCOFINS["vCOFINS"] = valorproduto * (Convert.ToDouble(drCOFINS["pCOFINS"].ToString()) / 100);

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
