using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Nfe.Infra.Configuracao;
using Nfe.Dominio.Entidades;

namespace Nfe.Infra.AcessoDados
{
    public class EmpresaInfo
    {
        public NfeInformacoesEmpresa GetEmpresaInfo()
        {
            try
            {
                
                string strConexao = "";
                //obtem a string de conexão do App.Config e retorna uma nova conexao
                strConexao = System.Configuration.ConfigurationManager.ConnectionStrings["MultimaxConnectionString"].ConnectionString.Replace('\\','\\');
                SqlConnection conn = new SqlConnection(strConexao);

                conn.Open();

                //define adaptador para a empresa
                SqlDataAdapter daParam = new SqlDataAdapter("Select * from ParametrosNFE ", conn);
                DataTable dtParam = new DataTable();

                //preenche o datatable
                daParam.Fill(dtParam);
                conn.Close();

                NfeInformacoesEmpresa emp = new NfeInformacoesEmpresa();
                RepositorioEndereco endereco = new RepositorioEndereco();

                if (dtParam.Rows.Count != 0)
                {

                    DataRow drParam = dtParam.Rows[0];

                    emp.CNPJ = drParam["CNPJ"].ToString();
                    emp.NomeEmpresa = drParam["RazaoSocial"].ToString();
                    emp.NomeFantasia = drParam["NomeFantasia"].ToString();
                    emp.InscEstadual = drParam["InscricaoEstadual"].ToString();
                    emp.InscMunicipal = drParam["InscricaoMunicipal"].ToString();
                    emp.CNAE = drParam["CNAE"].ToString();
                    emp.CRT = drParam["CRT"].ToString();
                    emp.Endereco = endereco.GetById(Convert.ToInt32(drParam["id_endereco"].ToString()));
                    emp.tpAmb = emp.Endereco.CodPais;
                    emp.tpEmis = emp.Endereco.CodPais;

                }

                return emp;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no momento de capturar as informações da Empresa: " + ex.Message);
            }
        }
    }
}
