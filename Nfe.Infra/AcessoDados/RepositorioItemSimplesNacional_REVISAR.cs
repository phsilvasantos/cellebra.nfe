using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Nfe.Dominio.Entidades;
using Nfe.Infra.Configuracao;

namespace Nfe.Infra.AcessoDados
{
    public class RepositorioItemSimplesNacional_REVISAR
    {
        public List<ItemSimplesNacional> listItem;

        public RepositorioItemSimplesNacional_REVISAR()
        {
            listItem = new List<ItemSimplesNacional>();
        }

        public List<ItemSimplesNacional> getById(int itemProd)
        {
            var dados = _model.SimpNacionals.Where(i => i.idItem == itemProd).Select(e => e);
            foreach (var item in dados)
            {
                listItem.Add(new ItemSimplesNacional(item.CSOSN,item.orig,item.pCredSN,item.vCredICMSSN,item.modBCST,item.pMVAST,item.pRedBCST,
                    item.vBCST,item.pICMSST,item.vICMSST,item.vBCSTRet,item.vICMSSTRet,item.modBC,item.vBC,item.pRedBC,item.pICMS,item.vICMS));
            }
            return listItem;
        }
    }
}
