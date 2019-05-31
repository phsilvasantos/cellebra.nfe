using System;
using System.Collections.Generic;
using System.Text;

namespace Nfe.Dominio.Entidades
{
    public class Recibo
    {
        public int tpAmb { get; set; }
        public string verAplic { get; set; }
        public int cStat { get; set; }
        public string xMotivo { get; set; }
        public int cUF { get; set; }
        public string nRec { get; set; }
        public Recibo(int tpamb, string veraplic, int cstat, string xmotivo, int cuf, string nrec)
        {
            this.tpAmb = tpamb;
            this.verAplic = veraplic;
            this.cStat = cstat;
            this.xMotivo = xmotivo;
            this.cUF = cuf;
            this.nRec = nrec;            
        }
    }
}
