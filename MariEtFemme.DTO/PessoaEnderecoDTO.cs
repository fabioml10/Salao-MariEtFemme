using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class PessoaEnderecoDTO
    {
        public PessoaEnderecoDTO()
        {
            Estado = new EstadoDTO();
        }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public EstadoDTO Estado { get; set; }
    }

    public class PessoaEnderecoCollectionDTO : List<PessoaEnderecoDTO>
    {
    }
}
