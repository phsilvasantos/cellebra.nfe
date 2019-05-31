using System;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;
//using System.Data;
//using System.Data.SqlClient;
using Nfe.Infra;
using Nfe.Infra.AcessoDados;
using System.Xml.Linq;
using System.Linq;

namespace Nfe.Infra.Configuracao
{
    public class ConfiguracaoApp
    {
        private RepositorioEmitente repositorioEmitente { get; set; }
        public string Cnpj { get; set; }
        public int tpAmb { get; set; }
        public int tpEmiss { get; set; }
        public int Uf { get; set; }
        public int codOrgRec { get; set; }
        public int codEvento { get; set; }
        public int codNSeqEvt { get; set; }

        public string arquivoConfiguracao { get; set; }
        public string PastaXmlEnvio { get; set; }
        public string PastaXmlRetorno { get; set; }
        public string PastaXmlEnviado { get; set; }
        public string PastaXmlEmProcessamento { get; set; }
        public string PastaXmlErro { get; set; }
        public string PastaXmlEmLote { get; set; }
        public string PastaValidar { get; set; }
        public string Certificado { get; set; }
        public string vLeiaute { get; set; }
        public string tzd { get; set; }

        public X509Certificate2 X509Certificado { get; set; }
        
        public SqlConnection getConexaoBD()
        {
            string strConexao = "";
            //obtem a string de conexão do App.Config e retorna uma nova conexao
            strConexao = System.Configuration.ConfigurationManager.ConnectionStrings["MultimaxConnectionString"].ConnectionString;
            return new SqlConnection(strConexao);
        }

        public ConfiguracaoApp(bool inicializaparametros = true)
        {
            if (inicializaparametros == true)
            {
                try
                {
                    repositorioEmitente = new RepositorioEmitente();
                    var emit = repositorioEmitente.getDefault();
                    this.Cnpj = emit.Cnpj;
                    arquivoConfiguracao = @"C:\Unimake\UniNFe\" + this.Cnpj.Trim() + @"\UniNfeConfig.xml";
                    carregaDados();
                }
                catch
                {
                    throw;
                }
            }
        }
        private void carregaDados()
        {
            XElement root = XElement.Load(arquivoConfiguracao);
            foreach (var item in root.Elements("AmbienteCodigo"))
            {
                tpAmb = Convert.ToInt32(item.Value.Trim());                
            }
            foreach (var item in root.Elements("tpEmis"))
            {
                tpEmiss = Convert.ToInt32(item.Value.Trim());
            }
            foreach (var item in root.Elements("UnidadeFederativaCodigo"))
            {
                Uf = Convert.ToInt32(item.Value.Trim());
            }
            foreach (var item in root.Elements("PastaXmlEnvio"))
            {
                PastaXmlEnvio = item.Value.Trim();
            }
            foreach (var item in root.Elements("PastaXmlRetorno"))
            {
                PastaXmlRetorno = item.Value.Trim();
            }
            foreach (var item in root.Elements("PastaXmlEnviado"))
            {
                PastaXmlEnviado = item.Value.Trim();
            }
            foreach (var item in root.Elements("PastaXmlErro"))
            {
                PastaXmlErro = item.Value.Trim();
            }
            foreach (var item in root.Elements("PastaXmlEmLote"))
            {
                PastaXmlEmLote = item.Value.Trim();
            }
            foreach (var item in root.Elements("PastaValidar"))
            {
                PastaValidar = item.Value.Trim();
            }
            foreach (var item in root.Elements("Certificado"))
            {
                Certificado = item.Value.Trim();
            }
            foreach (var item in root.Elements("CodigoDoOrgaoDaRecepcaoDoEvento"))
            {
                codOrgRec = Convert.ToInt32(item.Value.Trim());
            }
            foreach (var item in root.Elements("CodigoDoEvento"))
            {
                codEvento = Convert.ToInt32(item.Value.Trim());
            }
            foreach (var item in root.Elements("SequencialDoEvento"))
            {
                codNSeqEvt = Convert.ToInt32(item.Value.Trim());
            }
            foreach (var item in root.Elements("VersaoDoLeiaute"))
            {
                vLeiaute = item.Value.Trim();
            }

            PastaXmlEmProcessamento = @"C:\Unimake\UniNFe\" + this.Cnpj.Trim() + @"\Enviado\EmProcessamento";
            //Ajustar o certificado digital de String para o tipo X509Certificate2
            X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            // o parametro this.Certificado é a string com o subject do certificado para fim de comparacao na hora de selecionar o certificado
            X509Certificate2Collection collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindBySubjectDistinguishedName,this.Certificado, false);

            this.X509Certificado = null;
            for (int i = 0; i < collection1.Count; i++)
            {
                //Verificar a validade do certificado
                if (DateTime.Compare(DateTime.Now, collection1[i].NotAfter) == -1)
                {
                    this.X509Certificado = collection1[i];
                    break;
                }
            }
            //Se não encontrou nenhum certificado com validade correta, vou pegar o primeiro certificado, porem vai travar na hora de tentar enviar a nota fiscal, por conta da validade. Wandrey 06/04/2011
            if (this.X509Certificado == null & collection1.Count > 0)
                this.X509Certificado = collection1[0];
        }
        
    }
}
