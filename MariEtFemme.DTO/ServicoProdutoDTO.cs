using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class ServicoProdutoDTO
    {
        public ServicoProdutoDTO()
        {
            Servico = new ServicoDTO();
            Produto = new ProdutoDTO();
        }
        public ServicoDTO Servico { get; set; }
        public ProdutoDTO Produto { get; set; }
    }

    public class ServicoProdutoCollectionDTO : List<ServicoProdutoDTO>
    {
    }
}