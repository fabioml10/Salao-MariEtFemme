using System;
using System.Windows;
using System.Windows.Controls;
using MariEtFemme.Tools;
using MariEtFemme.BLL;
using MariEtFemme.DTO;

namespace MariEtFemme.View
{
    public partial class ServiceTool : Window
    {
        public ServiceTool()
        {
            InitializeComponent();
        }

        #region Variables

        /// <summary>
        /// Define qual ação o botão Confirmar deve tomar.
        /// </summary>
        private EnumApplyAction buttonApply;

        ServicoDTO servicoDTO;
        ServicoCollectionDTO servicoCollectionDTO;
        ServicoBLL servicoBLL = new ServicoBLL();

        ServicoProdutoDTO servicoProdutoDTO;
        ServicoProdutoCollectionDTO newCollection;
        ServicoProdutoCollectionDTO oldCollection;
        ServicoProdutoBLL servicoProdutoBLL = new ServicoProdutoBLL();

        ProdutoCollectionDTO produtoCollectionDTO;
        ProdutoBLL produtoBLL = new ProdutoBLL();

        #endregion

        #region Functions

        /// <summary>
        /// Preenche lista de serviços.
        /// </summary>
        private void FillServicesList()
        {
            servicoCollectionDTO = new ServicoCollectionDTO();
            servicoCollectionDTO = servicoBLL.ReadName(string.Empty);
            dataGridService.ItemsSource = null;
            dataGridService.ItemsSource = servicoCollectionDTO;

            dataGridStuff.ItemsSource = null;
            dataGridAddStuff.ItemsSource = null;
        }

        private void FillStuffs()
        {
            produtoCollectionDTO = produtoBLL.ReadName(string.Empty);

            cbStuff.Items.Clear();
            foreach (ProdutoDTO item in produtoCollectionDTO)
            {
                cbStuff.Items.Add(item.DescricaoProduto);
            }
        }

        /// <summary>
        /// Devolve à página suas condiçõies iniciais.
        /// </summary>
        private void InitialConditionPage()
        {
            switch (Session.LoggedUser.Usuario.Privilegio.IdPrivilegio)
            {
                default:
                    //Fill
                    FillServicesList();
                    FillStuffs();
                    buttonApply = new EnumApplyAction();

                    //Controls
                    txtServiço.Text = string.Empty;
                    cbStuff.SelectedIndex = -1;

                    //Lists
                    dataGridService.Visibility = dataGridStuff.Visibility = Visibility.Visible;
                    dataGridAddStuff.Visibility = Visibility.Hidden;

                    //Buttons
                    btnNew.Visibility = btnEdit.Visibility = btnRemove.Visibility = Visibility.Visible;
                    btnEdit.IsEnabled = btnRemove.IsEnabled = btnAddFeedStock.IsEnabled = btnRemoveFeedStock.IsEnabled = btnEditStuff.IsEnabled = false;
                    btnAddFeedStock.Visibility = btnRemoveFeedStock.Visibility = btnApply.Visibility = btnCancel.Visibility = Visibility.Hidden;
                    break;
            }
        }

        private void LiberarRemoveStuff()
        {
            if (dataGridAddStuff.Items.Count > 0)
            {
                btnRemoveFeedStock.IsEnabled = btnEditStuff.IsEnabled = true;
            }
            else
            {
                btnRemoveFeedStock.IsEnabled = btnEditStuff.IsEnabled = false;
            }
        }

        private void LiberarAddStuff()
        {
            if (cbStuff.SelectedIndex != -1)
            {
                float quantity = 0;
                if (float.TryParse(txtQty.Text, out quantity))
                {
                    if (quantity > 0)
                    {
                        btnAddFeedStock.IsEnabled = true;
                    }
                    else
                    {
                        btnAddFeedStock.IsEnabled = false;
                    }
                }
                else
                {
                    btnAddFeedStock.IsEnabled = false;
                }
            }
            else
            {
                btnAddFeedStock.IsEnabled = false;
            }
        }

