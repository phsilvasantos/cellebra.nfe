using System;
using System.Collections.Generic;
using System.Text;

namespace Nfe.Dominio.Entidades
{
    
    public class Entidade
    {        
        public int  Id { get; private set; }
        public void SetId(int id)
        {
            this.Id = id;
        }
    }
}
