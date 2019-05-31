using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nfe.Dominio.Entidades;
using Nfe.Infra.Configuracao;

namespace Nfe.Infra.AcessoDados
{
    public class RepositorioRecibo_REVISAR
    {
        private ConfiguracaoApp config { get; set; }
        public RepositorioRecibo_REVISAR()
        {
            config = new ConfiguracaoApp();
        }
        public RepositorioRecibo_REVISAR(string numero)
        {
            config = new ConfiguracaoApp();
        }
    }
}
