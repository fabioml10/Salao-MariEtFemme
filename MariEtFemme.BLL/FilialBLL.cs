using System;
using System.Data;
using System.Text;
using MariEtFemme.DAL;
using MariEtFemme.DTO;

namespace MariEtFemme.BLL
{
    public class FilialBLL
    {
        /// <summary>
        /// Instância de acesso ao banco.
        /// </summary>
        MySqlDatabaseAccess dataBaseAccess = new MySqlDatabaseAccess();
        private void PreencherObjetoPessoa(FilialDTO filial)
        {
            dataBaseAccess.AddParameters("_table_name", filial.NomeTabela);

            dataBaseAccess.AddParameters("_nomePessoa", filial.Pessoa.NomePessoa);
            dataBaseAccess.AddParameters("_tipoPessoa", filial.Pessoa.TipoPessoa);
            dataBaseAccess.AddParameters("_comentarios", filial.Pessoa.Comentarios);

            if (filial.Pessoa.TipoPessoa)
            {
                dataBaseAccess.AddParameters("_nascimento", filial.Pessoa.PessoaFisica.Nascimento);
                dataBaseAccess.AddParameters("_genero", filial.Pessoa.PessoaFisica.Genero);

                dataBaseAccess.AddParameters("_razaoSocial", null);
                dataBaseAccess.AddParameters("_cnpj", null);
            }
            else
            {
                dataBaseAccess.AddParameters("_razaoSocial", filial.Pessoa.PessoaJuridica.RazaoSocial);
                dataBaseAccess.AddParameters("_cnpj", filial.Pessoa.PessoaJuridica.CNPJ);

                dataBaseAccess.AddParameters("_nascimento", null);
                dataBaseAccess.AddParameters("_genero", null);
            }

            dataBaseAccess.AddParameters("_rua", filial.Pessoa.Endereco.Rua);
            dataBaseAccess.AddParameters("_numero", filial.Pessoa.Endereco.Numero);
            dataBaseAccess.AddParameters("_bairro", filial.Pessoa.Endereco.Bairro);
            dataBaseAccess.AddParameters("_cidade", filial.Pessoa.Endereco.Cidade);
            dataBaseAccess.AddParameters("_idEstado", filial.Pessoa.Endereco.Estado.IdEstado);

            dataBaseAccess.AddParameters("_telefone1", filial.Pessoa.Contato.Telefone1);
            dataBaseAccess.AddParameters("_idOperadora1", filial.Pessoa.Contato.Operadora1.IdOperadora);
            dataBaseAccess.AddParameters("_whatsApp1", filial.Pessoa.Contato.WhatsApp1);
            dataBaseAccess.AddParameters("_telefone2", filial.Pessoa.Contato.Telefone2);
            dataBaseAccess.AddParameters("_idOperadora2", filial.Pessoa.Contato.Operadora2.IdOperadora);
            dataBaseAccess.AddParameters("_whatsApp2", filial.Pessoa.Contato.WhatsApp1);
            dataBaseAccess.AddParameters("_telefone3", filial.Pessoa.Contato.Telefone3);
            dataBaseAccess.AddParameters("_idOperadora3", filial.Pessoa.Contato.Operadora3.IdOperadora);
            dataBaseAccess.AddParameters("_whatsApp3", filial.Pessoa.Contato.WhatsApp1);
            dataBaseAccess.AddParameters("_email", filial.Pessoa.Contato.Email);

            dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
        }

        /// <summary>     
        /// Consulta informações de privilegio por nome.
        /// </summary>     
        /// <param name="idFilial">Nome do privilegio que será consultado.</param>
        /// <returns>Informações do privilegio encontrado.</returns>
        public FilialDTO ReadId(int idFilial)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idFilial", idFilial);

                DataTable dataTable = new DataTable();
                FilialDTO filialDTO = new FilialDTO();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_filial_id");

                PessoaBLL pessoaBLL = new PessoaBLL();
                filialDTO.Pessoa = pessoaBLL.PreencherPessoa(dataTable.Rows[0]);

                return filialDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar filial por ID:\n\n").Append(ex.Message);
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
        /// <param name="filial">Nome do privilegio que será consultado.</param>
        /// <returns>Informações do privilegio encontrado.</returns>
        public FilialCollectionDTO ReadName(string filial)
        {
            FilialCollectionDTO filialCollectionDTO = new FilialCollectionDTO();

            try
            {
                dataBaseAccess.AddParameters("_filial", filial);

                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_filial_nome");

                foreach (DataRow row in dataTable.Rows)
                {
                    FilialDTO filialDTO = new FilialDTO();
                    PessoaBLL pessoaBLL = new PessoaBLL();
                    filialDTO.Pessoa = pessoaBLL.PreencherPessoa(row);

                    filialCollectionDTO.Add(filialDTO);
                }

                return filialCollectionDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar filial por nome:\n\n").Append(ex.Message);
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
        public FilialCollectionDTO ReadExcept(bool _tipoPessoa)
        {
            FilialCollectionDTO filialCollectionDTO = new FilialCollectionDTO();

            try
            {
                dataBaseAccess.AddParameters("_tipoPessoa", _tipoPessoa);

                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_filial_exceto");

                foreach (DataRow row in dataTable.Rows)
                {
                    FilialDTO filialDTO = new FilialDTO();
                    PessoaBLL pessoaBLL = new PessoaBLL();
                    filialDTO.Pessoa = pessoaBLL.PreencherPessoa(row);

                    filialCollectionDTO.Add(filialDTO);
                }

                return filialCollectionDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar filial por excessão:\n\n").Append(ex.Message);
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

        public string Create(FilialDTO filial)
        {
            try
            {
                dataBaseAccess.AddParameters("_idPessoa", filial.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_filial_criar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível cadastrar a filial: ").Append(ex.Message);
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
        /// <param name="filial">Objeto que contém as informações necessárias para atualizar o registro no banco.</param>
        public string Update(FilialDTO filial)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idPessoa", filial.Pessoa.IdPessoa);
                PreencherObjetoPessoa(filial);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_filial_Modificar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível alterar a filial: ").Append(ex.Message);
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
        /// <param name="filial">Objeto que contém as informações necessárias para remover o registro do banco.</param>
        public string Delete(FilialDTO filial)
        {
            try
            {
                dataBaseAccess.AddParameters("_idPessoa", filial.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_filial_remover");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível remover a filial:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }
    }
}