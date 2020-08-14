using System;
using System.Collections.Generic;
using System.Text;

namespace Nfe.Infra
{
    class MensagemRetorno
    {
        public string MensagemDeRetorno(int CodMensagem, string Mensagem)
        {

            if (CodMensagem == 100)
            {
                Mensagem = "Nota Fiscal enviada com sucesso";
            }

            if (CodMensagem == 101 || CodMensagem == 128 )
            {
                Mensagem = "Nota Fiscal cancelada com sucesso";
            }

            return Mensagem;
        }
    }

    
}
