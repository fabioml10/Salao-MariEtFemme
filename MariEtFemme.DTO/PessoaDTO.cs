using System;
using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class PessoaDTO
    {
        public PessoaDTO()
        {
            PessoaFisica = new PessoaFisicaDTO();
            PessoaJuridica = new PessoaJuridicaDTO();
            Endereco = new PessoaEnderecoDTO();
            Contato = new PessoaContatoDTO();
        }

        public int? IdPessoa { get; set; }
        public string NomePessoa { get; set; }
        public bool TipoPessoa { get; set; }
        public string DescricaoTipoPessoa { get; set; }
        public string Comentarios { get; set; }
        public DateTime DataCadastro { get; set; }
        public PessoaFisicaDTO PessoaFisica { get; set; }
        public PessoaJuridicaDTO PessoaJuridica { get; set; }
        public PessoaEnderecoDTO Endereco { get; set; }
        public PessoaContatoDTO Contato { get; set; }

        public override string ToString()
        {
            return NomePessoa;
        }
    }

    public class PessoaCollectionDTO : List<PessoaDTO>
    {
    }
}