using System;
using System.Collections.Generic;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using Nfe.Dominio.Entidades;
using Nfe.Infra.Configuracao;
using Nfe.Infra;

namespace Nfe.Infra.AcessoDados
{
    public class RepositorioNotaFiscal :Entidade
    {
       
        public NotaFiscal Nota { get; set; }
        private RepositorioEmitente repositorioEmitente { get; set; }
        private RepositorioClientes RepositorioCliente { get; set; }
        private RepositorioProduto RepositorioProduto { get;  set; }
        private RepositorioTransportadoras RepositorioTransportadora { get; set; }
        private NaturezaOperacao NatOperacao { get; set; }
        private FormaPagamento FPagto { get; set; }
        private ConfiguracaoApp config { get; set; }

        public RepositorioNotaFiscal()
        {
            config = new ConfiguracaoApp();
            Nota = new NotaFiscal();
            RepositorioCliente = new RepositorioClientes();
            RepositorioProduto = new RepositorioProduto();
            RepositorioTransportadora = new RepositorioTransportadoras();
            repositorioEmitente = new RepositorioEmitente();
            NatOperacao = new NaturezaOperacao();
            FPagto = new FormaPagamento();
        }

        public NotaFiscal GetById(int id, Int32 operacao)
        {
            
            Nota.SetId(id);

            ConfiguracaoApp config = new ConfiguracaoApp();
            SqlConnection conn = config.getConexaoBD();
            conn.Open();

            eTPNotaFiscal TipoNF = new eTPNotaFiscal();

            System.Threading.Thread.Sleep(500);
            Genericas Generica = new Genericas();
            TipoNF = Generica.RetornaTipoNotaFiscal(id);
            DataTable DtNotaFiscal = Generica.RetornaDTNotaFiscal(id,TipoNF);
            DataRow drProdNotaFiscal = DtNotaFiscal.Rows[0];
            
            var emit = repositorioEmitente.getDefault();
            var NaturezaDaOperacao = NatOperacao.GetById(Convert.ToInt32(drProdNotaFiscal["id_NaturezaOperacao"].ToString()));

            conn.Close();
            Nota.cUF = config.Uf; // UF de sao paulo
            string novocNF = null;
            if (id.ToString().Length < 8)
            {
                novocNF = id.ToString().PadLeft(8, '0');
            }
            Nota.cNF = novocNF.Trim();
            
            if (Convert.ToInt32(drProdNotaFiscal["NaturezaOperacaoSP"].ToString()) == 1 )//Verdadeiro
                Nota.natOp = NaturezaDaOperacao.CodSPOperacao;
            else
                Nota.natOp = NaturezaDaOperacao.CodForaSPOperacao;

            Nota.Emitente = emit;
            if (TipoNF == eTPNotaFiscal.eFaturamento)
                Nota.Cliente = RepositorioCliente.GetClienteById(Convert.ToInt32(drProdNotaFiscal["ID_Cliente"].ToString()));
            else
                Nota.Cliente = RepositorioCliente.GetClienteLocEntregaById(Convert.ToInt32(drProdNotaFiscal["ID_Cliente"].ToString()));

            if (drProdNotaFiscal["id_FormaPagamento"].ToString() == null)
            {
                throw new Exception("A forma de pagamento não foi selecionada");
            }
            else
            {
                if (TipoNF == eTPNotaFiscal.eFaturamento)
                {
                    Nota.indPag = Convert.ToInt32(FPagto.GetById(Convert.ToInt32(drProdNotaFiscal["id_FormaPagamento"].ToString())).codFormaPagtoNFE);
                }
                else
                {
                    Nota.indPag = 0;
                }
            }
            Nota.mod = 55; //modelo eltronico nfe
            Nota.serie = 1;
            Nota.nNF = drProdNotaFiscal["numeronfe"].ToString();
            Nota.dEmi = GeraDataDeEmissao();
            Nota.tpNF = Convert.ToInt32(drProdNotaFiscal["TipoNota"].ToString());
            Nota.cMunFG = emit.endereco.CodMunicipio; // mun do emitente
            Nota.tpImp = 1; // tipo A1
            Nota.tpEmis = config.tpEmiss;
            Nota.IDChave = drProdNotaFiscal["chaveacesso"].ToString();
            Nota.gerarChave(emit,operacao);
            Nota.tpAmb = config.tpAmb;
            Nota.finNFe = 1;
            Nota.procEmi = 0;
            Nota.verProc = "1.0";
            Nota.Produtos = getProdutos(Convert.ToInt32(drProdNotaFiscal["Id_NotaFiscal"].ToString()), TipoNF);
            Nota.Cliente = getCliente(Convert.ToInt32(drProdNotaFiscal["ID_Cliente"].ToString()), TipoNF);
            Nota.vBC = Convert.ToDecimal(drProdNotaFiscal["BaseCalculoICMSProduto"].ToString() == "" ? "0" : drProdNotaFiscal["BaseCalculoICMSProduto"].ToString());
            Nota.vICMS = Convert.ToDecimal(drProdNotaFiscal["ValorICMSProduto"].ToString() == "" ? "0" : drProdNotaFiscal["ValorICMSProduto"].ToString());
            Nota.vBCST = Convert.ToDecimal(drProdNotaFiscal["BaseCalculoICMSProduto"].ToString() == "" ? "0" : drProdNotaFiscal["BaseCalculoICMSProduto"].ToString());
            Nota.vST = Convert.ToDecimal(drProdNotaFiscal["BaseCalculoICMSST"].ToString() == "" ? "0" : drProdNotaFiscal["BaseCalculoICMSST"].ToString());
            Nota.vProd = Convert.ToDecimal(drProdNotaFiscal["ValorTotalProdutos"].ToString());
            Nota.vFrete = drProdNotaFiscal["ValorFrete"].ToString() == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(drProdNotaFiscal["ValorFrete"].ToString());
            Nota.vSeg = drProdNotaFiscal["ValorSeguro"].ToString() == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(drProdNotaFiscal["ValorSeguro"].ToString());
            Nota.vDesc = Convert.ToDecimal(drProdNotaFiscal["Desconto"].ToString());
            Nota.vII = Convert.ToDecimal(drProdNotaFiscal["Desconto"].ToString()); // dados.VII;
            Nota.vIPI = drProdNotaFiscal["ValorTotalIPI"].ToString() == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(drProdNotaFiscal["ValorTotalIPI"].ToString());
            Nota.vCOFINS = 0;
            Nota.vOutro = 0;
            Nota.vNF = Convert.ToDecimal(drProdNotaFiscal["ValorTotalNfe"].ToString());
            Nota.esp = drProdNotaFiscal["Especie"].ToString();
            Nota.marca = drProdNotaFiscal["Marca"].ToString();
            Nota.volume =  Convert.ToDecimal(drProdNotaFiscal["Quantidade"].ToString());
            Nota.pesoBruto = drProdNotaFiscal["PesoBruto"].ToString();
            Nota.pesoLiq = drProdNotaFiscal["PesoLiquido"].ToString();
            Nota.fatura = drProdNotaFiscal["Fatura"].ToString();
            Nota.lote = drProdNotaFiscal["Lote"].ToString();
            Nota.recibo = drProdNotaFiscal["Recibo"].ToString();
            Nota.protocolo = drProdNotaFiscal["numerodoprotocolo"].ToString();
            Nota.vPIS = 0; // colocado por Cristoffer dia 28/07/2011 provisoriamente
            if (drProdNotaFiscal["id_transportadora"].ToString() != "")
            {
                try
                {
                    Nota.PossuiTransportadora = true;
                    Nota.Transportadora = RepositorioTransportadora.getById(Convert.ToInt32(drProdNotaFiscal["id_transportadora"].ToString()));
                }
                catch (Exception ex)
                {

                    throw new Exception("Não foi possível recuperar os dados da Transportadora, "+ ex.Message);
                }               
            }
            else
            {
                Nota.PossuiTransportadora = false;
            }
             
             salvarChavenoBD(Nota.Id, Nota.IDChave);

            return Nota;
        }
        public List<Produtos> getProdutos(int idnota, eTPNotaFiscal TipoNotaFiscal)
        {
            try
            {
                return RepositorioProduto.getListaProdutosPorIdDaNota(idnota, TipoNotaFiscal);
            }
            catch (Exception ex)
            {                
                throw new Exception("Erro em produtos, "+ex.Message);
            }
            
        }
        public NFePessoa getCliente(int cliente, eTPNotaFiscal TipoNotaFiscal)
        {
            try
            {
                if (TipoNotaFiscal == eTPNotaFiscal.eFaturamento)
                    return RepositorioCliente.GetClienteById(cliente);
                else
                    return RepositorioCliente.GetClienteLocEntregaById(cliente);
            }
            catch (Exception ex)
            {

                throw new Exception("Erro nos dados de cliente "+ ex.Message);
            }
        }

