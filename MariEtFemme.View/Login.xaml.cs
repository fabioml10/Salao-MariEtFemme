using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MariEtFemme.BLL;
using MariEtFemme.DTO;

namespace MariEtFemme.View
{
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
        }

        #region Variables
        UsuarioBLL pessoaUsuarioBLL = new UsuarioBLL();
        FuncionarioBLL funcionarioBLL = new FuncionarioBLL();
        FilialBLL filialBLL = new FilialBLL();
        #endregion

        #region EventHandler
        public event EventHandler LoginSuccess;
        public event EventHandler ExitSuccess;
        #endregion

        #region Functions
        private void Privileges()
        {
            txtUsername.Text = string.Empty;
            txtUsername.Focus();
            txtPassword.Password = string.Empty;
            btnEnter.IsEnabled = false;
        }
        private void Authenticate()
        {
            try
            {
                //Verifica que resultado o banco devolveu na autenticação
                string result = pessoaUsuarioBLL.AuthenticateUser(txtUsername.Text, txtPassword.Password);
                switch (result)
                {
                    case "Autenticado":
                        //Coleta as informações do usuário logado
                        Session.LoggedUser = funcionarioBLL.ReadUser(txtUsername.Text);

                        //Monta e exibe a mensagem de boas vindas
                        StringBuilder message = new StringBuilder();
                        message.Append("Olá ").Append(Session.LoggedUser.Pessoa.NomePessoa).Append(", seja bem vindo ao sistema!");
                        MessageBox.Show(message.ToString(), "Bem Vindo", MessageBoxButton.OK, MessageBoxImage.Information);

                        //Ordena a troca de tela pela MainWindow
                        if (LoginSuccess != null)
                        {
                            LoginSuccess(this, new EventArgs());
                        }

                        break;
                    case "UsuarioIncorreto":
                        MessageBox.Show("Usuário inexistente.", "Usuário Incorreto", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtUsername.Focus();
                        txtUsername.SelectAll();
                        break;
                    case "SenhaIncorreto":
                        MessageBox.Show("Senha incorreta.", "Senha Incorreta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtPassword.Focus();
                        txtPassword.SelectAll();
                        break;
                    case "UsuarioBranco":
                        MessageBox.Show("Campo usuário não pode estar em branco.", "Usuário em Branco", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtUsername.Focus();
                        break;
                    case "SenhaBranco":
                        MessageBox.Show("Campo senha não pode estar em branco.", "Senha em Branco", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtPassword.Focus();
                        break;
                    case "UsuarioInativo":
                        MessageBox.Show("Não foi possível logar no sistema, usuário inativo.", "Usuário Inativo", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtUsername.Focus();
                        txtUsername.SelectAll();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ValidateForm()
        {
            if (!string.IsNullOrEmpty(txtUsername.Text) && !string.IsNullOrEmpty(txtPassword.Password))
            {
                if(!string.IsNullOrWhiteSpace(txtUsername.Text) && !string.IsNullOrWhiteSpace(txtPassword.Password))
                {
                    btnEnter.IsEnabled = true;
                }
                else
                {
                    btnEnter.IsEnabled = false;
                }                
            }
            else
            {
                btnEnter.IsEnabled = false;
            }
        }

        #endregion

        #region Events
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (ExitSuccess != null)
            {
                ExitSuccess(this, new EventArgs());
            }
            txtUsername.Focus();
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            Authenticate();
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Authenticate();
            }
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            ValidateForm();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Privileges();
        }

        #endregion
    }
}