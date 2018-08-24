using System;
using System.Drawing;
using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class AgendamentoDTO
    {
        public AgendamentoDTO()
        {
            Color = Color.White;
            BorderColor = Color.Blue;
            //TextColor = Color.White;
            Locked = false;
            DrawBorder = false;
            AllDayEvent = false;

            Cliente = new ClienteDTO();
            Funcionario = new FuncionarioDTO();
            Servicos = new ServicoCollectionDTO();
        }

        public int IdAgendamento { get; set; }
        public ClienteDTO Cliente { get; set; }
        public FuncionarioDTO Funcionario { get; set; }
        public ServicoCollectionDTO Servicos { get; set; }

        /// <summary>
        /// Escreve os serviços no retângulo do agendamento
        /// </summary>
        public string Title { get; set; }
        public string Observacoes { get; set; }

        /// <summary>
        /// Número da agenda do funcionário (Id do funcionário)
        /// </summary>
        public int Layer { get; set; }

        /// <summary>
        /// Não sei o que faz
        /// </summary>
        public int Atendido { get; set; }

        /// <summary>
        /// Não sei o que faz
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Data de início do agendamento
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Data do fim do agendamento
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Trava o agendamento, não se pode mover nem redimensionar
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// Cor de fundo do agendamento
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Não funciona
        /// </summary>
        public Color TextColor { get; set; }

        /// <summary>
        /// Desenha uma borda ao redor do agendamento
        /// </summary>
        public bool DrawBorder { get; set; }

        /// <summary>
        /// Cor da borda esquerda do agendamento
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// Seta o agendamento como para o dia inteiro
        /// </summary>
        public bool AllDayEvent { get; set; }

        /// <summary>
        /// Não sei o que faz
        /// </summary>
        public int conflictCount;
    }

    public class AgendamentoCollectionDTO : List<AgendamentoDTO>
    {
    }
}
