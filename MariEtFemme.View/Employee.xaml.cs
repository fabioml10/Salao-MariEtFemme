using System;
using System.Windows;
using System.Windows.Controls;
using MariEtFemme.BLL;
using MariEtFemme.DTO;
using MariEtFemme.Tools;

namespace MariEtFemme.View
{
    public partial class Employee : Window
    {
        public Employee()
        {
            InitializeComponent();
        }

        #region Variáveis

        IndividualRegistration frmRegistration;

        private EnumApplyAction buttonApply = new EnumApplyAction();

        FuncionarioDTO funcionarioDTO = new FuncionarioDTO();
        FuncionarioCollectionDTO funcionarioCollectionDTO;
        FuncionarioBLL funcionarioBLL = new FuncionarioBLL();

        PessoaBLL pessoaBLL = new PessoaBLL();

        FilialCollectionDTO filialCollectionDTO;
        FilialBLL filialBLL = new FilialBLL();

        CargoCollectionDTO cargoCollectionDTO;
        CargoBLL cargoBLL = new CargoBLL();

        #endregion

        #region Métodos
        private void ListarFuncionarios()
        {
            try
            {
                funcionarioCollectionDTO = new FuncionarioCollectionDTO();
                funcionarioCollectionDTO = funcionarioBLL.ReadName(string.Empty);
                dataGridEmployee.ItemsSource = null;
                dataGridEmployee.ItemsSource = funcionarioCollectionDTO;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ListarCargos()
        {
            try
            {
                cargoCollectionDTO = new CargoCollectionDTO();
                cargoCollectionDTO = cargoBLL.ReadName(string.Empty);

                cbPost.Items.Clear();
                foreach (CargoDTO item in cargoCollectionDTO)
                {
                    cbPost.Items.Add(item.DescricaoCargo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ListarFiliais()
        {
            try
            {
                filialCollectionDTO = new FilialCollectionDTO();
                filialCollectionDTO = filialBLL.ReadName(string.Empty);

                cbFilial.Items.Clear();
                foreach (FilialDTO item in filialCollectionDTO)
                {
                    cbFilial.Items.Add(item.Pessoa.NomePessoa);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DecidirFormulario()
        {
            try
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void PreencherCliente(FuncionarioDTO funcionario, bool novaInstancia)
        {
            try
            {
                if (novaInstancia)
                {
                    funcionario.Pessoa = new PessoaDTO();
                    funcionario.Pessoa.PessoaFisica = new PessoaFisicaDTO();
                    funcionario.Pessoa.PessoaJuridica = new PessoaJuridicaDTO();
                    funcionario.Pessoa.Endereco = new PessoaEnderecoDTO();
                    funcionario.Pessoa.Endereco.Estado = new EstadoDTO();
                    funcionario.Pessoa.Contato = new PessoaContatoDTO();
                    funcionario.Pessoa.Contato.Operadora1 = new OperadoraDTO();
                    funcionario.Pessoa.Contato.Operadora2 = new OperadoraDTO();
                    funcionario.Pessoa.Contato.Operadora3 = new OperadoraDTO();
                    funcionario.Cargo = new CargoDTO();
                    funcionario.Filial = new FilialDTO();
                    funcionario.Filial.Pessoa = new PessoaDTO();
                }

                funcionario.Pessoa.TipoPessoa = rbIndividual.IsChecked.Value;
                funcionario.Pessoa.Comentarios = frmRegistration.txtObs.Text;

                if (rbIndividual.IsChecked.Value)
                {
                    funcionario.Pessoa.NomePessoa = frmRegistration.txtPersonName.Text;
                    funcionario.Pessoa.PessoaFisica.Nascimento = frmRegistration.dpBirthDate.SelectedDate.Value;
                    funcionario.Pessoa.PessoaFisica.Genero = Convert.ToBoolean(frmRegistration.rbFemale.IsChecked);
                }
                else
                {
                    funcionario.Pessoa.NomePessoa = frmRegistration.txtCorporateName.Text;
                    funcionario.Pessoa.PessoaJuridica.RazaoSocial = frmRegistration.txtRazaoSocial.Text;
                    funcionario.Pessoa.PessoaJuridica.CNPJ = frmRegistration.txtCNPJ.Text;
                    funcionario.Pessoa.Endereco.Rua = frmRegistration.txtStreet.Text;
                    funcionario.Pessoa.Endereco.Numero = frmRegistration.txtNr.Text;
                }

                funcionario.Pessoa.Endereco.Bairro = frmRegistration.txtNeighborhood.Text;
                funcionario.Pessoa.Endereco.Cidade = frmRegistration.txtCity.Text;

                foreach (EstadoDTO item in frmRegistration.estadoCollectionDTO)
                {
                    if (string.Compare(item.SiglaEstado, frmRegistration.cbState.SelectedItem.ToString()).Equals(0))
                    {
                        funcionario.Pessoa.Endereco.Estado.IdEstado = item.IdEstado;
                        break;
                    }
                }

                funcionario.Pessoa.Contato.Email = frmRegistration.txtEmail.Text;
                funcionario.Pessoa.Contato.Telefone1 = frmRegistration.txtPhone1.Text;
                funcionario.Pessoa.Contato.WhatsApp1 = Convert.ToBoolean(frmRegistration.checkWhats1.IsChecked);

                foreach (OperadoraDTO item in frmRegistration.operadoraCollectionDTO)
                {
                    if (string.Compare(item.DescricaoOperadora, frmRegistration.cbOperatorPhone1.SelectedItem.ToString()).Equals(0))
                    {
                        funcionario.Pessoa.Contato.Operadora1.IdOperadora = item.IdOperadora;
                        break;
                    }
                }

                funcionario.Pessoa.Contato.Telefone2 = frmRegistration.txtPhone2.Text;
                funcionario.Pessoa.Contato.WhatsApp2 = Convert.ToBoolean(frmRegistration.checkWhats2.IsChecked);

                foreach (OperadoraDTO item in frmRegistration.operadoraCollectionDTO)
                {
                    if (string.Compare(item.DescricaoOperadora, frmRegistration.cbOperatorPhone2.SelectedItem.ToString()).Equals(0))
                    {
                        funcionario.Pessoa.Contato.Operadora2.IdOperadora = item.IdOperadora;
                        break;
                    }
                }

                funcionario.Pessoa.Contato.Telefone3 = frmRegistration.txtPhone3.Text;
                funcionario.Pessoa.Contato.WhatsApp3 = Convert.ToBoolean(frmRegistration.checkWhats3.IsChecked);

                foreach (OperadoraDTO item in frmRegistration.operadoraCollectionDTO)
                {
                    if (string.Compare(item.DescricaoOperadora, frmRegistration.cbOperatorPhone3.SelectedItem.ToString()).Equals(0))
                    {
                        funcionario.Pessoa.Contato.Operadora3.IdOperadora = item.IdOperadora;
                        break;
                    }
                }

                funcionario.Cargo.IdCargo = cargoBLL.ReadName(cbPost.SelectedItem.ToString())[0].IdCargo;

                funcionario.Filial.Pessoa.IdPessoa = filialBLL.ReadName(cbFilial.SelectedItem.ToString())[0].Pessoa.IdPessoa;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void PreencherFormulario(FuncionarioDTO funcionario)
        {
            try
            {
                if (funcionario.Pessoa.TipoPessoa)
                {
                    frmRegistration.txtPersonName.Text = funcionario.Pessoa.NomePessoa;
                    frmRegistration.dpBirthDate.SelectedDate = funcionario.Pessoa.PessoaFisica.Nascimento;
                    frmRegistration.rbFemale.IsChecked = funcionario.Pessoa.PessoaFisica.Genero;
                    frmRegistration.rbMale.IsChecked = !funcionario.Pessoa.PessoaFisica.Genero;
                }
                else
                {
                    frmRegistration.txtCorporateName.Text = funcionario.Pessoa.NomePessoa;
                    frmRegistration.txtRazaoSocial.Text = funcionario.Pessoa.PessoaJuridica.RazaoSocial;
                    frmRegistration.txtCNPJ.Text = funcionario.Pessoa.PessoaJuridica.CNPJ;
                    frmRegistration.txtStreet.Text = funcionario.Pessoa.Endereco.Rua;
                    frmRegistration.txtNr.Text = funcionario.Pessoa.Endereco.Numero;
                }

                frmRegistration.txtNeighborhood.Text = funcionario.Pessoa.Endereco.Bairro;
                frmRegistration.txtCity.Text = funcionario.Pessoa.Endereco.Cidade;
                frmRegistration.cbState.SelectedValue = funcionario.Pessoa.Endereco.Estado.SiglaEstado;

                frmRegistration.txtPhone1.Text = funcionario.Pessoa.Contato.Telefone1;
                frmRegistration.cbOperatorPhone1.SelectedItem = funcionario.Pessoa.Contato.Operadora1.DescricaoOperadora;
                frmRegistration.checkWhats1.IsChecked = funcionario.Pessoa.Contato.WhatsApp1;

                frmRegistration.txtPhone2.Text = funcionario.Pessoa.Contato.Telefone2;
                frmRegistration.cbOperatorPhone2.SelectedItem = funcionario.Pessoa.Contato.Operadora2.DescricaoOperadora;
                frmRegistration.checkWhats2.IsChecked = funcionario.Pessoa.Contato.WhatsApp2;

                frmRegistration.txtPhone3.Text = funcionario.Pessoa.Contato.Telefone3;
                frmRegistration.cbOperatorPhone3.SelectedItem = funcionario.Pessoa.Contato.Operadora3.DescricaoOperadora;
                frmRegistration.checkWhats3.IsChecked = funcionario.Pessoa.Contato.WhatsApp3;
                frmRegistration.txtEmail.Text = funcionario.Pessoa.Contato.Email;

                frmRegistration.txtObs.Text = funcionario.Pessoa.Comentarios;

                cbPost.SelectedItem = funcionario.Cargo.DescricaoCargo;

                cbFilial.SelectedItem = funcionario.Filial.Pessoa.NomePessoa;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Privilegios()
        {
            //Fill
            ListarFuncionarios();
            ListarCargos();
            ListarFiliais();

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
            dataGridEmployee.Visibility = Visibility.Visible;

            //Controls
            gbPersonType.IsEnabled = true;
            rbIndividual.IsChecked = true;
            lblFilial.Visibility = lblPost.Visibility = cbFilial.Visibility = cbPost.Visibility = Visibility.Visible;

            switch (Session.LoggedUser.Usuario.Privilegio.IdPrivilegio)
            {
                default:
                    break;
            }
        }
        private bool ValidarFormulário()
        {
            if(!cbFilial.SelectedIndex.Equals(-1))
            {
                if(!cbPost.SelectedIndex.Equals(-1))
                {
                    if(!string.IsNullOrEmpty(frmRegistration.txtPersonName.Text) || !string.IsNullOrEmpty(frmRegistration.txtCorporateName.Text))
                    {
                        if (rbIndividual.IsChecked.Value)
                        {
                            if (frmRegistration.rbFemale.IsChecked.Value || frmRegistration.rbMale.IsChecked.Value)
                            {
                                return true;
                            }
                            else
                            {
                                MessageBox.Show("Favor, selecionar um gênero.");
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Favor, preencher o campo Nome.");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Favor, selecionar um cargo.");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Favor, selecionar uma filial.");
                return false;
            }
        }

        #endregion

        #region Eventos
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            //Buttons
            btnNew.Visibility =
            btnEdit.Visibility = Visibility.Hidden;
            btnRemove.Visibility = Visibility.Hidden;

            btnApply.Visibility =
            btnCancel.Visibility = Visibility.Visible;

            //Lists
            dataGridEmployee.Visibility = Visibility.Hidden;

            rbIndividual.IsChecked = true;

            buttonApply = EnumApplyAction.Create;
            DecidirFormulario();
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
            dataGridEmployee.Visibility = Visibility.Hidden;

            //Values
            buttonApply = EnumApplyAction.Update;

            //Box
            gbPersonType.IsEnabled = false;

            funcionarioDTO = new FuncionarioDTO();
            funcionarioDTO = dataGridEmployee.SelectedItem as FuncionarioDTO;
            rbIndividual.IsChecked = funcionarioDTO.Pessoa.TipoPessoa;
            rbCorporate.IsChecked = !funcionarioDTO.Pessoa.TipoPessoa;
            DecidirFormulario();
            PreencherFormulario(funcionarioDTO);
        }
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (buttonApply)
                {
                    case EnumApplyAction.Create:
                        if (ValidarFormulário())
                        {
                            if (frmRegistration.gridCommon.IsVisible) //Pessoa não existe
                            {
                                PreencherCliente(funcionarioDTO, true);
                                string result = pessoaBLL.Create(funcionarioDTO.Pessoa);

                                int resultParse = 0;
                                switch (int.TryParse(result, out resultParse))
                                {
                                    case true:
                                        funcionarioDTO.Pessoa.IdPessoa = resultParse;
                                        if (string.Compare(funcionarioBLL.Create(funcionarioDTO), "Sucesso").Equals(0))
                                        {
                                            MessageBox.Show("Funcionário cadastrado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                                            Privilegios();
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
                            else //Pessoa já existe
                            {
                                funcionarioDTO.Cargo = new CargoDTO();
                                funcionarioDTO.Cargo = cargoBLL.ReadName(cbPost.SelectedItem.ToString())[0];

                                funcionarioDTO.Filial = new FilialDTO();
                                funcionarioDTO.Filial.Pessoa = new PessoaDTO();
                                funcionarioDTO.Filial.Pessoa.IdPessoa = filialBLL.ReadName(cbFilial.SelectedItem.ToString())[0].Pessoa.IdPessoa;

                                funcionarioBLL.Create(funcionarioDTO);
                                MessageBox.Show("Funcionario cadastrado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                                Privilegios();
                            }
                        }
                        break;

                    case EnumApplyAction.CreateClient:
                        funcionarioDTO = dataGridSearchPerson.SelectedItem as FuncionarioDTO;
                        frmRegistration.txtPersonName.Text = funcionarioDTO.Pessoa.NomePessoa;
                        frmRegistration.txtPersonName.IsReadOnly = true;

                        frmRegistration.gridCommon.Visibility =
                        frmRegistration.lblBirthDate.Visibility =
                        frmRegistration.dpBirthDate.Visibility =
                        frmRegistration.gbGender.Visibility = Visibility.Hidden;

                        frmRegistration.txtCorporateName.Text = funcionarioDTO.Pessoa.NomePessoa;
                        frmRegistration.txtCorporateName.IsReadOnly = true;
                        frmRegistration.txtRazaoSocial.Visibility =
                        frmRegistration.lblRazaoSocial.Visibility =
                        frmRegistration.txtCNPJ.Visibility =
                        frmRegistration.lblCNPJ.Visibility =
                        frmRegistration.txtStreet.Visibility =
                        frmRegistration.lblStreet.Visibility =
                        frmRegistration.txtNr.Visibility =
                        frmRegistration.lblNr.Visibility = Visibility.Hidden;

                        dataGridSearchPerson.Visibility = Visibility.Hidden;

                        buttonApply = EnumApplyAction.Create;
                        break;

                    case EnumApplyAction.Update:
                        PreencherCliente(funcionarioDTO, false);
                        string result2 = pessoaBLL.Update(funcionarioDTO.Pessoa);
                        switch (result2)
                        {
                            case "Sucesso":
                                if (ValidarFormulário())
                                {
                                    if (string.Compare(funcionarioBLL.Update(funcionarioDTO), "Sucesso").Equals(0))
                                    {
                                        MessageBox.Show("Funcionário modificado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                                        Privilegios();
                                    }
                                    else
                                    {
                                        MessageBox.Show(result2, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }
                                break;
                            default:
                                MessageBox.Show(result2, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                funcionarioDTO = new FuncionarioDTO();
                funcionarioDTO = dataGridEmployee.SelectedItem as FuncionarioDTO;

                if (MessageBox.Show("Realmente deseja excluir o funcionário " + funcionarioDTO.Pessoa.NomePessoa + "?", "Remover Funcionario", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    string result = funcionarioBLL.Delete(funcionarioDTO);
                    switch (result)
                    {
                        case "Sucesso":
                            MessageBox.Show("Funcionario excluído com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                            Privilegios();
                            break;
                        case "Fornecedor":
                            MessageBox.Show("Funcionario excluído com sucesso.\nEssa pessoa ainda está cadastrada como fornecedor.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                            Privilegios();
                            break;
                        case "Cliente":
                            MessageBox.Show("Funcionario excluído com sucesso.\nEssa pessoa ainda está cadastrada como cliente.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                            Privilegios();
                            break;
                        case "Filial":
                            MessageBox.Show("Funcionario excluído com sucesso.\nEssa pessoa ainda está cadastrada como filial.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                            Privilegios();
                            break;
                        default:
                            MessageBox.Show(result, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            switch (buttonApply)
            {
                case EnumApplyAction.CreateClient:
                    Privilegios();
                    //Buttons
                    btnNew.Visibility =
                      btnEdit.Visibility =
                    btnRemove.Visibility = Visibility.Hidden;

                    btnApply.Visibility =
                   btnCancel.Visibility = Visibility.Visible;

                    //Lists
                    dataGridEmployee.Visibility = Visibility.Hidden;
                    dataGridSearchPerson.Visibility = Visibility.Hidden;

                    buttonApply = EnumApplyAction.Create;
                    break;
                default:
                    Privilegios();
                    break;
            }
        }
        private void btnExistingPerson_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                funcionarioDTO = new FuncionarioDTO();
                FuncionarioCollectionDTO listarGrid = new FuncionarioCollectionDTO();
                listarGrid = funcionarioBLL.ReadExcept(rbIndividual.IsChecked.Value);

                dataGridSearchPerson.Visibility = Visibility.Visible;
                dataGridSearchPerson.ItemsSource = null;
                dataGridSearchPerson.ItemsSource = listarGrid;
                buttonApply = EnumApplyAction.CreateClient;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void rbCorporate_Unchecked(object sender, RoutedEventArgs e)
        {
            DecidirFormulario();
        }
        private void rbIndividual_Unchecked(object sender, RoutedEventArgs e)
        {
            DecidirFormulario();
        }
        private void dataGridEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = btnRemove.IsEnabled = true;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Privilegios();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}