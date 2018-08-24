using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MariEtFemme.Agendamento;
using MariEtFemme.Tools;
using MariEtFemme.BLL;
using MariEtFemme.DTO;
using System.Windows.Media.Imaging;
using System.Drawing;

//Implantar
//Exibir informações do próximo cliente a ser atendido, como ultimos serviços, comentários, aniversário, numero de atendimentos
//dia do calendario fica colorido quando tiver agendamentos
//Já no agendamento tirar as quantidade do estoque estoque movimentado
//Botão logoff não perde focus

namespace MariEtFemme.View
{
    public partial class Master : UserControl
    {
        public Master()
        {
            InitializeComponent();
        }

        #region Varibles

        private DaysToShow daysToShow = DaysToShow.One;

        /// <summary>
        /// Instancia a listagem que armazena todos os agendamentos
        /// </summary>
        AgendamentoCollectionDTO m_Appointments = new AgendamentoCollectionDTO();
        AgendamentoCollectionDTO m_Appointments2 = new AgendamentoCollectionDTO();

        AgendamentoBLL agendamentoBLL = new AgendamentoBLL();

        AgendamentoServicoBLL agendamentoServicoBLL = new AgendamentoServicoBLL();

        ClienteBLL clienteBLL = new ClienteBLL();

        FuncionarioCollectionDTO funcionarioCollectionDTO;
        FuncionarioBLL funcionarioBLL = new FuncionarioBLL();

        #endregion

        #region EventHandler
        public event EventHandler LogOffSuccess;
        #endregion

        #region Functions

