using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using MariEtFemme.Tools;
using MariEtFemme.DTO;
using MariEtFemme.BLL;

//Implantar
//Pesquisa dinâmica

namespace MariEtFemme.View
{
    public partial class UserTool : Window
    {
        public UserTool()
        {
            InitializeComponent();
        }

        #region Variables

        private EnumApplyAction buttonApply;        

        FuncionarioDTO funcionarioDTO;
        FuncionarioCollectionDTO funcionarioCollectionDTO;
        FuncionarioBLL funcionarioBLL = new FuncionarioBLL();

        UsuarioBLL pessoaUsuarioBLL = new UsuarioBLL();

        PrivilegioCollectionDTO privilegioCollectionDTO;
        PrivilegioBLL privilegioBLL = new PrivilegioBLL();

        #endregion

        #region Functions

        /// <summary>
        /// Preenche lista de pessoas com a possibilidade de se associar a um usuario.
        /// </summary>
        private void ListEmployeeNoUser()
        {
            try
            {
                FuncionarioCollectionDTO listNoUser = new FuncionarioCollectionDTO();
                listNoUser = funcionarioBLL.ReadEmployeeUser(false);
                dataGridPerson.ItemsSource = null;
                dataGridPerson.ItemsSource = listNoUser;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Preenche lista de usuários com todos os funcionários que tem usuário.
        /// </summary>
        private void ListEmployeeUser()
        {
            try
            {
                funcionarioCollectionDTO = new FuncionarioCollectionDTO();
                funcionarioCollectionDTO = funcionarioBLL.ReadEmployeeUser(true);
                dataGridUser.ItemsSource = null;
                dataGridUser.ItemsSource = funcionarioCollectionDTO;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Preenche a combobox com os tipos de permissão de acesso.
        /// </summary>
        private void ListPrivileges()
        {
            try
            {
                privilegioCollectionDTO = new PrivilegioCollectionDTO();
                privilegioCollectionDTO = privilegioBLL.ReadName(string.Empty);
                cbPermissionType.Items.Clear();
                foreach (PrivilegioDTO item in privilegioCollectionDTO)
                {
                    cbPermissionType.Items.Add(item.DescricaoPrivilegio);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Privileges()
        {
            ListPrivileges();
            buttonApply = new EnumApplyAction();
            dataGridUser.Visibility = Visibility.Visible;
            dataGridPerson.Visibility = Visibility.Hidden;
            btnApply.Visibility = btnCancel.Visibility = Visibility.Hidden;
            lblPersonName.Content = txtUser.Text = txtPassword.Password = txtConfirmPassword.Password = string.Empty;
            dataGridUser.SelectedIndex = -1;
            btnEdit.IsEnabled = btnRemove.IsEnabled = false;
            Width = 450;

            //Permissões
            switch (Session.LoggedUser.Usuario.Privilegio.IdPrivilegio)
            {
                case 1: //Administrador
                    ListEmployeeUser();
                    btnNew.Visibility = btnEdit.Visibility = btnRemove.Visibility = Visibility.Visible;
                    break;

                default: //Serviços //Relatórios //Financeiro
                    funcionarioCollectionDTO = new FuncionarioCollectionDTO();
                    funcionarioCollectionDTO.Add(Session.LoggedUser);
                    dataGridUser.ItemsSource = null;
                    dataGridUser.ItemsSource = funcionarioCollectionDTO;

                    cbPermissionType.IsEnabled = txtUser.IsEnabled = gbStatus.IsEnabled = false;
                    btnEdit.Visibility = Visibility.Visible;
                    btnNew.Visibility = btnRemove.Visibility = Visibility.Hidden;

                    Thickness margin = btnEdit.Margin;
                    margin.Top = 0;
                    btnEdit.Margin = margin;

                    break;
            }
        }

        private void PreencherObjeto(FuncionarioDTO user)
        {
            user.Usuario = new UsuarioDTO();
            user.Usuario.Usuario = txtUser.Text;
            user.Usuario.Senha = txtPassword.Password;
            user.Usuario.Privilegio = new PrivilegioDTO();
            user.Usuario.Privilegio = privilegioBLL.ReadName(cbPermissionType.SelectedValue.ToString())[0];
            user.Usuario.Situacao = rbActive.IsChecked.Value;
        }

        private void PreencherFormulario(FuncionarioDTO user)
        {
            lblPersonName.Content = user.Pessoa.NomePessoa;
            txtUser.Text = user.Usuario.Usuario;
            txtPassword.Password = txtConfirmPassword.Password = user.Usuario.Senha;
            cbPermissionType.SelectedValue = user.Usuario.Privilegio.DescricaoPrivilegio;
            rbActive.IsChecked = user.Usuario.Situacao;
            rbInactive.IsChecked = !user.Usuario.Situacao;
        }

        private bool ValidarUsuario()
        {
            if (!string.IsNullOrEmpty(lblPersonName.Content.ToString()))
            {
                if (!string.IsNullOrEmpty(txtUser.Text))
                {
                    if (!string.IsNullOrEmpty(txtPassword.Password))
                    {
                        if (!string.IsNullOrEmpty(txtConfirmPassword.Password))
                        {
                            if (!cbPermissionType.SelectedIndex.Equals(-1))
                            {
                                if (string.Compare(txtPassword.Password, txtConfirmPassword.Password) == 0)
                                {
                                    return true;
                                }
                                else
                                {
                                    MessageBox.Show("O campo senha e confirmação da senha não são iguais.", "Senha e Confimação da Senha", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                    dataGridUser.Focus();
                                    return false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Favor, selecionar um tipo de acesso.", "Permissão de Acesso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                dataGridUser.Focus();
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("O campo confirmação da senha não pode estar em branco.", "Senha e Confimação da Senha", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            dataGridUser.Focus();
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("O campo senha não pode estar em branco.", "Senha", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        dataGridUser.Focus();
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("O campo usuário não pode estar em branco.", "Usuário", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    dataGridUser.Focus();
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Favor, selecionar um funcionário.", "Funcionário", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                dataGridUser.Focus();
                return false;
            }
        }

        private bool ValidarRemocaoUsuario(FuncionarioDTO user)
        {
            if (String.Compare(user.Usuario.Usuario, Session.LoggedUser.Usuario.Usuario) == 0)
            {
                MessageBox.Show("Não é possível excluir um usuário logado.", "Usuário Logado", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region Events
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            dataGridUser.Visibility = Visibility.Hidden;
            btnNew.Visibility = btnEdit.Visibility = btnRemove.Visibility = Visibility.Hidden;
            btnApply.Visibility = btnCancel.Visibility = Visibility.Visible;
            buttonApply = EnumApplyAction.Create;
            txtUser.Focus();
            funcionarioDTO = new FuncionarioDTO();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            funcionarioDTO = new FuncionarioDTO();
            funcionarioDTO = dataGridUser.SelectedItem as FuncionarioDTO;
            PreencherFormulario(funcionarioDTO);

            dataGridUser.Visibility = Visibility.Hidden;
            btnNew.Visibility = btnEdit.Visibility = btnRemove.Visibility = Visibility.Hidden;
            btnApply.Visibility = btnCancel.Visibility = Visibility.Visible;
            buttonApply = EnumApplyAction.Update;
            txtUser.Focus();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                funcionarioDTO = new FuncionarioDTO();
                funcionarioDTO = dataGridUser.SelectedItem as FuncionarioDTO;

                if (ValidarRemocaoUsuario(funcionarioDTO))
                {
                    StringBuilder message = new StringBuilder();
                    message.Append("Realmente deseja excluir o usuário ").Append(funcionarioDTO.Usuario.Usuario).Append("?");
                    if (MessageBox.Show(message.ToString(), "Excluir Usuário", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        if(string.Compare(pessoaUsuarioBLL.Delete(funcionarioDTO), "Sucesso").Equals(0))
                        {
                            MessageBox.Show("Usuário excluído com sucesso", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                            Privileges();
                        }
                    }
                    dataGridUser.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (buttonApply)
                {
                    case EnumApplyAction.Create:
                        if (ValidarUsuario())
                        {                          
                            PreencherObjeto(funcionarioDTO);
                            string result = pessoaUsuarioBLL.Create(funcionarioDTO);
                            switch (result)
                            {
                                case "Sucesso":
                                    MessageBox.Show("Usuário cadastrado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                                    Privileges();
                                    break;
                                case "Existente":
                                    MessageBox.Show("Usuário já existe.", "Usuario", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                    txtUser.Focus();
                                    txtUser.SelectAll();
                                    break;
                                case "UsuarioBranco":
                                    MessageBox.Show("Compo usuário não pode estar em branco", "Usuario", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                    txtUser.Focus();
                                    break;
                                case "SenhaBranco":
                                    MessageBox.Show("Compo senha não pode estar em branco", "Senha", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                    txtPassword.Focus();
                                    break;
                                case "SituacaoBranco":
                                    MessageBox.Show("Favor, selecionar a situação do usuário", "Situação do Usuário", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                    gbStatus.Focus();
                                    break;
                                case "AcessoBranco":
                                    MessageBox.Show("Favor, selecionar um tipo de privilégio.", "Selecionar", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                    cbPermissionType.Focus();
                                    break;
                            }
                        }
                        break;

                    case EnumApplyAction.CreateClient:
                        funcionarioDTO = new FuncionarioDTO();
                        funcionarioDTO = dataGridPerson.SelectedItem as FuncionarioDTO;
                        lblPersonName.Content = funcionarioDTO.Pessoa.NomePessoa;
                        dataGridPerson.Visibility = Visibility.Hidden;
                        buttonApply = EnumApplyAction.Create;
                        break;

                    case EnumApplyAction.Update:
                        if (ValidarUsuario())
                        {
                            PreencherObjeto(funcionarioDTO);
                            
                            string result = pessoaUsuarioBLL.Update(funcionarioDTO);
                            switch (result)
                            {
                                case "Sucesso":
                                    MessageBox.Show("Usuário modificado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                                    Privileges();
                                    break;
                                case "Existente":
                                    MessageBox.Show("Usuário já existe.", "Usuario", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                    txtUser.Focus();
                                    txtUser.SelectAll();
                                    break;
                                case "UsuarioBranco":
                                    MessageBox.Show("Compo usuário não pode estar em branco", "Usuario", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                    txtUser.Focus();
                                    break;
                                case "SenhaBranco":
                                    MessageBox.Show("Compo senha não pode estar em branco", "Senha", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                    txtPassword.Focus();
                                    break;
                                case "SituacaoBranco":
                                    MessageBox.Show("Favor, selecionar a situação do usuário", "Situação do Usuário", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                    gbStatus.Focus();
                                    break;
                                case "AcessoBranco":
                                    MessageBox.Show("Favor, selecionar um tipo de privilégio.", "Selecionar", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                    cbPermissionType.Focus();
                                    break;
                            }
                        }
                        break;
                    case EnumApplyAction.UpdateClient:
                        funcionarioDTO = new FuncionarioDTO();
                        funcionarioDTO = dataGridPerson.SelectedItem as FuncionarioDTO;
                        lblPersonName.Content = funcionarioDTO.Pessoa.NomePessoa;
                        dataGridPerson.Visibility = Visibility.Hidden;
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
                case EnumApplyAction.Create:
                    Privileges();
                    break;
                case EnumApplyAction.CreateClient:
                    dataGridPerson.Visibility = Visibility.Hidden;
                    buttonApply = EnumApplyAction.Create;
                    btnApply.IsEnabled = true;
                    break;
                case EnumApplyAction.Update:
                    Privileges();
                    break;
                case EnumApplyAction.UpdateClient:
                    dataGridPerson.Visibility = Visibility.Hidden;
                    buttonApply = EnumApplyAction.Update;
                    btnApply.IsEnabled = true;
                    break;
                default:
                    Privileges();
                    break;
            }
            
        }

        private void btnSearchPerson_Click(object sender, RoutedEventArgs e)
        {
            ListEmployeeNoUser();
            dataGridPerson.Visibility = Visibility.Visible;
            btnApply.IsEnabled = false;

            switch (buttonApply)
            {
                case EnumApplyAction.Create:
                    buttonApply = EnumApplyAction.CreateClient;
                    break;
                case EnumApplyAction.Update:
                    buttonApply = EnumApplyAction.UpdateClient;
                    break;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dataGridPerson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnApply.IsEnabled = true;
        }

        private void dataGridUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = btnRemove.IsEnabled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Privileges();
        }

        #endregion
    }
}