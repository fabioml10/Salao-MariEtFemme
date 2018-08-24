using System;
using System.Windows;
using System.Windows.Controls;
using MariEtFemme.Tools;
using MariEtFemme.BLL;
using MariEtFemme.DTO;

namespace MariEtFemme.View
{
    public partial class Stuff : Window
    {
        public Stuff()
        {
            InitializeComponent();
        }

        #region Variáveis
        ProdutoDTO produtoDTO;
        ProdutoCollectionDTO produtoCollectionDTO;
        ProdutoBLL produtoBLL = new ProdutoBLL();

        UnidadeCollectionDTO unidadeCollectionDTO;
        UnidadeBLL unidadeBLL = new UnidadeBLL();

        private EnumApplyAction buttonApply;
        #endregion

        #region Métodos
        private void FillStuffList()
        {
            produtoCollectionDTO = produtoBLL.ReadName(string.Empty);
            dataGridStuff.ItemsSource = null;
            dataGridStuff.ItemsSource = produtoCollectionDTO;
        }
        private void FillUn()
        {
            unidadeCollectionDTO = unidadeBLL.ReadName(string.Empty);
            cbUn.Items.Clear();
            foreach (UnidadeDTO item in unidadeCollectionDTO)
            {
                cbUn.Items.Add(item.SiglaUnidade);
            }
        }
        private void InitialConditionPage()
        {
            switch (Session.LoggedUser.Usuario.Privilegio.IdPrivilegio)
            {
                default:
                    FillStuffList();
                    FillUn();
                    buttonApply = new EnumApplyAction();

                    //Lists
                    dataGridStuff.Visibility = Visibility.Visible;

                    //Controls
                    txtFeedStock.Text = string.Empty;

                    //Butons
                    btnEdit.IsEnabled = btnRemove.IsEnabled = false;
                    btnNew.Visibility = btnEdit.Visibility = btnRemove.Visibility = Visibility.Visible;
                    btnApply.Visibility = btnCancel.Visibility = Visibility.Hidden;
                    break;
            }
        }
        private void PreencherProduto(ProdutoDTO produto)
        {
            produto.DescricaoProduto = txtFeedStock.Text;
            produto.Unidade = new UnidadeDTO();
            produto.Unidade.IdUnidade = unidadeBLL.ReadName(cbUn.SelectedItem.ToString())[0].IdUnidade;
        }
        private void PreencherFormulario(ProdutoDTO produto)
        {
            txtFeedStock.Text = produto.DescricaoProduto;
            cbUn.SelectedValue = produto.Unidade.SiglaUnidade;
        }
        private bool ValidarProduto(ProdutoDTO produto)
        {
            if (!string.IsNullOrEmpty(txtFeedStock.Text))
            {
                bool result = true;

                foreach (ProdutoDTO item in produtoCollectionDTO)
                {
                    if (String.Compare(txtFeedStock.Text, item.DescricaoProduto) == 0 && produto.IdProduto != item.IdProduto)
                    {
                        result = false;
                    }
                }

                if (result)
                {
                    if (cbUn.SelectedIndex != -1)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Favor, selecionar uma unidade para o produto.", "Unidade", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Produto já existente.", "Produto", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("O campo Produto não pode estar em branco.", "Produto", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
        }
        #endregion

        #region Events

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            buttonApply = EnumApplyAction.Create;

            //Lists
            dataGridStuff.Visibility = Visibility.Hidden;

            //Buttons
            btnNew.Visibility = btnEdit.Visibility = btnRemove.Visibility = Visibility.Hidden;
            btnApply.Visibility = btnCancel.Visibility = Visibility.Visible;

            produtoDTO = new ProdutoDTO();
        }
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            produtoDTO = new ProdutoDTO();
            produtoDTO = dataGridStuff.SelectedItem as ProdutoDTO;

            if (MessageBox.Show("Realmente deseja excluir o produto " + produtoDTO.DescricaoProduto + "?", "Excluir Produto", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                produtoBLL.Delete(produtoDTO);
                InitialConditionPage();
            }
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            produtoDTO = new ProdutoDTO();
            produtoDTO = dataGridStuff.SelectedItem as ProdutoDTO;
            PreencherFormulario(produtoDTO);

            buttonApply = EnumApplyAction.Update;

            //Lists
            dataGridStuff.Visibility = Visibility.Hidden;

            //Buttons
            btnNew.Visibility = btnEdit.Visibility = btnRemove.Visibility = Visibility.Hidden;
            btnApply.Visibility = btnCancel.Visibility = Visibility.Visible;
        }
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            switch (buttonApply)
            {
                case EnumApplyAction.Create:

                    if (ValidarProduto(produtoDTO))
                    {
                        PreencherProduto(produtoDTO);
                        produtoBLL.Create(produtoDTO);
                        InitialConditionPage();
                    }

                    break;
                case EnumApplyAction.Update:
                    if (ValidarProduto(produtoDTO))
                    {
                        PreencherProduto(produtoDTO);
                        produtoBLL.Update(produtoDTO);
                        InitialConditionPage();
                    }
                    break;
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            InitialConditionPage();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitialConditionPage();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void cbUn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbUn.SelectedIndex != -1)
            {
                //Poderia procurar no banco, ver se desempenho faz diferença
                foreach (UnidadeDTO item in unidadeCollectionDTO)
                {
                    if (string.Compare(cbUn.SelectedItem.ToString(), item.SiglaUnidade) == 0)
                    {
                        lblUnDesc.Content = item.DescricaoUnidade;
                    }
                }
            }
        }
        private void dataGridStuff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = btnRemove.IsEnabled = true;
        }

        #endregion
    }
}