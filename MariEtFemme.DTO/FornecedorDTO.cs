using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class FornecedorDTO
    {
        public FornecedorDTO()
        {
            Pessoa = new PessoaDTO();
        }
        public PessoaDTO Pessoa { get; set; }

        private string _nomeTabela = "pessoa_fornecedor";
        public string NomeTabela
        {
            get
            {
                return _nomeTabela;
            }
        }
    }

    public class FornecedorCollectionDTO : List<FornecedorDTO>
    {
    }
}