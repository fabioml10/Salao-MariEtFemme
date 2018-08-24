using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class NotaProdutoDTO
    {
        public NotaProdutoDTO()
        {
            Nota = new NotaDTO();
            Produto = new ProdutoDTO();
        }
        public NotaDTO Nota { get; set; }
        public ProdutoDTO Produto { get; set; }
        public float QuantidadeComprada { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }
    }
    public class NotaProdutoCollectionDTO : List<NotaProdutoDTO>
    {

    }
}