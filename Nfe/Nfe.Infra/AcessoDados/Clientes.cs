using System;
using Nfe.Dominio.Entidades;
using Nfe.Infra.Configuracao;
using System.Data;
using System.Data.SqlClient;

namespace Nfe.Infra.AcessoDados
{
    public class RepositorioClientes 
    {
        
        //public int IdPessoaJuridica { get; set; }
        //public int IdPessoaFisica { get; set; }

        public NFePessoa GetClienteById(int id)
        {
            try
            {

                ConfiguracaoApp config = new ConfiguracaoApp();
                SqlConnection conn = config.getConexaoBD();
                conn.Open();

                //define adaptador para clientes

                SqlDataAdapter daClientes = new SqlDataAdapter(" SELECT * FROM gerenciamento_comum.dbo.vw_Clientes WHERE id_clien = " + id.ToString(), conn);
                DataTable dtClientes = new DataTable();

                //preenche o datatable
                daClientes.Fill(dtClientes);
                conn.Close();

                NFePessoa cliente = new NFePessoa();
                RepositorioEndereco endereco = new RepositorioEndereco();
                
                if (dtClientes.Rows.Count != 0)
                {

                    DataRow drCliente = dtClientes.Rows[0];

                    cliente.TipoPessoa = TipoPesssoa.Juridico;
                    cliente.Cnpj = drCliente["CnpjClien"].ToString();
                    cliente.NomeOURazao = drCliente["NomeClien"].ToString();
                    cliente.Telefone = drCliente["TeleClien"].ToString();
                    cliente.Email = drCliente["EMail"].ToString();
                    cliente.IE = drCliente["InesClien"].ToString();
                    cliente.IdEndereco = Convert.ToInt32(drCliente["Loc_Clien"].ToString());
                    cliente.Rg = drCliente["RGClien"].ToString();
                    cliente.Contato = drCliente["NomeContato"].ToString();
                    cliente.Fax = "";
                    cliente.Endereco = endereco.GetById(Convert.ToInt32(drCliente["Loc_Clien"].ToString()));
                }

                return cliente; 
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no momento de capturar as informações da Pessoa: "+ex.Message);
            }
                     
        }

        public NFePessoa GetClienteLocEntregaById(int id)
        {
            try
            {

                ConfiguracaoApp config = new ConfiguracaoApp();
                SqlConnection conn = config.getConexaoBD();
                conn.Open();

                //define adaptador para clientes

                SqlDataAdapter daClientes = new SqlDataAdapter(" SELECT * FROM gerenciamento_comum.dbo.vw_LocaisEntrega WHERE id_entrega = " + id.ToString(), conn);
                DataTable dtClientes = new DataTable();

                //preenche o datatable
                daClientes.Fill(dtClientes);
                conn.Close();

                NFePessoa cliente = new NFePessoa();
                RepositorioEndereco endereco = new RepositorioEndereco();

                if (dtClientes.Rows.Count != 0)
                {

                    DataRow drCliente = dtClientes.Rows[0];

                    cliente.TipoPessoa = TipoPesssoa.Juridico;
                    cliente.Cnpj = drCliente["cnpj"].ToString();
                    cliente.NomeOURazao = drCliente["LocalEntrega"].ToString();
                    cliente.Telefone = "";
                    cliente.Email = "";
                    cliente.IE = drCliente["InscricaoEstadual"].ToString();
                    cliente.IdEndereco = Convert.ToInt32(drCliente["id_ender"].ToString());
                    cliente.Rg = "";
                    cliente.Contato = "";
                    cliente.Fax = "";
                    cliente.Endereco = endereco.GetById(Convert.ToInt32(drCliente["id_Ender"].ToString()));
                }

                return cliente;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro no momento de capturar as informações da Pessoa: " + ex.Message);
            }

        }

    }
}
