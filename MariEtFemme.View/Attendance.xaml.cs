using System;
using System.Windows;
using System.Windows.Controls;
using MariEtFemme.DTO;
using MariEtFemme.BLL;
using MariEtFemme.Tools;

//Implantar
//Combobox com os nomes nas agendas, mudança de tema e carregamento de agendamento.
//Colocar os valores da combobox para definir qual agenda utilizar.

namespace MariEtFemme.View
{
    public partial class Attendance : Window
    {
        /// <summary>
        /// Contrutor da classe.
        /// </summary>
        public Attendance()
        {
            howOpen = true;
            InitializeComponent();
        }

        /// <summary>
        /// Construtor que leva um objeto AgendamentoDTO para o formuário.
        /// </summary>
        /// <param name="agendamento">Objeto AgendamentoDTO do qual serão extraídos dados.</param>
        public Attendance(AgendamentoDTO agendamento)
        {            
            howOpen = false;
            agendamentoDTO = agendamento;
            InitializeComponent();
        }

        #region Variáveis

        private bool howOpen;
        private AgendamentoDTO agendamentoDTO;
        private EnumApplyAction buttonApply;

        AtendimentoCollectionDTO atendimentoCollectionDTO;
        AtendimentoBLL atendimentoBLL = new AtendimentoBLL();

        AtendimentoServicoDTO atendimentoServicoDTO;
        AtendimentoServicoCollectionDTO newCollection;
        AtendimentoServicoCollectionDTO oldCollection;
        AtendimentoServicoBLL atendimentoServicoBLL = new AtendimentoServicoBLL();

        ServicoProdutoCollectionDTO servicoProdutoCollectionDTO;
        ServicoProdutoBLL servicoProdutoBLL = new ServicoProdutoBLL();

        ServicoCollectionDTO servicoCollectionDTO;
        ServicoBLL servicoBLL = new ServicoBLL();        
        
        ClienteCollectionDTO clienteCollectionDTO;
        ClienteBLL clienteBLL = new ClienteBLL();

        FuncionarioCollectionDTO funcionarioCollectionDTO;
        FuncionarioBLL funcionarioBLL = new FuncionarioBLL();

        EstoqueDTO estoqueDTO;
        EstoqueBLL estoqueBLL = new EstoqueBLL();

        AgendamentoBLL agendamentoBLL = new AgendamentoBLL();

        #endregion

        #region Métodos

        /// <summary>
        /// Lista todos os atendimentos
        /// </summary>
        private void ListAttendances()
        {
            atendimentoCollectionDTO = new AtendimentoCollectionDTO();
            atendimentoCollectionDTO = atendimentoBLL.ReadAll();
            dataGridAttendance.ItemsSource = null;
            dataGridAttendance.ItemsSource = atendimentoCollectionDTO;
        }

