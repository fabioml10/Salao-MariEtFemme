using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class EstoqueDTO
    {
        public EstoqueDTO()
        {
            Filial = new FilialDTO();
            Produto = new ProdutoDTO();
        }
        public FilialDTO Filial { get; set; }
        public ProdutoDTO Produto { get; set; }
        public float Quantidade { get; set; }
    }

    public class EstoqueCollectionDTO : List<EstoqueDTO>
    {
    }
}