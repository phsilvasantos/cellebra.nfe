using System;
using System.Collections.Generic;

namespace Nfe.Infra.AcessoDados
{
    public class Entidade
    {
        public int Id { get; private set; }

        public void SetId(int id)
        {
            this.Id = id;
        }
    }
}
