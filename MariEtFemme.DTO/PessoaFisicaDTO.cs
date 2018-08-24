using System;
using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class PessoaFisicaDTO
    {
        public DateTime? Nascimento { get; set; }
        public bool? Genero { get; set; }
    }

    public class PessoaFisicaCollectionDTO : List<PessoaFisicaDTO>
    {
    }
}