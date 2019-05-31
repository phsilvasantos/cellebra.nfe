using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Text;
using System.Data;
using Nfe.App;

namespace Nfe.FormApp
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        
        static void Main(string[] args)
        {
            Int32 Parametro = 0;
            Int32 operacao = 3;
            //string Parametro;

            if (args.Length != 0)
            {
                Parametro = Convert.ToInt32(args[0]);
                operacao = Convert.ToInt32(args[1]);
                //MessageBox.Show("Parametro: " + Parametro + " Operacao: " + operacao);
            }
            else
            {
                //Parametro = 119;
                //Parametro = 251;
                //Parametro = 252;

                Parametro = 5577;
            }
            
            if (operacao == 2)
            {
                new frmEnviarNotaFiscal(Parametro, operacao);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmEnviarNotaFiscal(Parametro, operacao));
            }

        }
    }
}
