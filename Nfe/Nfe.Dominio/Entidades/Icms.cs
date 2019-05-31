using System;
using System.Collections.Generic;
using System.Text;

namespace Nfe.Dominio.Entidades
{
    public class Icms : Entidade
    {
        public string CST { get; set; }
        public int orig { get; set; }
        public string modBC { get; set; }
        public string pRedBC { get; set; }
        public string vBC { get; set; }
        public string pICMS { get; set; }
        public decimal vICMS { get; set; }
        public string modBCST { get; set; }
        public string pMVAST { get; set; }
        public string pRedBCST { get; set; }
        public decimal vBCST { get; set; }
        public decimal pICMSST { get; set; }
        public decimal vICMSST { get; set; }

        public int QtErros { get; set; }
        public Icms(int id, string cst,int orig)
        {
            this.SetId(id);
            this.CST = cst;
        }
        public void Valida()
        {
            switch (this.CST)
            {
                case "00":
                    ValidaICMS00();
                    break;
                case "10":
                    ValidaICMS10();
                    break;
                case "20":
                    ValidaICMS20();
                    break;
                case "30":
                    ValidaICMS30();
                    break;
                case "40":
                    ValidaICMS40();
                    break;
                case "41":
                    ValidaICMS41();
                    break;
                case "50":
                    ValidaICMS50();
                    break;
                case "51":
                    ValidaICMS51();
                    break;
                case "60":
                    ValidaICMS60();
                    break;
                case "70":
                    ValidaICMS70();
                    break;
                case "90":
                    ValidaICMS90();
                    break;
            }
        }
        private void ValidaICMS00()
        {
            if (!OrigemValido()) { QtErros += 1; }
        }
        private void ValidaICMS10()
        {
            if (!OrigemValido()) { QtErros += 1; }
        }
        private void ValidaICMS20()
        {
            if (!OrigemValido()) { QtErros += 1; }
        }
        private void ValidaICMS30()
        {
            if (!OrigemValido()) { QtErros += 1; }
        }
        private void ValidaICMS40()
        {
            if (!OrigemValido()) { QtErros += 1; }
        }
        private void ValidaICMS41()
        {
            if (!OrigemValido()) { QtErros += 1; }
        }
        private void ValidaICMS50()
        {
            if (!OrigemValido()) { QtErros += 1; }
        }
        private void ValidaICMS51()
        {
            if (!OrigemValido()) { QtErros += 1; }
        }
        private void ValidaICMS60()
        {
            if (!OrigemValido()) { QtErros += 1; }
            if (!vBCSTEValido()) { QtErros += 1; }
            if (!vICMSSTEValido()) { QtErros += 1; }
        }
        private void ValidaICMS70()
        {
            if (!OrigemValido()) { QtErros += 1; }
        }
        private void ValidaICMS90()
        {
            if (!OrigemValido()) { QtErros += 1; }
        }
        private bool OrigemValido()
        {
            if (this.orig >= 0 && this.orig <=2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool vBCSTEValido()
        {
            if (this.vBCST > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool vICMSSTEValido()
        {
            if (vICMSST > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
