using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class EstadoDTO
    {
        public int IdEstado { get; set; }
        public string SiglaEstado { get; set; }
        public string DescricaoEstado { get; set; }
    }

    public class EstadoCollectionDTO : List<EstadoDTO>
    {
    }
}