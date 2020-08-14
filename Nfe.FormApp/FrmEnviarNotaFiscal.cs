using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Xml;
using Nfe.App;
using Nfe.Infra;
using Nfe.Infra.AcessoDados;

namespace Nfe.FormApp
{    
    public partial class frmEnviarNotaFiscal : Form
    {
        private Int32 IDNfe;
        private Int32 Operacao;

        public frmEnviarNotaFiscal(Int32 IDNotaFiscal, Int32 IDOperacao)
        {
            
            IDNfe = IDNotaFiscal;
            Operacao = IDOperacao;
            InitializeComponent();

            if (IDOperacao == 0)
            {
                cmdEnviar.Enabled = true;
                cmdCancelarNota.Enabled = false;
                cmdConsultar.Enabled = false;
                cmdEnviarCartaCorrecao.Enabled = false;
            }
            else if (IDOperacao == 1)
            {
                cmdEnviar.Enabled = false;
                cmdCancelarNota.Enabled = true;
                cmdConsultar.Enabled = false;
                cmdEnviarCartaCorrecao.Enabled = false;
            }
            else if (IDOperacao == 2)
            {
                cmdConsultar.Enabled = false;
                RepositorioNotaFiscal nota = new RepositorioNotaFiscal();
                nota.GetById(IDNotaFiscal,Operacao);
                this.Dispose();
            }
            else if (IDOperacao == 3)
            {
                cmdEnviar.Enabled = false;
                cmdCancelarNota.Enabled = false;
                cmdConsultar.Enabled = true;
                cmdEnviarCartaCorrecao.Enabled = false;
            }
            else if (IDOperacao == 4) // Carta de correção
            {
                cmdEnviar.Enabled = false;
                cmdConsultar.Enabled = false;
                cmdCancelarNota.Enabled = false;
                cmdEnviarCartaCorrecao.Enabled = true;
            }
        }

        private void cmdCancelar_Click(object sender, EventArgs e)
        {
            this.Dispose();

        }

        private void cmdEnviar_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            this.cmdEnviar.Enabled = false;
            RefreshButtons();
            Thread trd = new Thread(new ThreadStart(this.ProcessaEnvioNota));
            trd.IsBackground = false;
            trd.Start();
            this.Enabled = true;
        }

        private void RefreshButtons()
        {
            this.cmdEnviar.Refresh();
            this.cmdConsultar.Refresh();
            this.cmdCancelarNota.Refresh();
            this.cmdEnviarCartaCorrecao.Refresh();
        }


        #region PROCESSOS
        private void ProcessaConsultaNota()
        {
            Servicos servico = new Servicos();
            if (Chave.Text.Trim().Length != 44)
            {
                MessageBox.Show("Selecione a nota ");
            }
            else
            {
                RefreshButtons();
                SetControlPropertyValue(txtResultado, "Text", "Gerando Arquivo de Consulta....");
                SetControlPropertyValue(txtResultado, "Text", servico.gerarConsultaNota(Chave.Text.Trim(),IDNfe));
            }

        }
        private void ProcessaEnvioNota()
        {
            int idNota = 0;
            Servicos servico = new Servicos();
            if (!string.IsNullOrEmpty(lblNumeroNF.Text.Trim()))
            {
                idNota = IDNfe;
                SetControlPropertyValue(txtResultado, "Text", "Gerando Arquivo Nota Eletrônica....");
                SetControlPropertyValue(txtResultado, "Text", servico.gerarNota(idNota,Operacao));
            }
        }
        private void ProcessaStatusServico()
        {
            Servicos servico = new Servicos();
            SetControlPropertyValue(txtResultado, "Text", "Gerando Arquivo....");
            SetControlPropertyValue(txtResultado, "Text", servico.gerarConsultaStatusServico());
        }
        private void ProcessaCancelamentoNota()
        {
            int idNota = 0;
            Servicos servico = new Servicos();
            if (!string.IsNullOrEmpty(lblNumeroNF.Text.Trim()))
            {
                this.Enabled = false;
                cmdCancelarNota.Enabled = false;                
                idNota = IDNfe;
                SetControlPropertyValue(txtResultado, "Text", "Gerando arquivo de cancelamento da nota eletrônica....");
                SetControlPropertyValue(txtResultado, "Text", servico.GerarCancelamentoNota(idNota, txtJustificativa.Text, Chave.Text,Operacao));

                this.Enabled = true;
            }
        }

        private void ProcessaCartaCorrecaoNota()
        {
            int idNota = 0;
            Servicos servico = new Servicos();

            if (!string.IsNullOrEmpty(lblNumeroNF.Text.Trim()))
            {
                idNota = IDNfe;
                SetControlPropertyValue(txtResultado, "Text", "Gerando Arquivo....");
                SetControlPropertyValue(txtResultado, "Text", servico.GerarCartaCorrecaoNota(idNota, txtJustificativa.Text, Chave.Text, Operacao));

            }
            cmdEnviarCartaCorrecao.Enabled = false;
        }

        #endregion

        #region DELEGATES
        delegate void SetControlValueCallback(Control oControl, string propName, object propValue);
        private void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {
            if (oControl.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                oControl.Invoke(d, new object[] { oControl, propName, propValue });
            }
            else
            {
                Type t = oControl.GetType();
                PropertyInfo[] props = t.GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if (p.Name.ToUpper() == propName.ToUpper())
                    {
                        p.SetValue(oControl, propValue, null);
                    }
                }
            }
        }
        #endregion

        private void frmNotaFiscal_Load(object sender, EventArgs e)
        {
            try
            { 
                RepositorioNotaFiscal nota = new RepositorioNotaFiscal();

                var Nfe = nota.GetById(IDNfe,Operacao);
                lblNumeroNF.Text = Convert.ToString(Nfe.nNF);
                lblNomeCliente.Text = Nfe.Cliente.NomeOURazao;
                Chave.Text = Nfe.IDChave;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu o seguinte erro ao carregar configurações iniciais: \n" + ex.Message, "Erro", MessageBoxButtons.OK);
                this.Dispose();
            }

        }

        private void cmdConsultar_Click(object sender, EventArgs e)
        {
            Thread trd = new Thread(new ThreadStart(this.ProcessaConsultaNota));
            trd.IsBackground = true;
            trd.Start();
        }

        private void lblNumeroNF_Click(object sender, EventArgs e)
        {

        }

        private void cmdCancelarNota_Click(object sender, EventArgs e)
        {
            this.cmdCancelarNota.Enabled = false;
            this.Enabled = false;
            RefreshButtons();
            //Thread trd = new Thread(new ThreadStart(this.ProcessaCancelamentoNota));
            //trd.IsBackground = false;
            //trd.Start();
            this.ProcessaCancelamentoNota();
            this.Enabled = true;
        }

        private void cmdEnviarCartaCorrecao_Click(object sender, EventArgs e)
        {
            this.cmdEnviarCartaCorrecao.Enabled = false;
            this.Enabled = false;
            RefreshButtons();
            Thread trd = new Thread(new ThreadStart(this.ProcessaCartaCorrecaoNota));
            trd.IsBackground = false;
            trd.Start();
            this.Enabled = true;
        }

    }
}
