using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using Nfe.Infra.Configuracao;

namespace Nfe.Infra
{
    public enum eTPNotaFiscal { eFaturamento = 1, eExpedicao = 2, eExpedItens = 3, eSaidaEstoque = 4, eSaidaEstoqueTitulos = 5 };

    public class Genericas
    {
        public DataTable RetornaDTNotaFiscal(int id,eTPNotaFiscal TipoNotaFiscal)
        {
            ConfiguracaoApp config = new ConfiguracaoApp(false);
            SqlConnection conn = config.getConexaoBD();
            DataTable dtNotaFiscal = new DataTable(); ;
            string instrucao = "";

            conn.Open();
            if (TipoNotaFiscal == eTPNotaFiscal.eFaturamento)
                instrucao = " SELECT " +
                            "FP.id_produto," +
                            "FP.id_ProdutoOficial," +
                            "FP.id_notafiscal," +
                            "FP.numeronfe," +
                            "FP.id_NaturezaOperacao," +
                            "FP.NaturezaOperacaoSP," +
                            "FP.Fatura," +
                            "FP.id_item," +
                            "FP.NaturezaOperacao," +
                            "FP.chaveacesso," +
                            "FP.flentradasaida," +
                            "FP.numerodoprotocolo," +
                            "FP.FretePorConta," +
                            "FP.DescricaoTipoNota," +
                            "FP.DataLimiteEmissao," +
                            "FP.ConfirmaEnvioNFE," +
                            "FP.DadosAdicionais1," +
                            "FP.DadosAdicionais2," +
                            "FP.DadosAdicionais3," +
                            "FP.DadosAdicionais4," +
                            "FP.DadosAdicionais5," +
                            "FP.vDesc," +
                            "FP.VII," +
                            "FP.Lote," +
                            "FP.Recibo," +
                            "FP.InscEstSubsTrib," +
                            "FP.id_FormaPagamento," +
                            "FP.nomeemitente," +
                            "FP.enderecoemitente," +
                            "FP.cepemitente," +
                            "FP.cidadeemitente," +
                            "FP.ufemitente," +
                            "FP.foneemitente," +
                            "FP.ieemitente," +
                            "FP.cnpjemitente," +
                            "FP.iesubstitutotribemitente," +
                            "FP.id_cliente," +
                            "FP.nomedestinatario," +
                            "FP.cnpjdestinatario," +
                            "FP.enderecodestinatario," +
                            "FP.bairrodestinatario," +
                            "FP.cidadedestinatario," +
                            "FP.ufdestinatario," +
                            "FP.cepdestinatario," +
                            "FP.iedestinatario," +
                            "FP.fonedestinatario," +
                            "FP.RGDestinatario," +
                            "FP.dtemissao," +
                            "FP.dtsaida," +
                            "FP.hrsaida," +
                            "FP.numeronfe," +
                            "FP.serienfe," +
                            "FP.TipoNota," +
                            "FP.iesubstitutotrib," +
                            "FP.basecalculoicms," +
                            "FP.valoricms," +
                            "FP.basecalculoicmsst," +
                            "FP.valoricmsst," +
                            "SUM(F.valortotal) as valortotalprodutos," +
                            "FP.valorfrete," +
                            "FP.valorseguro," +
                            "FP.desconto," +
                            "FP.outrasdespesasacessorias," +
                            "FP.valortotalipi," +
                            "SUM(F.valortotal) as valortotalnfe," +//"FP.valortotalnfe," +
                            "FP.origemNFE," +
                            "FP.id_ExpedItens," +
                            "FP.id_notaexped," +
                            "FP.id_fechamento," +
                            "FP.codigoantt," +
                            "FP.nometransportador," +
                            "FP.cnpjtransportador," +
                            "FP.enderecotransportador," +
                            "FP.cidadetransportador," +
                            "FP.ceptransportador," +
                            "FP.uftransportador," +
                            "FP.ietransportador," +
                            "FP.quantidade," +
                            "FP.especie," +
                            "FP.marca," +
                            "FP.numero," +
                            "FP.pesobruto," +
                            "FP.pesoliquido," +
                            "FP.flfrete," +
                            "FP.placaveiculo," +
                            "FP.ufplacaveiculo," +
                            "FP.id_transportadora," +
                            "FP.codigo," +
                            "FP.descricao," +
                            "FP.ncmsh," +
                            "FP.csticms," +
                            "FP.CFOP," +
                            "FP.un," +
                            "FP.quantidadeItem," +
                            "FP.valorunitario," +
                            "FP.valortotal," +
                            "FP.basecalculoicmsproduto," +
                            "FP.valoricmsproduto," +
                            "FP.valoripi," +
                            "FP.aliquotaicms," +
                            "FP.aliquotaipi," +
                            "FP.valorFreteProduto," +
                            "FP.informacoescmpl01," +
                            "FP.chaveAcessoContingencia," +
                            "FP.marcador, " +
                            "FP.OrigemProduto " +
                            " FROM dbo.vw_DanfeFechamento FP" +
                            " INNER JOIN dbo.vw_DanfeFechamento F " +
                            "   on FP.id_notafiscal = F.id_notafiscal " +
                            " WHERE FP.id_notafiscal = " + id.ToString() +
                            " group by " +
                            "FP.id_produto," +
                            "FP.id_ProdutoOficial," +
                            "FP.id_notafiscal," +
                            "FP.numeronfe," +
                            "FP.id_NaturezaOperacao," +
                            "FP.NaturezaOperacaoSP," +
                            "FP.Fatura," +
                            "FP.id_item," +
                            "FP.NaturezaOperacao," +
                            "FP.chaveacesso," +
                            "FP.flentradasaida," +
                            "FP.numerodoprotocolo," +
                            "FP.FretePorConta," +
                            "FP.DescricaoTipoNota," +
                            "FP.DataLimiteEmissao," +
                            "FP.ConfirmaEnvioNFE," +
                            "FP.DadosAdicionais1," +
                            "FP.DadosAdicionais2," +
                            "FP.DadosAdicionais3," +
                            "FP.DadosAdicionais4," +
                            "FP.DadosAdicionais5," +
                            "FP.vDesc," +
                            "FP.VII," +
                            "FP.Lote," +
                            "FP.Recibo," +
                            "FP.InscEstSubsTrib," +
                            "FP.id_FormaPagamento," +
                            "FP.nomeemitente," +
                            "FP.enderecoemitente," +
                            "FP.cepemitente," +
                            "FP.cidadeemitente," +
                            "FP.ufemitente," +
                            "FP.foneemitente," +
                            "FP.ieemitente," +
                            "FP.cnpjemitente," +
                            "FP.iesubstitutotribemitente," +
                            "FP.id_cliente," +
                            "FP.nomedestinatario," +
                            "FP.cnpjdestinatario," +
                            "FP.enderecodestinatario," +
                            "FP.bairrodestinatario," +
                            "FP.cidadedestinatario," +
                            "FP.ufdestinatario," +
                            "FP.cepdestinatario," +
                            "FP.iedestinatario," +
                            "FP.fonedestinatario," +
                            "FP.RGDestinatario," +
                            "FP.dtemissao," +
                            "FP.dtsaida," +
                            "FP.hrsaida," +
                            "FP.numeronfe," +
                            "FP.serienfe," +
                            "FP.TipoNota," +
                            "FP.iesubstitutotrib," +
                            "FP.basecalculoicms," +
                            "FP.valoricms," +
                            "FP.basecalculoicmsst," +
                            "FP.valoricmsst," +
                            "F.valortotalprodutos," +
                            "FP.valorfrete," +
                            "FP.valorseguro," +
                            "FP.desconto," +
                            "FP.outrasdespesasacessorias," +
                            "FP.valortotalipi," +
                            "FP.valortotalnfe," +
                            "FP.origemNFE," +
                            "FP.id_ExpedItens," +
                            "FP.id_notaexped," +
                            "FP.id_fechamento," +
                            "FP.codigoantt," +
                            "FP.nometransportador," +
                            "FP.cnpjtransportador," +
                            "FP.enderecotransportador," +
                            "FP.cidadetransportador," +
                            "FP.ceptransportador," +
                            "FP.uftransportador," +
                            "FP.ietransportador," +
                            "FP.quantidade," +
                            "FP.especie," +
                            "FP.marca," +
                            "FP.numero," +
                            "FP.pesobruto," +
                            "FP.pesoliquido," +
                            "FP.flfrete," +
                            "FP.placaveiculo," +
                            "FP.ufplacaveiculo," +
                            "FP.id_transportadora," +
                            "FP.codigo," +
                            "FP.descricao," +
                            "FP.ncmsh," +
                            "FP.csticms," +
                            "FP.CFOP," +
                            "FP.un," +
                            "FP.quantidadeItem," +
                            "FP.valorunitario," +
                            "FP.valortotal," +
                            "FP.basecalculoicmsproduto," +
                            "FP.valoricmsproduto," +
                            "FP.valoripi," +
                            "FP.aliquotaicms," +
                            "FP.aliquotaipi," +
                            "FP.valorFreteProduto," +
                            "FP.informacoescmpl01," +
                            "FP.chaveAcessoContingencia," +
                            "FP.marcador, " +
                            "FP.OrigemProduto ";
            else if (TipoNotaFiscal == eTPNotaFiscal.eExpedicao)
                instrucao = " SELECT * FROM vw_DanfeExpedicao " +
                            " WHERE id_notafiscal = " + id.ToString();
            else if (TipoNotaFiscal == eTPNotaFiscal.eExpedItens)
                instrucao = " SELECT * FROM vw_DanfeExpedicaoItens " +
                            " WHERE id_notafiscal = " + id.ToString();
            else if (TipoNotaFiscal == eTPNotaFiscal.eSaidaEstoque)
                instrucao = " SELECT * FROM vw_DanfeSaidaDeEstoque " +
                            " WHERE id_notafiscal = " + id.ToString();
            else if (TipoNotaFiscal == eTPNotaFiscal.eSaidaEstoqueTitulos)
                instrucao = " SELECT * FROM vw_DanfeSaidaDeEstoqueTitulos " +
                            " WHERE id_notafiscal = " + id.ToString();

            SqlDataAdapter daProdNotaFiscal = new SqlDataAdapter(instrucao, conn);

            daProdNotaFiscal.Fill(dtNotaFiscal);
            conn.Close();

            return dtNotaFiscal;
        }

        public eTPNotaFiscal RetornaTipoNotaFiscal(int id)
        {
            ConfiguracaoApp config = new ConfiguracaoApp(false);
            SqlConnection conn = config.getConexaoBD();
            DataTable dtNotaFiscal = new DataTable();
            eTPNotaFiscal Tipo = new eTPNotaFiscal();
            conn.Open();

            SqlDataAdapter daProdNotaFiscal = new SqlDataAdapter("SELECT OrigemNFE from notasfiscais WHERE id_notafiscal = " + id.ToString(), conn);
            daProdNotaFiscal.Fill(dtNotaFiscal);
            if (dtNotaFiscal.Rows[0]["OrigemNFE"].Equals("Expedicao"))
                Tipo = eTPNotaFiscal.eExpedicao;
            else if (dtNotaFiscal.Rows[0]["OrigemNFE"].Equals("Fechamento"))
                Tipo = eTPNotaFiscal.eFaturamento;
            else if (dtNotaFiscal.Rows[0]["OrigemNFE"].Equals("ExpedItens"))
                Tipo = eTPNotaFiscal.eExpedItens;
            else
                Tipo = eTPNotaFiscal.eSaidaEstoque;

            conn.Close();

            return Tipo;
        }
    }
}
