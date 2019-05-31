using System;
using System.Threading;
using System.IO;
using System.Timers;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using Nfe.Infra.Configuracao;
using Nfe.Infra.AcessoDados;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;


namespace Nfe.Infra
{
    public class GerarCartaCorrecao
    {
        public int IdNota { get; set; }
        public string versao { get; set; }
        public string idLote { get; set; }
        public string Evento { get; set; }
        public string VersaoEvento { get; set; }
        public string infEvento { get; set; }
        public string Id { get; set; }
        public string cOrgao { get; set; }
        public int tpAmb { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string chave { get; set; }
        public DateTime dhEvento { get; set; }
        public string tzd { get; set; }
        public string tpEvento { get; set; }
        public int nSeqEvento { get; set; }
        public string verEvento { get; set; }
        public string detEvento { get; set; }
        public string versaoCartaCorrecao { get; set; }
        public string descEvento { get; set; }
        public string xCorrecao { get; set; }
        public string xCondUso { get; set; }
        public string nProt { get; set; }
        
        private XmlDocument doc { get; set; }
        public string PathDestinoXML;
        public string PathRetornoXML;
        public string PathRetornoXMLErr;
        public string resposta { get; set; }
        public string nomearquivo { get; set; }

        public GerarCartaCorrecao(int idNota, string chave, string codigoEstado, string cnpj)
        {
            this.IdNota = idNota;
            ConfiguracaoApp config = new ConfiguracaoApp();
            this.versao = "1.00";
            this.chave = chave;
            this.cOrgao = codigoEstado; //Código da UF da tabela do IBGE, que temos na tabela municipios do sistema
            this.CNPJ = cnpj;
            this.tpAmb = config.tpAmb;
            this.dhEvento = GetDate();
            //this.tzd = "-03:00"; // UCT - Universal Coordinated Time (Horário Normal)
            this.tzd = "-02:00"; // UCT - Universal Coordinated Time (Horário de Verão)
            this.tpEvento = "110110";
            this.doc = new XmlDocument();
            this.nSeqEvento = NumeroSequenciaEvento(idNota);
            this.descEvento = "Carta de Correcao";
            this.xCondUso = "A Carta de Correcao e disciplinada pelo paragrafo " +
                            "1o-A do art. 7o do Convenio S/N, de 15 de dezembro de 1970 " +
                            "e pode ser utilizada para regularizacao de erro ocorrido na emissao " +
                            "de documento fiscal, desde que o erro nao esteja relacionado com: " +
                            "I - as variaveis que determinam o valor do imposto tais como: base de calculo, " +
                            "aliquota, diferenca de preco, quantidade, valor da operacao ou da prestacao; " +
                            "II - a correcao de dados cadastrais que implique mudanca do remetente ou do destinatario; " +
                            "III - a data de emissao ou de saida.";
            this.idLote = RetornaNumeroLote().ToString();
            this.verEvento = "1.00";
            this.nomearquivo = chave;
            this.PathDestinoXML = config.PastaXmlEnvio + "\\" + nomearquivo + "-" + this.nSeqEvento + "-env-cce.xml";
            this.PathRetornoXML = config.PastaXmlRetorno + "\\"+ nomearquivo + "-" + this.nSeqEvento + "-ret-env-cce.xml";
            this.PathRetornoXMLErr = config.PastaXmlRetorno + "\\" + nomearquivo + "-" + this.nSeqEvento + "-ret-env-cce.err";
        }
        public bool PodeSerGerado(string Justificativa,int operacao)
        {
            int err = 0;
            string resp = null;
            RepositorioNotaFiscal rep = new RepositorioNotaFiscal();
            var nota = rep.GetById(this.IdNota,operacao);
            if (nota.protocolo.Length < 5) { err += 1; resp = "Não há numero de protocolo cadastrado, verifique se a nota foi gerada ou gere uma consulta"; }
            else { this.nProt = nota.protocolo; }
            if (Justificativa.Length < 1 || Justificativa.Length > 1000) 
                { err += 1; resp += "Preencha corretamente o motivo da carta de correção no campo justificativa (Nó mínimo 15 caracteres)"; }
            else { 
                    string quebraLinha = System.Environment.NewLine.ToString();
                    this.xCorrecao = Justificativa.Replace(quebraLinha, "");
            }
            if (err > 0)
            {
                resposta += resp;
                return false;                
            }
            else
            {
                return true;
            }
        }
        public void SalvaXML()
        {

            XmlElement envEvento = doc.CreateElement("envEvento");
            envEvento.SetAttribute("versao", this.versao);
            envEvento.SetAttribute("xmlns", "http://www.portalfiscal.inf.br/nfe");
            XmlElement idLote = doc.CreateElement("idLote"); idLote.InnerText = this.idLote.ToString();
            XmlElement evento = doc.CreateElement("evento");
            evento.SetAttribute("versao", this.versao);
            evento.SetAttribute("xmlns", "http://www.portalfiscal.inf.br/nfe");
            XmlElement infEvento = doc.CreateElement("infEvento");
            infEvento.SetAttribute("Id", "ID" + this.tpEvento + this.chave + this.nSeqEvento.ToString("00"));
            XmlElement cOrgao = doc.CreateElement("cOrgao"); cOrgao.InnerText = this.cOrgao.ToString();            
            XmlElement tpAmb = doc.CreateElement("tpAmb"); tpAmb.InnerText = this.tpAmb.ToString();
            XmlElement CNPJ = doc.CreateElement("CNPJ"); CNPJ.InnerText = this.CNPJ; 
            XmlElement chNFE = doc.CreateElement("chNFe"); chNFE.InnerText = this.chave;
            XmlElement dhEvento = doc.CreateElement("dhEvento"); dhEvento.InnerText = this.dhEvento.ToString("yyyy-MM-ddTHH:mm:ss") + this.tzd + this.dhEvento.ToString("%K");
            XmlElement tpEvento = doc.CreateElement("tpEvento"); tpEvento.InnerText = this.tpEvento;
            XmlElement nSeqEvento = doc.CreateElement("nSeqEvento"); nSeqEvento.InnerText = this.nSeqEvento.ToString();
            XmlElement verEvento = doc.CreateElement("verEvento"); verEvento.InnerText = this.verEvento.ToString();
            XmlElement detEvento = doc.CreateElement("detEvento");
            detEvento.SetAttribute("versao", "1.00");
            XmlElement descEvento = doc.CreateElement("descEvento"); descEvento.InnerText = this.descEvento.Trim();
            XmlElement xCorrecao = doc.CreateElement("xCorrecao"); xCorrecao.InnerText = this.xCorrecao.Trim();
            XmlElement xCondUso = doc.CreateElement("xCondUso"); xCondUso.InnerText = this.xCondUso.Trim();

            detEvento.AppendChild(descEvento);
            detEvento.AppendChild(xCorrecao);
            detEvento.AppendChild(xCondUso);
            infEvento.AppendChild(cOrgao);
            infEvento.AppendChild(tpAmb);
            infEvento.AppendChild(CNPJ);
            infEvento.AppendChild(chNFE);
            infEvento.AppendChild(dhEvento);
            infEvento.AppendChild(tpEvento);
            infEvento.AppendChild(nSeqEvento);
            infEvento.AppendChild(verEvento);
            infEvento.AppendChild(detEvento);

            evento.AppendChild(infEvento);
            envEvento.AppendChild(idLote);
            envEvento.AppendChild(evento);

            XmlElement root = doc.DocumentElement;
            XmlDeclaration xmlDec = doc.CreateXmlDeclaration("1.0", "UTF-8", "no");
            doc.InsertBefore(xmlDec, root);
            doc.AppendChild(envEvento);
            doc.Save(this.PathDestinoXML);
        }

        public void GetResposta()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(3000);
                if (verResposta())
                {
                    break;
                }
                else
                {
                    continue;   
                }
            }            
        }

