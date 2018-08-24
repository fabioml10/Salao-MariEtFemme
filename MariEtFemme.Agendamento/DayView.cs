using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using MariEtFemme.DTO;

namespace MariEtFemme.Agendamento
{
    public partial class DayView : Control
    {
        #region Variables

        private UserControl1 meuControle;
        private VScrollBar scrollbar;
        private DrawTool drawTool;
        private SelectionTool selectionTool;
        private int allDayEventsHeaderHeight = 20;

        private DateTime workStart;
        private DateTime workEnd;

        /// <summary>
        /// Cronometro para evitar agendamentos acidentais
        /// </summary>
        Stopwatch stopwatch;

        #endregion

        #region Constants

        /// <summary>
        /// Espessura do cabeçalho da indicação das horas
        /// </summary>
        private int hourLabelWidth = 50;
        private int hourLabelIndent = 2;
        /// <summary>
        /// Altura do cabeçalho do dia ou dias da semanda
        /// </summary>
        private int dayHeadersHeight = 30;
        /// <summary>
        /// Espessura do espaço em branco entre o cabeçalho das horas e os agendamentos(cor da borda)
        /// </summary>
        private int appointmentGripWidth = 5;
        private int horizontalAppointmentHeight = 20;

        public enum AppHeightDrawMode
        {
            TrueHeightAll = 0, // all appointments have height proportional to true length
            FullHalfHourBlocksAll = 1, // all appts drawn in half hour blocks
            EndHalfHourBlocksAll = 2, // all appts boxes start at actual time, end in half hour blocks
            FullHalfHourBlocksShort = 3, // short (< 30 mins) appts drawn in half hour blocks
            EndHalfHourBlocksShort = 4, // short appts boxes start at actual time, end in half hour blocks
        }

        #endregion

        #region c.tor

        public DayView()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, true);

            scrollbar = new VScrollBar();
            scrollbar.SmallChange = halfHourHeight;
            scrollbar.LargeChange = halfHourHeight * slotsPerHour;
            scrollbar.Dock = DockStyle.Right;
            scrollbar.Visible = allowScroll;
            scrollbar.Scroll += new ScrollEventHandler(scrollbar_Scroll);
            AdjustScrollbar();
            scrollbar.Value = (startHour * slotsPerHour * halfHourHeight);
            this.Controls.Add(scrollbar);

            meuControle = new UserControl1();
            meuControle.Visible = false;
            meuControle.KeyUp += new KeyEventHandler(editbox_KeyUp);
            meuControle.btnSave.Click += new EventHandler(salvar_Click);
            meuControle.btnCancel.Click += new EventHandler(cancelar_Click);
            meuControle.btnRemove.Click += new EventHandler(ReceberDeletarApontamento);
            meuControle.btnAttendance.Click += new EventHandler(IniciarAtendimentoSuccess);
            meuControle.VerifiqueAtendimento += new EventHandler(VerificarAtendimento);
            meuControle.txtColor.Click += new EventHandler(CordeFundo);
            meuControle.txtBorderColor.Click += new EventHandler(CordaBorda);
            meuControle.BorderStyle = BorderStyle.FixedSingle;
            meuControle.AutoScaleMode = AutoScaleMode.None;
            this.Controls.Add(meuControle);

            drawTool = new DrawTool();
            drawTool.DayView = this;

            selectionTool = new SelectionTool();
            selectionTool.DayView = this;
            selectionTool.Complete += new EventHandler(selectionTool_Complete);
            //Indica quando completa a seleção
            drawTool.Complete += new EventHandler(drawTool_Complete);

            activeTool = drawTool;

            UpdateWorkingHours();

