using System;
using System.Windows;
using System.Windows.Controls;
using MariEtFemme.Tools;
using MariEtFemme.BLL;
using MariEtFemme.DTO;

//Implantar
//Formatar o valor unitário para moeda

namespace MariEtFemme.View
{
    public partial class InvoiceTool : Window
    {
        public InvoiceTool()
        {
            InitializeComponent();
        }

        #region Variáveis

        NotaDTO notaDTO;
        NotaCollectionDTO notaCollectionDTO;
        NotaBLL notaBLL = new NotaBLL();

        ProdutoCollectionDTO produtoCollectionDTO;
        ProdutoBLL produtoBLL = new ProdutoBLL();

        FornecedorCollectionDTO fornecedorCollectionDTO;
        FornecedorBLL fornecedorBLL = new FornecedorBLL();

        FilialCollectionDTO filialCollectionDTO;
        FilialBLL filialBLL = new FilialBLL();

        NotaProdutoDTO notaProdutoDTO;
        NotaProdutoCollectionDTO newCollection;
        NotaProdutoCollectionDTO oldCollection;
        NotaProdutoBLL notaProdutoBLL = new NotaProdutoBLL();

        EstoqueDTO estoqueDTO;
        EstoqueBLL estoqueBLL = new EstoqueBLL();

        private EnumApplyAction buttonApply;

        #endregion