        public string GeraDataDeEmissao()
        {
            string ano = DateTime.Now.Year.ToString();
            string mes = DateTime.Now.Month.ToString();
            string dia = DateTime.Now.Day.ToString();
            if (mes.Length ==1)
            {
                mes = "0" + mes;
            }
            if (dia.Length == 1)
            {
                dia = "0" + dia;
            }
            return ano+"-"+mes+"-"+dia;        
           
        }
        private string getDateNow()
        {
            var dt = DateTime.Now;
            return dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString();
        }
        private void salvarChavenoBD(int idNota, string chave)
        {
            //var nota = _model.NotasFiscais.Where(i => i.id_NotaFiscal == idNota).First();
            //nota.chave = chave;
            //_model.SaveChanges();

            ConfiguracaoApp config = new ConfiguracaoApp();
            SqlConnection conn = config.getConexaoBD();
            NatOperacao natoper = new NatOperacao();
            //Usado para fazer valer o update do dataAdapter daNota. (cristoffer 05/10/2011)
            SqlCommandBuilder cmdBuilder;
            conn.Open();

            //O update só funciona usando o SqlCommandBuilder se a consulta trouxer ao menos
            //uma coluna que seja chave primária(cristoffer 05/10/2011)
            SqlDataAdapter daNota = new SqlDataAdapter("Select id_notafiscal, chave from NotasFiscais WHERE id_NotaFiscal = " + idNota.ToString(), conn);
            DataSet dsNota = new DataSet();
            cmdBuilder = new SqlCommandBuilder(daNota);

            daNota.Fill(dsNota, "NotasFiscais");

            dsNota.Tables["NotasFiscais"].Rows[0]["chave"] = chave;

            daNota.Update(dsNota,"NotasFiscais");
        }

