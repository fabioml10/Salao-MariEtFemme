using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class PessoaJuridicaDTO
    {
        public string RazaoSocial { get; set; }
        public string CNPJ { get; set; }
    }

    public class PessoaJuridicaCollectionDTO : List<PessoaJuridicaDTO>
    {
    }
}
