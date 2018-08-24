using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class ClienteDTO
    {
        public ClienteDTO()
        {
            Pessoa = new PessoaDTO();
        }
        public PessoaDTO Pessoa { get; set; }
        private string _nome_tabela = "pessoa_cliente";
        public string NomeTabela
        {
            get
            {
                return _nome_tabela;
            }
        }
    }

    public class ClienteCollectionDTO : List<ClienteDTO>
    {
    }
}