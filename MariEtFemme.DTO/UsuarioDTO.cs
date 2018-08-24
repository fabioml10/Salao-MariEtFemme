using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class UsuarioDTO
    {
        public UsuarioDTO()
        {
            Privilegio = new PrivilegioDTO();
        }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public PrivilegioDTO Privilegio { get; set; }
        public bool Situacao { get; set; }
        public string DescricaoSituacao { get; set; }
    }

    public class PessoaUsuarioCollectionDTO : List<UsuarioDTO>
    {
    }
}