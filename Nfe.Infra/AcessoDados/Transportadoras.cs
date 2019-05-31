using System;
using Nfe.Dominio.Entidades;
using Nfe.Infra.Configuracao;
using System.Data;
using System.Data.SqlClient;

namespace Nfe.Infra.AcessoDados
{
    public class RepositorioTransportadoras
    {
        
        public Nfe.Dominio.Entidades.NFePessoa transp { get; set; }
        public Nfe.Dominio.Entidades.Transportadora transportadora { get; set; }
        public RepositorioClientes Clientes { get; set; }
        public NFePessoa Pessoa { get; set; }

        public RepositorioTransportadoras()
        {
           
            transportadora = new Nfe.Dominio.Entidades.Transportadora();
            transp = new NFePessoa();
            Pessoa = new NFePessoa();
            Clientes = new RepositorioClientes();
            
        }
        public Nfe.Dominio.Entidades.Transportadora getById(int idTrans)
        {
            try
            {

                ConfiguracaoApp config = new ConfiguracaoApp(false);
                SqlConnection conn = config.getConexaoBD();
                RepositorioEndereco endereco = new RepositorioEndereco();

                conn.Open();

                //define adaptador para Transportadoras
                SqlDataAdapter daTransp = new SqlDataAdapter("Select * from Gerenciamento_Comum.dbo.vw_Transportadoras where id_transportadora = " + idTrans.ToString(), conn);
                DataTable dtTransp = new DataTable();

                //preenche o datatable
                daTransp.Fill(dtTransp);
                conn.Close();

                if (dtTransp.Rows.Count != 0)
                {

                    DataRow drTransp = dtTransp.Rows[0];

                    transp.Contato = drTransp["Contato"].ToString();
                    transp.Email = drTransp["EMail"].ToString();
                    transp.IdEndereco = Convert.ToInt32(drTransp["id_endereco"].ToString());
                    transp.Endereco = endereco.GetById(transp.IdEndereco);
                    transp.Fax = drTransp["Fax"].ToString();
                    transp.NomeOURazao = drTransp["Nome"].ToString();

                    if (drTransp["DescricaoPessoa"].ToString().Trim() == "PESSOA FISICA") 
                    {
                        transp.TipoPessoa = TipoPesssoa.Fisico;
                        transp.Cpf = drTransp["Cpf"].ToString();
                        transp.Rg = drTransp["RG"].ToString();
                    }
                    else
                    {
                        transp.TipoPessoa = TipoPesssoa.Juridico;
                        transp.IE = drTransp["InscricaoEstadual"].ToString();
                        transp.Cnpj = drTransp["Cnpj"].ToString();
                    }

                    transportadora.DadosEmpresa = transp;

                }

            }
            catch(Exception ex)
            {
                throw new Exception("Erro ao tentar capturar informações da Transportadora: " + ex.Message);
            }
            return transportadora;
        }
    }
}
