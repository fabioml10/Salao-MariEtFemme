using System;
using System.Windows;
using System.Windows.Controls;
using MariEtFemme.Tools;
using MariEtFemme.DTO;
using MariEtFemme.BLL;

//Implantar
//Alerta para aniversariante
//Pesquisa dinâmica
//Melhorar máscara de telefone
//Colocar código (id) do cliente

namespace MariEtFemme.View
{
    public partial class ClientTool : Window
    {
        public ClientTool()
        {
            InitializeComponent();
        }

        #region Variables

        IndividualRegistration frmRegistration;
        /// <summary>
        /// Define qual ação o botão Confirmar deve tomar.
        /// </summary>
        private EnumApplyAction buttonApply;

        ClienteDTO clienteDTO;
        ClienteCollectionDTO clienteCollectionDTO;
        ClienteBLL clienteBLL = new ClienteBLL();
        PessoaBLL pessoaBLL = new PessoaBLL();

        #endregion

        #region Functions
        private void DecidirFormulario()
        {
            if (gridControl.Children.Contains(frmRegistration))
            {
                gridControl.Children.Clear();
            }
            
            frmRegistration = new IndividualRegistration();
            if (rbIndividual.IsChecked.Value)
            {
                frmRegistration.gridIndividualData.Visibility = Visibility.Visible;
                frmRegistration.gridCorporateData.Visibility = Visibility.Hidden;
            }
            else
            {
                frmRegistration.gridIndividualData.Visibility = Visibility.Hidden;
                frmRegistration.gridCorporateData.Visibility = Visibility.Visible;
            }

            gridControl.Children.Add(frmRegistration);

        }
        private void FillClientList()
        {
            clienteCollectionDTO = new ClienteCollectionDTO();
            clienteCollectionDTO = clienteBLL.ReadName(string.Empty);

            dataGridClient.ItemsSource = null;
            dataGridClient.ItemsSource = clienteCollectionDTO;
        }

