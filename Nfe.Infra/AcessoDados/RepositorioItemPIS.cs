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

        public List<ItemPIS> getById(int ID, eTPNotaFiscal TPNotaFiscal, string TipoNota, int IDFechamento = 0, string OrigemNFE = "", double valorproduto = 0.00)
        {
            ConfiguracaoApp config = new ConfiguracaoApp(false);
            SqlConnection conn = config.getConexaoBD();

            conn.Open();

            string instrucao = "";

            if (TipoNota.Equals("Saída"))
                instrucao = "SELECT * From dbo.NFEPIS where id_expedTiposProd = " + ID;
            else
                if (TPNotaFiscal == eTPNotaFiscal.eFaturamento || TPNotaFiscal == eTPNotaFiscal.eExpedicao || TPNotaFiscal == eTPNotaFiscal.eExpedItens)
                    if (OrigemNFE == "PRODUTOS")

                        instrucao = " SELECT " +
                                    "  CST = CodigoSituacaoTributariaPis," +
                                    "  vBC = valorBaseCalculoPis," +
                                    "  pPIS = porcentagemPis," +
                                    "  vPIS = valorPis," +
                                    "  qBCProd = quantidadeVendidaPis," +
                                    "  vAliqProd = valorAliquotaPis " +
                                    " FROM dbo.vw_ImpostosConfiguradosProdutos where id_produto = " + ID;
                    else

                        instrucao = "SELECT " +
                                    "  CST = CodigoSituacaoTributariaPis," +
                                    "  vBC = valorBaseCalculoPis," +
                                    "  pPIS = porcentagemPis," +
                                    "  vPIS = valorPis," +
                                    "  qBCProd = quantidadeVendidaPis," +
                                    "  vAliqProd = valorAliquotaPis " +
                                    " FROM dbo.vw_ImpostosConfiguradosItens where id_item = " + ID;
                else
                    instrucao = "SELECT " +
                                "CST = CodigoSituacaoTributariaPis," +
                                "vBC = valorBaseCalculoPis," +
                                "pPIS = porcentagemPis," +
                                "vPIS = valorPis," +
                                "qBCProd = quantidadeVendidaPis," +
                                "vAliqProd = valorAliquotaPis " +
                                "From dbo.vw_ImpostosConfiguradosItens where id_item = " + ID;

            SqlDataAdapter daPIS = new SqlDataAdapter(instrucao, conn);

            DataTable dtPIS = new DataTable();
            daPIS.Fill(dtPIS);
            conn.Close();
            for (int i = 0; i < dtPIS.Rows.Count; i++)
            {
                DataRow drPIS = dtPIS.Rows[i];

                if (valorproduto != 0)
                    drPIS["vBC"] = valorproduto.ToString();

                if (drPIS["pPIS"].ToString()=="")
                    drPIS["pPIS"] = 0.00;

                if (Convert.ToDouble(drPIS["pPIS"].ToString()) != 0 && valorproduto != 0.00)
                    drPIS["vPIS"] = valorproduto * (Convert.ToDouble(drPIS["pPIS"].ToString()) / 100);

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