        public bool ValidarServico(ServicoDTO service)
        {
            if (!String.IsNullOrEmpty(txtServiço.Text))
            {
                return true;
            }
            else
            {
                MessageBox.Show("O campo serviço não pode estar em branco.");
                return false;
            }
        }

        #endregion

        #region Events

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            dataGridService.Visibility =
               dataGridStuff.Visibility = Visibility.Hidden;

            dataGridAddStuff.Visibility = Visibility.Visible;

            btnNew.Visibility =
            btnEdit.Visibility =
            btnRemove.Visibility = Visibility.Hidden;

            btnAddFeedStock.Visibility =
            btnRemoveFeedStock.Visibility =
            btnApply.Visibility =
            btnCancel.Visibility = Visibility.Visible;

            buttonApply = EnumApplyAction.Create;

            newCollection = new ServicoProdutoCollectionDTO();
            
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            buttonApply = EnumApplyAction.Update;

            //Lists
            dataGridService.Visibility = dataGridStuff.Visibility = Visibility.Hidden;
            dataGridAddStuff.Visibility = Visibility.Visible;

            //Buttons
            btnNew.Visibility = btnEdit.Visibility = btnRemove.Visibility = Visibility.Hidden;
            btnAddFeedStock.Visibility = btnRemoveFeedStock.Visibility = btnApply.Visibility = btnCancel.Visibility = Visibility.Visible;

            servicoDTO = new ServicoDTO();
            servicoDTO = dataGridService.SelectedItem as ServicoDTO;
            dataGridAddStuff.ItemsSource = newCollection;
            txtServiço.Text = servicoDTO.DescricaoServico;
            LiberarRemoveStuff();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            servicoDTO = dataGridService.SelectedItem as ServicoDTO;

