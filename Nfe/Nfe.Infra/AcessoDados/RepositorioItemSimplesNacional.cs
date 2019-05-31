using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Nfe.Dominio.Entidades;
using Nfe.Infra.Configuracao;

namespace Nfe.Infra.AcessoDados
{
    public class RepositorioItemSimplesNacional
    {
        public List<ItemSimplesNacional> listItem;

        public RepositorioItemSimplesNacional()
        {
            listItem = new List<ItemSimplesNacional>();
        }

        public List<ItemSimplesNacional> getById(int ID,eTPNotaFiscal TPNotaFiscal, string TipoNota, int IDFechamento = 0 )
        {

            ConfiguracaoApp config = new ConfiguracaoApp();
            SqlConnection conn = config.getConexaoBD();
            conn.Open();

            string instrucao="";

            //if(TPNotaFiscal == eTPNotaFiscal.eFaturamento)
            //    instrucao = " SELECT * FROM vw_danfefechamento WHERE id_fechamento = " + IDFechamento.ToString() + " AND id_item = ";
            //else if(TPNotaFiscal == eTPNotaFiscal.eExpedicao)
            //    instrucao = " SELECT * FROM vw_danfeexpedicao WHERE id_item = ";
            //else if(TPNotaFiscal == eTPNotaFiscal.eExpedItens)
            //    instrucao = " SELECT * FROM vw_danfeexpedicaoitens WHERE id_item = ";
            //else if(TPNotaFiscal == eTPNotaFiscal.eSaidaEstoque)
            //    instrucao = " SELECT * FROM vw_danfesaidaestoque WHERE id_saidaIteTit = ";

            if (TipoNota.Equals("Saída"))
            {
                instrucao = "SELECT * From dbo.NFESimpNacional where id_expedTiposProd = " + ID;
            }
            else
            {
                instrucao = "SELECT " +
                            "CSOSN = situacaoTributariaIcms," +
                            "orig = origem," +
                            "pCredSN = aliquotaAplicavelCalculoCreditoIcms," +
                            "vCredICMSSN = valorCreditoIcmsAproveitado," +
                            "modBCST = modalidadeBaseCalculoIcmsST," +
                            "pMVAST = porcentagemMargemValorAdicionalIcmsST," +
                            "pRedBCST = porcentagemReducaoBaseCalculoIcmsSt," +
                            "vBCST = valorBaseCalculoIcmsST," +
                            "pICMSST = aliquotaIcmsST," +
                            "vICMSST = valorIcmsST," +
                            "vBCSTRet = valorBaseCalculoIcmsSTRetido," +
                            "vICMSSTRet = valorIcmsStRetido," +
                            "modBC = modalidadeBaseCalculoIcms," +
                            "vBC = valorBaseCalculoIcms," +
                            "pRedBC = porcentagemReducaoBaseCalculoIcms," +
                            "pICMS = aliquotaIcms," +
                            "MotDesICMS = MotivoDesoneracaoICMS," +
                            "vICMS = valorIcms " +
                            "From dbo.vw_ImpostosConfiguradosItens where id_item = " + ID;
            }
            SqlDataAdapter daSimples = new SqlDataAdapter(instrucao, conn);
            DataTable dtSimples = new DataTable();
            daSimples.Fill(dtSimples);
            conn.Close();

            for (int i = 0; i < dtSimples.Rows.Count; i++)
            {
                DataRow drSimples = dtSimples.Rows[i];
                listItem.Add(new ItemSimplesNacional(drSimples["CSOSN"].ToString() == "" ? Convert.ToInt32("0") : Convert.ToInt32(drSimples["CSOSN"].ToString()),
                                                     drSimples["orig"].ToString() == "" ? Convert.ToInt32("0") : Convert.ToInt32(drSimples["orig"].ToString()),
                                                     drSimples["pCredSN"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drSimples["pCredSN"].ToString()),
                                                     drSimples["vCredICMSSN"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drSimples["vCredICMSSN"].ToString()),
                                                     drSimples["modBCST"].ToString() == "" ? Convert.ToInt32("0") : Convert.ToInt32(drSimples["modBCST"].ToString()),
                                                     drSimples["pMVAST"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drSimples["pMVAST"].ToString()),
                                                     drSimples["pRedBCST"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drSimples["pRedBCST"].ToString()),
                                                     drSimples["vBCST"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drSimples["vBCST"].ToString()),
                                                     drSimples["pICMSST"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drSimples["pICMSST"].ToString()),
                                                     drSimples["vICMSST"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drSimples["vICMSST"].ToString()),
                                                     drSimples["vBCSTRet"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drSimples["vBCSTRet"].ToString()),
                                                     drSimples["vICMSSTRet"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drSimples["vICMSSTRet"].ToString()),
                                                     drSimples["modBC"].ToString() == "" ? Convert.ToInt32("0") : Convert.ToInt32(drSimples["modBC"].ToString()),
                                                     drSimples["vBC"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drSimples["vBC"].ToString()),
                                                     drSimples["pRedBC"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drSimples["pRedBC"].ToString()),
                                                     drSimples["pICMS"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drSimples["pICMS"].ToString()),
                                                     drSimples["vICMS"].ToString() == "" ? Convert.ToDecimal("0.00") : Convert.ToDecimal(drSimples["vICMS"].ToString()),
                                                     drSimples["motDesICMS"].ToString() == "" ? Convert.ToInt32("0") : Convert.ToInt32(drSimples["motDesICMS"].ToString())));

            }
            return listItem;
        }
    }
}