        //Usado para retornar o status do cancelamento e updatar a nota
        public void GetResposta(int idNota)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(3000);
                if (verResposta(idNota))
                {
                    break;
                }
                else
                {
                    continue;
                }
            }
        }

        //public void verResposta(object source, ElapsedEventArgs e)
        public bool verResposta()
        {
            bool retorno = false;
            FileInfo arq = new FileInfo(this.PathRetornoXML);
            FileInfo arqErr = new FileInfo(this.PathRetornoXMLErr);
            if (arq.Exists)
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(this.PathRetornoXML);
                XmlNodeList retCancNFe = null;
                retCancNFe = xml.GetElementsByTagName("retEvento");
                foreach (XmlNode consStatServNode in retCancNFe)
                {
                    XmlNodeList infCanc = xml.GetElementsByTagName("infEvento");
                    foreach (XmlNode infRecNode in infCanc)
                     {
                         XmlElement consStatServNodeElement = (XmlElement)infRecNode;
                         this.resposta = consStatServNodeElement.GetElementsByTagName("xMotivo")[0].InnerText;
                         retorno = true;
                     }
                    
                }
                
            }
            else if (arqErr.Exists)
            {
                using (StreamReader sr = arqErr.OpenText())
                {
                    string s = "";
                    string resp = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        resp += s;
                        retorno = true;
                    }
                    UTF8Encoding utf88 = new UTF8Encoding();
                    byte[] byteArray = Encoding.ASCII.GetBytes(resp);
                    byte[] utf8Array = Encoding.Convert(Encoding.ASCII, Encoding.UTF8, byteArray);
                    string finalString = utf88.GetString(utf8Array);
                    this.resposta = finalString;
                }
            }
            else
            {
                retorno = false;
                this.resposta = "Não foi possivel enviar a carta de correção, tente novamente mais tarde ou entre em contato com o suporte";
            }
            return retorno;            
        }

        public bool verResposta(int idNota)
        {
            bool retorno = false;
            FileInfo arq = new FileInfo(this.PathRetornoXML);
            FileInfo arqErr = new FileInfo(this.PathRetornoXMLErr);
            if (arq.Exists)
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(this.PathRetornoXML);
                XmlNodeList retCancNFe = null;
                retCancNFe = xml.GetElementsByTagName("retEvento");
                foreach (XmlNode consStatServNode in retCancNFe)
                {
                    XmlNodeList infCanc = xml.GetElementsByTagName("infEvento");
                    foreach (XmlNode infRecNode in infCanc)
                    {
                        XmlElement consStatServNodeElement = (XmlElement)infRecNode;
                        
                        this.resposta = new MensagemRetorno().MensagemDeRetorno(Convert.ToInt32(consStatServNodeElement.GetElementsByTagName("cStat")[0].InnerText), consStatServNodeElement.GetElementsByTagName("xMotivo")[0].InnerText);
                        RepositorioNotaFiscal rep = new RepositorioNotaFiscal();
                        if (Convert.ToInt32(consStatServNodeElement.GetElementsByTagName("cStat")[0].InnerText) != 420)
                        {

                            this.GravaCartaCorrecao(idNota, consStatServNodeElement.GetElementsByTagName("xMotivo")[0].InnerText);
                            
                        }
                        retorno = true;
                    }

                }

            }
            else if (arqErr.Exists)
            {
                using (StreamReader sr = arqErr.OpenText())
                {
                    string s = "";
                    string resp = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        resp += s;
                        retorno = true;
                    }
                    UTF8Encoding utf88 = new UTF8Encoding();
                    byte[] byteArray = Encoding.ASCII.GetBytes(resp);
                    byte[] utf8Array = Encoding.Convert(Encoding.ASCII, Encoding.UTF8, byteArray);
                    string finalString = utf88.GetString(utf8Array);
                    this.resposta = finalString;
                }
            }
            else
            {
                retorno = false;
                this.resposta = "Não foi possivel enviar o cancelamento, tente novamente mais tarde ou entre em contato com o suporte";
            }
            return retorno;
        }

        public void GravaCartaCorrecao(int idNota, string xMotivo)
        {

            ConfiguracaoApp config = new ConfiguracaoApp();
            SqlConnection conn = config.getConexaoBD();
            string instrucao;
            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            
            instrucao = "Insert INTO NFECartaCorrecaoEletronica(id_notafiscal,xMotivo)Values(" + idNota + ",'" + xMotivo + "')";

            SqlCommand comm = new SqlCommand(instrucao, conn,trans);

            try
            {
                comm.ExecuteNonQuery();
                trans.Commit();
                conn.Close();
            }
            catch(Exception ex)
            {
                trans.Rollback();
                conn.Close();
                throw new Exception("Problemas ao gravar as informações referentes a carta de correção, " + ex.Message);
            }
            
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
        public int NumeroSequenciaEvento(int id_NotaFiscal)
        {

            int NumeroSequenciaEvento;
            string instrucao;

            ConfiguracaoApp config = new ConfiguracaoApp();
            SqlConnection conn = config.getConexaoBD();
            conn.Open();

            instrucao = "select count(id_notaFiscal) + 1 as numeroSequenciaEvento from " +
                        " NFECartaCorrecaoEletronica where id_notaFiscal = " + id_NotaFiscal;

            SqlDataAdapter daNumeroSequenciaEvento = new SqlDataAdapter(instrucao, conn);
            DataTable dtNumeroSequenciaEvento = new DataTable();

            //preenche o datatable
            daNumeroSequenciaEvento.Fill(dtNumeroSequenciaEvento);
            DataRow drNumeroSequenciaEvento = dtNumeroSequenciaEvento.Rows[0];

            NumeroSequenciaEvento = Convert.ToInt16(drNumeroSequenciaEvento["NumeroSequenciaEvento"].ToString());
            conn.Close();

            return NumeroSequenciaEvento;
        }

        //De acordo com o manual, o webservice não faz qualquer uso deste identificador, 
        //sendo a responsabilidade de gerar este número por conta do usuário
        public int RetornaNumeroLote()
        {

            int NumeroLote;
            string instrucao;

            ConfiguracaoApp config = new ConfiguracaoApp();
            SqlConnection conn = config.getConexaoBD();
            conn.Open();

            instrucao = "select count(id_notaFiscal) + 1 as numeroLote from NFECartaCorrecaoEletronica";

            SqlDataAdapter daNumeroLote = new SqlDataAdapter(instrucao, conn);
            DataTable dtNumeroLote = new DataTable();

            //preenche o datatable
            daNumeroLote.Fill(dtNumeroLote);
            DataRow drNumeroLote = dtNumeroLote.Rows[0];

            NumeroLote = Convert.ToInt16(drNumeroLote["numeroLote"].ToString());
            conn.Close();

            return NumeroLote;
        }
    }
}