        public void AtualizarLote(int idNota, string loteInfo)
        {
            //var nota = _model.NotasFiscais.Where(i => i.id_NotaFiscal == idNota).First();
            //nota.Lote = loteInfo.Trim();
            //_model.SaveChanges();

            ConfiguracaoApp config = new ConfiguracaoApp();
            SqlConnection conn = config.getConexaoBD();
            NatOperacao natoper = new NatOperacao();
            SqlCommandBuilder cmdBuilder;
            conn.Open();

            SqlDataAdapter daNota = new SqlDataAdapter("Select id_notafiscal, lote from dbo.NotasFiscais WHERE id_NotaFiscal = " + idNota.ToString(), conn);
            DataSet dsNota = new DataSet();

            cmdBuilder = new SqlCommandBuilder(daNota);

            daNota.Fill(dsNota, "NotasFiscais");

            dsNota.Tables["NotasFiscais"].Rows[0]["lote"] = loteInfo.Trim();
            daNota.Update(dsNota,"NotasFiscais");
        }

        public void AtualizarRecibo(int idNota, string ReciboInfo)
        {
            //var nota = _model.NotasFiscais.Where(i => i.id_NotaFiscal == idNota).First();
            //nota.Recibo = ReciboInfo.Trim();
            //_model.SaveChanges();

            ConfiguracaoApp config = new ConfiguracaoApp();
            SqlConnection conn = config.getConexaoBD();
            NatOperacao natoper = new NatOperacao();
            SqlCommandBuilder cmdBuilder;
            conn.Open();

            SqlDataAdapter daNota = new SqlDataAdapter("Select id_notafiscal, Recibo from dbo.NotasFiscais WHERE id_NotaFiscal = " + idNota.ToString(), conn);
            DataSet dsNota = new DataSet();

            cmdBuilder = new SqlCommandBuilder(daNota);

            daNota.Fill(dsNota, "NotasFiscais");

            dsNota.Tables["NotasFiscais"].Rows[0]["Recibo"] = ReciboInfo.Trim();
            daNota.Update(dsNota,"NotasFiscais");
        }

        public void AtualizarStatus(int idNota, string statusInfo)
        {

            ConfiguracaoApp config = new ConfiguracaoApp();
            SqlConnection conn = config.getConexaoBD();
            SqlCommandBuilder cmdBuilder;
            conn.Open();

            SqlDataAdapter daNota = new SqlDataAdapter("Select id_notafiscal, Status from NotasFiscais WHERE id_NotaFiscal = " + idNota.ToString(), conn);
            DataSet dsNota = new DataSet();

            cmdBuilder = new SqlCommandBuilder(daNota);

            daNota.Fill(dsNota, "NotasFiscais");

            dsNota.Tables["NotasFiscais"].Rows[0]["Status"] = statusInfo.Trim();
            daNota.Update(dsNota,"NotasFiscais");
        }

