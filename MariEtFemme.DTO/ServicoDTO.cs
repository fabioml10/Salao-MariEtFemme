using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class ServicoDTO
    {
        public int IdServico { get; set; }
        public string DescricaoServico { get; set; }
    }
    public class ServicoCollectionDTO : List<ServicoDTO>
    {
    }
}
