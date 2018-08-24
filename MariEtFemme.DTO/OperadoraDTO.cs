using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class OperadoraDTO
    {
        public int IdOperadora { get; set; }
        public string DescricaoOperadora { get; set; }
    }

    public class OperadoraCollectionDTO : List<OperadoraDTO>
    {
    }
}