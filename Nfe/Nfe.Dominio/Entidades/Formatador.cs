using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nfe.Dominio
{
    public static class Formatador
    {
        public static string FormatDecimal(decimal? num)
        {
            if (num != null)
            {
                decimal nume = Convert.ToDecimal(num);
                return nume.ToString("0.00").Replace(',', '.');
            }
            else
            {
                return string.Empty;
            }
        }
      
    }
}