            this.Renderer = new Office12Renderer();
        }

        #endregion

        #region Properties

        private AppHeightDrawMode appHeightMode = AppHeightDrawMode.TrueHeightAll;

        public AppHeightDrawMode AppHeightMode
        {
            get { return appHeightMode; }
            set { appHeightMode = value; }
        }

        private int halfHourHeight = 18;

        [DefaultValue(18)]
        public int HalfHourHeight
        {
            get
            {
                return halfHourHeight;
            }
            set
            {
                halfHourHeight = value;
                OnHalfHourHeightChanged();
            }
        }

        private void OnHalfHourHeightChanged()
        {
            AdjustScrollbar();
            Invalidate();
        }

        private AbstractRenderer renderer;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AbstractRenderer Renderer
        {
            get
            {
                return renderer;
            }
            set
            {
                renderer = value;
                OnRendererChanged();
            }
        }

        private void OnRendererChanged()
        {
            this.Font = renderer.BaseFont;
            this.Invalidate();
        }

        private bool ampmdisplay = false;

        public bool AmPmDisplay
        {
            get
            {
                return ampmdisplay;
            }
            set
            {
                ampmdisplay = value;
                OnAmPmDisplayChanged();
            }
        }

        private void OnAmPmDisplayChanged()
        {
            this.Invalidate();
        }

        private bool drawAllAppBorder = false;

        public bool DrawAllAppBorder
        {
            get
            {
                return drawAllAppBorder;
            }
            set
            {
                drawAllAppBorder = value;
                OnDrawAllAppBorderChanged();
            }
        }

        private void OnDrawAllAppBorderChanged()
        {
            this.Invalidate();
        }

        private bool minHalfHourApp = false;

        public bool MinHalfHourApp
        {
            get
            {
                return minHalfHourApp;
            }
            set
            {
                minHalfHourApp = value;
                Invalidate();
            }
        }

        private int daysToShow = 1;

        [DefaultValue(1)]
        public int DaysToShow
        {
            get
            {
                return daysToShow;
            }
            set
            {
                daysToShow = value;
                OnDaysToShowChanged();
            }
        }

        protected virtual void OnDaysToShowChanged()
        {
            if (this.CurrentlyEditing)
                FinishEditing(true);

            Invalidate();
        }

        private SelectionType selection;

        [Browsable(false)]
        public SelectionType Selection
        {
            get
            {
                return selection;
            }
        }

        private DateTime startDate;

        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;
                OnStartDateChanged();
            }
        }

        protected virtual void OnStartDateChanged()
        {
            startDate = startDate.Date;

            selectedAppointment = null;
            selectedAppointmentIsNew = false;
            selection = SelectionType.DateRange;

            Invalidate();
        }

        private int startHour = 8;

        [DefaultValue(8)]
        public int StartHour
        {
            get
            {
                return startHour;
            }
            set
            {
                startHour = value;
                OnStartHourChanged();
            }
        }

        protected virtual void OnStartHourChanged()
        {
            if ((startHour * slotsPerHour * halfHourHeight) > scrollbar.Maximum) //maximum is lower on larger forms
            {
                scrollbar.Value = scrollbar.Maximum;
            }
            else
            {
                scrollbar.Value = (startHour * slotsPerHour * halfHourHeight);
            }

            Invalidate();
        }

        private AgendamentoDTO selectedAppointment;

        [Browsable(false)]
        public AgendamentoDTO SelectedAppointment
        {
            get { return selectedAppointment; }
            //Linha que permite indicar um apontamento como selecionado
            set { selectedAppointment = value; }
        }

        private DateTime selectionStart;

        public DateTime SelectionStart
        {
            get { return selectionStart; }
            set { selectionStart = value; }
        }

        private DateTime selectionEnd;

        public DateTime SelectionEnd
        {
            get { return selectionEnd; }
            set { selectionEnd = value; }
        }

        private ITool activeTool;

        [Browsable(false)]
        public ITool ActiveTool
        {
            get { return activeTool; }
            set { activeTool = value; }
        }

        [Browsable(false)]
        public bool CurrentlyEditing
        {
            get
            {
                return meuControle.Visible;                
            }
        }

        private int workingHourStart = 8;

        [DefaultValue(8)]
        public int WorkingHourStart
        {
            get
            {
                return workingHourStart;
            }
            set
            {
                workingHourStart = value;
                UpdateWorkingHours();
            }
        }

        private int workingMinuteStart = 30;

        [DefaultValue(30)]
        public int WorkingMinuteStart
        {
            get
            {
                return workingMinuteStart;
            }
            set
            {
                workingMinuteStart = value;
                UpdateWorkingHours();
            }
        }

        private int workingHourEnd = 18;

        [DefaultValue(18)]
        public int WorkingHourEnd
        {
            get
            {
                return workingHourEnd;
            }
            set
            {
                workingHourEnd = value;
                UpdateWorkingHours();
            }
        }

        private int workingMinuteEnd = 30;

        [DefaultValue(30)]
        public int WorkingMinuteEnd
        {
            get { return workingMinuteEnd; }
            set
            {
                workingMinuteEnd = value;
                UpdateWorkingHours();
            }
        }

        private int slotsPerHour = 4;

        [DefaultValue(4)]
        public int SlotsPerHour
        {
            get
            {
                return slotsPerHour;
            }
            set
            {
                slotsPerHour = value;
                Invalidate();
            }
        }

        private void UpdateWorkingHours()
        {
            workStart = new DateTime(1, 1, 1, workingHourStart, workingMinuteStart, 0);
            workEnd = new DateTime(1, 1, 1, workingHourEnd, workingMinuteEnd, 0);

            Invalidate();
        }

        bool selectedAppointmentIsNew;

        public bool SelectedAppointmentIsNew
        {
            get
            {
                return selectedAppointmentIsNew;
            }
        }

        private bool allowScroll = true;

        [DefaultValue(true)]
        public bool AllowScroll
        {
            get
            {
                return allowScroll;
            }
            set
            {
                allowScroll = value;
                OnAllowScrollChanged();
            }
        }

        private void OnAllowScrollChanged()
        {
            this.scrollbar.Visible = this.AllowScroll;
        }

        private bool allowInplaceEditing = true;

        [DefaultValue(true)]
        public bool AllowInplaceEditing
        {
            get
            {
                return allowInplaceEditing;
            }
            set
            {
                allowInplaceEditing = value;
            }
        }

        private bool allowNew = true;

        [DefaultValue(true)]
        public bool AllowNew
        {
            get
            {
                return allowNew;
            }
            set
            {
                allowNew = value;
            }
        }        

        private int HeaderHeight
        {
            get
            {
                return dayHeadersHeight + allDayEventsHeaderHeight;
            }
        }

        #endregion

        #region Event Handlers

        #region Teste
        //Eventos para o formulário
        private void VerificarAtendimento(object sender, EventArgs e)
        {
            if (selectedAppointment.Atendido.Equals(0))
            {
                meuControle.btnAttendance.Enabled = true;
            }
            else
            {
                meuControle.btnAttendance.Enabled = false;
            }
        }

        void salvar_Click(object sender, EventArgs e)
        {
            //Cursor.Clip = new Rectangle();
            //ClipCursor(IntPtr.Zero);
            FinishEditing(false);
        }

        void cancelar_Click(object sender, EventArgs e)
        {
            //Cursor.Clip = new Rectangle();
            //ClipCursor(IntPtr.Zero);
            FinishEditing(true);
        }

        private void CordaBorda(object sender, EventArgs e)
        {
            selectedAppointment.BorderColor = meuControle.PegarCorBorda();
        }
        private void CordeFundo(object sender, EventArgs e)
        {
            selectedAppointment.Color = meuControle.PegarCorFundo();
        }

        /// <summary>
        /// Cria um novo apontamento quando o botão do mouse é solto. vem da classe DrawTool, metodo MouseUp
        /// </summary>
        public void drawTool_Complete(object sender, EventArgs e)
        {
            if (newSuccess != null)
            {
                newSuccess(this, new EventArgs());
            }

            //Inicia um novo apontamento com a tela de edição aberta
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(EnterEditMode));
        }
        #endregion

        void editbox_KeyUp(object sender, KeyEventArgs e)
        {
            //Quando o esc é pressionado calcelam as alterações
            if (e.KeyCode == Keys.Escape)
            {
                meuControle.Visible = false;
                e.Handled = true;
                FinishEditing(true);
            }
        }

        void selectionTool_Complete(object sender, EventArgs e)
        {
            if (selectedAppointment != null)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(EnterEditMode));
            }
        }

        /// <summary>
        /// Ver para que serve
        /// </summary>
        void scrollbar_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate();

            if (meuControle.Visible)
                //scroll text box too
                meuControle.Top += e.OldValue - e.NewValue;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, height, specified);
            AdjustScrollbar();
        }

        private void AdjustScrollbar()
        {
            scrollbar.Maximum = (slotsPerHour * halfHourHeight * 25) - this.Height + this.HeaderHeight;
            scrollbar.Minimum = 0;
        }

        /*protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Flicker free
        }*/

        /// <summary>
        /// Dispara quando se clica no retângulo
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            //Inicia o cronômetro para saber quanto tempo o mouse ficou clicado. Para evitar inserção de agendamentos de maneira indevida.
            stopwatch = Stopwatch.StartNew();

            //Capture focus
            this.Focus();

            if (CurrentlyEditing)
            {
                FinishEditing(true);
            }

            if (selectedAppointmentIsNew)
            {
                RaiseNewAppointment();
            }

            ITool newTool = null;

            AgendamentoDTO appointment = GetAppointmentAt(e.X, e.Y);

            if (e.Y < HeaderHeight && e.Y > dayHeadersHeight && appointment == null)
            {
                if (selectedAppointment != null)
                {
                    selectedAppointment = null;
                    Invalidate();
                }

                newTool = drawTool;
                selection = SelectionType.None;

                base.OnMouseDown(e);
                return;
            }

            if (appointment == null)
            {
                if (selectedAppointment != null)
                {
                    selectedAppointment = null;
                    Invalidate();
                }

                newTool = drawTool;
                selection = SelectionType.DateRange;
            }
            else
            {
                newTool = selectionTool;
                selectedAppointment = appointment;
                selection = SelectionType.Appointment;

                Invalidate();
            }

            if (activeTool != null)
            {
                activeTool.MouseDown(e);
            }

            if ((activeTool != newTool) && (newTool != null))
            {
                newTool.Reset();
                newTool.MouseDown(e);
            }

            activeTool = newTool;

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (activeTool != null)
                activeTool.MouseMove(e);

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Dispara quando se clica no retângulo
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            //Verifica quanto tempo o mouse ficou clicado. Para evitar inserção de agendamentos de maneira indevida.
            stopwatch.Stop();
            long milliseconds = stopwatch.ElapsedMilliseconds;

            if(selectedAppointment == null)
            {
                if (milliseconds > 300)
                {
                    if (activeTool != null)
                        activeTool.MouseUp(e);

                    base.OnMouseUp(e);
                }
            }
            else
            {
                if (activeTool != null)
                    activeTool.MouseUp(e);

                base.OnMouseUp(e);
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta < 0)
            {//mouse wheel scroll down
                ScrollMe(true);
            }
            else
            {//mouse wheel scroll up
                ScrollMe(false);
            }
        }

        System.Collections.Hashtable cachedAppointments = new System.Collections.Hashtable();

        protected virtual void OnResolveAppointments(ResolveAppointmentsEventArgs args)
        {
            if (ResolveAppointments != null)
                ResolveAppointments(this, args);

            this.allDayEventsHeaderHeight = 0;

            // cache resolved appointments in hashtable by days.
            cachedAppointments.Clear();

            if ((selectedAppointmentIsNew) && (selectedAppointment != null))
            {
                if ((selectedAppointment.StartDate > args.StartDate) && (selectedAppointment.StartDate < args.EndDate))
                {
                    args.Appointments.Add(selectedAppointment);
                }
            }

            foreach (AgendamentoDTO appointment in args.Appointments)
            {
                int key;
                AgendamentoCollectionDTO list;

                if (appointment.StartDate.Day == appointment.EndDate.Day && appointment.AllDayEvent == false)
                {
                    key = appointment.StartDate.Day;
                }
                else
                {
                    key = -1;
                }

                list = (AgendamentoCollectionDTO)cachedAppointments[key];

                if (list == null)
                {
                    list = new AgendamentoCollectionDTO();
                    cachedAppointments[key] = list;
                }

                list.Add(appointment);
            }
        }

        internal void RaiseSelectionChanged(EventArgs e)
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(this, e);
            }
        }

        internal void RaiseAppointmentMove(AppointmentEventArgs e)
        {
            if (AppointmentMove != null)
            {
                AppointmentMove(this, e);
            }

            if (UpdateSuccess != null)
            {
                UpdateSuccess(this, new EventArgs());
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if ((allowNew) && char.IsLetterOrDigit(e.KeyChar))
            {
                if ((this.Selection == SelectionType.DateRange))
                {
                    if (!selectedAppointmentIsNew)
                        EnterNewAppointmentMode(e.KeyChar);
                }
            }
        }

        private void EnterNewAppointmentMode(char key)
        {
            AgendamentoDTO appointment = new AgendamentoDTO();
            appointment.StartDate = selectionStart;
            appointment.EndDate = selectionEnd;
            appointment.Title = key.ToString();

            selectedAppointment = appointment;
            selectedAppointmentIsNew = true;

            activeTool = selectionTool;

            Invalidate();

            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(EnterEditMode));
        }

        private delegate void StartEditModeDelegate(object state);

        private void EnterEditMode(object state)
        {
            if (!allowInplaceEditing)
                return;

            if (this.InvokeRequired)
            {
                AgendamentoDTO selectedApp = selectedAppointment;

                System.Threading.Thread.Sleep(200);

                if (selectedApp == selectedAppointment)
                    this.Invoke(new StartEditModeDelegate(EnterEditMode), state);
            }
            else
            {
                StartEditing();
            }
        }

        internal void RaiseNewAppointment()
        {
            NewAppointmentEventArgs args = new NewAppointmentEventArgs(selectedAppointment.Title, selectedAppointment.StartDate, selectedAppointment.EndDate);

            if (NewAppointment != null)
            {
                NewAppointment(this, args);
            }

            selectedAppointment = null;
            selectedAppointmentIsNew = false;

            Invalidate();
        }

        #endregion

        #region Public Methods

        public void ScrollMe(bool down)
        {
            if (this.AllowScroll)
            {
                int newScrollValue;

                if (down)
                {//mouse wheel scroll down
                    newScrollValue = this.scrollbar.Value + this.scrollbar.SmallChange;

                    if (newScrollValue < this.scrollbar.Maximum)
                        this.scrollbar.Value = newScrollValue;
                    else
                        this.scrollbar.Value = this.scrollbar.Maximum;
                }
                else
                {//mouse wheel scroll up
                    newScrollValue = this.scrollbar.Value - this.scrollbar.SmallChange;

                    if (newScrollValue > this.scrollbar.Minimum)
                        this.scrollbar.Value = newScrollValue;
                    else
                        this.scrollbar.Value = this.scrollbar.Minimum;
                }

                this.Invalidate();
            }
        }

        public Rectangle GetTrueRectangle()
        {
            Rectangle truerect;

            truerect = this.ClientRectangle;
            truerect.X += hourLabelWidth + hourLabelIndent;
            truerect.Width -= scrollbar.Width + hourLabelWidth + hourLabelIndent;
            truerect.Y += this.HeaderHeight;
            truerect.Height -= this.HeaderHeight;

            return truerect;
        }

        public Rectangle GetFullDayApptsRectangle()
        {
            Rectangle fulldayrect;
            fulldayrect = this.ClientRectangle;
            fulldayrect.Height = this.HeaderHeight - dayHeadersHeight;
            fulldayrect.Y += dayHeadersHeight;
            fulldayrect.Width -= (hourLabelWidth + hourLabelIndent + this.scrollbar.Width);
            fulldayrect.X += hourLabelWidth + hourLabelIndent;

            return fulldayrect;
        }

        public void StartEditing()
        {
            if (!selectedAppointment.Locked && appointmentViews.ContainsKey(selectedAppointment))
            {
                Rectangle editBounds = appointmentViews[selectedAppointment].Rectangle;

                editBounds.Inflate(-3, -3);
                //Posição do controle na edição do retângulo
                if (editBounds.X < 200)
                {
                    editBounds.X += appointmentGripWidth - 2;
                }
                else
                {
                    editBounds.X += appointmentGripWidth - 125;
                }

                editBounds.Height = meuControle.Height;
                editBounds.Width = meuControle.Width;

                meuControle.Bounds = editBounds;

                //Passa as informações das propriedades para o formulário
                meuControle.txtClientName.Text = selectedAppointment.Cliente.Pessoa.NomePessoa;
                meuControle.txtComments.Text = selectedAppointment.Observacoes;
                meuControle.CarregarDataGrid(selectedAppointment);
                meuControle.txtColor.BackColor = selectedAppointment.Color;
                meuControle.txtBorderColor.BackColor = selectedAppointment.BorderColor;
                meuControle.Visible = true;
                meuControle.Focus();
            }
        }

        public void FinishEditing(bool cancel)
        {
            meuControle.Visible = false;
            if (!cancel)
            {
                //Passa as informações do formulário para as propriedades
                if (selectedAppointment != null)
                {
                    selectedAppointment.Cliente.Pessoa.NomePessoa = meuControle.txtClientName.Text;
                    ////////////////////////////////////////////////////////////////////////////////////
                    selectedAppointment.Funcionario = Session.LoggedUser;
                    selectedAppointment.Observacoes = meuControle.txtComments.Text;
                    selectedAppointment.Servicos = meuControle.newCollection;
                }

                if (selectedAppointment.Servicos != null && selectedAppointment.Servicos.Count > 0)
                {
                    int temp = 1;
                    selectedAppointment.Title = string.Empty;
                    foreach (ServicoDTO item in selectedAppointment.Servicos)
                    {
                        
                        if (selectedAppointment.Servicos.Count == temp)
                        {
                            selectedAppointment.Title = selectedAppointment.Title + item.DescricaoServico;
                        }
                        else
                        {
                            selectedAppointment.Title = selectedAppointment.Title + item.DescricaoServico + ", ";
                        }
                        temp++;
                    }
                }

                //Lança o evento para fazer o update
                if (UpdateSuccess != null)
                {
                    UpdateSuccess(this, new EventArgs());
                }
            }
            else
            {
                if (selectedAppointmentIsNew)
                {
                    selectedAppointment = null;
                    selectedAppointmentIsNew = false;
                }
            }
            Invalidate();
            this.Focus();
        }

        public DateTime GetTimeAt(int x, int y)
        {
            int dayWidth = (this.Width - (scrollbar.Width + hourLabelWidth + hourLabelIndent)) / daysToShow;

            int hour = (y - this.HeaderHeight + scrollbar.Value) / halfHourHeight;
            x -= hourLabelWidth;

            DateTime date = startDate;

            date = date.Date;
            date = date.AddDays(x / dayWidth);

            if ((hour > 0) && (hour < 24 * slotsPerHour))
                date = date.AddMinutes((hour * (60 / slotsPerHour)));

            return date;
        }

        public AgendamentoDTO GetAppointmentAt(int x, int y)
        {

            foreach (AppointmentView view in appointmentViews.Values)
                if (view.Rectangle.Contains(x, y))
                    return view.Appointment;

            foreach (AppointmentView view in longappointmentViews.Values)
                if (view.Rectangle.Contains(x, y))
                    return view.Appointment;

            return null;
        }

        #endregion

        #region Drawing Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (CurrentlyEditing)
            {
                FinishEditing(false);
            }

            // resolve appointments on visible date range.
            ResolveAppointmentsEventArgs args = new ResolveAppointmentsEventArgs(this.StartDate, this.StartDate.AddDays(daysToShow));
            OnResolveAppointments(args);

            using (SolidBrush backBrush = new SolidBrush(renderer.BackColor))
                e.Graphics.FillRectangle(backBrush, this.ClientRectangle);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Calculate visible rectangle
            Rectangle rectangle = new Rectangle(0, 0, this.Width - scrollbar.Width, this.Height);

            Rectangle daysRectangle = rectangle;
            daysRectangle.X += hourLabelWidth + hourLabelIndent;
            daysRectangle.Y += this.HeaderHeight;
            daysRectangle.Width -= (hourLabelWidth + hourLabelIndent);

            if (e.ClipRectangle.IntersectsWith(daysRectangle))
            {
                DrawDays(e, daysRectangle);
            }            

            Rectangle hourLabelRectangle = rectangle;

            hourLabelRectangle.Y += this.HeaderHeight;

            DrawHourLabels(e, hourLabelRectangle);

            Rectangle headerRectangle = rectangle;

            headerRectangle.X += hourLabelWidth + hourLabelIndent;
            headerRectangle.Width -= (hourLabelWidth + hourLabelIndent);
            headerRectangle.Height = dayHeadersHeight;

            
            if (e.ClipRectangle.IntersectsWith(headerRectangle))
                DrawDayHeaders(e, headerRectangle);

            Rectangle scrollrect = rectangle;

            if (this.AllowScroll == false)
            {
                scrollrect.X = headerRectangle.Width + hourLabelWidth + hourLabelIndent;
                scrollrect.Width = scrollbar.Width;
                using (SolidBrush backBrush = new SolidBrush(renderer.BackColor))
                    e.Graphics.FillRectangle(backBrush, scrollrect);
            }

            
        }

        private void DrawHourLabels(PaintEventArgs e, Rectangle rect)
        {
            e.Graphics.SetClip(rect);

            for (int m_Hour = 0; m_Hour < 24; m_Hour++)
            {
                Rectangle hourRectangle = rect;

                hourRectangle.Y = rect.Y + (m_Hour * slotsPerHour * halfHourHeight) - scrollbar.Value;
                hourRectangle.X += hourLabelIndent;
                hourRectangle.Width = hourLabelWidth;

                renderer.DrawHourLabel(e.Graphics, hourRectangle, m_Hour, this.ampmdisplay);

                for (int slot = 0; slot < slotsPerHour; slot++)
                {
                    renderer.DrawMinuteLine(e.Graphics, hourRectangle);
                    hourRectangle.Y += halfHourHeight;
                }
            }

            e.Graphics.ResetClip();
        }

        private void DrawDayHeaders(PaintEventArgs e, Rectangle rect)
        {
            int dayWidth = rect.Width / daysToShow;

            // one day header rectangle
            Rectangle dayHeaderRectangle = new Rectangle(rect.Left, rect.Top, dayWidth, rect.Height);
            DateTime headerDate = startDate;

            for (int day = 0; day < daysToShow; day++)
            {
                if (day == 0)
                    dayHeaderRectangle.Width += (rect.Width % daysToShow) - 1;

                renderer.DrawDayHeader(e.Graphics, dayHeaderRectangle, headerDate);

                dayHeaderRectangle.X += dayWidth;
                headerDate = headerDate.AddDays(1);
            }
        }

        private Rectangle GetHourRangeRectangle(DateTime start, DateTime end, Rectangle baseRectangle)
        {
            Rectangle rect = baseRectangle;

            int startY;
            int endY;

            startY = (start.Hour * halfHourHeight * slotsPerHour) + ((start.Minute * halfHourHeight) / /*30*/(60 / slotsPerHour));
            endY = (end.Hour * halfHourHeight * slotsPerHour) + ((end.Minute * halfHourHeight) / /*30*/(60 / slotsPerHour));

            rect.Y = startY - scrollbar.Value + this.HeaderHeight;

            rect.Height = System.Math.Max(1, endY - startY);

            return rect;
        }

        private void DrawDay(PaintEventArgs e, Rectangle rect, DateTime time)
        {
            renderer.DrawDayBackground(e.Graphics, rect);

            Rectangle workingHoursRectangle = GetHourRangeRectangle(workStart, workEnd, rect);

            if (workingHoursRectangle.Y < this.HeaderHeight)
            {
                workingHoursRectangle.Height -= this.HeaderHeight - workingHoursRectangle.Y;
                workingHoursRectangle.Y = this.HeaderHeight;
            }

            if (!((time.DayOfWeek == DayOfWeek.Saturday) || (time.DayOfWeek == DayOfWeek.Sunday))) //weekends off -> no working hours
                renderer.DrawHourRange(e.Graphics, workingHoursRectangle, false, false);

            if ((selection == SelectionType.DateRange) && (time.Day == selectionStart.Day))
            {
                Rectangle selectionRectangle = GetHourRangeRectangle(selectionStart, selectionEnd, rect);
                if (selectionRectangle.Top + 1 > this.HeaderHeight)
                    renderer.DrawHourRange(e.Graphics, selectionRectangle, false, true);
            }

            e.Graphics.SetClip(rect);

            for (int hour = 0; hour < 24 * slotsPerHour; hour++)
            {
                int y = rect.Top + (hour * halfHourHeight) - scrollbar.Value;

                Color color1 = renderer.HourSeperatorColor;
                Color color2 = renderer.HalfHourSeperatorColor;
                using (Pen pen = new Pen(((hour % slotsPerHour) == 0 ? color1 : color2)))
                    e.Graphics.DrawLine(pen, rect.Left, y, rect.Right, y);

                if (y > rect.Bottom)
                    break;
            }

            renderer.DrawDayGripper(e.Graphics, rect, appointmentGripWidth);

            e.Graphics.ResetClip();

            AgendamentoCollectionDTO appointments = (AgendamentoCollectionDTO)cachedAppointments[time.Day];

            if (appointments != null)
            {
                List<string> groups = new List<string>();

                foreach (AgendamentoDTO app in appointments)
                    if (!groups.Contains(app.Group))
                        groups.Add(app.Group);

                Rectangle rect2 = rect;
                rect2.Width = rect2.Width / groups.Count;

                groups.Sort();

                foreach (string group in groups)
                {
                    DrawAppointments(e, rect2, time, group);

                    rect2.X += rect2.Width;
                }
            }
        }

        internal Dictionary<AgendamentoDTO, AppointmentView> appointmentViews = new Dictionary<AgendamentoDTO, AppointmentView>();
        internal Dictionary<AgendamentoDTO, AppointmentView> longappointmentViews = new Dictionary<AgendamentoDTO, AppointmentView>();

        private void DrawAppointments(PaintEventArgs e, Rectangle rect, DateTime time, string group)
        {
            DateTime timeStart = time.Date;
            DateTime timeEnd = timeStart.AddHours(24);
            timeEnd = timeEnd.AddSeconds(-1);

            AgendamentoCollectionDTO appointments = (AgendamentoCollectionDTO)cachedAppointments[time.Day];

            if (appointments != null)
            {
                HalfHourLayout[] layout = GetMaxParalelAppointments(appointments, this.slotsPerHour);

                List<AgendamentoDTO> drawnItems = new List<AgendamentoDTO>();

                for (int halfHour = 0; halfHour < 24 * slotsPerHour; halfHour++)
                {
                    HalfHourLayout hourLayout = layout[halfHour];

                    if ((hourLayout != null) && (hourLayout.Count > 0))
                    {
                        for (int appIndex = 0; appIndex < hourLayout.Count; appIndex++)
                        {
                            AgendamentoDTO appointment = hourLayout.Appointments[appIndex];

                            if (appointment.Group != group)
                                continue;

                            if (drawnItems.IndexOf(appointment) < 0)
                            {
                                Rectangle appRect = rect;
                                int appointmentWidth;
                                AppointmentView view;

                                appointmentWidth = rect.Width / appointment.conflictCount;

                                int lastX = 0;

                                foreach (AgendamentoDTO app in hourLayout.Appointments)
                                {
                                    if ((app != null) && (app.Group == appointment.Group) && (appointmentViews.ContainsKey(app)))
                                    {
                                        view = appointmentViews[app];

                                        if (lastX < view.Rectangle.X)
                                            lastX = view.Rectangle.X;
                                    }
                                }

                                if ((lastX + (appointmentWidth * 2)) > (rect.X + rect.Width))
                                    lastX = 0;

                                appRect.Width = appointmentWidth - 5;

                                if (lastX > 0)
                                    appRect.X = lastX + appointmentWidth;

                                DateTime appstart = appointment.StartDate;
                                DateTime append = appointment.EndDate;

                                // Draw the appts boxes depending on the height display mode                           

                                // If small appts are to be drawn in half-hour blocks
                                if (this.AppHeightMode == AppHeightDrawMode.FullHalfHourBlocksShort && appointment.EndDate.Subtract(appointment.StartDate).TotalMinutes < (60 / slotsPerHour))
                                {
                                    // Round the start/end time to the last/next halfhour
                                    appstart = appointment.StartDate.AddMinutes(-appointment.StartDate.Minute);
                                    append = appointment.EndDate.AddMinutes((60 / slotsPerHour) - appointment.EndDate.Minute);

                                    // Make sure we've rounded it to the correct halfhour :)
                                    if (appointment.StartDate.Minute >= (60 / slotsPerHour))
                                        appstart = appstart.AddMinutes((60 / slotsPerHour));
                                    if (appointment.EndDate.Minute > (60 / slotsPerHour))
                                        append = append.AddMinutes((60 / slotsPerHour));
                                }

                                // This is basically the same as previous mode, but for all appts
                                else if (this.AppHeightMode == AppHeightDrawMode.FullHalfHourBlocksAll)
                                {
                                    appstart = appointment.StartDate.AddMinutes(-appointment.StartDate.Minute);
                                    if (appointment.EndDate.Minute != 0 && appointment.EndDate.Minute != (60 / slotsPerHour))
                                        append = appointment.EndDate.AddMinutes((60 / slotsPerHour) - appointment.EndDate.Minute);
                                    else
                                        append = appointment.EndDate;

                                    if (appointment.StartDate.Minute >= (60 / slotsPerHour))
                                        appstart = appstart.AddMinutes((60 / slotsPerHour));
                                    if (appointment.EndDate.Minute > (60 / slotsPerHour))
                                        append = append.AddMinutes((60 / slotsPerHour));
                                }

                                // Based on previous code
                                else if (this.AppHeightMode == AppHeightDrawMode.EndHalfHourBlocksShort && appointment.EndDate.Subtract(appointment.StartDate).TotalMinutes < (60 / slotsPerHour))
                                {
                                    // Round the end time to the next halfhour
                                    append = appointment.EndDate.AddMinutes((60 / slotsPerHour) - appointment.EndDate.Minute);

                                    // Make sure we've rounded it to the correct halfhour :)
                                    if (appointment.EndDate.Minute > (60 / slotsPerHour))
                                        append = append.AddMinutes((60 / slotsPerHour));
                                }

                                else if (this.AppHeightMode == AppHeightDrawMode.EndHalfHourBlocksAll)
                                {
                                    // Round the end time to the next halfhour
                                    if (appointment.EndDate.Minute != 0 && appointment.EndDate.Minute != (60 / slotsPerHour))
                                        append = appointment.EndDate.AddMinutes((60 / slotsPerHour) - appointment.EndDate.Minute);
                                    else
                                        append = appointment.EndDate;
                                    // Make sure we've rounded it to the correct halfhour :)
                                    if (appointment.EndDate.Minute > (60 / slotsPerHour))
                                        append = append.AddMinutes((60 / slotsPerHour));
                                }

                                appRect = GetHourRangeRectangle(appstart, append, appRect);

                                view = new AppointmentView();
                                view.Rectangle = appRect;
                                view.Appointment = appointment;

                                appointmentViews[appointment] = view;

                                e.Graphics.SetClip(rect);

                                if (this.DrawAllAppBorder)
                                    appointment.DrawBorder = true;

                                // Procedure for gripper rectangle is always the same
                                Rectangle gripRect = GetHourRangeRectangle(appointment.StartDate, appointment.EndDate, appRect);
                                gripRect.Width = appointmentGripWidth;

                                renderer.DrawAppointment(e.Graphics, appRect, appointment, appointment == selectedAppointment, gripRect);

                                e.Graphics.ResetClip();

                                drawnItems.Add(appointment);
                            }
                        }
                    }
                }
            }
        }

        private static HalfHourLayout[] GetMaxParalelAppointments(AgendamentoCollectionDTO appointments, int slotsPerHour)
        {
            HalfHourLayout[] appLayouts = new HalfHourLayout[24 * 20];

            foreach (AgendamentoDTO appointment in appointments)
            {
                appointment.conflictCount = 1;
            }

            foreach (AgendamentoDTO appointment in appointments)
            {
                int firstHalfHour = appointment.StartDate.Hour * slotsPerHour + (appointment.StartDate.Minute / /*30*/(60 / slotsPerHour));
                int lastHalfHour = appointment.EndDate.Hour * slotsPerHour + (appointment.EndDate.Minute / /*30*/(60 / slotsPerHour));

                // Added to allow small parts been displayed
                if (lastHalfHour == firstHalfHour)
                {
                    if (lastHalfHour < 24 * slotsPerHour)
                        lastHalfHour++;
                    else
                        firstHalfHour--;
                }

                for (int halfHour = firstHalfHour; halfHour < lastHalfHour; halfHour++)
                {
                    HalfHourLayout layout = appLayouts[halfHour];

                    if (layout == null)
                    {
                        layout = new HalfHourLayout();
                        layout.Appointments = new AgendamentoDTO[20];
                        appLayouts[halfHour] = layout;
                    }

                    layout.Appointments[layout.Count] = appointment;

                    layout.Count++;

                    List<string> groups = new List<string>();

                    foreach (AgendamentoDTO app2 in layout.Appointments)
                    {
                        if ((app2 != null) && (!groups.Contains(app2.Group)))
                            groups.Add(app2.Group);
                    }

                    layout.Groups = groups;

                    // update conflicts
                    foreach (AgendamentoDTO app2 in layout.Appointments)
                    {
                        if ((app2 != null) && (app2.Group == appointment.Group))
                            if (app2.conflictCount < layout.Count)
                                app2.conflictCount = layout.Count - (layout.Groups.Count - 1);
                    }
                }
            }

            return appLayouts;
        }

        private void DrawDays(PaintEventArgs e, Rectangle rect)
        {
            int dayWidth = rect.Width / daysToShow;

            AgendamentoCollectionDTO longAppointments = (AgendamentoCollectionDTO)cachedAppointments[-1];

            AgendamentoCollectionDTO drawnLongApps = new AgendamentoCollectionDTO();

            AppointmentView view;

            int y = dayHeadersHeight;
            bool intersect = false;

            List<int> layers = new List<int>();

            if (longAppointments != null)
            {

                foreach (AgendamentoDTO appointment in longAppointments)
                {
                    appointment.Layer = 0;

                    if (drawnLongApps.Count != 0)
                    {
                        foreach (AgendamentoDTO app in drawnLongApps)
                            if (!layers.Contains(app.Layer))
                                layers.Add(app.Layer);

                        foreach (int lay in layers)
                        {
                            foreach (AgendamentoDTO app in drawnLongApps)
                            {
                                if (app.Layer == lay)
                                    if (appointment.StartDate.Date >= app.EndDate.Date || appointment.EndDate.Date <= app.StartDate.Date)
                                        intersect = false;
                                    else
                                    {
                                        intersect = true;
                                        break;
                                    }
                                appointment.Layer = lay;
                            }

                            if (!intersect)
                                break;
                        }

                        if (intersect)
                            appointment.Layer = layers.Count;
                    }

                    drawnLongApps.Add(appointment);
                }

                foreach (AgendamentoDTO app in drawnLongApps)
                    if (!layers.Contains(app.Layer))
                        layers.Add(app.Layer);

                allDayEventsHeaderHeight = layers.Count * (horizontalAppointmentHeight + 5) + 5;

                Rectangle backRectangle = rect;
                backRectangle.Y = y;
                backRectangle.Height = allDayEventsHeaderHeight;

                renderer.DrawAllDayBackground(e.Graphics, backRectangle);

                foreach (AgendamentoDTO appointment in longAppointments)
                {
                    Rectangle appointmenRect = rect;
                    int spanDays = appointment.EndDate.Subtract(appointment.StartDate).Days;

                    if (appointment.EndDate.Day != appointment.StartDate.Day && appointment.EndDate.TimeOfDay < appointment.StartDate.TimeOfDay)
                        spanDays += 1;

                    appointmenRect.Width = dayWidth * spanDays - 5;
                    appointmenRect.Height = horizontalAppointmentHeight;
                    appointmenRect.X += (appointment.StartDate.Subtract(startDate).Days) * dayWidth;
                    appointmenRect.Y = y + appointment.Layer * (horizontalAppointmentHeight + 5) + 5;

                    view = new AppointmentView();
                    view.Rectangle = appointmenRect;
                    view.Appointment = appointment;

                    longappointmentViews[appointment] = view;

                    Rectangle gripRect = appointmenRect;
                    gripRect.Width = appointmentGripWidth;

                    renderer.DrawAppointment(e.Graphics, appointmenRect, appointment, appointment == selectedAppointment, gripRect);

                }
            }

            DateTime time = startDate;
            Rectangle rectangle = rect;
            rectangle.Width = dayWidth;
            rectangle.Y += allDayEventsHeaderHeight;
            rectangle.Height -= allDayEventsHeaderHeight;

            appointmentViews.Clear();
            layers.Clear();

            for (int day = 0; day < daysToShow; day++)
            {
                if (day == 0)
                    rectangle.Width += (rect.Width % daysToShow) - 1;
                DrawDay(e, rectangle, time);

                rectangle.X += dayWidth;

                time = time.AddDays(1);
            }
        }

        #endregion

        #region Internal Utility Classes

        class HalfHourLayout
        {
            public int Count;
            public List<string> Groups;
            public AgendamentoDTO[] Appointments;
        }

        internal class AppointmentView
        {
            public AgendamentoDTO Appointment;
            public Rectangle Rectangle;
        }

        #endregion

        #region Events

        public event EventHandler SelectionChanged;
        public event ResolveAppointmentsEventHandler ResolveAppointments;
        public event NewAppointmentEventHandler NewAppointment;
        public event EventHandler<AppointmentEventArgs> AppointmentMove;

        #endregion

        #region Teste

        public event EventHandler UpdateSuccess;
        public event EventHandler newSuccess;
        public event EventHandler deletarApontamentoMaster;
        public event EventHandler IniciarAtendimento;

        /// <summary>
        /// Manda a master page deletar o apontamento
        /// </summary>
        public void ReceberDeletarApontamento(object sender, EventArgs e)
        {
            //Sinaliza ao método seguinte para que não entre em modo de edição
            meuControle.Visible = false;

            if (deletarApontamentoMaster != null)
            {
                deletarApontamentoMaster(this, new EventArgs());
            }
        }

        /// <summary>
        /// Master page abre a tela de atendimento
        /// </summary>
        public void IniciarAtendimentoSuccess(object sender, EventArgs e)
        {
            //Fecha o apontamento sem salvar
            FinishEditing(true);
            if (IniciarAtendimento != null)
            {
                IniciarAtendimento(this, new EventArgs());
            }
        }

        #endregion
    }
}