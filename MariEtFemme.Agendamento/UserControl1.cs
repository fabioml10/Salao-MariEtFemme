using System;
using System.Windows.Forms;
using MariEtFemme.DTO;
using MariEtFemme.BLL;
using System.Drawing;

namespace MariEtFemme.Agendamento
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            this.BackColor = Color.Gray;

            // Initialize the DataGridView.
            dataGridViewClients.AutoGenerateColumns = false;
            dataGridViewClients.DataSource = clienteCollectionDTO;
            DataGridViewColumn columnClient = new DataGridViewTextBoxColumn();
            columnClient.DataPropertyName = "Pessoa".ToString();
            columnClient.HeaderText = "Nome";
            dataGridViewClients.Columns.Add(columnClient);

            // Initialize the DataGridView.
            dataGridViewServices.AutoGenerateColumns = false;
            DataGridViewColumn columnService = new DataGridViewTextBoxColumn();
            columnService.DataPropertyName = "DescricaoServico".ToString();
            columnService.HeaderText = "Servico";
            dataGridViewServices.Columns.Add(columnService);
        }

        AgendamentoDTO agendamentoDTO;

        AgendamentoServicoBLL agendamentoServicoBLL = new AgendamentoServicoBLL();

        ClienteBLL clienteBLL = new ClienteBLL();
        ClienteCollectionDTO clienteCollectionDTO;

        ServicoBLL servicoBLL = new ServicoBLL();
        ServicoCollectionDTO servicoCollectionDTO;
        public ServicoCollectionDTO newCollection = new ServicoCollectionDTO();

        EstoqueDTO estoqueDTO;
        EstoqueBLL estoqueBLL = new EstoqueBLL();

        public event EventHandler VerifiqueAtendimento;

        #region Functions

        private void PreencherClientes()
        {
            clienteCollectionDTO = new ClienteCollectionDTO();
            clienteCollectionDTO = clienteBLL.ReadName(string.Empty);
            dataGridViewClients.DataSource = null;
            dataGridViewClients.DataSource = clienteCollectionDTO;
        }
        private void PreencherServicos()
        {
            cbServices.Items.Clear();
            servicoCollectionDTO = new ServicoCollectionDTO();
            servicoCollectionDTO = servicoBLL.ReadName(string.Empty);

            foreach (ServicoDTO item in servicoCollectionDTO)
            {
                cbServices.Items.Add(item.DescricaoServico);
            }
        }
        public void PreencherAgendamento()
        {
            agendamentoDTO = new AgendamentoDTO();
            agendamentoDTO.Cliente = new ClienteDTO();
            agendamentoDTO.Servicos = new ServicoCollectionDTO();

            agendamentoDTO.Cliente.Pessoa.NomePessoa = txtClientName.Text;
            agendamentoDTO.Servicos = newCollection;
            agendamentoDTO.Observacoes = txtComments.Text;
        }

        #endregion

        private void EditAppointment_Load(object sender, EventArgs e)
        {
            dataGridViewClients.Visible = false;
            btnApplyClient.Visible = false;
            btnCancelClient.Visible = false;
        }

        private void btnAddService_Click(object sender, EventArgs e)
        {
            ServicoDTO servicoDTO = new ServicoDTO();
            servicoDTO = servicoBLL.ReadName(cbServices.SelectedItem.ToString())[0];
            newCollection.Add(servicoDTO);
            dataGridViewServices.DataSource = null;
            dataGridViewServices.DataSource = newCollection;
        }

        private void btnRemoveService_Click(object sender, EventArgs e)
        {
            ServicoDTO servicoDTO = new ServicoDTO();
            newCollection.Remove(dataGridViewServices.SelectedRows[0].DataBoundItem as ServicoDTO);
            dataGridViewServices.DataSource = null;
            dataGridViewServices.DataSource = newCollection;
        }

        public void CarregarDataGrid(AgendamentoDTO agendamento)
        {
            newCollection = new ServicoCollectionDTO();
            newCollection = agendamentoServicoBLL.ReadService(agendamento);
            dataGridViewServices.DataSource = null;
            dataGridViewServices.DataSource = newCollection;

            cbServices.SelectedIndex = -1;
        }

        private void EditAppointment_Enter(object sender, EventArgs e)
        {
            PreencherClientes();
            PreencherServicos();

            if (VerifiqueAtendimento != null)
            {
                VerifiqueAtendimento(this, new EventArgs());
            }
        }

        private void btnSearchClient_Click(object sender, EventArgs e)
        {
            dataGridViewClients.ClearSelection();
            dataGridViewClients.Visible = true;
            btnApplyClient.Visible = true;
            btnCancelClient.Visible = true;
            btnCancel.Visible = false;
            btnRemove.Visible = false;
        }

        private void btnApplyClient_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridViewClients.SelectedRows)
            {
                ClienteDTO clienteDTO = new ClienteDTO();
                clienteDTO = item.DataBoundItem as ClienteDTO;
                txtClientName.Text = clienteDTO.Pessoa.NomePessoa;
            }

            dataGridViewClients.Visible = false;
            btnApplyClient.Visible = false;
            btnCancelClient.Visible = false;
            btnCancel.Visible = true;
            btnRemove.Visible = true;
        }

        private void btnCancelClient_Click(object sender, EventArgs e)
        {
            dataGridViewClients.Visible = false;
            btnApplyClient.Visible = false;
            btnCancelClient.Visible = false;
            btnCancel.Visible = true;
            btnRemove.Visible = true;
        }  

        public Color PegarCorFundo()
        {
            colorDialog.Color = txtColor.BackColor;
            colorDialog.ShowDialog();            
            txtColor.BackColor = colorDialog.Color;
            return colorDialog.Color;
        }

        public Color PegarCorBorda()
        {
            colorDialog.Color = txtBorderColor.BackColor;
            colorDialog.ShowDialog();
            txtBorderColor.BackColor = colorDialog.Color;
            return colorDialog.Color;
        }

        public EventHandler RemoveEstoque;
        private void RemoverDoEstoque()
        {
            ServicoProdutoCollectionDTO servicoProdutoCollectionDTO = new ServicoProdutoCollectionDTO();
            ServicoProdutoBLL servicoProdutoBLL = new ServicoProdutoBLL();
            servicoProdutoCollectionDTO = servicoProdutoBLL.ReadService(newCollection[0]);

            foreach (ServicoProdutoDTO item2 in servicoProdutoCollectionDTO)
            {
                estoqueDTO = new EstoqueDTO();
                estoqueDTO.Produto = item2.Produto;
                estoqueDTO.Filial = new FilialDTO();
                estoqueDTO.Filial.Pessoa.IdPessoa = Session.LoggedUser.Filial.Pessoa.IdPessoa; //Verificar como o sistema define a filial no Session
                estoqueDTO.Quantidade = item2.Produto.Consumo;
                estoqueBLL.Delete(estoqueDTO);
            }
        }
    }
}