        /// <summary>
        /// Devolve à página as suas condiçõies iniciais.
        /// </summary>
        private void Privileges()
        {
            buttonApply = new EnumApplyAction();
            FillClientList();
            DecidirFormulario();            

            switch (Session.LoggedUser.Usuario.Privilegio.IdPrivilegio)
            {
                default:
                    //Buttons
                    btnNew.Visibility =
                      btnEdit.Visibility =
                    btnRemove.Visibility =
                    btnExistingPerson.Visibility = Visibility.Visible;

                    btnEdit.IsEnabled =
                  btnRemove.IsEnabled = false;

                    btnApply.Visibility =
                          btnCancel.Visibility = Visibility.Hidden;

                    //Lists
                    dataGridClient.Visibility = Visibility.Visible;

                    //
                    gbPersonType.IsEnabled = true;
                    rbIndividual.IsChecked = true;
                    break;
            }
        }
        private void PreencherCliente(ClienteDTO cliente)
        {
            cliente.Pessoa.TipoPessoa = rbIndividual.IsChecked.Value;
            cliente.Pessoa.Comentarios = frmRegistration.txtObs.Text;

            cliente.Pessoa.Endereco = new PessoaEnderecoDTO();

            if (rbIndividual.IsChecked.Value)
            {
                cliente.Pessoa.NomePessoa = frmRegistration.txtPersonName.Text;

                cliente.Pessoa.PessoaFisica = new PessoaFisicaDTO();
                cliente.Pessoa.PessoaFisica.Nascimento = frmRegistration.dpBirthDate.SelectedDate.Value;
                cliente.Pessoa.PessoaFisica.Genero = Convert.ToBoolean(frmRegistration.rbFemale.IsChecked);
            }
            else
            {
                cliente.Pessoa.PessoaJuridica = new PessoaJuridicaDTO();
                cliente.Pessoa.NomePessoa = frmRegistration.txtCorporateName.Text;
                cliente.Pessoa.PessoaJuridica.RazaoSocial = frmRegistration.txtRazaoSocial.Text;
                cliente.Pessoa.PessoaJuridica.CNPJ = frmRegistration.txtCNPJ.Text;
                cliente.Pessoa.Endereco.Rua = frmRegistration.txtStreet.Text;
                cliente.Pessoa.Endereco.Numero = frmRegistration.txtNr.Text;
            }

            cliente.Pessoa.Endereco.Bairro = frmRegistration.txtNeighborhood.Text;
            cliente.Pessoa.Endereco.Cidade = frmRegistration.txtCity.Text;

            cliente.Pessoa.Endereco.Estado = new EstadoDTO();
            foreach (EstadoDTO item in frmRegistration.estadoCollectionDTO)
            {
                if(string.Compare(item.SiglaEstado, frmRegistration.cbState.SelectedItem.ToString()).Equals(0))
                cliente.Pessoa.Endereco.Estado.IdEstado = item.IdEstado;
            }           

            cliente.Pessoa.Contato = new PessoaContatoDTO();
            cliente.Pessoa.Contato.Email = frmRegistration.txtEmail.Text;
            cliente.Pessoa.Contato.Telefone1 = frmRegistration.txtPhone1.Text;
            cliente.Pessoa.Contato.WhatsApp1 = Convert.ToBoolean(frmRegistration.checkWhats1.IsChecked);
            cliente.Pessoa.Contato.Operadora1 = new OperadoraDTO();
            foreach (OperadoraDTO item in frmRegistration.operadoraCollectionDTO)
            {
                if (string.Compare(item.DescricaoOperadora, frmRegistration.cbOperatorPhone1.SelectedItem.ToString()).Equals(0))
                {
                    cliente.Pessoa.Contato.Operadora1.IdOperadora = item.IdOperadora;
                    break;
                }
            }

            cliente.Pessoa.Contato.Telefone2 = frmRegistration.txtPhone2.Text;
            cliente.Pessoa.Contato.WhatsApp2 = Convert.ToBoolean(frmRegistration.checkWhats2.IsChecked);
            cliente.Pessoa.Contato.Operadora2 = new OperadoraDTO();
            foreach (OperadoraDTO item in frmRegistration.operadoraCollectionDTO)
            {
                if (string.Compare(item.DescricaoOperadora, frmRegistration.cbOperatorPhone2.SelectedItem.ToString()).Equals(0))
                {
                    cliente.Pessoa.Contato.Operadora2.IdOperadora = item.IdOperadora;
                    break;
                }
            }

            cliente.Pessoa.Contato.Telefone3 = frmRegistration.txtPhone3.Text;
            cliente.Pessoa.Contato.WhatsApp3 = Convert.ToBoolean(frmRegistration.checkWhats3.IsChecked);
            cliente.Pessoa.Contato.Operadora3 = new OperadoraDTO();
            foreach (OperadoraDTO item in frmRegistration.operadoraCollectionDTO)
            {
                if (string.Compare(item.DescricaoOperadora, frmRegistration.cbOperatorPhone3.SelectedItem.ToString()).Equals(0))
                {
                    cliente.Pessoa.Contato.Operadora3.IdOperadora = item.IdOperadora;
                    break;
                }
            }   
        }
        private void PreencherFormulario(ClienteDTO cliente)
        {
            if(cliente.Pessoa.TipoPessoa)
            {
                frmRegistration.txtPersonName.Text = cliente.Pessoa.NomePessoa;
                frmRegistration.dpBirthDate.SelectedDate = cliente.Pessoa.PessoaFisica.Nascimento;
                frmRegistration.rbFemale.IsChecked = cliente.Pessoa.PessoaFisica.Genero;
                frmRegistration.rbMale.IsChecked = !cliente.Pessoa.PessoaFisica.Genero;
            }
            else
            {
                frmRegistration.txtCorporateName.Text = cliente.Pessoa.NomePessoa;
                frmRegistration.txtRazaoSocial.Text = cliente.Pessoa.PessoaJuridica.RazaoSocial;
                frmRegistration.txtCNPJ.Text = cliente.Pessoa.PessoaJuridica.CNPJ;
                frmRegistration.txtStreet.Text = cliente.Pessoa.Endereco.Rua;
                frmRegistration.txtNr.Text = cliente.Pessoa.Endereco.Numero;
            }

            frmRegistration.txtNeighborhood.Text = cliente.Pessoa.Endereco.Bairro;
            frmRegistration.txtCity.Text = cliente.Pessoa.Endereco.Cidade;
            frmRegistration.cbState.SelectedValue = cliente.Pessoa.Endereco.Estado.SiglaEstado;
            
            frmRegistration.txtPhone1.Text = cliente.Pessoa.Contato.Telefone1;
            frmRegistration.cbOperatorPhone1.SelectedItem = cliente.Pessoa.Contato.Operadora1.DescricaoOperadora;
            frmRegistration.checkWhats1.IsChecked = cliente.Pessoa.Contato.WhatsApp1;

            frmRegistration.txtPhone2.Text = cliente.Pessoa.Contato.Telefone2;
            frmRegistration.cbOperatorPhone2.SelectedItem = cliente.Pessoa.Contato.Operadora2.DescricaoOperadora;
            frmRegistration.checkWhats2.IsChecked = cliente.Pessoa.Contato.WhatsApp2;

            frmRegistration.txtPhone3.Text = cliente.Pessoa.Contato.Telefone3;
            frmRegistration.cbOperatorPhone3.SelectedItem = cliente.Pessoa.Contato.Operadora3.DescricaoOperadora;
            frmRegistration.checkWhats3.IsChecked = cliente.Pessoa.Contato.WhatsApp3;
            frmRegistration.txtEmail.Text = cliente.Pessoa.Contato.Email;

            frmRegistration.txtObs.Text = cliente.Pessoa.Comentarios;
        }

