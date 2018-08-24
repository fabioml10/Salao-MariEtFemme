using System;
using System.Windows;
using System.Windows.Controls;
using MariEtFemme.BLL;
using MariEtFemme.DTO;
using MariEtFemme.Tools;

namespace MariEtFemme.View
{
    public partial class Filial : Window
    {
        public Filial()
        {
            InitializeComponent();
        }

        #region Variáveis

        IndividualRegistration frmRegistration;

        private EnumApplyAction buttonApply;

        FilialDTO filialDTO;
        FilialCollectionDTO filialCollectionDTO;
        FilialBLL filialBLL = new FilialBLL();

        PessoaBLL pessoaBLL = new PessoaBLL();

        #endregion

        #region Métodos
        private void ListarFiliais()
        {
            try
            {
                filialCollectionDTO = new FilialCollectionDTO();
                filialCollectionDTO = filialBLL.ReadName(string.Empty);

                dataGridFilial.ItemsSource = null;
                dataGridFilial.ItemsSource = filialCollectionDTO;
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
        private void PreencherFormulario(FilialDTO filial)
        {
            if (filial.Pessoa.TipoPessoa)
            {
                frmRegistration.txtPersonName.Text = filial.Pessoa.NomePessoa;
                frmRegistration.dpBirthDate.SelectedDate = filial.Pessoa.PessoaFisica.Nascimento;
                frmRegistration.rbFemale.IsChecked = filial.Pessoa.PessoaFisica.Genero;
                frmRegistration.rbMale.IsChecked = !filial.Pessoa.PessoaFisica.Genero;
            }
            else
            {
                frmRegistration.txtCorporateName.Text = filial.Pessoa.NomePessoa;
                frmRegistration.txtRazaoSocial.Text = filial.Pessoa.PessoaJuridica.RazaoSocial;
                frmRegistration.txtCNPJ.Text = filial.Pessoa.PessoaJuridica.CNPJ;
                frmRegistration.txtStreet.Text = filial.Pessoa.Endereco.Rua;
                frmRegistration.txtNr.Text = filial.Pessoa.Endereco.Numero;
            }

            frmRegistration.txtNeighborhood.Text = filial.Pessoa.Endereco.Bairro;
            frmRegistration.txtCity.Text = filial.Pessoa.Endereco.Cidade;
            frmRegistration.cbState.SelectedValue = filial.Pessoa.Endereco.Estado.SiglaEstado;

            frmRegistration.txtPhone1.Text = filial.Pessoa.Contato.Telefone1;
            frmRegistration.cbOperatorPhone1.SelectedItem = filial.Pessoa.Contato.Operadora1.DescricaoOperadora;
            frmRegistration.checkWhats1.IsChecked = filial.Pessoa.Contato.WhatsApp1;

            frmRegistration.txtPhone2.Text = filial.Pessoa.Contato.Telefone2;
            frmRegistration.cbOperatorPhone2.SelectedItem = filial.Pessoa.Contato.Operadora2.DescricaoOperadora;
            frmRegistration.checkWhats2.IsChecked = filial.Pessoa.Contato.WhatsApp2;

            frmRegistration.txtPhone3.Text = filial.Pessoa.Contato.Telefone3;
            frmRegistration.cbOperatorPhone3.SelectedItem = filial.Pessoa.Contato.Operadora3.DescricaoOperadora;
            frmRegistration.checkWhats3.IsChecked = filial.Pessoa.Contato.WhatsApp3;
            frmRegistration.txtEmail.Text = filial.Pessoa.Contato.Email;

            frmRegistration.txtObs.Text = filial.Pessoa.Comentarios;
        }
        private void PreencherFilial(bool novaInstancia)
        {
            if(novaInstancia)
            {
                filialDTO.Pessoa.PessoaFisica = new PessoaFisicaDTO();
                filialDTO.Pessoa.PessoaJuridica = new PessoaJuridicaDTO();
                filialDTO.Pessoa.Endereco = new PessoaEnderecoDTO();
                filialDTO.Pessoa.Endereco.Estado = new EstadoDTO();
                filialDTO.Pessoa.Contato = new PessoaContatoDTO();
                filialDTO.Pessoa.Contato.Operadora1 = new OperadoraDTO();
                filialDTO.Pessoa.Contato.Operadora2 = new OperadoraDTO();
                filialDTO.Pessoa.Contato.Operadora3 = new OperadoraDTO();
            }

            filialDTO.Pessoa.TipoPessoa = rbIndividual.IsChecked.Value;
            filialDTO.Pessoa.Comentarios = frmRegistration.txtObs.Text;

            if (rbIndividual.IsChecked.Value)
            {
                filialDTO.Pessoa.NomePessoa = frmRegistration.txtPersonName.Text;
                filialDTO.Pessoa.PessoaFisica.Nascimento = frmRegistration.dpBirthDate.SelectedDate.Value;
                filialDTO.Pessoa.PessoaFisica.Genero = Convert.ToBoolean(frmRegistration.rbFemale.IsChecked);
            }
            else
            {
                filialDTO.Pessoa.NomePessoa = frmRegistration.txtCorporateName.Text;
                filialDTO.Pessoa.PessoaJuridica.RazaoSocial = frmRegistration.txtRazaoSocial.Text;
                filialDTO.Pessoa.PessoaJuridica.CNPJ = frmRegistration.txtCNPJ.Text;
                filialDTO.Pessoa.Endereco.Rua = frmRegistration.txtStreet.Text;
                filialDTO.Pessoa.Endereco.Numero = frmRegistration.txtNr.Text;
            }

            filialDTO.Pessoa.Endereco.Bairro = frmRegistration.txtNeighborhood.Text;
            filialDTO.Pessoa.Endereco.Cidade = frmRegistration.txtCity.Text;

            
            foreach (EstadoDTO item in frmRegistration.estadoCollectionDTO)
            {
                if (string.Compare(item.SiglaEstado, frmRegistration.cbState.SelectedItem.ToString()).Equals(0))
                {
                    filialDTO.Pessoa.Endereco.Estado.IdEstado = item.IdEstado;
                    break;
                }
            }

            filialDTO.Pessoa.Contato.Email = frmRegistration.txtEmail.Text;
            filialDTO.Pessoa.Contato.Telefone1 = frmRegistration.txtPhone1.Text;
            filialDTO.Pessoa.Contato.WhatsApp1 = Convert.ToBoolean(frmRegistration.checkWhats1.IsChecked);
            
            foreach (OperadoraDTO item in frmRegistration.operadoraCollectionDTO)
            {
                if (string.Compare(item.DescricaoOperadora, frmRegistration.cbOperatorPhone1.SelectedItem.ToString()).Equals(0))
                {
                    filialDTO.Pessoa.Contato.Operadora1.IdOperadora = item.IdOperadora;
                    break;
                }
            }

            filialDTO.Pessoa.Contato.Telefone2 = frmRegistration.txtPhone2.Text;
            filialDTO.Pessoa.Contato.WhatsApp2 = Convert.ToBoolean(frmRegistration.checkWhats2.IsChecked);

            foreach (OperadoraDTO item in frmRegistration.operadoraCollectionDTO)
            {
                if (string.Compare(item.DescricaoOperadora, frmRegistration.cbOperatorPhone2.SelectedItem.ToString()).Equals(0))
                {
                    filialDTO.Pessoa.Contato.Operadora2.IdOperadora = item.IdOperadora;
                    break;
                }
            }

            filialDTO.Pessoa.Contato.Telefone3 = frmRegistration.txtPhone3.Text;
            filialDTO.Pessoa.Contato.WhatsApp3 = Convert.ToBoolean(frmRegistration.checkWhats3.IsChecked);

            foreach (OperadoraDTO item in frmRegistration.operadoraCollectionDTO)
            {
                if (string.Compare(item.DescricaoOperadora, frmRegistration.cbOperatorPhone3.SelectedItem.ToString()).Equals(0))
                {
                    filialDTO.Pessoa.Contato.Operadora3.IdOperadora = item.IdOperadora;
                    break;
                }
            }
        }
        private void Privilegios()
        {
            //Fill
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
            dataGridFilial.Visibility = Visibility.Visible;

            //Controls
            gbPersonType.IsEnabled = true;
            rbCorporate.IsChecked = true;
            rbIndividual.IsChecked = false;
            buttonApply = new EnumApplyAction();

            switch (Session.LoggedUser.Usuario.Privilegio.IdPrivilegio)
            {
                default:
                    break;
            }
        }
        private bool ValidarFormulário()
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
            dataGridFilial.Visibility = Visibility.Hidden;

            filialDTO = new FilialDTO();

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
            dataGridFilial.Visibility = Visibility.Hidden;

            //Values
            buttonApply = EnumApplyAction.Update;

            //Box
            gbPersonType.IsEnabled = false;

            filialDTO = new FilialDTO();
            filialDTO = dataGridFilial.SelectedItem as FilialDTO;
            rbIndividual.IsChecked = filialDTO.Pessoa.TipoPessoa;
            rbCorporate.IsChecked = !filialDTO.Pessoa.TipoPessoa;
            DecidirFormulario();
            PreencherFormulario(filialDTO);
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
                            if (ValidarFormulário())
                            {
                                PreencherFilial(true);
                                string result = pessoaBLL.Create(filialDTO.Pessoa);

                                int resultParse = 0;
                                switch (int.TryParse(result, out resultParse))
                                {
                                    case true:
                                        filialDTO.Pessoa.IdPessoa = resultParse;
                                        if (string.Compare(filialBLL.Create(filialDTO), "Sucesso").Equals(0))
                                        {
                                            MessageBox.Show("Filial cadastrada com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
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
                            filialBLL.Create(filialDTO);
                            MessageBox.Show("Filial cadastrada com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                            Privilegios();
                        }
                        break;

                    case EnumApplyAction.CreateClient:
                        filialDTO = dataGridSearchPerson.SelectedItem as FilialDTO;
                        frmRegistration.txtPersonName.Text = filialDTO.Pessoa.NomePessoa;
                        frmRegistration.txtPersonName.IsReadOnly = true;

                        dataGridSearchPerson.Visibility = Visibility.Hidden;
                        frmRegistration.gridCommon.Visibility =
                        frmRegistration.lblBirthDate.Visibility =
                        frmRegistration.dpBirthDate.Visibility =
                        frmRegistration.gbGender.Visibility = Visibility.Hidden;

                        frmRegistration.txtCorporateName.Text = filialDTO.Pessoa.NomePessoa;
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
                        if (ValidarFormulário())
                        {
                            PreencherFilial(false);
                            string result2 = pessoaBLL.Update(filialDTO.Pessoa);
                            switch (result2)
                            {
                                case "Sucesso":
                                    if (string.Compare(filialBLL.Update(filialDTO), "Sucesso").Equals(0))
                                    {
                                        MessageBox.Show("Funcionário modificado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
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
                filialDTO = new FilialDTO();
                filialDTO = dataGridFilial.SelectedItem as FilialDTO;

                if (MessageBox.Show("Realmente deseja excluir o cliente " + filialDTO.Pessoa.NomePessoa + "?", "Remover Cliente", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    string result = filialBLL.Delete(filialDTO);
                    switch (result)
                    {
                        case "Sucesso":
                            MessageBox.Show("Filial excluído com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                            Privilegios();
                            break;
                        case "Funcionario":
                            MessageBox.Show("Filial excluído com sucesso.\nEssa pessoa ainda está cadastrada como funcionário.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                            Privilegios();
                            break;
                        case "Cliente":
                            MessageBox.Show("Filial excluído com sucesso.\nEssa pessoa ainda está cadastrada como cliente.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                            Privilegios();
                            break;
                        case "Fornecedor":
                            MessageBox.Show("Filial excluído com sucesso.\nEssa pessoa ainda está cadastrada como Fornecedor.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    dataGridFilial.Visibility = Visibility.Hidden;
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
            FilialCollectionDTO listGrid = new FilialCollectionDTO();
            listGrid = filialBLL.ReadExcept(rbIndividual.IsChecked.Value);
            dataGridSearchPerson.Visibility = Visibility.Visible;
            dataGridSearchPerson.ItemsSource = null;
            dataGridSearchPerson.ItemsSource = listGrid;
            buttonApply = EnumApplyAction.CreateClient;
        }
        private void rbIndividual_Unchecked(object sender, RoutedEventArgs e)
        {
            DecidirFormulario();
        }
        private void rbCorporate_Unchecked(object sender, RoutedEventArgs e)
        {
            DecidirFormulario();
        }
        private void dataGridFilial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = btnRemove.IsEnabled = true;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Privilegios();
        }

        #endregion

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}