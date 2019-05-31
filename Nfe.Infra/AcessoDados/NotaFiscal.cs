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
            
            //Por causa do cliente PANINI houve a solicitação de imprimir a descrição ao invés do código
            //if (Convert.ToInt32(drProdNotaFiscal["NaturezaOperacaoSP"].ToString()) == 1 )//Verdadeiro
            //    Nota.natOp = NaturezaDaOperacao.CodSPOperacao;
            //else
            //    Nota.natOp = NaturezaDaOperacao.CodForaSPOperacao;

            Nota.natOp = NaturezaDaOperacao.Descricao;

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
                    Nota.tPag = FPagto.GetById(Convert.ToInt32(drProdNotaFiscal["id_FormaPagamento"].ToString())).codMeioPagamento.ToString();
                }
                else
                {
                    Nota.indPag = 0;
                    Nota.tPag = "01";
                }
            }

            //string tzd = "-03:00"; // UCT - Universal Coordinated Time (Normal)
            string tzd = "-02:00"; // UCT - Universal Coordinated Time (No horário de verão)

            Nota.mod = 55; //modelo eltronico nfe
            Nota.serie = 1;
            Nota.nNF = drProdNotaFiscal["numeronfe"].ToString();
            Nota.dEmi = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + tzd; //GetDate();
            //Nota.dEmi = dtProdNotaFis
            Nota.tpNF = Convert.ToInt32(drProdNotaFiscal["TipoNota"].ToString());
            Nota.cMunFG = emit.endereco.CodMunicipio; // mun do emitente
            Nota.tpImp = 1; // tipo A1 - DANFE normal, Retrato;
            Nota.tpEmis = config.tpEmiss;
            Nota.IDChave = drProdNotaFiscal["chaveacesso"].ToString();
            Nota.gerarChave(emit,operacao);
            Nota.tpAmb = config.tpAmb;

            if (CFOPDevolucao(Nota.Id) == true)
                Nota.finNFe = 4; // 4 - Devolução de mercadoria
            else
                Nota.finNFe = 1; // 1 - NFE Normal

            Nota.procEmi = 0;
            Nota.verProc = "1.0";
            Nota.indFinal = 0;
            Nota.indPres = 0;

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
            Nota.infadicionais = getInfAdicionaisNFE(id).Replace("-"," ").Replace("\\n","").Replace("\\","").Replace(" – ","");
            Nota.vPIS = 0;
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

            if (drProdNotaFiscal["id_fechamento"].ToString() != "")
                Nota.idfechamento = Convert.ToInt32(drProdNotaFiscal["id_fechamento"].ToString());
            else
                Nota.idfechamento = 0;
             
             salvarChavenoBD(Nota.Id, Nota.IDChave);

            return Nota;
        }

        private bool CFOPDevolucao(int id_notafiscal)
        {

            ConfiguracaoApp config = new ConfiguracaoApp();
            SqlConnection conn = config.getConexaoBD();
            NatOperacao natoper = new NatOperacao();

            string instrucao = "";
            int NaturezaOperacaoRefDevolucaoMercadoria = 0;

            conn.Open();

            instrucao = " SELECT cfopDevMercadoria ";
            instrucao += " FROM gerenciamento_comum.dbo.NaturOperacoes N ";
            instrucao += "      inner join NotasFiscais NF ";
            instrucao += "              on N.Id_NaturOper = NF.id_NaturezaOperacao ";
            instrucao += " WHERE NF.id_NotaFiscal = " + id_notafiscal.ToString();

            SqlDataAdapter daNota = new SqlDataAdapter(instrucao, conn);

            DataTable dtNota = new DataTable();

            daNota.Fill(dtNota); 
            conn.Close();

            if (dtNota.Rows.Count != 0)
            {
                DataRow drNota = dtNota.Rows[0];
                NaturezaOperacaoRefDevolucaoMercadoria = int.Parse(drNota["cfopDevMercadoria"].ToString());
            }

            if (NaturezaOperacaoRefDevolucaoMercadoria == 1)
                return true;
            else
                return false;
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
            conn.Close();
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
            conn.Close();
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
            conn.Close();
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
            conn.Close();
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
            conn.Close();
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

        private string getInfAdicionaisNFE(int idNota) 
        {
            string DadosAdicionais = "";
            ConfiguracaoApp config = new ConfiguracaoApp(false);
            SqlConnection conn = config.getConexaoBD();
            conn.Open();

            SqlDataAdapter daNota = new SqlDataAdapter(" Select DadosAdicionais1, DadosAdicionais2, DadosAdicionais3, DadosAdicionais4, " +
                                                       "        DadosAdicionais5, DadosAdicionais6, DadosAdicionais7 " + 
                                                       " from dbo.NotasFiscais WHERE id_NotaFiscal = " + idNota.ToString(), conn);
            DataTable dtNota = new DataTable();

            //preenche o datatable
            daNota.Fill(dtNota);
            conn.Close();

            if (dtNota.Rows.Count != 0)
            {

                DataRow drNota = dtNota.Rows[0];

                if (drNota["DadosAdicionais1"].ToString() != "")
                    DadosAdicionais = drNota["DadosAdicionais1"].ToString();
                if (drNota["DadosAdicionais2"].ToString() != "")
                    DadosAdicionais += " " + drNota["DadosAdicionais2"].ToString();
                if (drNota["DadosAdicionais3"].ToString() != "")
                    DadosAdicionais += " " + drNota["DadosAdicionais3"].ToString();
                if (drNota["DadosAdicionais4"].ToString() != "")
                    DadosAdicionais += " " + drNota["DadosAdicionais4"].ToString();
                if (drNota["DadosAdicionais5"].ToString() != "")
                    DadosAdicionais += " " + drNota["DadosAdicionais5"].ToString();
                if (drNota["DadosAdicionais6"].ToString() != "")
                    DadosAdicionais += " " + drNota["DadosAdicionais6"].ToString();
                if (drNota["DadosAdicionais7"].ToString() != "")
                    DadosAdicionais += " " + drNota["DadosAdicionais7"].ToString();
            }
            return DadosAdicionais;
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
            conn.Close();
            return data;
        } 
    }
}
