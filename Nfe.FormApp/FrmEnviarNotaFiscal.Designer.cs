namespace Nfe.FormApp
{
    partial class frmEnviarNotaFiscal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEnviarNotaFiscal));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblNumeroNF = new System.Windows.Forms.Label();
            this.lblNomeCliente = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtResultado = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmdCancelar = new System.Windows.Forms.Button();
            this.imgImagens = new System.Windows.Forms.ImageList(this.components);
            this.cmdEnviar = new System.Windows.Forms.Button();
            this.lblOperacao = new System.Windows.Forms.Label();
            this.cmdCancelarNota = new System.Windows.Forms.Button();
            this.cmdConsultar = new System.Windows.Forms.Button();
            this.Chave = new System.Windows.Forms.Label();
            this.txtJustificativa = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmdEnviarCartaCorrecao = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nº nota fiscal";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(102, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(603, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Cliente";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNumeroNF
            // 
            this.lblNumeroNF.BackColor = System.Drawing.Color.White;
            this.lblNumeroNF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNumeroNF.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumeroNF.Location = new System.Drawing.Point(12, 71);
            this.lblNumeroNF.Name = "lblNumeroNF";
            this.lblNumeroNF.Size = new System.Drawing.Size(79, 21);
            this.lblNumeroNF.TabIndex = 2;
            this.lblNumeroNF.Click += new System.EventHandler(this.lblNumeroNF_Click);
            // 
            // lblNomeCliente
            // 
            this.lblNomeCliente.BackColor = System.Drawing.Color.White;
            this.lblNomeCliente.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNomeCliente.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNomeCliente.Location = new System.Drawing.Point(102, 71);
            this.lblNomeCliente.Name = "lblNomeCliente";
            this.lblNomeCliente.Size = new System.Drawing.Size(603, 21);
            this.lblNomeCliente.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(693, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "Resultado";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtResultado
            // 
            this.txtResultado.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtResultado.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtResultado.Location = new System.Drawing.Point(12, 126);
            this.txtResultado.Multiline = true;
            this.txtResultado.Name = "txtResultado";
            this.txtResultado.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultado.Size = new System.Drawing.Size(693, 91);
            this.txtResultado.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(-3, 340);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(723, 48);
            this.label5.TabIndex = 8;
            // 
            // cmdCancelar
            // 
            this.cmdCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdCancelar.ImageIndex = 1;
            this.cmdCancelar.ImageList = this.imgImagens;
            this.cmdCancelar.Location = new System.Drawing.Point(596, 351);
            this.cmdCancelar.Name = "cmdCancelar";
            this.cmdCancelar.Size = new System.Drawing.Size(111, 27);
            this.cmdCancelar.TabIndex = 14;
            this.cmdCancelar.Text = "Cancelar";
            this.cmdCancelar.UseVisualStyleBackColor = true;
            this.cmdCancelar.Click += new System.EventHandler(this.cmdCancelar_Click);
            // 
            // imgImagens
            // 
            this.imgImagens.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgImagens.ImageStream")));
            this.imgImagens.TransparentColor = System.Drawing.Color.Transparent;
            this.imgImagens.Images.SetKeyName(0, "run_16x16.ico");
            this.imgImagens.Images.SetKeyName(1, "Sair1616.ico");
            this.imgImagens.Images.SetKeyName(2, "Cancelar_2.ico");
            this.imgImagens.Images.SetKeyName(3, "visualizar_16x16.ico");
            // 
            // cmdEnviar
            // 
            this.cmdEnviar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdEnviar.ImageIndex = 0;
            this.cmdEnviar.ImageList = this.imgImagens;
            this.cmdEnviar.Location = new System.Drawing.Point(11, 352);
            this.cmdEnviar.Name = "cmdEnviar";
            this.cmdEnviar.Size = new System.Drawing.Size(111, 27);
            this.cmdEnviar.TabIndex = 10;
            this.cmdEnviar.Text = "Enviar";
            this.cmdEnviar.UseVisualStyleBackColor = true;
            this.cmdEnviar.Click += new System.EventHandler(this.cmdEnviar_Click);
            // 
            // lblOperacao
            // 
            this.lblOperacao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblOperacao.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOperacao.Location = new System.Drawing.Point(14, 10);
            this.lblOperacao.Name = "lblOperacao";
            this.lblOperacao.Size = new System.Drawing.Size(691, 24);
            this.lblOperacao.TabIndex = 12;
            this.lblOperacao.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmdCancelarNota
            // 
            this.cmdCancelarNota.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdCancelarNota.ImageIndex = 2;
            this.cmdCancelarNota.ImageList = this.imgImagens;
            this.cmdCancelarNota.Location = new System.Drawing.Point(257, 352);
            this.cmdCancelarNota.Name = "cmdCancelarNota";
            this.cmdCancelarNota.Size = new System.Drawing.Size(119, 27);
            this.cmdCancelarNota.TabIndex = 12;
            this.cmdCancelarNota.Text = "Cancelar NFE";
            this.cmdCancelarNota.UseVisualStyleBackColor = true;
            this.cmdCancelarNota.Click += new System.EventHandler(this.cmdCancelarNota_Click);
            // 
            // cmdConsultar
            // 
            this.cmdConsultar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdConsultar.ImageIndex = 3;
            this.cmdConsultar.ImageList = this.imgImagens;
            this.cmdConsultar.Location = new System.Drawing.Point(128, 352);
            this.cmdConsultar.Name = "cmdConsultar";
            this.cmdConsultar.Size = new System.Drawing.Size(123, 27);
            this.cmdConsultar.TabIndex = 11;
            this.cmdConsultar.Text = "Consultar";
            this.cmdConsultar.UseVisualStyleBackColor = true;
            this.cmdConsultar.Click += new System.EventHandler(this.cmdConsultar_Click);
            // 
            // Chave
            // 
            this.Chave.AutoSize = true;
            this.Chave.Location = new System.Drawing.Point(12, 324);
            this.Chave.Name = "Chave";
            this.Chave.Size = new System.Drawing.Size(38, 13);
            this.Chave.TabIndex = 15;
            this.Chave.Text = "Chave";
            // 
            // txtJustificativa
            // 
            this.txtJustificativa.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtJustificativa.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtJustificativa.Location = new System.Drawing.Point(12, 259);
            this.txtJustificativa.MaxLength = 250;
            this.txtJustificativa.Multiline = true;
            this.txtJustificativa.Name = "txtJustificativa";
            this.txtJustificativa.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtJustificativa.Size = new System.Drawing.Size(693, 55);
            this.txtJustificativa.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 235);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(693, 20);
            this.label3.TabIndex = 17;
            this.label3.Text = "Justificativa";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cmdEnviarCartaCorrecao
            // 
            this.cmdEnviarCartaCorrecao.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdEnviarCartaCorrecao.ImageIndex = 2;
            this.cmdEnviarCartaCorrecao.ImageList = this.imgImagens;
            this.cmdEnviarCartaCorrecao.Location = new System.Drawing.Point(382, 352);
            this.cmdEnviarCartaCorrecao.Name = "cmdEnviarCartaCorrecao";
            this.cmdEnviarCartaCorrecao.Size = new System.Drawing.Size(123, 27);
            this.cmdEnviarCartaCorrecao.TabIndex = 13;
            this.cmdEnviarCartaCorrecao.Text = "Enviar CC-e";
            this.cmdEnviarCartaCorrecao.UseVisualStyleBackColor = true;
            this.cmdEnviarCartaCorrecao.Click += new System.EventHandler(this.cmdEnviarCartaCorrecao_Click);
            // 
            // frmEnviarNotaFiscal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(717, 388);
            this.Controls.Add(this.cmdEnviarCartaCorrecao);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtJustificativa);
            this.Controls.Add(this.Chave);
            this.Controls.Add(this.cmdConsultar);
            this.Controls.Add(this.cmdCancelarNota);
            this.Controls.Add(this.lblOperacao);
            this.Controls.Add(this.cmdEnviar);
            this.Controls.Add(this.cmdCancelar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtResultado);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblNomeCliente);
            this.Controls.Add(this.lblNumeroNF);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmEnviarNotaFiscal";
            this.Text = "Emissão de nota fiscal eletrônica";
            this.Load += new System.EventHandler(this.frmNotaFiscal_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblNumeroNF;
        private System.Windows.Forms.Label lblNomeCliente;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtResultado;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button cmdCancelar;
        private System.Windows.Forms.ImageList imgImagens;
        private System.Windows.Forms.Button cmdEnviar;
        private System.Windows.Forms.Label lblOperacao;
        private System.Windows.Forms.Button cmdCancelarNota;
        private System.Windows.Forms.Button cmdConsultar;
        private System.Windows.Forms.Label Chave;
        private System.Windows.Forms.TextBox txtJustificativa;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button cmdEnviarCartaCorrecao;
    }
}