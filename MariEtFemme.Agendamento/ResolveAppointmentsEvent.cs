using System;
using MariEtFemme.DTO;

namespace MariEtFemme.Agendamento
{
    public class ResolveAppointmentsEventArgs : EventArgs
    {
        public ResolveAppointmentsEventArgs(DateTime start, DateTime end)
        {
            m_StartDate = start;
            m_EndDate = end;
            m_Appointments = new AgendamentoCollectionDTO();
        }

        private DateTime m_StartDate;

        public DateTime StartDate
        {
            get { return m_StartDate; }
            set { m_StartDate = value; }
        }

        private DateTime m_EndDate;

        public DateTime EndDate
        {
            get { return m_EndDate; }
            set { m_EndDate = value; }
        }

        private AgendamentoCollectionDTO m_Appointments;

        public AgendamentoCollectionDTO Appointments
        {
            get { return m_Appointments; }
            set { m_Appointments = value; }
        }
    }

    public delegate void ResolveAppointmentsEventHandler(object sender, ResolveAppointmentsEventArgs args);

}