        #region Métodos
        private void InitialConditionPage()
        {
            switch (Session.LoggedUser.Usuario.Privilegio.IdPrivilegio)
            {
                default:
                    ListInvoice();
                    ListFilial();
                    ListProviders();
                    ListStuff();
                    buttonApply = new EnumApplyAction();
                    newCollection = null;
                    oldCollection = null;

                    //Lists
                    dataGridInvoice.Visibility = Visibility.Visible;
                    dataGridAddStuff.ItemsSource = null;

                    //Buttons
                    btnEdit.IsEnabled = btnRemove.IsEnabled = btnAddStuff.IsEnabled = btnRemoveStuff.IsEnabled = btnEditStuff.IsEnabled = false;
                    btnNew.Visibility = btnEdit.Visibility = btnRemove.Visibility = Visibility.Visible;
                    btnApply.Visibility = btnCancel.Visibility = Visibility.Hidden;

                    //Controls
                    txtInvoice.Text = txtStuffQt.Text = string.Empty;
                    dpDate.SelectedDate = null;
                    lblUn.Content = string.Empty;
                    break;
            }
        }
        private void ListInvoice()
        {
            try
            {
                notaCollectionDTO = notaBLL.ReadAll();
                dataGridInvoice.ItemsSource = null;
                dataGridInvoice.ItemsSource = notaCollectionDTO;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ListStuff()
        {
            try
            {
                produtoCollectionDTO = produtoBLL.ReadName(string.Empty);
                cbStuff.Items.Clear();
                foreach (ProdutoDTO item in produtoCollectionDTO)
                {
                    cbStuff.Items.Add(item.DescricaoProduto);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ListProviders()
        {
            try
            {
                fornecedorCollectionDTO = fornecedorBLL.ReadName(string.Empty);
                cbProvider.Items.Clear();
                foreach (FornecedorDTO item in fornecedorCollectionDTO)
                {
                    cbProvider.Items.Add(item.Pessoa.NomePessoa);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
}
        private void ListFilial()
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
        public bool InvoiceValidation()
        {
            if (!string.IsNullOrEmpty(txtInvoice.Text))
            {
                int temp;
                if (int.TryParse(txtInvoice.Text, out temp) || temp > 0)
                {
                    if (dpDate.SelectedDate != null)
                    {
                        if (cbProvider.SelectedIndex != -1)
                        {
                            if (newCollection.Count > 0)
                            {
                                return true;
                            }
                            else
                            {
                                MessageBox.Show("Favor, adicione produtos à nota", "Produtos da Nota", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Favor, indicar o fornecedor da nota", "Fornecedor da Nota", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Favor, selecionar a data da nota.", "Data da Nota", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Número da nota inválido.", "Número da Nota", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("O campo Número da Nota não pode estar em branco.", "Número da Nota", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
        }

        #endregion

        #region Events

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            buttonApply = EnumApplyAction.Create;

            //Lists
            dataGridInvoice.Visibility = Visibility.Hidden;

            //Buttons
            btnNew.Visibility = btnEdit.Visibility = btnRemove.Visibility = Visibility.Hidden;
            btnApply.Visibility = btnCancel.Visibility = Visibility.Visible;

            notaProdutoDTO = new NotaProdutoDTO();
            newCollection = new NotaProdutoCollectionDTO();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                buttonApply = EnumApplyAction.Update;

                //Lists
                dataGridInvoice.Visibility = Visibility.Hidden;

                //Buttons
                btnNew.Visibility = btnEdit.Visibility = btnRemove.Visibility = Visibility.Hidden;
                btnApply.Visibility = btnCancel.Visibility = Visibility.Visible;

                //Pegando informações das notas apenas
                notaDTO = new NotaDTO();
                notaDTO = dataGridInvoice.SelectedItem as NotaDTO;

                txtInvoice.Text = notaDTO.NumeroNota.ToString();
                dpDate.SelectedDate = notaDTO.DataNota;
                cbProvider.SelectedValue = notaDTO.Fornecedor.Pessoa.NomePessoa;
                cbFilial.SelectedValue = notaDTO.Filial.Pessoa.NomePessoa;

                newCollection = notaProdutoBLL.ReadInvoice(notaDTO);
                oldCollection = notaProdutoBLL.ReadInvoice(notaDTO);

                dataGridAddStuff.ItemsSource = newCollection;
                LiberarRemoveStuff();
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
                        //Criar nota apenas
                        if (InvoiceValidation())
                        {
                            notaProdutoDTO.Nota = new NotaDTO();
                            notaProdutoDTO.Nota.NumeroNota = txtInvoice.Text;
                            notaProdutoDTO.Nota.DataNota = dpDate.SelectedDate.Value;

                            foreach (FornecedorDTO item in fornecedorCollectionDTO)
                            {
                                if (string.Compare(item.Pessoa.NomePessoa, cbProvider.SelectedItem.ToString()) == 0)
                                {
                                    notaProdutoDTO.Nota.Fornecedor = new FornecedorDTO();
                                    notaProdutoDTO.Nota.Fornecedor.Pessoa.IdPessoa = item.Pessoa.IdPessoa;
                                }
                            }

                            foreach (FilialDTO item in filialCollectionDTO)
                            {
                                if (string.Compare(item.Pessoa.NomePessoa, cbFilial.SelectedItem.ToString()) == 0)
                                {
                                    notaProdutoDTO.Nota.Filial = new FilialDTO();
                                    notaProdutoDTO.Nota.Filial.Pessoa.IdPessoa = item.Pessoa.IdPessoa;
                                }
                            }

                            int idTemp = Convert.ToInt32(notaBLL.Create(notaProdutoDTO.Nota));

                            //Criar Produtos relacionados às notas
                            foreach (NotaProdutoDTO item in newCollection)
                            {
                                item.Nota = notaProdutoDTO.Nota;
                                item.Nota.IdNota = idTemp;
                                notaProdutoBLL.Create(item);

                                estoqueDTO = new EstoqueDTO();
                                estoqueDTO.Produto = item.Produto;
                                estoqueDTO.Filial = notaProdutoDTO.Nota.Filial;
                                estoqueDTO.Quantidade = item.QuantidadeComprada;
                                estoqueBLL.Create(estoqueDTO);
                            }

                            InitialConditionPage();
                        }
                        break;
                    case EnumApplyAction.Update:
                        if (InvoiceValidation())
                        {
                            //Fazer Update apenas das notas
                            notaDTO.NumeroNota = txtInvoice.Text;
                            notaDTO.DataNota = dpDate.SelectedDate.Value;

                            foreach (FornecedorDTO item in fornecedorCollectionDTO)
                            {
                                if (string.Compare(item.Pessoa.NomePessoa, cbProvider.SelectedItem.ToString()) == 0)
                                {
                                    notaDTO.Fornecedor = new FornecedorDTO();
                                    notaDTO.Fornecedor.Pessoa.IdPessoa = item.Pessoa.IdPessoa;
                                }
                            }

                            foreach (FilialDTO item in filialCollectionDTO)
                            {
                                if (string.Compare(item.Pessoa.NomePessoa, cbFilial.SelectedItem.ToString()) == 0)
                                {
                                    notaDTO.Filial = new FilialDTO();
                                    notaDTO.Filial.Pessoa.IdPessoa = item.Pessoa.IdPessoa;
                                }
                            }

                            notaBLL.Update(notaDTO);
                            notaProdutoBLL.Delete(notaDTO);

                            foreach (NotaProdutoDTO item in oldCollection)
                            {
                                estoqueDTO = new EstoqueDTO();
                                estoqueDTO.Produto = item.Produto;
                                estoqueDTO.Filial = item.Nota.Filial;
                                estoqueDTO.Quantidade = item.QuantidadeComprada;
                                estoqueBLL.Delete(estoqueDTO);
                            }

                            //Adiciona todos
                            foreach (NotaProdutoDTO item in newCollection)
                            {
                                item.Nota = new NotaDTO();
                                item.Nota = notaDTO;
                                notaProdutoBLL.Create(item);

                                estoqueDTO = new EstoqueDTO();
                                estoqueDTO.Produto = item.Produto;
                                estoqueDTO.Filial = item.Nota.Filial;
                                estoqueDTO.Quantidade = item.QuantidadeComprada;
                                estoqueBLL.Create(estoqueDTO);
                            }
                            InitialConditionPage();
                        }
                        break;
                }
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
                notaDTO = dataGridInvoice.SelectedItem as NotaDTO;

                if (MessageBox.Show("Realmente deseja excluir a nota " + notaDTO.NumeroNota + "?", "Remover Nota", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    oldCollection = notaProdutoBLL.ReadInvoice(notaDTO);
                    notaProdutoBLL.Delete(notaDTO);
                    notaBLL.Delete(notaDTO);

                    foreach (NotaProdutoDTO item in oldCollection)
                    {
                        estoqueDTO = new EstoqueDTO();
                        estoqueDTO.Produto = item.Produto;
                        estoqueDTO.Filial = item.Nota.Filial;
                        estoqueDTO.Quantidade = item.QuantidadeComprada;
                        estoqueBLL.Delete(estoqueDTO);
                    }

                    InitialConditionPage();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddStuff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                notaProdutoDTO = new NotaProdutoDTO();
                notaProdutoDTO.Produto = new ProdutoDTO();
                notaProdutoDTO.Produto = produtoBLL.ReadName(cbStuff.SelectedValue.ToString())[0];
                notaProdutoDTO.QuantidadeComprada = float.Parse(txtStuffQt.Text.Replace(".", ","));
                notaProdutoDTO.ValorUnitario = Convert.ToDecimal(txtUnitaryValue.Text.Replace(".", ","));
                notaProdutoDTO.ValorTotal = (decimal)notaProdutoDTO.QuantidadeComprada * notaProdutoDTO.ValorUnitario;

                newCollection.Add(notaProdutoDTO);
                dataGridAddStuff.ItemsSource = null;
                dataGridAddStuff.ItemsSource = newCollection;
                LiberarRemoveStuff();

                cbStuff.SelectedIndex = -1;
                txtStuffQt.Text = string.Empty;
                txtUnitaryValue.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEditStuff_Click(object sender, RoutedEventArgs e)
        {
            notaProdutoDTO = new NotaProdutoDTO();
            notaProdutoDTO = dataGridAddStuff.SelectedItem as NotaProdutoDTO;

            cbStuff.SelectedItem = notaProdutoDTO.Produto.DescricaoProduto;
            txtStuffQt.Text = notaProdutoDTO.QuantidadeComprada.ToString().Replace(".", ",");
            txtUnitaryValue.Text = notaProdutoDTO.ValorUnitario.ToString().Replace(".", ",");

            newCollection.Remove(notaProdutoDTO);
            dataGridAddStuff.Items.Refresh();
        }

        private void btnRemoveStuff_Click(object sender, RoutedEventArgs e)
        {
            notaProdutoDTO = new NotaProdutoDTO();
            notaProdutoDTO = dataGridAddStuff.SelectedItem as NotaProdutoDTO;

            newCollection.Remove(notaProdutoDTO);
            dataGridAddStuff.ItemsSource = null;
            dataGridAddStuff.ItemsSource = newCollection;
            LiberarRemoveStuff();
        }

        private void cbStuff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtStuffQt.Text = string.Empty;
            lblUn.Content = string.Empty;
            txtUnitaryValue.Text = string.Empty;

            if (cbStuff.SelectedIndex != -1)
            {
                //Poder ser buscado direto do banco, ver
                foreach (ProdutoDTO item in produtoCollectionDTO)
                {
                    if (string.Compare(item.DescricaoProduto, cbStuff.SelectedItem.ToString()) == 0)
                    {
                        lblUn.Content = item.Unidade.SiglaUnidade;
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            InitialConditionPage();
        }

        private void dataGridInvoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = btnRemove.IsEnabled = true;
        }

        private void LiberarRemoveStuff()
        {
            if (dataGridAddStuff.Items.Count > 0)
            {
                btnRemoveStuff.IsEnabled = btnEditStuff.IsEnabled = true;
            }
            else
            {
                btnRemoveStuff.IsEnabled = btnEditStuff.IsEnabled = false;
            }
        }
        private void LiberarAddStuff()
        {
            if (cbStuff.SelectedIndex != -1)
            {
                float quantity = 0;
                if (float.TryParse(txtStuffQt.Text, out quantity))
                {
                    if (quantity > 0)
                    {
                        btnAddStuff.IsEnabled = true;
                    }
                    else
                    {
                        btnAddStuff.IsEnabled = false;
                    }
                }
                else
                {
                    btnAddStuff.IsEnabled = false;
                }
            }
            else
            {
                btnAddStuff.IsEnabled = false;
            }
        }
        private void txtStuffQt_TextChanged(object sender, TextChangedEventArgs e)
        {
            LiberarAddStuff();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitialConditionPage();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}