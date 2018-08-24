using System;
using MariEtFemme.DTO;

namespace MariEtFemme.Agendamento
{
    public class AppointmentEventArgs : EventArgs
    {
        public AppointmentEventArgs(AgendamentoDTO appointment)
        {
            m_Appointment = appointment;
        }

        private AgendamentoDTO m_Appointment;

        public AgendamentoDTO Appointment
        {
            get { return m_Appointment; }
        }

    }
}