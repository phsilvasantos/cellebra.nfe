using System;
using System.Collections.Generic;
using Nfe.Dominio.Entidades;
using Nfe.Infra.Configuracao;

namespace Nfe.Infra.AcessoDados
{
    public class RepositorioRecibo
    {
        private ConfiguracaoApp config { get; set; }
        public RepositorioRecibo()
        {
            config = new ConfiguracaoApp();
        }
        public RepositorioRecibo(string numero)
        {
            config = new ConfiguracaoApp();
        }
    }
}