        public void AtualizarProtocolo(int idNota, string prot)
        {
            //var nota = _model.NotasFiscais.Where(i => i.id_NotaFiscal == idNota).First();
            //nota.Protocolo = prot;
            //_model.SaveChanges();
            ConfiguracaoApp config = new ConfiguracaoApp();
            SqlConnection conn = config.getConexaoBD();
            NatOperacao natoper = new NatOperacao();
            SqlCommandBuilder cmdBuilder;
            conn.Open();

            SqlDataAdapter daNota = new SqlDataAdapter("Select id_notafiscal, Protocolo from NotasFiscais WHERE id_NotaFiscal = " + idNota.ToString(), conn);
            DataSet dsNota = new DataSet();

            cmdBuilder = new SqlCommandBuilder(daNota);

            daNota.Fill(dsNota, "NotasFiscais");

            dsNota.Tables["NotasFiscais"].Rows[0]["Protocolo"] = prot.Trim();
            daNota.Update(dsNota,"NotasFiscais");
        }

        public void AtualizarConfirmaEnvio(int idNota)
        {
            //var nota = _model.NotasFiscais.Where(i => i.id_NotaFiscal == idNota).First();
            //nota.ConfirmaEnvioNFE = 1;
            //nota.dataEnvioNFE = GetDate();
            //_model.SaveChanges();

            ConfiguracaoApp config = new ConfiguracaoApp();
            SqlConnection conn = config.getConexaoBD();
            NatOperacao natoper = new NatOperacao();
            SqlCommandBuilder cmdBuilder;
            conn.Open();

            SqlDataAdapter daNota = new SqlDataAdapter("Select id_notafiscal, ConfirmaEnvioNFE,dataEnvioNFE from dbo.NotasFiscais WHERE id_NotaFiscal = " + idNota.ToString(), conn);
            DataSet dsNota = new DataSet();
            cmdBuilder = new SqlCommandBuilder(daNota);

            daNota.Fill(dsNota, "NotasFiscais");

            dsNota.Tables["NotasFiscais"].Rows[0]["ConfirmaEnvioNFE"] = 1;
            dsNota.Tables["NotasFiscais"].Rows[0]["DataEnvioNFE"] = GetDate();
            daNota.Update(dsNota,"NotasFiscais");
            conn.Close();
        }

        public void AtualizarConfirmaCancelamento(int idNota, string justificativa, string Data = "")
        {

            ConfiguracaoApp config = new ConfiguracaoApp();
            SqlConnection conn = config.getConexaoBD();
            SqlCommandBuilder cmdBuilder;
            conn.Open();

            SqlDataAdapter daNota = new SqlDataAdapter("Select id_notafiscal, ConfirmaEnvioNFE,dataCancelamentoNFE,xJust from dbo.NotasFiscais WHERE id_NotaFiscal = " + idNota.ToString(), conn);
            DataSet dsNota = new DataSet();
            cmdBuilder = new SqlCommandBuilder(daNota);

            daNota.Fill(dsNota, "NotasFiscais");

            dsNota.Tables["NotasFiscais"].Rows[0]["ConfirmaEnvioNFE"] = 2;
            dsNota.Tables["NotasFiscais"].Rows[0]["DataCancelamentoNFE"] = Data == "" ? GetDate() : Convert.ToDateTime(Data);
            dsNota.Tables["NotasFiscais"].Rows[0]["xJust"] = justificativa.Trim();
            daNota.Update(dsNota, "NotasFiscais");
            conn.Close();
        }

        public DateTime GetDate()
        {
            
            DateTime data = new DateTime();

            ConfiguracaoApp config = new ConfiguracaoApp();
            SqlConnection conn = config.getConexaoBD();
            conn.Open();

            SqlDataAdapter daDataHora = new SqlDataAdapter("Select getdate() as DataHora", conn);
            DataTable dtDataHora = new DataTable();

            //preenche o datatable
            daDataHora.Fill(dtDataHora);
            DataRow drDataHora = dtDataHora.Rows[0];

            data = Convert.ToDateTime(drDataHora["DataHora"].ToString());

            return data;
        } 
    }
}