            if (MessageBox.Show("Realmente deseja excluir o Serviço " + servicoDTO.DescricaoServico + "?", "Remover Serviço", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                /*AttendanceServiceCollectionDTO attendanceServiceCollectionDTO = new AttendanceServiceCollectionDTO();
                AttendanceServiceBLL attendanceServiceBLL = new AttendanceServiceBLL();
                attendanceServiceCollectionDTO = attendanceServiceBLL.ReadAttendanceServiceService(serviceDTO);

                if (attendanceServiceCollectionDTO.Count > 0)
                {
                    MessageBox.Show("Impossível excluir. O serviço foi utilizado em um atendimento.", "Excluir Serviço", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {*/
                    servicoBLL.Delete(servicoDTO);
                    MessageBox.Show("Servico Removido com sucesso.");
                    InitialConditionPage();
                //}
            }
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            
            switch (buttonApply)
            {
                case EnumApplyAction.Create:
                    servicoDTO = new ServicoDTO();
                    if (ValidarServico(servicoDTO))
                    {
                        if (newCollection.Count > 0)
                        {
                            servicoDTO.DescricaoServico = txtServiço.Text;
                            servicoDTO.IdServico = Convert.ToInt32(servicoBLL.Create(servicoDTO));

                            //Criar as relações serviço_produto
                            foreach (ServicoProdutoDTO item in newCollection)
                            {
                                item.Servico = new ServicoDTO();
                                item.Servico.IdServico = servicoDTO.IdServico;
                                servicoProdutoBLL.Create(item);
                            }

                            MessageBox.Show("Serviço criado com sucesso.");
                            InitialConditionPage();
                        }
                        else
                        {
                            servicoDTO.DescricaoServico = txtServiço.Text;
                            servicoBLL.Create(servicoDTO);
                            MessageBox.Show("Serviço criado com sucesso.");
                            InitialConditionPage();
                        }
                    }
                    break;

                case EnumApplyAction.Update:
                    if (ValidarServico(servicoDTO))
                    {
                        //Update do Serviço
                        servicoDTO.DescricaoServico = txtServiço.Text;
                        servicoBLL.Update(servicoDTO);

                        //Remove os produtos antigos
                        //Pode deletar todos de uma vez ao invés de ir um por um
                        foreach (ServicoProdutoDTO item in oldCollection)
                        {
                            servicoProdutoBLL.Delete(item.Servico);
                        }

                        //Adiciona os novos produtos relacionados com o serviço
                        if (newCollection.Count > 0)
                        {
                            //Criar as relações serviço_produto
                            foreach (ServicoProdutoDTO item in newCollection)
                            {
                                item.Servico = new ServicoDTO();
                                item.Servico.IdServico = servicoDTO.IdServico;
                                servicoProdutoBLL.Create(item);
                            }

                            MessageBox.Show("Serviço modificado com sucesso.");
                            InitialConditionPage();
                        }
                    }
                    break;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            InitialConditionPage();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAddFeedStock_Click(object sender, RoutedEventArgs e)
        {
            servicoProdutoDTO = new ServicoProdutoDTO();
            servicoProdutoDTO.Produto = new ProdutoDTO();
            servicoProdutoDTO.Produto = produtoBLL.ReadName(cbStuff.SelectedItem.ToString())[0];
            servicoProdutoDTO.Produto.Consumo = float.Parse(txtQty.Text.Replace(".", ","));

            newCollection.Add(servicoProdutoDTO);
            dataGridAddStuff.ItemsSource = null;
            dataGridAddStuff.ItemsSource = newCollection;
            txtQty.Text = string.Empty;
            cbStuff.SelectedIndex = -1;
            LiberarRemoveStuff();
        }

        private void btnRemoveFeedStock_Click(object sender, RoutedEventArgs e) //Verificado
        {
            servicoProdutoDTO = new ServicoProdutoDTO();
            servicoProdutoDTO = dataGridAddStuff.SelectedItem as ServicoProdutoDTO;

            newCollection.Remove(servicoProdutoDTO);
            dataGridAddStuff.ItemsSource = null;
            dataGridAddStuff.ItemsSource = newCollection;
            LiberarRemoveStuff();
        }

        private void btnEditStuff_Click(object sender, RoutedEventArgs e)
        {
            servicoProdutoDTO = new ServicoProdutoDTO();
            servicoProdutoDTO = dataGridAddStuff.SelectedItem as ServicoProdutoDTO;

            cbStuff.SelectedItem = servicoProdutoDTO.Produto.DescricaoProduto;
            txtQty.Text = servicoProdutoDTO.Produto.Consumo.ToString().Replace(".", ",");
            newCollection.Remove(servicoProdutoDTO);
            dataGridAddStuff.Items.Refresh();
        }

        private void dataGridService_SelectionChanged(object sender, SelectionChangedEventArgs e)//Verificado
        {
            if (dataGridService.SelectedIndex != -1)
            {
                btnEdit.IsEnabled = btnRemove.IsEnabled = true;
                servicoDTO = dataGridService.SelectedItem as ServicoDTO;

                oldCollection = servicoProdutoBLL.ReadService(servicoDTO);
                newCollection = servicoProdutoBLL.ReadService(servicoDTO);

                dataGridStuff.ItemsSource = null;
                dataGridStuff.ItemsSource = newCollection;
            }
        }

        private void cbStuff_SelectionChanged(object sender, SelectionChangedEventArgs e) //Verificado
        {
            txtQty.Text = string.Empty;
            lblUn.Content = string.Empty;

            if (cbStuff.SelectedIndex != -1)
            {
                foreach (ProdutoDTO item in produtoCollectionDTO)
                {
                    if (string.Compare(item.DescricaoProduto, cbStuff.SelectedItem.ToString()) == 0)
                    {
                        lblUn.Content = item.Unidade.SiglaUnidade;
                        LiberarAddStuff();
                    }
                }
            }
        }

        private void txtQty_TextChanged(object sender, TextChangedEventArgs e)
        {
            LiberarAddStuff();
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitialConditionPage();
        }
    }
}
