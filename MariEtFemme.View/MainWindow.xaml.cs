using System;
using System.Windows;

namespace MariEtFemme.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Width = 340;
            this.Height = 365;
        }

        private void OnLoginSuccess(object sender, EventArgs e)
        {
            loginPage.Visibility = Visibility.Collapsed;
            this.WindowState = WindowState.Maximized;
            this.ResizeMode = ResizeMode.CanResize;
            this.WindowStyle = WindowStyle.SingleBorderWindow;

            Master masterPage = new Master();            
            master.Children.Add(masterPage);
            masterPage.Visibility = Visibility.Visible;
            masterPage.Focus();
            masterPage.LogOffSuccess += OnLogOffSuccess;
            this.MinHeight = 600;
            this.MinWidth = 800;
        }

        private void OnLogOffSuccess(object sender, EventArgs e)
        {
            if (MessageBox.Show("Realmente deseja fazer LogOff?", "LogOff", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                MainWindow newMainWindow = new MainWindow();
                Close();
                newMainWindow.Show();
            }
        }
        private void OnExitSuccess(object sender, EventArgs e)
        {
            if (MessageBox.Show("Realmente deseja sair do sistema?", "Sair", MessageBoxButton.YesNo, MessageBoxImage.Question).Equals(MessageBoxResult.Yes))
            {
                Application.Current.Shutdown();
            }
        }
    }
}