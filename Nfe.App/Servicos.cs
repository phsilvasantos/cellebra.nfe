using System;
using System.Data.Entity;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.ComponentModel;
using Nfe.Dominio.Entidades;
using Nfe.Infra.AcessoDados;
using Nfe.Infra;
using Nfe.Infra.Configuracao;
using System.Xml;
using System.Threading;

using System.Runtime.InteropServices;

namespace Nfe.App
{

    public class Servicos
    {
        private string Msg;
        public bool PossoCancelarNota { get; set; }
        public ConfiguracaoApp config { get; set; }
        private XmlDocument doc { get; set; }

        /// <summary>
        /// -nfe.xml
        /// </summary>
        /// <param name="idNotaFiscal"></param>
        /// <returns></returns>
        public Servicos()
        {
            
        }
        public  string gerarNota(int idNotaFiscal,int operacao)
        {
            try
            {
                RepositorioNotaFiscal nota = new RepositorioNotaFiscal();
                var novaNota = nota.GetById(idNotaFiscal,operacao);

                ServicoXML servico = new ServicoXML(novaNota);
                servico.geraNotaEmXML();
                Msg = servico.Resposta;
                
            }
            catch (Exception ex)
            {                
                Msg += ex.Message;
                return "Erro em Servicos.gerarNota(int) "+ ex.Message;
            }
            finally
            {
                GravaLog();
            }

            //string appPath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Servicos)).CodeBase);
            return Msg;
        }
        /// <summary>
        /// -ped-sit.xml
        /// </summary>
        /// <param name="chave"></param>
        /// <returns></returns>
        public string gerarConsultaNota(string chave, int idNota, string justificativa = "")
        {
            try
            {
                //Mudado de 2.00 para 2.01 a versão por conta que isso é necessário para retornar o protocoloEvento
                //da carta de correção eletrônica (caso exista) de acordo com o manual do sefaz versão 5
                StatusNFe status = new StatusNFe(chave, "4.00", "CONSULTAR");
                StatusNFe status2 = new StatusNFe(chave, "4.00", "CONSULTAR");
                RepositorioNotaFiscal nota = new RepositorioNotaFiscal();

                status.SalvaXML();
                status.GetResposta();
                if (status.resposta.Equals("Autorizado o uso da NF-e"))
                {
                    status2.SalvaXML(status.resposta);
                    status.GetResposta();
                    nota.AtualizarProtocolo(idNota, status.protocolo);
                    nota.AtualizarConfirmaEnvio(idNota);
                    nota.AtualizarStatus(idNota, status.resposta.ToString());
                }

                //Gravação das informações de cancelamento passou a ser efetuado na própria verfiricação de resposta do XML de cancelamento
                //else if(status.resposta.Equals("Cancelamento de NF-e homologado")) //(Essa mensagem era retornada quando o cancelamento era um processo
                //{
                //    nota.AtualizarProtocolo(idNota, status.protocolo);
                //    nota.AtualizarConfirmaCancelamento(idNota, justificativa,status.DataCancelamentoOuEnvioNFE);
                //    nota.AtualizarStatus(idNota, status.resposta.ToString());
                //}

                return status.resposta;

            }
            catch (Exception ex)
            {
                Msg = "Erro em Servicos.gerarConsultaNota() "+ex.Message;
            }
            finally
            {
                GravaLog();
            }
            return Msg;
        }

        public string gerarConsultaCadastro()
        {
            return "Não Implementado";
        }
        /// <summary>
        /// AAAAMMDDTHHMMSS-ped-sta.xml
        /// </summary>
        /// <returns></returns>
        public  string gerarConsultaStatusServico()
        {
            try
            {
                StatusServico novo = new StatusServico("4.00", "STATUS");
                novo.SalvaXML();
                novo.GetResposta();
                return novo.resposta;
            }
            catch (Exception ex)
            {
                Msg = "Erro Servicos.gerarConsultaStatusServico() "+ex.Message;
            }
            finally
            {
                GravaLog();
            }
            return Msg;           
        }
        public  string GerarCancelamentoNota(int id, string Justificativa, string chave, int operacao)
        {
            try
            {
                RepositorioNotaFiscal nota = new RepositorioNotaFiscal();
                var NotaCanc = nota.GetById(id, operacao);

                GerarCancelamento cancel = new GerarCancelamento(id, chave, Convert.ToString(NotaCanc.cUF), NotaCanc.Emitente.Cnpj.ToString());
                this.PossoCancelarNota = cancel.PodeSerCancelado(Justificativa,operacao);
                if (this.PossoCancelarNota)
                {
                    cancel.SalvaXML(operacao);
                    cancel.GetResposta(id,Justificativa,chave);
                    return cancel.resposta;
                }
                else
                {
                    return cancel.resposta;
                }  
            }
            catch (Exception ex )
            {
               Msg = "Erro em Servicos.GerarCancelamentoNota() " + ex.Message;
            }
            return Msg;
        }
        public string GerarCartaCorrecaoNota(int id, string Justificativa, string chave, int operacao)
        {
            try
            {
                RepositorioNotaFiscal nota = new RepositorioNotaFiscal();
                var novaNota = nota.GetById(id,operacao);
                ServicoXML servico = new ServicoXML(novaNota);

                GerarCartaCorrecao CartaCorrecao = new GerarCartaCorrecao(id, chave,Convert.ToString(novaNota.cUF),novaNota.Emitente.Cnpj.ToString());
                if (CartaCorrecao.NumeroSequenciaEvento(id) < 20)
                {
                    this.PossoCancelarNota = CartaCorrecao.PodeSerGerado(Justificativa, operacao);
                    if (this.PossoCancelarNota)
                    {
                        CartaCorrecao.SalvaXML();
                        Thread.Sleep(4000);
                        CartaCorrecao.GetResposta(id);
                        return CartaCorrecao.resposta;
                    }
                    else
                    {
                        return CartaCorrecao.resposta;
                    }
                }
                else
                {
                    return "O limite máximo de cartas de correção para esta nota é 19!";
                }
            }
            catch (Exception ex)
            {
                Msg = "Erro em Servicos.GeraCartaCorrecaoNota() " + ex.Message;
            }
            return Msg;
        }
        public string GerarInutilizacaoNumero(int idNotaFiscal)
        {
            return "Não Implementado";
        }
        public void GravaLog()
        {
            Log.Save(Msg);
        }
           
    }
}
