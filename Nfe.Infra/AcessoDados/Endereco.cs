using System;
using Nfe.Dominio.Entidades;
using System.Data;
using System.Data.SqlClient;
using Nfe.Infra.Configuracao;

namespace Nfe.Infra.AcessoDados
{
    public class RepositorioEndereco
    {
        private NfeEndereco endereco { get; set; }

        public RepositorioEndereco()
        {
        }
        
        public NfeEndereco GetById(int id)
        {

            try
            {
                ConfiguracaoApp config = new ConfiguracaoApp(false);
                SqlConnection conn = config.getConexaoBD();
                
                NfeEndereco endRet = new NfeEndereco();
                conn.Open();

                //define adaptador para os Endereços

                SqlDataAdapter daEnderecos = new SqlDataAdapter("Select * from Gerenciamento_comum.dbo.vw_Enderecos Endereco where id_ender = " + id.ToString(), conn);
                
                DataTable dtEnderecos = new DataTable();

                //preenche o datatable
                daEnderecos.Fill(dtEnderecos);
                conn.Close();

                DataRow drEndereco = dtEnderecos.Rows[0];

                endRet.Logradouro = drEndereco["NomeEnder"].ToString();
                endRet.Numero = drEndereco["Numero"].ToString();
                endRet.Complemento = drEndereco["CompEnder"].ToString();
                endRet.Bairro = drEndereco["BairEnder"].ToString();
                endRet.Cep = drEndereco["CeppEnder"].ToString();
                endRet.Pais = "BRASIL";
                endRet.CodPais = 1058;
                endRet.Municipio = drEndereco["CidaEnder"].ToString().Trim();
                if ((drEndereco["CodigoMunicipioNFE"].ToString()) != "")
                {
                    endRet.CodMunicipio = Convert.ToInt32(drEndereco["CodigoMunicipioNFE"].ToString());
                }
                else
                {
                    endRet.CodMunicipio = null;
                }
                
                
                endRet.UF = drEndereco["EstaEnder"].ToString();

                return endRet;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro no momento de capturar as informações do Endereço: " + ex.Message);
            }
 
        }

    }
}