        /// <summary>
        /// Lista todos os cliente
        /// </summary>
        private void ListClients()
        {
            try
            {
                clienteCollectionDTO = new ClienteCollectionDTO();
                clienteCollectionDTO = clienteBLL.ReadName(string.Empty);
                dataGridAddClient.ItemsSource = null;
                dataGridAddClient.ItemsSource = clienteCollectionDTO;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ListEmployees()
        {
            try
            {
                funcionarioCollectionDTO = new FuncionarioCollectionDTO();
                funcionarioCollectionDTO = funcionarioBLL.ReadName(string.Empty);
                dataGridAddEmployee.ItemsSource = null;
                dataGridAddEmployee.ItemsSource = funcionarioCollectionDTO;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Lista todos os serviços
        /// </summary>
        private void ListServices()
        {
            try
            {
                servicoCollectionDTO = new ServicoCollectionDTO();
                servicoCollectionDTO = servicoBLL.ReadName(string.Empty);
                dataGridAddService.ItemsSource = null;
                dataGridAddService.ItemsSource = servicoCollectionDTO;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Privileges()
        {
            //Fill
            ListAttendances();

            //Lists
            dataGridAttendance.Visibility = Visibility.Visible;
            dataGridAddClient.Visibility = dataGridAddService.Visibility = dataGridAddEmployee.Visibility = Visibility.Hidden;

            //Buttons
            btnNew.Visibility = btnEdit.Visibility = btnRemove.Visibility = Visibility.Visible;
            btnApply.Visibility = btnCancel.Visibility = Visibility.Hidden;
            btnEdit.IsEnabled = btnRemove.IsEnabled = false;

            //Controls
            buttonApply = new EnumApplyAction();
            dpDate.SelectedDate = DateTime.Now;
            lblClientName.Content = string.Empty;
            txtlblObsPersonal.Text = txtObsAttend.Text = string.Empty;
            dataGridService.ItemsSource = dataGridStuff.ItemsSource = null;            

            switch (Session.LoggedUser.Usuario.Privilegio.IdPrivilegio)
            {
                default:
                    break;
            }
        }
        private ServicoProdutoCollectionDTO FillStuffsList(AtendimentoServicoDTO atendimentoServico)
        {
            try
            {
                servicoProdutoCollectionDTO = new ServicoProdutoCollectionDTO();
                return servicoProdutoCollectionDTO = servicoProdutoBLL.ReadService(atendimentoServico.Servico);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return servicoProdutoCollectionDTO;
            }
        }
        private void PreencherFormulario()
        {
            //Atendimento
            atendimentoServicoDTO.Atendimento = dataGridAttendance.SelectedItem as AtendimentoDTO;
            dpDate.SelectedDate = atendimentoServicoDTO.Atendimento.DataAtendimento;
            lblClientName.Content = atendimentoServicoDTO.Atendimento.Cliente.Pessoa.NomePessoa;
            lblEmployeeName.Content = atendimentoServicoDTO.Atendimento.Funcionario.Pessoa.NomePessoa;
            txtObsAttend.Text = atendimentoServicoDTO.Atendimento.ComenariosAtendimento;
            txtlblObsPersonal.Text = atendimentoServicoDTO.Atendimento.Cliente.Pessoa.Comentarios;

            //Atendimento_serviços
            oldCollection = atendimentoServicoBLL.ReadAttendance(atendimentoServicoDTO.Atendimento);
            newCollection = atendimentoServicoBLL.ReadAttendance(atendimentoServicoDTO.Atendimento);
            dataGridService.ItemsSource = null;
            dataGridService.ItemsSource = newCollection;
        }
        private bool ValidarAtendimento()
        {
            if (dpDate.SelectedDate != null)
            {
                if (!string.IsNullOrEmpty(lblClientName.Content.ToString()))
                {
                    if (newCollection.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Favor, selecione um ou mais serviços para o atendimento.", "Serviços", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Favor, selecione um cliente para o atendimento.", "Cliente", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Favor, selecione uma data para o atendimento.", "Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
        }
        public void IniciarAtendimento(AgendamentoDTO agendamento)
        {
            TelaAdicionarNovoAtendimento();

            atendimentoServicoDTO.Atendimento = new AtendimentoDTO();
            atendimentoServicoDTO.Atendimento.Cliente = new ClienteDTO();
            /////////////////////////////////////////////////////////////////////////////////////
            atendimentoServicoDTO.Atendimento.Funcionario = new FuncionarioDTO();
            atendimentoServicoDTO.Atendimento.Funcionario = agendamento.Layer.Equals(1) ? funcionarioBLL.ReadName("Marcos Antônio")[0] : funcionarioBLL.ReadName("Fernanda")[0];

            foreach (ServicoDTO item in agendamento.Servicos)
            {
                if (!agendamento.Cliente.Pessoa.IdPessoa.Equals(null))
                {
                    atendimentoServicoDTO.Atendimento.Cliente = agendamento.Cliente;
                }
                                
                atendimentoServicoDTO.Servico = item;
                newCollection.Add(atendimentoServicoDTO);
            }

            lblClientName.Content = atendimentoServicoDTO.Atendimento.Cliente.Pessoa.NomePessoa;
            lblEmployeeName.Content = atendimentoServicoDTO.Atendimento.Funcionario.Pessoa.NomePessoa;
            txtObsAttend.Text = atendimentoServicoDTO.Atendimento.ComenariosAtendimento;
            dataGridService.ItemsSource = null;
            dataGridService.ItemsSource = newCollection;
        }

        #endregion

        #region Evetos
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Privileges();

            if (!howOpen)
            {
                IniciarAtendimento(agendamentoDTO);
            }
        }
        private void TelaAdicionarNovoAtendimento()
        {
            //Fill
            buttonApply = EnumApplyAction.Create;
            ListServices();
            ListClients();

            //Lists
            dataGridAttendance.Visibility = Visibility.Hidden;
            dataGridAddClient.Visibility = Visibility.Hidden;
            dataGridAddService.Visibility = Visibility.Hidden;

            //Buttons
            btnNew.Visibility = btnEdit.Visibility = btnRemove.Visibility = Visibility.Hidden;
            btnApply.Visibility = btnCancel.Visibility = Visibility.Visible;

            atendimentoServicoDTO = new AtendimentoServicoDTO();
            newCollection = new AtendimentoServicoCollectionDTO();
        }
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            TelaAdicionarNovoAtendimento();
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                buttonApply = EnumApplyAction.Update;
                ListServices();
                ListClients();

                //Lists
                dataGridAttendance.Visibility = Visibility.Hidden;

                //Buttons
                btnNew.Visibility = btnEdit.Visibility = btnRemove.Visibility = Visibility.Hidden;
                btnApply.Visibility = btnCancel.Visibility = Visibility.Visible;

                //Dados do atendimento apenas
                atendimentoServicoDTO = new AtendimentoServicoDTO();
                newCollection = new AtendimentoServicoCollectionDTO();
                oldCollection = new AtendimentoServicoCollectionDTO();
                newCollection = atendimentoServicoBLL.ReadAttendance(atendimentoServicoDTO.Atendimento);
                oldCollection = atendimentoServicoBLL.ReadAttendance(atendimentoServicoDTO.Atendimento);
                PreencherFormulario();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Remover apenas atendimento
                if (MessageBox.Show("Realmente deseja excluir o atendimento ?", "Remover Atendimento", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    //Pega o objeto a ser excluído
                    atendimentoServicoDTO = new AtendimentoServicoDTO();                    
                    atendimentoServicoDTO.Atendimento = dataGridAttendance.SelectedItem as AtendimentoDTO;

                    newCollection = new AtendimentoServicoCollectionDTO();
                    newCollection = atendimentoServicoBLL.ReadAttendance(atendimentoServicoDTO.Atendimento);

                    //Devolver a quantidade dos produtos ao estoque
                    foreach (AtendimentoServicoDTO item in newCollection)
                    {
                        servicoProdutoCollectionDTO = new ServicoProdutoCollectionDTO();
                        servicoProdutoCollectionDTO = FillStuffsList(item);

                        foreach (ServicoProdutoDTO item2 in servicoProdutoCollectionDTO)
                        {
                            estoqueDTO = new EstoqueDTO();
                            estoqueDTO.Produto = item2.Produto;
                            estoqueDTO.Filial = atendimentoServicoDTO.Atendimento.Funcionario.Filial;
                            estoqueDTO.Quantidade = item2.Produto.Consumo;
                            estoqueBLL.Create(estoqueDTO);
                        }
                    }
                    //Remove as relações atendimento_servico
                    atendimentoServicoBLL.Delete(atendimentoServicoDTO.Atendimento);
                    //Remove o objeto atendimento
                    atendimentoBLL.Delete(atendimentoServicoDTO.Atendimento);
                    Privileges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (buttonApply)
                {
                    case EnumApplyAction.Create:
                        if (ValidarAtendimento())
                        {
                            //Adicionar o objeto atendimento
                            atendimentoServicoDTO.Atendimento.DataAtendimento = dpDate.SelectedDate.Value;
                            atendimentoServicoDTO.Atendimento.ComenariosAtendimento = txtObsAttend.Text;
                            atendimentoServicoDTO.Atendimento.IdAtendimento = Convert.ToInt32(atendimentoBLL.Create(atendimentoServicoDTO.Atendimento));

                            foreach (AtendimentoServicoDTO item in newCollection)
                            {
                                //Criando relações atendimento x serviço
                                item.Atendimento = atendimentoServicoDTO.Atendimento;
                                atendimentoServicoBLL.Create(item);

                                //Consumindo o produto do estoque
                                servicoProdutoCollectionDTO = new ServicoProdutoCollectionDTO();
                                servicoProdutoCollectionDTO = FillStuffsList(item);

                                foreach (ServicoProdutoDTO item2 in servicoProdutoCollectionDTO)
                                {
                                    estoqueDTO = new EstoqueDTO();
                                    estoqueDTO.Produto = item2.Produto;
                                    estoqueDTO.Filial = atendimentoServicoDTO.Atendimento.Funcionario.Filial;
                                    estoqueDTO.Quantidade = item2.Produto.Consumo;
                                    estoqueBLL.Delete(estoqueDTO);
                                }
                            }
                            //Informar ao banco que o cliente foi atendido.
                            agendamentoBLL.Atendido(agendamentoDTO, atendimentoServicoDTO.Atendimento);
                            //Redesenhar as agendas


                            Privileges();
                        }
                        break;

                    case EnumApplyAction.CreateClient:
                        //Adiciona o cliente na tela
                        atendimentoServicoDTO.Atendimento.Cliente = new ClienteDTO();
                        atendimentoServicoDTO.Atendimento.Cliente = dataGridAddClient.SelectedItem as ClienteDTO;
                        lblClientName.Content = atendimentoServicoDTO.Atendimento.Cliente.Pessoa.NomePessoa;
                        txtlblObsPersonal.Text = atendimentoServicoDTO.Atendimento.Cliente.Pessoa.Comentarios;

                        dataGridAddClient.Visibility = Visibility.Hidden;
                        buttonApply = EnumApplyAction.Create;
                        break;

                    case EnumApplyAction.CreateService:
                        //Adiciona os serviços na lista genérica
                        atendimentoServicoDTO.Servico = new ServicoDTO();
                        atendimentoServicoDTO.Servico = dataGridAddService.SelectedItem as ServicoDTO;

                        newCollection.Add(atendimentoServicoDTO);

                        dataGridService.ItemsSource = null;
                        dataGridService.ItemsSource = newCollection;

                        dataGridAddService.Visibility = Visibility.Hidden;
                        buttonApply = EnumApplyAction.Create;
                        break;

                    case EnumApplyAction.CreateEmployee:
                        //Adiciona o Funcionario na tela
                        atendimentoServicoDTO.Atendimento.Funcionario = new FuncionarioDTO();
                        atendimentoServicoDTO.Atendimento.Funcionario = dataGridAddEmployee.SelectedItem as FuncionarioDTO;
                        lblEmployeeName.Content = atendimentoServicoDTO.Atendimento.Funcionario.Pessoa.NomePessoa;
                        dataGridAddEmployee.Visibility = Visibility.Hidden;
                        buttonApply = EnumApplyAction.Create;
                        break;

                    case EnumApplyAction.Update:

                        if (ValidarAtendimento())
                        {
                            //Adicionar o objeto atendimento
                            if (atendimentoServicoDTO.Atendimento.Cliente.Pessoa.IdPessoa < 1)
                            {
                                atendimentoServicoDTO.Atendimento.Cliente = clienteBLL.ReadName(lblClientName.Content.ToString())[0];
                            }

                            atendimentoServicoDTO.Atendimento.Funcionario = new FuncionarioDTO();
                            atendimentoServicoDTO.Atendimento.Funcionario.Pessoa.IdPessoa = agendamentoDTO.Funcionario.Pessoa.IdPessoa;
                            atendimentoServicoDTO.Atendimento.DataAtendimento = dpDate.SelectedDate.Value;
                            atendimentoServicoDTO.Atendimento.ComenariosAtendimento = txtObsAttend.Text;
                            atendimentoBLL.Update(atendimentoServicoDTO.Atendimento);

                            //Remover Relações
                            foreach (AtendimentoServicoDTO item in oldCollection)
                            {
                                servicoProdutoCollectionDTO = new ServicoProdutoCollectionDTO();
                                servicoProdutoCollectionDTO = FillStuffsList(item);

                                foreach (ServicoProdutoDTO item2 in servicoProdutoCollectionDTO)
                                {
                                    estoqueDTO = new EstoqueDTO();
                                    estoqueDTO.Produto = item2.Produto;
                                    estoqueDTO.Filial = new FilialDTO();
                                    estoqueDTO.Filial.Pessoa.IdPessoa = agendamentoDTO.Funcionario.Pessoa.IdPessoa; //Verificar como o sistema define a filial no Session
                                    estoqueDTO.Quantidade = item2.Produto.Consumo;
                                    estoqueBLL.Create(estoqueDTO);
                                }
                            }
                            //Remove as relações atendimento_servico
                            atendimentoServicoBLL.Delete(atendimentoServicoDTO.Atendimento);

                            //Criar as novas relações
                            foreach (AtendimentoServicoDTO item in newCollection)
                            {
                                //Criando relações atendimento x serviço
                                item.Atendimento = atendimentoServicoDTO.Atendimento;
                                atendimentoServicoBLL.Create(item);

                                //Consumindo o produto do estoque
                                EstoqueDTO estoqueDTO;
                                EstoqueBLL estoqueBLL = new EstoqueBLL();
                                servicoProdutoCollectionDTO = new ServicoProdutoCollectionDTO();
                                servicoProdutoCollectionDTO = FillStuffsList(item);

                                foreach (ServicoProdutoDTO item2 in servicoProdutoCollectionDTO)
                                {
                                    estoqueDTO = new EstoqueDTO();
                                    estoqueDTO.Produto = item2.Produto;
                                    estoqueDTO.Filial = new FilialDTO();
                                    estoqueDTO.Filial.Pessoa.IdPessoa = agendamentoDTO.Funcionario.Pessoa.IdPessoa;
                                    estoqueDTO.Quantidade = item2.Produto.Consumo;
                                    estoqueBLL.Delete(estoqueDTO);
                                }
                            }
                            Privileges();
                        }
                        break;

                    case EnumApplyAction.UpdateClient:
                        atendimentoServicoDTO.Atendimento.Cliente = dataGridAddClient.SelectedItem as ClienteDTO;
                        lblClientName.Content = atendimentoServicoDTO.Atendimento.Cliente.Pessoa.NomePessoa;
                        txtlblObsPersonal.Text = atendimentoServicoDTO.Atendimento.Cliente.Pessoa.Comentarios;

                        dataGridAddClient.Visibility = Visibility.Hidden;
                        buttonApply = EnumApplyAction.Update;
                        break;
                    case EnumApplyAction.UpdateService:

                        atendimentoServicoDTO.Servico = new ServicoDTO();
                        atendimentoServicoDTO.Servico = dataGridAddService.SelectedItem as ServicoDTO;

                        newCollection.Add(atendimentoServicoDTO);

                        dataGridService.ItemsSource = null;
                        dataGridService.ItemsSource = newCollection;

                        dataGridAddService.Visibility = Visibility.Hidden;
                        buttonApply = EnumApplyAction.Update;
                        break;

                    case EnumApplyAction.UpdateEmployee:
                        atendimentoServicoDTO.Atendimento.Funcionario = dataGridAddEmployee.SelectedItem as FuncionarioDTO;
                        lblEmployeeName.Content = atendimentoServicoDTO.Atendimento.Funcionario.Pessoa.NomePessoa;
                        dataGridAddEmployee.Visibility = Visibility.Hidden;
                        buttonApply = EnumApplyAction.Update;
                        break;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            switch (buttonApply)
            {
                case EnumApplyAction.CreateClient:
                    dataGridAddClient.Visibility = Visibility.Hidden;
                    dataGridAddClient.SelectedIndex = -1;
                    buttonApply = EnumApplyAction.Create;
                    break;
                case EnumApplyAction.UpdateClient:
                    dataGridAddClient.Visibility = Visibility.Hidden;
                    dataGridAddClient.SelectedIndex = -1;
                    buttonApply = EnumApplyAction.Update;
                    break;
                case EnumApplyAction.CreateService:
                    dataGridAddService.Visibility = Visibility.Hidden;
                    dataGridAddService.SelectedIndex = -1;
                    buttonApply = EnumApplyAction.Create;
                    break;
                case EnumApplyAction.UpdateService:
                    dataGridAddService.Visibility = Visibility.Hidden;
                    dataGridAddService.SelectedIndex = -1;
                    buttonApply = EnumApplyAction.Update;
                    break;
                default:
                    Privileges();
                    break;
            }
        }
        private void btnSearchClient_Click(object sender, RoutedEventArgs e)
        {
            dataGridAddClient.Visibility = Visibility.Visible;

            switch (buttonApply)
            {
                case EnumApplyAction.Create:
                    buttonApply = EnumApplyAction.CreateClient;
                    break;
                case EnumApplyAction.CreateService:
                    buttonApply = EnumApplyAction.CreateClient;
                    break;
                case EnumApplyAction.Update:
                    buttonApply = EnumApplyAction.UpdateClient;
                    break;
                case EnumApplyAction.UpdateService:
                    buttonApply = EnumApplyAction.UpdateClient;
                    break;
            }
        }
        private void btnSearchService_Click(object sender, RoutedEventArgs e)
        {
            dataGridAddService.Visibility = Visibility.Visible;
            switch (buttonApply)
            {
                case EnumApplyAction.Create:
                    buttonApply = EnumApplyAction.CreateService;
                    break;
                case EnumApplyAction.CreateClient:
                    buttonApply = EnumApplyAction.CreateService;
                    break;
                case EnumApplyAction.Update:
                    buttonApply = EnumApplyAction.UpdateService;
                    break;
                case EnumApplyAction.UpdateClient:
                    buttonApply = EnumApplyAction.UpdateService;
                    break;
            }
        }
        private void btnRemoveService_Click(object sender, RoutedEventArgs e)
        {
            atendimentoServicoDTO = new AtendimentoServicoDTO();
            atendimentoServicoDTO = dataGridService.SelectedItem as AtendimentoServicoDTO;
            newCollection.Remove(atendimentoServicoDTO);

            dataGridService.ItemsSource = null;
            dataGridService.ItemsSource = newCollection;
        }
        private void dataGridService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridService.SelectedIndex != -1)
            {
                atendimentoServicoDTO = new AtendimentoServicoDTO();
                atendimentoServicoDTO = dataGridService.SelectedItem as AtendimentoServicoDTO;

                dataGridStuff.ItemsSource = null;
                dataGridStuff.ItemsSource = FillStuffsList(atendimentoServicoDTO);
            }
        }
        private void dataGridAttendance_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = btnRemove.IsEnabled = true;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnSearchEmployee_Click(object sender, RoutedEventArgs e)
        {
            ListEmployees();
            dataGridAddEmployee.Visibility = Visibility.Visible;

            switch (buttonApply)
            {
                case EnumApplyAction.Create:
                    buttonApply = EnumApplyAction.CreateEmployee;
                    break;
                case EnumApplyAction.CreateService:
                    buttonApply = EnumApplyAction.CreateEmployee;
                    break;
                case EnumApplyAction.Update:
                    buttonApply = EnumApplyAction.UpdateEmployee;
                    break;
                case EnumApplyAction.UpdateService:
                    buttonApply = EnumApplyAction.UpdateEmployee;
                    break;
            }
        }

        #endregion
    }
}