        /// <summary>
        ///Valida todos os campos antes de criar os registros no banco.
        /// </summary>
        private bool ValidarCliente()
        {
            if (!string.IsNullOrEmpty(frmRegistration.txtPersonName.Text))
            {
                if (!frmRegistration.dpBirthDate.SelectedDate.Equals(null))
                {
                    if (frmRegistration.rbFemale.IsChecked.Value || frmRegistration.rbMale.IsChecked.Value)
                    {
                        if (!frmRegistration.cbState.SelectedIndex.Equals(-1))
                        {
                            if (!frmRegistration.cbOperatorPhone1.SelectedIndex.Equals(-1) && !frmRegistration.cbOperatorPhone2.SelectedIndex.Equals(-1) && !frmRegistration.cbOperatorPhone3.SelectedIndex.Equals(-1))
                            {
                                return true;
                            }
                            else
                            {
                                MessageBox.Show("Favor, selecionar uma operadora de telefone ou selecionar nenhum.");
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Favor, selecionar um estado ou selecionar nenhum.");
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Favor, selecionar um gênero.");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Favor, selecionar uma data de nascimento.");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("O campo Nome Completo não pode estar em branco.");
                return false;
            }
        }

        #endregion

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            DecidirFormulario();
            //Buttons
            btnNew.Visibility =
              btnEdit.Visibility =
            btnRemove.Visibility = Visibility.Hidden;

            btnApply.Visibility =
           btnCancel.Visibility = Visibility.Visible;

            //Lists
            dataGridClient.Visibility = Visibility.Hidden;

            rbIndividual.IsChecked = true;

            clienteDTO = new ClienteDTO();
            buttonApply = EnumApplyAction.Create;            
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (buttonApply)
                {
                    case EnumApplyAction.Create:
                        if (frmRegistration.gridCommon.IsVisible)
                        {
                            if (ValidarCliente())
                            {
                                PreencherCliente(clienteDTO);
                                string result = pessoaBLL.Create(clienteDTO.Pessoa);

                                int resultParse = 0;                                       
                                
                                switch (int.TryParse(result, out resultParse))
                                {
                                    case true:
                                        clienteDTO.Pessoa.IdPessoa = resultParse;
                                        if (string.Compare(clienteBLL.Create(clienteDTO), "Sucesso").Equals(0))
                                        {
                                            MessageBox.Show("Cliente cadastrado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                                            Privileges();
                                        }
                                        else
                                        {
                                            MessageBox.Show(result, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                                        }                                        
                                        break;
                                    default:
                                        MessageBox.Show(result, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                                        break;
                                }
                            }
                        }
                        else
                        {
                            clienteBLL.Create(clienteDTO);
                            MessageBox.Show("Cliente cadastrado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                            Privileges();
                        }
                        break;

                    case EnumApplyAction.CreateClient:
                        clienteDTO = new ClienteDTO();
                        clienteDTO = dataGridSearchPerson.SelectedItem as ClienteDTO;
                        frmRegistration.txtPersonName.Text = clienteDTO.Pessoa.NomePessoa;
                        frmRegistration.txtPersonName.IsReadOnly = true;

                        dataGridSearchPerson.Visibility = Visibility.Hidden;
                        frmRegistration.gridCommon.Visibility =
                        frmRegistration.lblBirthDate.Visibility =
                        frmRegistration.dpBirthDate.Visibility =
                        frmRegistration.gbGender.Visibility = Visibility.Hidden;

                        frmRegistration.txtCorporateName.Text = clienteDTO.Pessoa.NomePessoa;
                        frmRegistration.txtCorporateName.IsReadOnly = true;
                        frmRegistration.txtRazaoSocial.Visibility =
                        frmRegistration.lblRazaoSocial.Visibility =
                        frmRegistration.txtCNPJ.Visibility =
                        frmRegistration.lblCNPJ. Visibility =
                        frmRegistration.txtStreet.Visibility =
                        frmRegistration.lblStreet.Visibility =
                        frmRegistration.txtNr.Visibility =
                        frmRegistration.lblNr.Visibility = Visibility.Hidden;
                        buttonApply = EnumApplyAction.Create;
                        break;

                    case EnumApplyAction.Update:
                        if (ValidarCliente())
                        {
                            PreencherCliente(clienteDTO);
                            string result = clienteBLL.Update(clienteDTO);
                            switch (result)
                            {
                                case "Sucesso":
                                    MessageBox.Show("Cliente modificado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                                    Privileges();
                                    break;
                                default:
                                    MessageBox.Show(result, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                                    break;
                            }
                        }
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
                    Privileges();
                    //Buttons
                    btnNew.Visibility =
                      btnEdit.Visibility =
                    btnRemove.Visibility = Visibility.Hidden;

                    btnApply.Visibility =
                   btnCancel.Visibility = Visibility.Visible;

                    //Lists
                    dataGridClient.Visibility = Visibility.Hidden;
                    dataGridSearchPerson.Visibility = Visibility.Hidden;

                    clienteDTO = new ClienteDTO();
                    buttonApply = EnumApplyAction.Create;
                    break;
                default:
                    Privileges();
                    break;
            }
            
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            //Buttons
            btnNew.Visibility =
              btnEdit.Visibility =
            btnRemove.Visibility =
            btnExistingPerson.Visibility = Visibility.Hidden;

            btnApply.Visibility =
           btnCancel.Visibility = Visibility.Visible;

            //Lists
            dataGridClient.Visibility = Visibility.Hidden;

            //Values
            buttonApply = EnumApplyAction.Update;

            //Box
            gbPersonType.IsEnabled = false;
            
            clienteDTO = dataGridClient.SelectedItem as ClienteDTO;
            rbIndividual.IsChecked = clienteDTO.Pessoa.TipoPessoa;
            rbCorporate.IsChecked = !clienteDTO.Pessoa.TipoPessoa;
            DecidirFormulario();
            PreencherFormulario(clienteDTO);            
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clienteDTO = dataGridClient.SelectedItem as ClienteDTO;

                if (MessageBox.Show("Realmente deseja excluir o cliente " + clienteDTO.Pessoa.NomePessoa + "?", "Remover Cliente", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    string result = clienteBLL.Delete(clienteDTO);
                    switch (result)
                    {
                        case "Sucesso":
                            MessageBox.Show("Cliente excluído com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                            Privileges();
                            break;
                        case "Funcionario":
                            MessageBox.Show("Cliente excluído com sucesso.\nEssa pessoa ainda está cadastrada como funcionário.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                            Privileges();
                            break;
                        case "Fornecedor":
                            MessageBox.Show("Cliente excluído com sucesso.\nEssa pessoa ainda está cadastrada como fornecedor.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                            Privileges();
                            break;
                        case "Filial":
                            MessageBox.Show("Cliente excluído com sucesso.\nEssa pessoa ainda está cadastrada como filial.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                            Privileges();
                            break;
                        default:
                            MessageBox.Show(result, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dataGridClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = btnRemove.IsEnabled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Privileges();
        }

        private void rbCorporate_Unchecked(object sender, RoutedEventArgs e)
        {
            DecidirFormulario();
        }

        private void rbIndividual_Unchecked(object sender, RoutedEventArgs e)
        {
            DecidirFormulario();
        }

        private void btnExistingPerson_Click(object sender, RoutedEventArgs e)
        {
            ClienteCollectionDTO clienteCollectionDTOTeste = new ClienteCollectionDTO();
            clienteCollectionDTOTeste = clienteBLL.ReadExcept(rbIndividual.IsChecked.Value);
            dataGridSearchPerson.Visibility = Visibility.Visible;
            dataGridSearchPerson.ItemsSource = null;
            dataGridSearchPerson.ItemsSource = clienteCollectionDTOTeste;
            buttonApply = EnumApplyAction.CreateClient;
        }
    }
}