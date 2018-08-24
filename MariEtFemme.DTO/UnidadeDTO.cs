using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class UnidadeDTO
    {
        public int IdUnidade { get; set; }
        public string SiglaUnidade { get; set; }
        public string DescricaoUnidade { get; set; }
    }

    public class UnidadeCollectionDTO : List<UnidadeDTO>
    {
    }
}
