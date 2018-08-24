﻿using System;
using System.Drawing;
using System.Windows.Forms;
using MariEtFemme.DTO;

namespace MariEtFemme.Agendamento
{
    public class SelectionTool : ITool
    {
        private DayView dayView;
        public DayView DayView
        {
            get
            {
                return dayView;
            }
            set
            {
                dayView = value;
            }
        }

        #region Variables

        private DateTime startDate;
        private TimeSpan length;
        private Mode mode;
        private TimeSpan delta;

        #endregion

        #region Event Handler

        public event EventHandler Complete;

        #endregion

        /// <summary>
        /// Não sei o que este método faz
        /// </summary>>
        public void Reset()
        {
            length = TimeSpan.Zero;
            delta = TimeSpan.Zero;
        }

        /// <summary>
        /// Responsável pelas alterações de posição e tamanho do retângulo, reagendamento e modificação da duração do agendamento
        /// </summary>>
        public void MouseMove(MouseEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            AgendamentoDTO selection = dayView.SelectedAppointment;
            Rectangle viewrect = dayView.GetTrueRectangle();
            Rectangle fdrect = dayView.GetFullDayApptsRectangle();

            if (viewrect.Contains(e.Location) || fdrect.Contains(e.Location))
            {
                if ((selection != null) && (!selection.Locked))
                {
                    switch (e.Button)
                    {                        
                        case MouseButtons.Left:
                            // Get time at mouse position
                            DateTime m_Date = dayView.GetTimeAt(e.X, e.Y);
                            //Move o retangulo de posição
                            switch (mode)
                            {
                                case Mode.Move:
                                    // This works for regular (i.e. non full-day or multi-day appointments)
                                    if (!selection.AllDayEvent && viewrect.Contains(e.Location))
                                    {
                                        // add delta value
                                        m_Date = m_Date.Add(delta);

                                        if (length == TimeSpan.Zero)
                                        {
                                            startDate = selection.StartDate;
                                            length = selection.EndDate - startDate;
                                        }
                                        else
                                        {
                                            DateTime m_EndDate = m_Date.Add(length);

                                            if (m_EndDate.Day == m_Date.Day)
                                            {
                                                selection.StartDate = m_Date;
                                                selection.EndDate = m_EndDate;
                                                dayView.Invalidate();
                                                dayView.RaiseAppointmentMove(new AppointmentEventArgs(selection));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (fdrect.Contains(e.Location))
                                        {
                                            m_Date = m_Date.Add(delta);

                                            int m_DateDiff = m_Date.Subtract(selection.StartDate).Days;

                                            if (m_DateDiff != 0)
                                            {
                                                if (selection.StartDate.AddDays(m_DateDiff) > dayView.StartDate)
                                                {
                                                    selection.StartDate = selection.StartDate.AddDays(m_DateDiff);
                                                    selection.EndDate = selection.EndDate.AddDays(m_DateDiff);
                                                    dayView.Invalidate();
                                                    dayView.RaiseAppointmentMove(new AppointmentEventArgs(selection));
                                                }
                                            }
                                        }
                                    }

                                    break;

                                case Mode.ResizeBottom:

                                    if (m_Date > selection.StartDate)
                                    {
                                        if (selection.EndDate.Day == m_Date.Day)
                                        {
                                            selection.EndDate = m_Date;
                                            dayView.Invalidate();
                                            dayView.RaiseAppointmentMove(new AppointmentEventArgs(selection));
                                        }
                                    }

                                    break;

                                case Mode.ResizeTop:

                                    if (m_Date < selection.EndDate)
                                    {
                                        if (selection.StartDate.Day == m_Date.Day)
                                        {
                                            selection.StartDate = m_Date;
                                            dayView.Invalidate();
                                            dayView.RaiseAppointmentMove(new AppointmentEventArgs(selection));
                                        }
                                    }
                                    break;

                                case Mode.ResizeLeft:
                                    if (m_Date.Date < selection.EndDate.Date)
                                    {
                                        selection.StartDate = m_Date.Date;
                                        dayView.Invalidate();
                                        dayView.RaiseAppointmentMove(new AppointmentEventArgs(selection));
                                    }
                                    break;

                                case Mode.ResizeRight:
                                    if (m_Date.Date >= selection.StartDate.Date)
                                    {
                                        selection.EndDate = m_Date.Date.AddDays(1);
                                        dayView.Invalidate();
                                        dayView.RaiseAppointmentMove(new AppointmentEventArgs(selection));
                                    }
                                    break;
                            }
                            break;

                        default:

                            Mode tmpNode = GetMode(e);

                            switch (tmpNode)
                            {
                                case Mode.Move:
                                    dayView.Cursor = Cursors.Default;
                                    break;
                                case Mode.ResizeBottom:
                                case Mode.ResizeTop:
                                    if (!selection.AllDayEvent)
                                        dayView.Cursor = Cursors.SizeNS;
                                    break;
                                case Mode.ResizeLeft:
                                case Mode.ResizeRight:
                                    if (selection.AllDayEvent)
                                        DayView.Cursor = Cursors.SizeWE;
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Não sei o que este método faz
        /// </summary>>
        private Mode GetMode(MouseEventArgs e)
        {
            DayView.AppointmentView view = null;
            Boolean gotview = false;

            if (dayView.SelectedAppointment == null)
                return Mode.None;

            if (dayView.appointmentViews.ContainsKey(dayView.SelectedAppointment))
            {
                view = dayView.appointmentViews[dayView.SelectedAppointment];
                gotview = true;
            }

            else if (dayView.longappointmentViews.ContainsKey(dayView.SelectedAppointment))
            {
                view = dayView.longappointmentViews[dayView.SelectedAppointment];
                gotview = true;
            }

            if (gotview)
            {
                Rectangle topRect = view.Rectangle;
                Rectangle bottomRect = view.Rectangle;
                Rectangle leftRect = view.Rectangle;
                Rectangle rightRect = view.Rectangle;

                bottomRect.Y = bottomRect.Bottom - 5;
                bottomRect.Height = 5;
                topRect.Height = 5;
                leftRect.Width = 5;
                rightRect.X += rightRect.Width - 5;
                rightRect.Width = 5;

                if (topRect.Contains(e.Location))
                    return Mode.ResizeTop;
                else if (bottomRect.Contains(e.Location))
                    return Mode.ResizeBottom;
                else if (rightRect.Contains(e.Location))
                    return Mode.ResizeRight;
                else if (leftRect.Contains(e.Location))
                    return Mode.ResizeLeft;
                else
                    return Mode.Move;
            }

            return Mode.None;
        }

        /// <summary>
        /// Método que inicia quando o retângulo é clicado
        /// </summary>
        public void MouseUp(MouseEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            if (e.Button == MouseButtons.Right)
            {
                if (Complete != null)
                    Complete(this, EventArgs.Empty);
            }

            dayView.RaiseSelectionChanged(EventArgs.Empty);
            mode = Mode.Move;
            delta = TimeSpan.Zero;
        }

        /// <summary>
        /// Método que inicia quando o retângulo é clicado
        /// </summary>
        public void MouseDown(MouseEventArgs e)
        {
            if (dayView.SelectedAppointmentIsNew)
            {
                dayView.RaiseNewAppointment();
            }

            if (dayView.CurrentlyEditing)
                dayView.FinishEditing(false);

            mode = GetMode(e);

            if (dayView.SelectedAppointment != null)
            {
                DateTime downPos = dayView.GetTimeAt(e.X, e.Y);
                // Calculate delta time between selection and clicked point
                delta = dayView.SelectedAppointment.StartDate - downPos;
            }
            else
            {
                delta = TimeSpan.Zero;
            }

            length = TimeSpan.Zero;
        }        

        enum Mode
        {
            ResizeTop,
            ResizeBottom,
            ResizeLeft,
            ResizeRight,
            Move,
            None
        }
    }
}