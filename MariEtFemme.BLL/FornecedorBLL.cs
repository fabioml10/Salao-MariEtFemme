using System;
using System.Data;
using System.Text;
using MariEtFemme.DAL;
using MariEtFemme.DTO;

namespace MariEtFemme.BLL
{
    public class FornecedorBLL
    {
        /// <summary>
        /// Instância de acesso ao banco.
        /// </summary>
        MySqlDatabaseAccess dataBaseAccess = new MySqlDatabaseAccess();

        /// <summary>     
        /// Consulta informações de privilegio por nome.
        /// </summary>     
        /// <param name="fornecedor">Nome do privilegio que será consultado.</param>
        /// <returns>Informações do privilegio encontrado.</returns>
        public FornecedorCollectionDTO ReadName(string fornecedor)
        {
            FornecedorCollectionDTO fornecedorCollectionDTO = new FornecedorCollectionDTO();

            try
            {
                dataBaseAccess.AddParameters("_fornecedor", fornecedor);

                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_fornecedor_nome");

                foreach (DataRow row in dataTable.Rows)
                {
                    FornecedorDTO fornecedorDTO = new FornecedorDTO();
                    PessoaBLL pessoaBLL = new PessoaBLL();
                    fornecedorDTO.Pessoa = pessoaBLL.PreencherPessoa(row);

                    fornecedorCollectionDTO.Add(fornecedorDTO);
                }

                return fornecedorCollectionDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar fornecedor por nome:\n\n").Append(ex.Message);
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
        public FornecedorCollectionDTO ReadExcept(bool _tipoPessoa)
        {
            FornecedorCollectionDTO fornecedorCollectionDTO = new FornecedorCollectionDTO();

            try
            {
                dataBaseAccess.AddParameters("_tipoPessoa", _tipoPessoa);

                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_fornecedor_exceto");

                foreach (DataRow row in dataTable.Rows)
                {
                    FornecedorDTO fornecedorDTO = new FornecedorDTO();
                    PessoaBLL pessoaBLL = new PessoaBLL();
                    fornecedorDTO.Pessoa = pessoaBLL.PreencherPessoa(row);

                    fornecedorCollectionDTO.Add(fornecedorDTO);
                }

                return fornecedorCollectionDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar fornecedor por excessão:\n\n").Append(ex.Message);
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
        /// 
        public string Create(FornecedorDTO fornecedorDTO)
        {
            try
            {
                dataBaseAccess.AddParameters("_idPessoa", fornecedorDTO.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_fornecedor_criar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível cadastrar o fornecedor: ").Append(ex.Message);
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
        /// <param name="fornecedorDTO">Objeto que contém as informações necessárias para atualizar o registro no banco.</param>
        public string Update(FornecedorDTO fornecedorDTO)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idPessoa", fornecedorDTO.Pessoa.IdPessoa);
                PreencherObjetoPessoa(fornecedorDTO);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_fornecedor_modificar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível alterar o fornecedor: ").Append(ex.Message);
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
        /// <param name="fornecedorDTO">Objeto que contém as informações necessárias para remover o registro do banco.</param>
        public string Delete(FornecedorDTO fornecedorDTO)
        {
            try
            {
                dataBaseAccess.AddParameters("_idPessoa", fornecedorDTO.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_fornecedor_remover");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível remover o fornecedor:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }
        private void PreencherObjetoPessoa(FornecedorDTO fornecedorDTO)
        {
            dataBaseAccess.AddParameters("_table_name", fornecedorDTO.NomeTabela);

            dataBaseAccess.AddParameters("_nomePessoa", fornecedorDTO.Pessoa.NomePessoa);
            dataBaseAccess.AddParameters("_tipoPessoa", fornecedorDTO.Pessoa.TipoPessoa);
            dataBaseAccess.AddParameters("_comentarios", fornecedorDTO.Pessoa.Comentarios);

            if (fornecedorDTO.Pessoa.TipoPessoa)
            {
                dataBaseAccess.AddParameters("_nascimento", fornecedorDTO.Pessoa.PessoaFisica.Nascimento);
                dataBaseAccess.AddParameters("_genero", fornecedorDTO.Pessoa.PessoaFisica.Genero);

                dataBaseAccess.AddParameters("_razaoSocial", null);
                dataBaseAccess.AddParameters("_cnpj", null);
            }
            else
            {
                dataBaseAccess.AddParameters("_razaoSocial", fornecedorDTO.Pessoa.PessoaJuridica.RazaoSocial);
                dataBaseAccess.AddParameters("_cnpj", fornecedorDTO.Pessoa.PessoaJuridica.CNPJ);

                dataBaseAccess.AddParameters("_nascimento", null);
                dataBaseAccess.AddParameters("_genero", null);
            }

            dataBaseAccess.AddParameters("_rua", fornecedorDTO.Pessoa.Endereco.Rua);
            dataBaseAccess.AddParameters("_numero", fornecedorDTO.Pessoa.Endereco.Numero);
            dataBaseAccess.AddParameters("_bairro", fornecedorDTO.Pessoa.Endereco.Bairro);
            dataBaseAccess.AddParameters("_cidade", fornecedorDTO.Pessoa.Endereco.Cidade);
            dataBaseAccess.AddParameters("_idEstado", fornecedorDTO.Pessoa.Endereco.Estado.IdEstado);

            dataBaseAccess.AddParameters("_telefone1", fornecedorDTO.Pessoa.Contato.Telefone1);
            dataBaseAccess.AddParameters("_idOperadora1", fornecedorDTO.Pessoa.Contato.Operadora1.IdOperadora);
            dataBaseAccess.AddParameters("_whatsApp1", fornecedorDTO.Pessoa.Contato.WhatsApp1);
            dataBaseAccess.AddParameters("_telefone2", fornecedorDTO.Pessoa.Contato.Telefone2);
            dataBaseAccess.AddParameters("_idOperadora2", fornecedorDTO.Pessoa.Contato.Operadora2.IdOperadora);
            dataBaseAccess.AddParameters("_whatsApp2", fornecedorDTO.Pessoa.Contato.WhatsApp1);
            dataBaseAccess.AddParameters("_telefone3", fornecedorDTO.Pessoa.Contato.Telefone3);
            dataBaseAccess.AddParameters("_idOperadora3", fornecedorDTO.Pessoa.Contato.Operadora3.IdOperadora);
            dataBaseAccess.AddParameters("_whatsApp3", fornecedorDTO.Pessoa.Contato.WhatsApp1);
            dataBaseAccess.AddParameters("_email", fornecedorDTO.Pessoa.Contato.Email);

            dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
        }

    }
}