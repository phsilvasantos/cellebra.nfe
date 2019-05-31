using System;
using System.Collections.Generic;
using System.Xml;
using Nfe.Dominio.Entidades;
using Nfe.Infra.Configuracao;
using System.Data;
using System.Data.SqlClient;

namespace Nfe.Infra.AcessoDados
{
    public class RepositorioProduto
    {

        List<XmlElement> listaProdutosXML { get; set; }

        public List<Produtos> getListaProdutosPorIdDaNota(int id, eTPNotaFiscal TipoNotaFiscal)
        {
            try
            {
                List<Produtos> lista = new List<Produtos>();

                Genericas Generica = new Genericas();

                DataTable dtProdNotaFiscal = Generica.RetornaDTNotaFiscal(id, TipoNotaFiscal);

                
                char[] Ponto = { '.'};
                for (int i = 0;i < dtProdNotaFiscal.Rows.Count;i++)
                {
                    RepositorioItemSimplesNacional simples = new RepositorioItemSimplesNacional();
                    RepositorioItemCOFINS cofins = new RepositorioItemCOFINS();
                    RepositorioItemCOFINSST confinsST = new RepositorioItemCOFINSST();
                    RepositorioItemPIS pis = new RepositorioItemPIS();
                    RepositorioItemPISST pisST = new RepositorioItemPISST();
                    Produtos prod = new Produtos();

                    DataRow drProdNotaFiscal = dtProdNotaFiscal.Rows[i];

                    //var prd = _model.Produtos.Where(e => e.id_Produto == item.id_produto).First();
                    prod.SetId(Convert.ToInt32(drProdNotaFiscal["id_produto"].ToString()));
                    prod.IdNota = Convert.ToInt32(drProdNotaFiscal["id_produto"].ToString());
                    try
                    {
                        prod.cProd = drProdNotaFiscal["codigo"].ToString() == "" ? 0 : Convert.ToInt32(drProdNotaFiscal["codigo"].ToString().Replace(".", ""));
                    }
                    catch (Exception)
                    {
                        prod.cProd = 0;
                    }

                    prod.xProd = drProdNotaFiscal["descricao"].ToString();
                    prod.NCM = drProdNotaFiscal["ncmsh"].ToString(); ;// desde 01/2010 passou a ser obrigatório no xml o NCM. Verificar
                    prod.CFOP = Convert.ToInt32(drProdNotaFiscal["CFOP"].ToString());
                    prod.uCom = drProdNotaFiscal["un"].ToString();
                    prod.qCom = Convert.ToDecimal(drProdNotaFiscal["quantidadeItem"].ToString());
                    prod.vUnCom = Convert.ToDecimal(drProdNotaFiscal["ValorUnitario"].ToString());
                    prod.vProd = Convert.ToDecimal(drProdNotaFiscal["ValorTotal"].ToString());
                    prod.uTrib = prod.uCom;
                    prod.qTrib = prod.qCom;
                    prod.vUnTrib = prod.vUnCom;
                    prod.indTot = 1;

                    if (drProdNotaFiscal["DescricaoTipoNota"].ToString().Equals("Saída"))
                    {
                        prod.SimpNac = simples.getById(Convert.ToInt32(drProdNotaFiscal["id_item"].ToString()), TipoNotaFiscal,drProdNotaFiscal["DescricaoTipoNota"].ToString());
                        prod.COFINS = cofins.getById(Convert.ToInt32(drProdNotaFiscal["id_item"].ToString()), TipoNotaFiscal, drProdNotaFiscal["DescricaoTipoNota"].ToString());
                        prod.COFINSST = confinsST.getById(Convert.ToInt32(drProdNotaFiscal["id_item"].ToString()), TipoNotaFiscal, drProdNotaFiscal["DescricaoTipoNota"].ToString());
                        prod.PIS = pis.getById(Convert.ToInt32(drProdNotaFiscal["id_item"].ToString()), TipoNotaFiscal, drProdNotaFiscal["DescricaoTipoNota"].ToString());
                        prod.PISST = pisST.getById(Convert.ToInt32(drProdNotaFiscal["id_item"].ToString()), TipoNotaFiscal, drProdNotaFiscal["DescricaoTipoNota"].ToString());
                    }
                    else
                    {
                        string codigoproduto;
                        if (drProdNotaFiscal["id_produto"] != drProdNotaFiscal["id_ProdutoOficial"] && !String.IsNullOrEmpty(drProdNotaFiscal["id_ProdutoOficial"].ToString()))
                            codigoproduto = drProdNotaFiscal["id_ProdutoOficial"].ToString();
                        else
                            codigoproduto = drProdNotaFiscal["id_produto"].ToString();

                        prod.SimpNac = simples.getById(Convert.ToInt32(codigoproduto), TipoNotaFiscal, drProdNotaFiscal["DescricaoTipoNota"].ToString(),0, drProdNotaFiscal["OrigemProduto"].ToString());
                        prod.COFINS = cofins.getById(Convert.ToInt32(codigoproduto), TipoNotaFiscal, drProdNotaFiscal["DescricaoTipoNota"].ToString(), 0, drProdNotaFiscal["OrigemProduto"].ToString(), Convert.ToDouble(drProdNotaFiscal["ValorTotal"].ToString()));
                        prod.COFINSST = confinsST.getById(Convert.ToInt32(codigoproduto), TipoNotaFiscal, drProdNotaFiscal["DescricaoTipoNota"].ToString(), 0, drProdNotaFiscal["OrigemProduto"].ToString(), Convert.ToDouble(drProdNotaFiscal["ValorTotal"].ToString()));
                        prod.PIS = pis.getById(Convert.ToInt32(codigoproduto), TipoNotaFiscal, drProdNotaFiscal["DescricaoTipoNota"].ToString(), 0, drProdNotaFiscal["OrigemProduto"].ToString(), Convert.ToDouble(drProdNotaFiscal["ValorTotal"].ToString()));
                        prod.PISST = pisST.getById(Convert.ToInt32(codigoproduto), TipoNotaFiscal, drProdNotaFiscal["DescricaoTipoNota"].ToString(), 0, drProdNotaFiscal["OrigemProduto"].ToString(), Convert.ToDouble(drProdNotaFiscal["ValorTotal"].ToString()));
                    }
                    prod.vFrete = drProdNotaFiscal["valorFreteProduto"].ToString() == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(drProdNotaFiscal["valorFreteProduto"].ToString());
                    lista.Add(prod);
                    //System.Threading.Thread.Sleep(200);
                }
                return lista;
            }
            catch (Exception ex)
            {                
                throw new Exception("Erro em Rep Produtos: "+ex.Message);
            }
            
        }
        public string getUnitMedida(int id)
        {
            ConfiguracaoApp config = new ConfiguracaoApp();
            SqlConnection conn = config.getConexaoBD();

            conn.Open();

            string AbreviacaoUnidMedida;
            string instrucao = "SELECT AbrUnid FROM gerenciamento_comum.dbo.Tab_UnidMedida WHERE id_unid = " + id.ToString();

            //define adaptador para produtos
            SqlDataAdapter daUnidMedida = new SqlDataAdapter(instrucao, conn);
            DataTable dtUnidMedida = new DataTable();

            //preenche o datatable
            daUnidMedida.Fill(dtUnidMedida);
            DataRow drUnidMedida = dtUnidMedida.Rows[1];
            if (dtUnidMedida.Rows.Count != 0)
                AbreviacaoUnidMedida = "";
            else
                AbreviacaoUnidMedida = drUnidMedida["AbrUnid"].ToString();

            conn.Close();
            return AbreviacaoUnidMedida;
        }
    }
}
