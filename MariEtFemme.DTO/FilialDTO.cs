using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class FilialDTO
    {
        public FilialDTO()
        {
            Pessoa = new PessoaDTO();
        }
        public PessoaDTO Pessoa { get; set; }
        private string _nomeTabela = "pessoa_filial";
        public string NomeTabela
        {
            get
            {
                return _nomeTabela;
            }
        }
    }

    public class FilialCollectionDTO : List<FilialDTO>
    {
    }
}