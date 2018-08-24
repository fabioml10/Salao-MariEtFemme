using System;
using System.Data;
using System.Text;
using MariEtFemme.DAL;
using MariEtFemme.DTO;

namespace MariEtFemme.BLL
{
    public class ClienteBLL
    {
        /// <summary>
        /// Instância de acesso ao banco.
        /// </summary>
        MySqlDatabaseAccess dataBaseAccess = new MySqlDatabaseAccess();
        private void PreencherObjetoPessoa(ClienteDTO clienteDTO)
        {
            dataBaseAccess.AddParameters("_table_name", clienteDTO.NomeTabela);

            dataBaseAccess.AddParameters("_nomePessoa", clienteDTO.Pessoa.NomePessoa);
            dataBaseAccess.AddParameters("_tipoPessoa", clienteDTO.Pessoa.TipoPessoa);
            dataBaseAccess.AddParameters("_comentarios", clienteDTO.Pessoa.Comentarios);

            if (clienteDTO.Pessoa.TipoPessoa)
            {
                dataBaseAccess.AddParameters("_nascimento", clienteDTO.Pessoa.PessoaFisica.Nascimento);
                dataBaseAccess.AddParameters("_genero", clienteDTO.Pessoa.PessoaFisica.Genero);

                dataBaseAccess.AddParameters("_razaoSocial", null);
                dataBaseAccess.AddParameters("_cnpj", null);
            }
            else
            {
                dataBaseAccess.AddParameters("_razaoSocial", clienteDTO.Pessoa.PessoaJuridica.RazaoSocial);
                dataBaseAccess.AddParameters("_cnpj", clienteDTO.Pessoa.PessoaJuridica.CNPJ);

                dataBaseAccess.AddParameters("_nascimento", null);
                dataBaseAccess.AddParameters("_genero", null);
            }

            dataBaseAccess.AddParameters("_rua", clienteDTO.Pessoa.Endereco.Rua);
            dataBaseAccess.AddParameters("_numero", clienteDTO.Pessoa.Endereco.Numero);
            dataBaseAccess.AddParameters("_bairro", clienteDTO.Pessoa.Endereco.Bairro);
            dataBaseAccess.AddParameters("_cidade", clienteDTO.Pessoa.Endereco.Cidade);
            dataBaseAccess.AddParameters("_idEstado", clienteDTO.Pessoa.Endereco.Estado.IdEstado);

            dataBaseAccess.AddParameters("_telefone1", clienteDTO.Pessoa.Contato.Telefone1);
            dataBaseAccess.AddParameters("_idOperadora1", clienteDTO.Pessoa.Contato.Operadora1.IdOperadora);
            dataBaseAccess.AddParameters("_whatsApp1", clienteDTO.Pessoa.Contato.WhatsApp1);
            dataBaseAccess.AddParameters("_telefone2", clienteDTO.Pessoa.Contato.Telefone2);
            dataBaseAccess.AddParameters("_idOperadora2", clienteDTO.Pessoa.Contato.Operadora2.IdOperadora);
            dataBaseAccess.AddParameters("_whatsApp2", clienteDTO.Pessoa.Contato.WhatsApp1);
            dataBaseAccess.AddParameters("_telefone3", clienteDTO.Pessoa.Contato.Telefone3);
            dataBaseAccess.AddParameters("_idOperadora3", clienteDTO.Pessoa.Contato.Operadora3.IdOperadora);
            dataBaseAccess.AddParameters("_whatsApp3", clienteDTO.Pessoa.Contato.WhatsApp1);
            dataBaseAccess.AddParameters("_email", clienteDTO.Pessoa.Contato.Email);

            dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
        }

        /// <summary>     
        /// Consulta informações de privilegio por nome.
        /// </summary>     
        /// <param name="id">Nome do privilegio que será consultado.</param>
        /// <returns>Informações do privilegio encontrado.</returns>
        public ClienteDTO ReadId(int id)
        {
            try
            {
                dataBaseAccess.AddParameters("_idCliente", id);
                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_cliente_id");
                ClienteDTO clienteDTO = new ClienteDTO();

                foreach (DataRow row in dataTable.Rows)
                {                    
                    PessoaBLL pessoaBLL = new PessoaBLL();
                    clienteDTO.Pessoa = pessoaBLL.PreencherPessoa(row);
                }

                return clienteDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar cliente por Id:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }

        /// <summary>     
        /// Consulta informações de privilegio por nome.
        /// </summary>     
        /// <param name="client">Nome do privilegio que será consultado.</param>
        /// <returns>Informações do privilegio encontrado.</returns>
        public ClienteCollectionDTO ReadName(string client)
        {
            ClienteCollectionDTO clienteCollectionDTO = new ClienteCollectionDTO();

            try
            {
                dataBaseAccess.AddParameters("_cliente", client);
                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_cliente_nome");

                foreach (DataRow row in dataTable.Rows)
                {
                    ClienteDTO clienteDTO = new ClienteDTO();
                    PessoaBLL pessoaBLL = new PessoaBLL();
                    clienteDTO.Pessoa = pessoaBLL.PreencherPessoa(row);
                    clienteCollectionDTO.Add(clienteDTO);
                }

                return clienteCollectionDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar cliente por nome:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }

        /// <summary>     
        /// Consulta informações de privilegio por nome.
        /// </summary>     
        /// <returns>Informações do privilegio encontrado.</returns>
        public ClienteCollectionDTO ReadExcept(bool _tipoPessoa)
        {
            ClienteCollectionDTO clienteCollectionDTO = new ClienteCollectionDTO();

            try
            {
                dataBaseAccess.AddParameters("_tipoPessoa", _tipoPessoa);

                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_cliente_exceto");

                foreach (DataRow row in dataTable.Rows)
                {
                    ClienteDTO clienteDTO = new ClienteDTO();
                    PessoaBLL pessoaBLL = new PessoaBLL();
                    clienteDTO.Pessoa = pessoaBLL.PreencherPessoa(row);
                    clienteCollectionDTO.Add(clienteDTO);
                }

                return clienteCollectionDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar cliente por excessão:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }

        /// <summary>     
        /// Consulta informações de privilegio por nome.
        /// </summary>     
        /// <returns>Informações do privilegio encontrado.</returns>
        public string Create(ClienteDTO clienteDTO)
        {
            try
            {
                dataBaseAccess.AddParameters("_idPessoa", clienteDTO.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_cliente_criar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível cadastrar o cliente: ").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }

        /// <summary>     
        /// Remove o registro do banco.     
        /// </summary>     
        /// <param name="clienteDTO">Objeto que contém as informações necessárias para remover o registro do banco.</param>
        public string Delete(ClienteDTO clienteDTO)
        {
            try
            {
                dataBaseAccess.AddParameters("_idPessoa", clienteDTO.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_cliente_remover");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível remover o cliente:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }

        /// <summary>     
        /// Atualiza o registro no banco.     
        /// </summary>     
        /// <param name="clienteDTO">Objeto que contém as informações necessárias para atualizar o registro no banco.</param>
        public string Update(ClienteDTO clienteDTO)
        {
            try
            {
                dataBaseAccess.AddParameters("_idPessoa", clienteDTO.Pessoa.IdPessoa);
                PreencherObjetoPessoa(clienteDTO);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_cliente_modificar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível alterar o cliente: ").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }
    }
}