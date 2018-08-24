using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using MariEtFemme.BLL;
using MariEtFemme.DTO;
using MariEtFemme.Tools;

namespace MariEtFemme.View
{
    public partial class IndividualRegistration : UserControl
    {
        public IndividualRegistration()
        {
            InitializeComponent();

            checkWhats1.Checked += VerificarImagem;
            checkWhats1.Unchecked += VerificarImagem;

            checkWhats2.Checked += VerificarImagem;
            checkWhats2.Unchecked += VerificarImagem;

            checkWhats3.Checked += VerificarImagem;
            checkWhats3.Unchecked += VerificarImagem;
        }

        #region Variables

        public EstadoCollectionDTO estadoCollectionDTO;
        EstadoBLL estadoBLL = new EstadoBLL();

        public OperadoraCollectionDTO operadoraCollectionDTO;
        OperadoraBLL operadoraBLL = new OperadoraBLL();

        #endregion

        #region Form
        private void FillOperators()
        {
            operadoraCollectionDTO = operadoraBLL.ReadName(string.Empty);

            cbOperatorPhone1.Items.Clear();
            cbOperatorPhone2.Items.Clear();
            cbOperatorPhone3.Items.Clear();
            foreach (OperadoraDTO item in operadoraCollectionDTO)
            {
                cbOperatorPhone1.Items.Add(item.DescricaoOperadora);
                cbOperatorPhone2.Items.Add(item.DescricaoOperadora);
                cbOperatorPhone3.Items.Add(item.DescricaoOperadora);
            }
        }
        private void FillStates()
        {
            estadoCollectionDTO = estadoBLL.ReadName(string.Empty);

            cbState.Items.Clear();
            foreach (EstadoDTO state in estadoCollectionDTO)
            {
                cbState.Items.Add(state.SiglaEstado);
            }
        }

        private void VerificarImagem(object sender, RoutedEventArgs e)
        {
            if (checkWhats1.IsChecked.Value)
            {
                imgWhatsApp01.Source = new BitmapImage(new Uri("img/cadastro/whatsapp01L.png", UriKind.Relative));
            }
            else
            {
                imgWhatsApp01.Source = new BitmapImage(new Uri("img/cadastro/whatsapp01D.png", UriKind.Relative));
            }

            if (checkWhats2.IsChecked.Value)
            {
                imgWhatsApp02.Source = new BitmapImage(new Uri("img/cadastro/whatsapp01L.png", UriKind.Relative));
            }
            else
            {
                imgWhatsApp02.Source = new BitmapImage(new Uri("img/cadastro/whatsapp01D.png", UriKind.Relative));
            }

            if (checkWhats3.IsChecked.Value)
            {
                imgWhatsApp03.Source = new BitmapImage(new Uri("img/cadastro/whatsapp01L.png", UriKind.Relative));
            }
            else
            {
                imgWhatsApp03.Source = new BitmapImage(new Uri("img/cadastro/whatsapp01D.png", UriKind.Relative));
            }
        }

        #endregion

        #region Events
        private void _DatePicker_CalendarOpened(object sender, RoutedEventArgs e)
        {
            Popup popup = (Popup)dpBirthDate.Template.FindName("PART_Popup", dpBirthDate);
            Calendar cal = (Calendar)popup.Child;
            cal.DisplayMode = CalendarMode.Decade;
        }

        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtPersonName.Focus();
            cbState.SelectedItem = "--";
            cbOperatorPhone1.SelectedItem = "--";
            cbOperatorPhone2.SelectedItem = "--";
            cbOperatorPhone3.SelectedItem = "--";
            FillOperators();
            FillStates();
        }

        private void txtPhone1_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtPhone1.Text.Length > 13)
            {
                txtPhone1.Mask = MaskedTextBox.TextBoxMask.Phone11Digit;
            }
        }

        private void txtPhone2_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtPhone2.Text.Length > 13)
            {
                txtPhone2.Mask = MaskedTextBox.TextBoxMask.Phone11Digit;
            }
        }

        private void txtPhone3_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtPhone3.Text.Length > 13)
            {
                txtPhone3.Mask = MaskedTextBox.TextBoxMask.Phone11Digit;
            }
        }
    }
}
