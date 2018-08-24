using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class PessoaContatoDTO
    {
        public PessoaContatoDTO()
        {
            Operadora1 = new OperadoraDTO();
            Operadora2 = new OperadoraDTO();
            Operadora3 = new OperadoraDTO();
        }
        public string Telefone1 { get; set; }
        public OperadoraDTO Operadora1 { get; set; }
        public bool WhatsApp1 { get; set; }
        public string Telefone2 { get; set; }
        public OperadoraDTO Operadora2 { get; set; }
        public bool WhatsApp2 { get; set; }
        public string Telefone3 { get; set; }
        public OperadoraDTO Operadora3 { get; set; }
        public bool WhatsApp3 { get; set; }
        public string Email { get; set; }
    }

    public class PessoaContatoCollectionDTO : List<PessoaContatoDTO>
    {
    }
}