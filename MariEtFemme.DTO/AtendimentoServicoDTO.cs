using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class AtendimentoServicoDTO
    {
        public AtendimentoServicoDTO()
        {
            Atendimento = new AtendimentoDTO();
            Servico = new ServicoDTO();
        }
        public AtendimentoDTO Atendimento { get; set; }
        public ServicoDTO Servico { get; set; }
    }

    public class AtendimentoServicoCollectionDTO : List<AtendimentoServicoDTO>
    {
    }
}
