using System;
using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class AtendimentoDTO
    {
        public AtendimentoDTO()
        {
            Funcionario = new FuncionarioDTO();
            Cliente = new ClienteDTO();
        }
        public int IdAtendimento { get; set; }
        public FuncionarioDTO Funcionario { get; set; }
        public ClienteDTO Cliente { get; set; }
        public DateTime DataAtendimento { get; set; }
        public string ComenariosAtendimento { get; set; }
    }

    public class AtendimentoCollectionDTO : List<AtendimentoDTO>
    {
    }
}
