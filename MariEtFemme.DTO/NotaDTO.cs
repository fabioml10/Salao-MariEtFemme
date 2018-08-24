using System;
using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class NotaDTO
    {
        public NotaDTO()
        {
            Filial = new FilialDTO();
            Fornecedor = new FornecedorDTO();
        }
        public int IdNota { get; set; }
        public FilialDTO Filial { get; set; }
        public FornecedorDTO Fornecedor { get; set; }
        public string NumeroNota { get; set; }
        public DateTime DataNota { get; set; }
    }

    public class NotaCollectionDTO : List<NotaDTO>
    {
    }
}
