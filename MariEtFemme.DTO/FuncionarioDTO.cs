using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class FuncionarioDTO
    {
        public FuncionarioDTO()
        {
            Pessoa = new PessoaDTO();
            Cargo = new CargoDTO();
            Filial = new FilialDTO();
            Usuario = new UsuarioDTO();
        }
        public PessoaDTO Pessoa { get; set; }
        public CargoDTO Cargo { get; set; }
        public FilialDTO Filial { get; set; }
        public UsuarioDTO Usuario { get; set; }

        private string _nomeTabela = "pessoa_funcionario";
        public string NomeTabela
        {
            get
            {
                return _nomeTabela;
            }
        }
    }

    public class FuncionarioCollectionDTO : List<FuncionarioDTO>
    {
    }
}