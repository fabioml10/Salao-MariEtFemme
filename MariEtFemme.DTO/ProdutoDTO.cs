using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class ProdutoDTO
    {
        public ProdutoDTO()
        {
            Unidade = new UnidadeDTO();
        }
        public int IdProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public UnidadeDTO Unidade { get; set; }
        public float Consumo { get; set; }
    }

    public class ProdutoCollectionDTO : List<ProdutoDTO>
    {
    }
}