        static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings[key] ?? "Not Found";;
            }
            catch (ConfigurationErrorsException ex)
            {
                return ex.Message;
            }
        }

        static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        private void Privileges()
        {
            switch (Session.LoggedUser.Usuario.Privilegio.IdPrivilegio)
            {
                default:
                    FillSchemas();
                    FillEmployee();
                    //Buttons
                    btnQtDaysRemove.IsEnabled = false;

                    //Events
                    dayView1.StartDate = DateTime.Now;
                    dayView1.NewAppointment += new NewAppointmentEventHandler(dayView1_NewAppointment);
                    dayView1.ResolveAppointments += new ResolveAppointmentsEventHandler(dayView1_ResolveAppointments);
                    dayView1.DaysToShow = (int)daysToShow;

                    dayView2.StartDate = DateTime.Now;
                    dayView2.NewAppointment += new NewAppointmentEventHandler(dayView2_NewAppointment);
                    dayView2.ResolveAppointments += new ResolveAppointmentsEventHandler(dayView2_ResolveAppointments);
                    dayView2.DaysToShow = (int)daysToShow;

                    calendarAppointment.SelectedDate = DateTime.Today;
                    calendarAppointment.SelectedDatesChanged += calendarAppointment_SelectedDatesChanged;                    

                    cbSchema.SelectionChanged += cbSchema_SelectionChanged;

                    //Trava ou destrava a agenda
                    checkBoxLockDayView.Checked += TravarDestravarAgenda;
                    checkBoxLockDayView.Unchecked += TravarDestravarAgenda;                    
                    
                    cbDayView1.SelectedIndex = Convert.ToInt32(ReadSetting("Setting1"));
                    cbDayView2.SelectedIndex = Convert.ToInt32(ReadSetting("Setting2"));

                    //Fill                    
                    FillAppointments();
                    break;
            }
        }

        private void FillSchemas()
        {
            cbSchema.Items.Clear();
            cbSchema.Items.Add("Office 11");
            cbSchema.Items.Add("Office 12");
            cbSchema.SelectedItem = "Office 12";
        }

        private void FillEmployee()
        {
            cbDayView1.Items.Clear();
            cbDayView2.Items.Clear();
            funcionarioCollectionDTO = new FuncionarioCollectionDTO();
            funcionarioCollectionDTO = funcionarioBLL.ReadName(string.Empty);

            foreach (FuncionarioDTO item in funcionarioCollectionDTO)
            {
                cbDayView1.Items.Add(item.Pessoa.NomePessoa);
                cbDayView2.Items.Add(item.Pessoa.NomePessoa);
            }
        }

        private AgendamentoCollectionDTO ListarApontamentos(int agenda)
        {
            AgendamentoCollectionDTO listaAgendamento = new AgendamentoCollectionDTO();

            listaAgendamento = agendamentoBLL.ReadDateRange(calendarAppointment.SelectedDate.Value, calendarAppointment.SelectedDate.Value.AddDays(7), agenda);

            foreach (AgendamentoDTO item in listaAgendamento)
            {
                item.Servicos = new ServicoCollectionDTO();
                item.Servicos = agendamentoServicoBLL.ReadService(item);

                item.Title = string.Empty;
                int temp = 1; ;
                if (item.Servicos != null && item.Servicos.Count > 0)
                {
                    foreach (ServicoDTO servico in item.Servicos)
                    {
                        if (item.Servicos.Count == temp)
                        {
                            item.Title = item.Title + servico.DescricaoServico;
                        }
                        else
                        {
                            item.Title = item.Title + servico.DescricaoServico + ", ";
                        }
                        temp++;
                    }
                }
            }
            return listaAgendamento;
        }

        /// <summary>
        /// Preenche todos os apontamentos solicitados para o banco no dayview
        /// </summary>
        private void FillAppointments()
        {
            m_Appointments.Clear();
            m_Appointments2.Clear();
            m_Appointments = ListarApontamentos(Convert.ToInt32(funcionarioBLL.ReadName(cbDayView1.SelectedItem.ToString())[0].Pessoa.IdPessoa));
            m_Appointments2 = ListarApontamentos(Convert.ToInt32(funcionarioBLL.ReadName(cbDayView2.SelectedItem.ToString())[0].Pessoa.IdPessoa));
        }

        private void AtualizarAgendas(object sender, EventArgs e)
        {
            FillAppointments();
        }

        /// <summary>
        /// Corrige o tamanho das agendas conforme o tamanho da janela main
        /// </summary>
        private void FixLayout()
        {
            double gridWidth = contentBox.ActualWidth - 100;
            cbDayView1.Width = cbDayView2.Width = gridDayView.Width = gridDayView2.Width = gridWidth / 2;
        }

        #endregion

        #region Tools Buttons Events
        private void btnUsersTools_Click(object sender, RoutedEventArgs e)
        {
            UserTool userToolPage = new UserTool();
            userToolPage.ShowDialog();
        }

        private void btnClientTools_Click(object sender, RoutedEventArgs e)
        {
            ClientTool clientToPage = new ClientTool();
            clientToPage.ShowDialog();
        }

        private void btnAttendanceTools_Click(object sender, RoutedEventArgs e)
        {
            Attendance attendancePage = new Attendance();
            attendancePage.ShowDialog();
        }

        private void btnServicesTools_Click(object sender, RoutedEventArgs e)
        {
            ServiceTool serviceTool = new ServiceTool();
            serviceTool.ShowDialog();
        }

        private void btnStuffTools_Click(object sender, RoutedEventArgs e)
        {
            Stuff stuffPage = new Stuff();
            stuffPage.ShowDialog();
        }

        private void btnProvidersTools_Click(object sender, RoutedEventArgs e)
        {
            ProviderTool providerToolPage = new ProviderTool();
            providerToolPage.ShowDialog();
        }

        private void btnStockTools_Click(object sender, RoutedEventArgs e)
        {
            InvoiceTool invoiceTool = new InvoiceTool();
            invoiceTool.ShowDialog();
        }

        private void btnPersonalTools_Click(object sender, RoutedEventArgs e)
        {
            Employee employeePage = new Employee();
            employeePage.ShowDialog();
        }

        private void btnFilialTools_Click(object sender, RoutedEventArgs e)
        {
            Filial filialPage = new Filial();
            filialPage.ShowDialog();
        }

        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            Reports reportsPage = new Reports();
            reportsPage.ShowDialog();
        }

        private void btnPreferences_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnLogoff_Click(object sender, RoutedEventArgs e)
        {
            if (LogOffSuccess != null)
            {
                LogOffSuccess(this, new EventArgs());
            }
        }

        #endregion

        #region Appointment Events

        private void calendarAppointment_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!dayView1.CurrentlyEditing && !dayView2.CurrentlyEditing)
            {
                dayView1.StartDate = calendarAppointment.SelectedDate.Value;
                dayView2.StartDate = calendarAppointment.SelectedDate.Value;
                FillAppointments();
            }

            //Tira o Focus do calendário
            Mouse.Capture(null);
        }

        /// <summary>
        /// É chamado a partir do formulário de edição do agendamento, botão excluir
        /// </summary>
        private void cbSchema_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*switch (cbSchema.SelectedItem.ToString())
            {
                case "Office 11":
                    dayView1.Renderer = new Office11Renderer();
                    break;
                case "Office 12":
                    dayView1.Renderer = new Office12Renderer();
                    break;
                default:
                    dayView1.Renderer = new Office12Renderer();
                    break;
            }*/
        }
        private void btnQtDaysAdd_Click(object sender, RoutedEventArgs e)
        {
            switch (daysToShow)
            {
                case DaysToShow.One:
                    btnQtDaysRemove.IsEnabled = true;
                    btnQtDaysAdd.IsEnabled = true;
                    daysToShow = DaysToShow.Three;
                    dayView1.DaysToShow = (int)DaysToShow.Three;
                    dayView2.DaysToShow = (int)DaysToShow.Three;
                    break;
                case DaysToShow.Three:
                    btnQtDaysRemove.IsEnabled = true;
                    btnQtDaysAdd.IsEnabled = true;
                    daysToShow = DaysToShow.Five;
                    dayView1.DaysToShow = (int)DaysToShow.Five;
                    dayView2.DaysToShow = (int)DaysToShow.Five;
                    break;
                case DaysToShow.Five:
                    btnQtDaysRemove.IsEnabled = true;
                    btnQtDaysAdd.IsEnabled = false;
                    daysToShow = DaysToShow.Seven;
                    dayView1.DaysToShow = (int)DaysToShow.Seven;
                    dayView2.DaysToShow = (int)DaysToShow.Seven;
                    break;
            }
        }
        private void btnQtDaysRemove_Click(object sender, RoutedEventArgs e)
        {
            switch (daysToShow)
            {
                case DaysToShow.Seven:
                    btnQtDaysRemove.IsEnabled = true;
                    btnQtDaysAdd.IsEnabled = true;
                    daysToShow = DaysToShow.Five;
                    dayView1.DaysToShow = (int)DaysToShow.Five;
                    dayView2.DaysToShow = (int)DaysToShow.Five;
                    break;
                case DaysToShow.Five:
                    btnQtDaysRemove.IsEnabled = true;
                    btnQtDaysAdd.IsEnabled = true;
                    daysToShow = DaysToShow.Three;
                    dayView1.DaysToShow = (int)DaysToShow.Three;
                    dayView2.DaysToShow = (int)DaysToShow.Three;
                    break;
                case DaysToShow.Three:
                    btnQtDaysRemove.IsEnabled = false;
                    btnQtDaysAdd.IsEnabled = true;
                    daysToShow = DaysToShow.One;
                    dayView1.DaysToShow = (int)DaysToShow.One;
                    dayView2.DaysToShow = (int)DaysToShow.One;
                    break;
            }
        }

        #endregion

        #region DayView1 Events

        private void OnDeletarApontamento(object sender, EventArgs e)
        {
            AgendamentoDTO m_App = new AgendamentoDTO();
            m_App = dayView1.SelectedAppointment;
            m_Appointments.Remove(m_App);
            agendamentoServicoBLL.Delete(m_App);
            agendamentoBLL.Delete(m_App);
            //Atualiza o grid dos agendamentos
            dayView1.Invalidate();
        }
        private void OnUpdateSuccess(object sender, EventArgs e)
        {
            string temp = agendamentoBLL.ReadeExists(dayView1.SelectedAppointment);
            ClienteCollectionDTO colectionTemp = new ClienteCollectionDTO();
            colectionTemp = clienteBLL.ReadName(dayView1.SelectedAppointment.Cliente.Pessoa.NomePessoa);

            switch (temp)
            {
                case "existe": //Update
                    if(dayView1.SelectedAppointment.Cliente.Pessoa.IdPessoa.Equals(null) && colectionTemp.Count.Equals(0))
                    {
                        agendamentoBLL.Update(dayView1.SelectedAppointment);
                        agendamentoServicoBLL.Delete(dayView1.SelectedAppointment);

                        foreach (ServicoDTO item in dayView1.SelectedAppointment.Servicos)
                        {
                            agendamentoServicoBLL.Create(dayView1.SelectedAppointment, item);
                        }
                    }
                    else
                    {
                        dayView1.SelectedAppointment.Cliente = colectionTemp[0];
                        agendamentoBLL.Update(dayView1.SelectedAppointment);
                        agendamentoServicoBLL.Delete(dayView1.SelectedAppointment);

                        foreach (ServicoDTO item in dayView1.SelectedAppointment.Servicos)
                        {
                            agendamentoServicoBLL.Create(dayView1.SelectedAppointment, item);
                        }
                    }
                    break;

                case "naoExiste": //criar
                    if (dayView1.SelectedAppointment.Cliente.Pessoa.IdPessoa.Equals(null) && colectionTemp.Count.Equals(0))
                    {
                        //////////////////////
                        dayView1.SelectedAppointment.IdAgendamento = Convert.ToInt32(agendamentoBLL.Create(dayView1.SelectedAppointment));

                        foreach (ServicoDTO item in dayView1.SelectedAppointment.Servicos)
                        {
                            agendamentoServicoBLL.Create(dayView1.SelectedAppointment, item);
                        }
                    }
                    else
                    {
                        dayView1.SelectedAppointment.Cliente = colectionTemp[0];
                        dayView1.SelectedAppointment.IdAgendamento = Convert.ToInt32(agendamentoBLL.Create(dayView1.SelectedAppointment));

                        foreach (ServicoDTO item in dayView1.SelectedAppointment.Servicos)
                        {
                            agendamentoServicoBLL.Create(dayView1.SelectedAppointment, item);
                        }
                    }
                    break;
            }
        }
        private void dayView1_ResolveAppointments(object sender, ResolveAppointmentsEventArgs args)
        {
            AgendamentoCollectionDTO m_Apps = new AgendamentoCollectionDTO();
            foreach (AgendamentoDTO m_App in m_Appointments)
            {
                if ((m_App.StartDate >= args.StartDate) && (m_App.StartDate <= args.EndDate))
                {
                    m_Apps.Add(m_App);
                }
            }
            args.Appointments = m_Apps;
        }

        /// <summary>
        /// Responsável por criar um novo apontamento assim que o botão do mouse é solto
        /// </summary>
        public void OnNewSuccess(object sender, EventArgs args)
        {
            AgendamentoDTO m_App = new AgendamentoDTO();
            m_App.Cliente = new ClienteDTO();
            m_App.Cliente.Pessoa = new PessoaDTO();
            m_App.Servicos = new ServicoCollectionDTO();            
            m_App.StartDate = dayView1.SelectionStart;
            m_App.EndDate = dayView1.SelectionEnd;
            m_App.BorderColor = Color.Red;
            m_App.Layer =Convert.ToInt16(funcionarioBLL.ReadName(cbDayView1.SelectedItem.ToString())[0].Pessoa.IdPessoa);
            m_Appointments.Add(m_App);
            dayView1.SelectedAppointment = m_App;
            dayView1.Invalidate();
        }
        private void dayView1_NewAppointment(object sender, NewAppointmentEventArgs args)
        {
            AgendamentoDTO m_Appointment = new AgendamentoDTO();
            m_Appointment.StartDate = args.StartDate;
            m_Appointment.EndDate = args.EndDate;
            m_Appointment.Title = args.Title;
            m_Appointments.Add(m_Appointment);
        }
        private void dayView1_MouseEnter(object sender, EventArgs e)
        {
            dayView1.Focus();
        }
        private void OnIniciarAtendimento(object sender, EventArgs e)
        {
            Attendance attendancePage = new Attendance(dayView1.SelectedAppointment);
            attendancePage.ShowDialog();
        }
        private void cbDayView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_Appointments.Clear();
            FuncionarioDTO funcionario = funcionarioBLL.ReadName(cbDayView1.SelectedItem.ToString())[0];
            m_Appointments = ListarApontamentos(Convert.ToInt32(funcionario.Pessoa.IdPessoa));
            AddUpdateAppSettings("Setting1", cbDayView1.SelectedIndex.ToString());
            dayView1.Invalidate();
            bool result = false;
            if (Boolean.TryParse(funcionario.Pessoa.PessoaFisica.Genero.ToString(), out result))
            {
                if (Convert.ToBoolean(funcionario.Pessoa.PessoaFisica.Genero))
                {
                    dayView1.Renderer = new Office11Renderer();
                }
                else
                {
                    dayView1.Renderer = new Office12Renderer();
                }
            }
            else
            {
                dayView1.Renderer = new Office12Renderer();
            }
        }

        #endregion

        #region DayView2 Events

        private void OnDeletarApontamento2(object sender, EventArgs e)
        {
            AgendamentoDTO m_App = new AgendamentoDTO();
            m_App = dayView2.SelectedAppointment;
            m_Appointments2.Remove(m_App);
            agendamentoServicoBLL.Delete(m_App);
            agendamentoBLL.Delete(m_App);
            //Atualiza o grid dos agendamentos
            dayView2.Invalidate();
        }
        private void OnUpdateSuccess2(object sender, EventArgs e)
        {
            string temp = agendamentoBLL.ReadeExists(dayView2.SelectedAppointment);
            ClienteCollectionDTO colectionTemp = new ClienteCollectionDTO();
            colectionTemp = clienteBLL.ReadName(dayView2.SelectedAppointment.Cliente.Pessoa.NomePessoa);
            switch (temp)
            {
                case "existe": //Update
                    if (dayView2.SelectedAppointment.Cliente.Pessoa.IdPessoa.Equals(null) && colectionTemp.Count.Equals(0))
                    {
                        agendamentoBLL.Update(dayView2.SelectedAppointment);
                        agendamentoServicoBLL.Delete(dayView2.SelectedAppointment);

                        foreach (ServicoDTO item in dayView2.SelectedAppointment.Servicos)
                        {
                            agendamentoServicoBLL.Create(dayView2.SelectedAppointment, item);
                        }
                    }
                    else
                    {
                        dayView2.SelectedAppointment.Cliente = colectionTemp[0];
                        agendamentoBLL.Update(dayView2.SelectedAppointment);
                        agendamentoServicoBLL.Delete(dayView2.SelectedAppointment);

                        foreach (ServicoDTO item in dayView2.SelectedAppointment.Servicos)
                        {
                            agendamentoServicoBLL.Create(dayView2.SelectedAppointment, item);
                        }
                    }
                    break;

                case "naoExiste": //criar
                    if (dayView2.SelectedAppointment.Cliente.Pessoa.IdPessoa.Equals(null) && colectionTemp.Count.Equals(0))
                    {
                        dayView2.SelectedAppointment.IdAgendamento = Convert.ToInt32(agendamentoBLL.Create(dayView2.SelectedAppointment));

                        foreach (ServicoDTO item in dayView2.SelectedAppointment.Servicos)
                        {
                            agendamentoServicoBLL.Create(dayView2.SelectedAppointment, item);
                        }
                    }
                    else
                    {
                        dayView2.SelectedAppointment.Cliente = colectionTemp[0];
                        dayView2.SelectedAppointment.IdAgendamento = Convert.ToInt32(agendamentoBLL.Create(dayView2.SelectedAppointment));

                        foreach (ServicoDTO item in dayView2.SelectedAppointment.Servicos)
                        {
                            agendamentoServicoBLL.Create(dayView2.SelectedAppointment, item);
                        }
                    }
                    break;
            }
        }
        private void dayView2_ResolveAppointments(object sender, ResolveAppointmentsEventArgs args)
        {
            AgendamentoCollectionDTO m_Apps = new AgendamentoCollectionDTO();
            foreach (AgendamentoDTO m_App in m_Appointments2)
            {
                if ((m_App.StartDate >= args.StartDate) && (m_App.StartDate <= args.EndDate))
                {
                    m_Apps.Add(m_App);
                }
            }
            args.Appointments = m_Apps;
        }

        /// <summary>
        /// Responsável por criar um novo apontamento assim que o botão do mouse é solto
        /// </summary>
        public void OnNewSuccess2(object sender, EventArgs args)
        {
            AgendamentoDTO m_App = new AgendamentoDTO();
            m_App.Cliente = new ClienteDTO();
            m_App.Cliente.Pessoa = new PessoaDTO();
            m_App.Servicos = new ServicoCollectionDTO();
            m_App.StartDate = dayView2.SelectionStart;
            m_App.EndDate = dayView2.SelectionEnd;
            m_App.BorderColor = Color.Red;
            m_App.Layer = Convert.ToInt16(funcionarioBLL.ReadName(cbDayView2.SelectedItem.ToString())[0].Pessoa.IdPessoa);
            m_Appointments2.Add(m_App);
            dayView2.SelectedAppointment = m_App;
            dayView2.Invalidate();
        }
        private void dayView2_NewAppointment(object sender, NewAppointmentEventArgs args)
        {
            AgendamentoDTO m_Appointment = new AgendamentoDTO();
            m_Appointment.StartDate = args.StartDate;
            m_Appointment.EndDate = args.EndDate;
            m_Appointment.Title = args.Title;
            m_Appointments2.Add(m_Appointment);
        }
        private void dayView2_MouseEnter(object sender, EventArgs e)
        {
            dayView2.Focus();
        }
        private void OnIniciarAtendimento2(object sender, EventArgs e)
        {
            Attendance attendancePage = new Attendance(dayView2.SelectedAppointment);
            attendancePage.ShowDialog();
        }
        private void cbDayView2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_Appointments2.Clear();
            FuncionarioDTO funcionario = funcionarioBLL.ReadName(cbDayView2.SelectedItem.ToString())[0];
            m_Appointments2 = ListarApontamentos(Convert.ToInt32(funcionario.Pessoa.IdPessoa));
            AddUpdateAppSettings("Setting2", cbDayView2.SelectedIndex.ToString());
            dayView2.Invalidate();
            bool result = false;
            if(Boolean.TryParse(funcionario.Pessoa.PessoaFisica.Genero.ToString(), out result))
            {
                if (Convert.ToBoolean(funcionario.Pessoa.PessoaFisica.Genero))
                {
                    dayView2.Renderer = new Office11Renderer();
                }
                else
                {
                    dayView2.Renderer = new Office12Renderer();
                }
            }
            else
            {
                dayView2.Renderer = new Office12Renderer();
            }
        }

        #endregion

        #region Control Events
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FixLayout();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Privileges();
            FixLayout();
        }

        private void TravarDestravarAgenda(object sender, RoutedEventArgs e)
        {
            if (checkBoxLockDayView.IsChecked.Value)
            {
                dayView1.Enabled = false;
                dayView2.Enabled = false;

                imgLock.Source = new BitmapImage(new Uri("img/principal/lockClose02D.png", UriKind.RelativeOrAbsolute));
            }
            else
            {
                dayView1.Enabled = true;
                dayView2.Enabled = true;

                imgLock.Source = new BitmapImage(new Uri("img/principal/lockOpen02D.png", UriKind.RelativeOrAbsolute));
            }
        }

        #endregion
    }
}