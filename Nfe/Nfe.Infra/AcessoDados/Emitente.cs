using System;
using Nfe.Dominio.Entidades;
using Nfe.Infra.AcessoDados;

namespace Nfe.Infra.AcessoDados
{
    
    public class RepositorioEmitente
    {
        private Emitente emitente { get; set; }
        private RepositorioEndereco endereco { get; set; }

        public RepositorioEmitente()
        {
            emitente = new Emitente();
        }

        public Emitente getDefault()
        {
            EmpresaInfo InfoClass = new EmpresaInfo();
            NfeInformacoesEmpresa Empresa = new NfeInformacoesEmpresa();

            Empresa = InfoClass.GetEmpresaInfo();

            emitente.TipoPessoa = TipoPesssoa.Juridico;
            emitente.Cnpj = Empresa.CNPJ;
            emitente.RazaoSocial = Empresa.NomeEmpresa;
            emitente.NomeFantasia = Empresa.NomeFantasia;
            emitente.IE = Empresa.InscEstadual;
            emitente.IM = Empresa.InscMunicipal;
            emitente.CNAE = Empresa.CNAE;
            emitente.CRT = Empresa.CRT;
            emitente.endereco = Empresa.Endereco;
            return emitente;
        }
    }
}
