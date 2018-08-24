using System;
using System.Windows;
using System.Windows.Controls;
using MariEtFemme.BLL;
using MariEtFemme.DTO;
using MariEtFemme.Tools;

//Implantar
//Sistema de avaliação do fornecedor.

namespace MariEtFemme.View
{
    public partial class ProviderTool : Window
    {
        public ProviderTool()
        {
            InitializeComponent();
        }

        #region Variables

        IndividualRegistration frmRegistration;

        private EnumApplyAction buttonApply;        

        FornecedorDTO fornecedorDTO;
        FornecedorCollectionDTO fornecedorCollectionDTO;
        FornecedorBLL fornecedorBLL = new FornecedorBLL();

        PessoaBLL pessoaBLL = new PessoaBLL();

        #endregion

        #region Métodos

        private void ListarFornecedores()
        {
            try
            {
                fornecedorCollectionDTO = new FornecedorCollectionDTO();
                fornecedorCollectionDTO = fornecedorBLL.ReadName(string.Empty);

                dataGridProvider.ItemsSource = null;
                dataGridProvider.ItemsSource = fornecedorCollectionDTO;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
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
        private void PreencherFormulario(FornecedorDTO fornecedor)
        {
            if (fornecedor.Pessoa.TipoPessoa)
            {
                frmRegistration.txtPersonName.Text = fornecedor.Pessoa.NomePessoa;
                frmRegistration.dpBirthDate.SelectedDate = fornecedor.Pessoa.PessoaFisica.Nascimento;
                frmRegistration.rbFemale.IsChecked = fornecedor.Pessoa.PessoaFisica.Genero;
                frmRegistration.rbMale.IsChecked = !fornecedor.Pessoa.PessoaFisica.Genero;
            }
            else
            {
                frmRegistration.txtCorporateName.Text = fornecedor.Pessoa.NomePessoa;
                frmRegistration.txtRazaoSocial.Text = fornecedor.Pessoa.PessoaJuridica.RazaoSocial;
                frmRegistration.txtCNPJ.Text = fornecedor.Pessoa.PessoaJuridica.CNPJ;
                frmRegistration.txtStreet.Text = fornecedor.Pessoa.Endereco.Rua;
                frmRegistration.txtNr.Text = fornecedor.Pessoa.Endereco.Numero;
            }

            frmRegistration.txtNeighborhood.Text = fornecedor.Pessoa.Endereco.Bairro;
            frmRegistration.txtCity.Text = fornecedor.Pessoa.Endereco.Cidade;
            frmRegistration.cbState.SelectedValue = fornecedor.Pessoa.Endereco.Estado.SiglaEstado;

            frmRegistration.txtPhone1.Text = fornecedor.Pessoa.Contato.Telefone1;
            frmRegistration.cbOperatorPhone1.SelectedItem = fornecedor.Pessoa.Contato.Operadora1.DescricaoOperadora;
            frmRegistration.checkWhats1.IsChecked = fornecedor.Pessoa.Contato.WhatsApp1;

            frmRegistration.txtPhone2.Text = fornecedor.Pessoa.Contato.Telefone2;
            frmRegistration.cbOperatorPhone2.SelectedItem = fornecedor.Pessoa.Contato.Operadora2.DescricaoOperadora;
            frmRegistration.checkWhats2.IsChecked = fornecedor.Pessoa.Contato.WhatsApp2;

            frmRegistration.txtPhone3.Text = fornecedor.Pessoa.Contato.Telefone3;
            frmRegistration.cbOperatorPhone3.SelectedItem = fornecedor.Pessoa.Contato.Operadora3.DescricaoOperadora;
            frmRegistration.checkWhats3.IsChecked = fornecedor.Pessoa.Contato.WhatsApp3;
            frmRegistration.txtEmail.Text = fornecedor.Pessoa.Contato.Email;

            frmRegistration.txtObs.Text = fornecedor.Pessoa.Comentarios;
        }
        private void PreencherFornecedor(bool newInstance)
        {
            if(newInstance)
            {
                fornecedorDTO.Pessoa = new PessoaDTO();
                fornecedorDTO.Pessoa.PessoaFisica = new PessoaFisicaDTO();
                fornecedorDTO.Pessoa.PessoaJuridica = new PessoaJuridicaDTO();
                fornecedorDTO.Pessoa.Endereco = new PessoaEnderecoDTO();
                fornecedorDTO.Pessoa.Endereco.Estado = new EstadoDTO();
                fornecedorDTO.Pessoa.Contato = new PessoaContatoDTO();
                fornecedorDTO.Pessoa.Contato.Operadora1 = new OperadoraDTO();
                fornecedorDTO.Pessoa.Contato.Operadora2 = new OperadoraDTO();
                fornecedorDTO.Pessoa.Contato.Operadora3 = new OperadoraDTO();
            }

            fornecedorDTO.Pessoa.TipoPessoa = rbIndividual.IsChecked.Value;
            fornecedorDTO.Pessoa.Comentarios = frmRegistration.txtObs.Text;            

            if (rbIndividual.IsChecked.Value)
            {
                fornecedorDTO.Pessoa.NomePessoa = frmRegistration.txtPersonName.Text;
                fornecedorDTO.Pessoa.PessoaFisica.Nascimento = frmRegistration.dpBirthDate.SelectedDate.Value;
                fornecedorDTO.Pessoa.PessoaFisica.Genero = Convert.ToBoolean(frmRegistration.rbFemale.IsChecked);
            }
            else
            {
                fornecedorDTO.Pessoa.NomePessoa = frmRegistration.txtCorporateName.Text;
                fornecedorDTO.Pessoa.PessoaJuridica.RazaoSocial = frmRegistration.txtRazaoSocial.Text;
                fornecedorDTO.Pessoa.PessoaJuridica.CNPJ = frmRegistration.txtCNPJ.Text;
                fornecedorDTO.Pessoa.Endereco.Rua = frmRegistration.txtStreet.Text;
                fornecedorDTO.Pessoa.Endereco.Numero = frmRegistration.txtNr.Text;
            }

            fornecedorDTO.Pessoa.Endereco.Bairro = frmRegistration.txtNeighborhood.Text;
            fornecedorDTO.Pessoa.Endereco.Cidade = frmRegistration.txtCity.Text;

            foreach (EstadoDTO item in frmRegistration.estadoCollectionDTO)
            {
                if (string.Compare(item.SiglaEstado, frmRegistration.cbState.SelectedItem.ToString()).Equals(0))
                {
                    fornecedorDTO.Pessoa.Endereco.Estado.IdEstado = item.IdEstado;
                    break;
                }
            }

            fornecedorDTO.Pessoa.Contato.Email = frmRegistration.txtEmail.Text;
            fornecedorDTO.Pessoa.Contato.Telefone1 = frmRegistration.txtPhone1.Text;
            fornecedorDTO.Pessoa.Contato.WhatsApp1 = Convert.ToBoolean(frmRegistration.checkWhats1.IsChecked);

            foreach (OperadoraDTO item in frmRegistration.operadoraCollectionDTO)
            {
                if (string.Compare(item.DescricaoOperadora, frmRegistration.cbOperatorPhone1.SelectedItem.ToString()).Equals(0))
                {
                    fornecedorDTO.Pessoa.Contato.Operadora1.IdOperadora = item.IdOperadora;
                    break;
                }
            }

            fornecedorDTO.Pessoa.Contato.Telefone2 = frmRegistration.txtPhone2.Text;
            fornecedorDTO.Pessoa.Contato.WhatsApp2 = Convert.ToBoolean(frmRegistration.checkWhats2.IsChecked);

            foreach (OperadoraDTO item in frmRegistration.operadoraCollectionDTO)
            {
                if (string.Compare(item.DescricaoOperadora, frmRegistration.cbOperatorPhone2.SelectedItem.ToString()).Equals(0))
                {
                    fornecedorDTO.Pessoa.Contato.Operadora2.IdOperadora = item.IdOperadora;
                    break;
                }
            }

            fornecedorDTO.Pessoa.Contato.Telefone3 = frmRegistration.txtPhone3.Text;
            fornecedorDTO.Pessoa.Contato.WhatsApp3 = Convert.ToBoolean(frmRegistration.checkWhats3.IsChecked);

            foreach (OperadoraDTO item in frmRegistration.operadoraCollectionDTO)
            {
                if (string.Compare(item.DescricaoOperadora, frmRegistration.cbOperatorPhone3.SelectedItem.ToString()).Equals(0))
                {
                    fornecedorDTO.Pessoa.Contato.Operadora3.IdOperadora = item.IdOperadora;
                    break;
                }
            }
        }
        private void Privilegios()
        {
            buttonApply = new EnumApplyAction();
            ListarFornecedores();
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
                    dataGridProvider.Visibility = Visibility.Visible;
                    dataGridSearchPerson.Visibility = Visibility.Hidden;

                    //Controls
                    gbPersonType.IsEnabled = true;
                    rbIndividual.IsChecked = false;
                    rbCorporate.IsChecked = true;
                    break;
            }
        }
        private bool ValidarFornecedor()
        {
            if (!string.IsNullOrEmpty(frmRegistration.txtPersonName.Text) || !string.IsNullOrEmpty(frmRegistration.txtCorporateName.Text))
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
            dataGridProvider.Visibility = Visibility.Hidden;

            fornecedorDTO = new FornecedorDTO();

            buttonApply = EnumApplyAction.Create;

            DecidirFormulario();
        }
        private void btnExistingPerson_Click(object sender, RoutedEventArgs e)
        {
            FornecedorCollectionDTO listGrid = new FornecedorCollectionDTO();
            listGrid = fornecedorBLL.ReadExcept(rbIndividual.IsChecked.Value);
            dataGridSearchPerson.Visibility = Visibility.Visible;
            dataGridSearchPerson.ItemsSource = null;
            dataGridSearchPerson.ItemsSource = listGrid;
            buttonApply = EnumApplyAction.CreateClient;
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
            dataGridProvider.Visibility = Visibility.Hidden;

            //Values
            buttonApply = EnumApplyAction.Update;

            //Box
            gbPersonType.IsEnabled = false;

            fornecedorDTO = new FornecedorDTO();
            fornecedorDTO = dataGridProvider.SelectedItem as FornecedorDTO;
            rbIndividual.IsChecked = fornecedorDTO.Pessoa.TipoPessoa;
            rbCorporate.IsChecked = !fornecedorDTO.Pessoa.TipoPessoa;
            DecidirFormulario();
            PreencherFormulario(fornecedorDTO);
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
                            if (ValidarFornecedor())
                            {
                                PreencherFornecedor(true);
                                string result = pessoaBLL.Create(fornecedorDTO.Pessoa);

                                int resultParse = 0;
                                switch (int.TryParse(result, out resultParse))
                                {
                                    case true:
                                        fornecedorDTO.Pessoa.IdPessoa = resultParse;
                                        if (string.Compare(fornecedorBLL.Create(fornecedorDTO), "Sucesso").Equals(0))
                                        {
                                            MessageBox.Show("Fornecedor cadastrada com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
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
                        }
                        else
                        {
                            fornecedorBLL.Create(fornecedorDTO);
                            MessageBox.Show("Fornecedor cadastrado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                            Privilegios();
                        }
                        break;

                    case EnumApplyAction.CreateClient:
                        fornecedorDTO = dataGridSearchPerson.SelectedItem as FornecedorDTO;
                        frmRegistration.txtPersonName.Text = fornecedorDTO.Pessoa.NomePessoa;
                        frmRegistration.txtPersonName.IsReadOnly = true;

                        dataGridSearchPerson.Visibility = Visibility.Hidden;
                        frmRegistration.gridCommon.Visibility =
                        frmRegistration.lblBirthDate.Visibility =
                        frmRegistration.dpBirthDate.Visibility =
                        frmRegistration.gbGender.Visibility = Visibility.Hidden;

                        frmRegistration.txtCorporateName.Text = fornecedorDTO.Pessoa.NomePessoa;
                        frmRegistration.txtCorporateName.IsReadOnly = true;
                        frmRegistration.txtRazaoSocial.Visibility =
                        frmRegistration.lblRazaoSocial.Visibility =
                        frmRegistration.txtCNPJ.Visibility =
                        frmRegistration.lblCNPJ.Visibility =
                        frmRegistration.txtStreet.Visibility =
                        frmRegistration.lblStreet.Visibility =
                        frmRegistration.txtNr.Visibility =
                        frmRegistration.lblNr.Visibility = Visibility.Hidden;
                        buttonApply = EnumApplyAction.Create;
                        break;

                    case EnumApplyAction.Update:
                        if (ValidarFornecedor())
                        {
                            PreencherFornecedor(false);
                            string result2 = pessoaBLL.Update(fornecedorDTO.Pessoa);
                            switch (result2)
                            {
                                case "Sucesso":
                                    if (string.Compare(fornecedorBLL.Update(fornecedorDTO), "Sucesso").Equals(0))
                                    {
                                        MessageBox.Show("Fornecedor modificado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                                        Privilegios();
                                    }
                                    else
                                    {
                                        MessageBox.Show(result2, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                    break;
                                default:
                                    MessageBox.Show(result2, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                                    break;
                            }
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
                fornecedorDTO = new FornecedorDTO();
                fornecedorDTO = dataGridProvider.SelectedItem as FornecedorDTO;

                if (MessageBox.Show("Realmente deseja excluir o fornecedor " + fornecedorDTO.Pessoa.NomePessoa + "?", "Remover Fornecedor", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    string result = fornecedorBLL.Delete(fornecedorDTO);
                    switch (result)
                    {
                        case "Sucesso":
                            MessageBox.Show("Fornecedor excluído com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                            Privilegios();
                            break;
                        case "Funcionario":
                            MessageBox.Show("Fornecedor excluído com sucesso.\nEssa pessoa ainda está cadastrada como funcionário.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                            Privilegios();
                            break;
                        case "Cliente":
                            MessageBox.Show("Fornecedor excluído com sucesso.\nEssa pessoa ainda está cadastrada como cliente.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                            Privilegios();
                            break;
                        case "Filial":
                            MessageBox.Show("Fornecedor excluído com sucesso.\nEssa pessoa ainda está cadastrada como filial.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    dataGridProvider.Visibility = Visibility.Hidden;
                    dataGridSearchPerson.Visibility = Visibility.Hidden;

                    buttonApply = EnumApplyAction.Create;
                    break;
                default:
                    Privilegios();
                    break;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void dataGridProvider_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = btnRemove.IsEnabled = true;
        }
        private void rbIndividual_Unchecked(object sender, RoutedEventArgs e)
        {
            DecidirFormulario();
        }
        private void rbCorporate_Unchecked(object sender, RoutedEventArgs e)
        {
            DecidirFormulario();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Privilegios();
        }

        #endregion

    }
}