using System;
using System.Data;
using System.Text;
using MariEtFemme.DAL;
using MariEtFemme.DTO;

namespace MariEtFemme.BLL
{
    public class FuncionarioBLL
    {
        /// <summary>
        /// Instância de acesso ao banco.
        /// </summary>
        MySqlDatabaseAccess dataBaseAccess = new MySqlDatabaseAccess();

        private void PreencherObjetoPessoa(FuncionarioDTO funcionario)
        {
            dataBaseAccess.AddParameters("_table_name", funcionario.NomeTabela);

            dataBaseAccess.AddParameters("_nomePessoa", funcionario.Pessoa.NomePessoa);
            dataBaseAccess.AddParameters("_tipoPessoa", funcionario.Pessoa.TipoPessoa);
            dataBaseAccess.AddParameters("_comentarios", funcionario.Pessoa.Comentarios);

            if (funcionario.Pessoa.TipoPessoa)
            {
                dataBaseAccess.AddParameters("_nascimento", funcionario.Pessoa.PessoaFisica.Nascimento);
                dataBaseAccess.AddParameters("_genero", funcionario.Pessoa.PessoaFisica.Genero);

                dataBaseAccess.AddParameters("_razaoSocial", null);
                dataBaseAccess.AddParameters("_cnpj", null);
            }
            else
            {
                dataBaseAccess.AddParameters("_razaoSocial", funcionario.Pessoa.PessoaJuridica.RazaoSocial);
                dataBaseAccess.AddParameters("_cnpj", funcionario.Pessoa.PessoaJuridica.CNPJ);

                dataBaseAccess.AddParameters("_nascimento", null);
                dataBaseAccess.AddParameters("_genero", null);
            }

            dataBaseAccess.AddParameters("_rua", funcionario.Pessoa.Endereco.Rua);
            dataBaseAccess.AddParameters("_numero", funcionario.Pessoa.Endereco.Numero);
            dataBaseAccess.AddParameters("_bairro", funcionario.Pessoa.Endereco.Bairro);
            dataBaseAccess.AddParameters("_cidade", funcionario.Pessoa.Endereco.Cidade);
            dataBaseAccess.AddParameters("_idEstado", funcionario.Pessoa.Endereco.Estado.IdEstado);

            dataBaseAccess.AddParameters("_telefone1", funcionario.Pessoa.Contato.Telefone1);
            dataBaseAccess.AddParameters("_idOperadora1", funcionario.Pessoa.Contato.Operadora1.IdOperadora);
            dataBaseAccess.AddParameters("_whatsApp1", funcionario.Pessoa.Contato.WhatsApp1);
            dataBaseAccess.AddParameters("_telefone2", funcionario.Pessoa.Contato.Telefone2);
            dataBaseAccess.AddParameters("_idOperadora2", funcionario.Pessoa.Contato.Operadora2.IdOperadora);
            dataBaseAccess.AddParameters("_whatsApp2", funcionario.Pessoa.Contato.WhatsApp1);
            dataBaseAccess.AddParameters("_telefone3", funcionario.Pessoa.Contato.Telefone3);
            dataBaseAccess.AddParameters("_idOperadora3", funcionario.Pessoa.Contato.Operadora3.IdOperadora);
            dataBaseAccess.AddParameters("_whatsApp3", funcionario.Pessoa.Contato.WhatsApp1);
            dataBaseAccess.AddParameters("_email", funcionario.Pessoa.Contato.Email);

            dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
        }

        /// <summary>     
        /// Lista os funcionários cadastrados
        /// </summary>     
        /// <param name="funcionario">Nome do funcionário que será consultado.</param>
        /// <returns>Informações do(s) funcionário(s) encontrado.</returns>
        public FuncionarioCollectionDTO ReadName(string funcionario)
        {
            FuncionarioCollectionDTO funcionarioCollectionDTO = new FuncionarioCollectionDTO();

            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_funcionario", funcionario);

                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_funcionario_nome");

                foreach (DataRow row in dataTable.Rows)
                {
                    FuncionarioDTO funcionarioDTO = new FuncionarioDTO();

                    PessoaBLL pessoaBLL = new PessoaBLL();
                    funcionarioDTO.Pessoa = pessoaBLL.PreencherPessoa(row);

                    FilialBLL filialBLL = new FilialBLL();
                    funcionarioDTO.Filial = filialBLL.ReadId(Convert.ToInt32(row["IdFilial"]));

                    funcionarioDTO.Cargo.IdCargo = Convert.ToInt32(row["IdCargo"]);
                    funcionarioDTO.Cargo.DescricaoCargo = row["DescricaoCargo"].ToString();

                    #region Usuario e Privilégio

                    if (!string.IsNullOrEmpty(row["Usuario"].ToString()))
                    {
                        funcionarioDTO.Usuario.Usuario = (row["Usuario"].ToString());
                        funcionarioDTO.Usuario.Senha = (row["Senha"].ToString());
                        funcionarioDTO.Usuario.Situacao = Convert.ToBoolean(row["Situacao"]);
                        funcionarioDTO.Usuario.DescricaoSituacao = row["DescricaoSituacao"].ToString();

                        funcionarioDTO.Usuario.Privilegio.IdPrivilegio = Convert.ToInt32(row["IdPrivilegio"]);
                        funcionarioDTO.Usuario.Privilegio.DescricaoPrivilegio = row["DescricaoPrivilegio"].ToString();
                    }

                    #endregion

                    funcionarioCollectionDTO.Add(funcionarioDTO);
                }

                return funcionarioCollectionDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar funcionario por nome:\n\n").Append(ex.Message);
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
        public FuncionarioCollectionDTO ReadExcept(bool _tipoPessoa)
        {
            FuncionarioCollectionDTO funcionarioCollectionDTO = new FuncionarioCollectionDTO();

            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_tipoPessoa", _tipoPessoa);

                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_funcionario_exceto");

                foreach (DataRow row in dataTable.Rows)
                {
                    FuncionarioDTO funcionarioDTO = new FuncionarioDTO();
                    PessoaBLL pessoaBLL = new PessoaBLL();
                    //funcionarioDTO.Pessoa = new PessoaDTO();
                    funcionarioDTO.Pessoa = pessoaBLL.PreencherPessoa(row);

                    funcionarioCollectionDTO.Add(funcionarioDTO);
                }

                return funcionarioCollectionDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar funcionário por excessão:\n\n").Append(ex.Message);
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

        public string Create(FuncionarioDTO funcionario)
        {
            try
            {
                dataBaseAccess.AddParameters("_idPessoa", funcionario.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_idFilial", funcionario.Filial.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_idCargo", funcionario.Cargo.IdCargo);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_funcionario_criar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível cadastrar o funcionário: ").Append(ex.Message);
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
        /// <param name="funcionario">Objeto que contém as informações necessárias para atualizar o registro no banco.</param>
        public string Update(FuncionarioDTO funcionario)
        {
            try
            {
                dataBaseAccess.AddParameters("_idPessoa", funcionario.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_idFilial", funcionario.Filial.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_idCargo", funcionario.Cargo.IdCargo);
                PreencherObjetoPessoa(funcionario);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_funcionario_modificar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível alterar o funcionário: ").Append(ex.Message);
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
        /// <param name="funcionario">Objeto que contém as informações necessárias para remover o registro do banco.</param>
        public string Delete(FuncionarioDTO funcionario)
        {
            try
            {
                dataBaseAccess.AddParameters("_idPessoa", funcionario.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_funcionario_remover");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível remover o funcionário:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }

        #region Utilizados no login

        /// <summary>     
        /// Consulta um funcionário através de um nome de usuário.
        /// </summary>     
        /// <param name="username">Nome do usuário que será consultado.</param>
        /// <returns>Funcionário encontrado.</returns>
        public FuncionarioDTO ReadUser(string username)
        {
            try
            {
                dataBaseAccess.AddParameters("_username", username);
                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_funcionario_nome_usuario");
                FuncionarioDTO funcionarioDTO = new FuncionarioDTO();

                PessoaBLL pessoaBLL = new PessoaBLL();
                funcionarioDTO.Pessoa = pessoaBLL.PreencherPessoa(dataTable.Rows[0]);

                FilialBLL filialBLL = new FilialBLL();
                funcionarioDTO.Filial = filialBLL.ReadId(Convert.ToInt32(dataTable.Rows[0]["IdFilial"]));

                funcionarioDTO.Cargo.IdCargo = Convert.ToInt32(dataTable.Rows[0]["IdCargo"]);
                funcionarioDTO.Cargo.DescricaoCargo = dataTable.Rows[0]["DescricaoCargo"].ToString();

                funcionarioDTO.Usuario.Usuario = (dataTable.Rows[0]["Usuario"].ToString());
                funcionarioDTO.Usuario.Senha = (dataTable.Rows[0]["Senha"].ToString());
                funcionarioDTO.Usuario.Situacao = Convert.ToBoolean(dataTable.Rows[0]["Situacao"]);
                funcionarioDTO.Usuario.DescricaoSituacao = dataTable.Rows[0]["DescricaoSituacao"].ToString();
                funcionarioDTO.Usuario.Privilegio.IdPrivilegio = Convert.ToInt32(dataTable.Rows[0]["IdPrivilegio"]);
                funcionarioDTO.Usuario.Privilegio.DescricaoPrivilegio = dataTable.Rows[0]["DescricaoPrivilegio"].ToString();

                return funcionarioDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar funcionario por nome:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }

        #endregion

        #region Utilizados no gerenciamento de usuários
        public FuncionarioCollectionDTO ReadEmployeeUser(bool user)
        {
            FuncionarioCollectionDTO funcionarioCollectionDTO = new FuncionarioCollectionDTO();

            try
            {
                dataBaseAccess.AddParameters("_user", user);
                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_funcionario_nome_usuario_todos");
                PessoaBLL pessoaBLL = new PessoaBLL();
                FilialBLL filialBLL = new FilialBLL();

                foreach (DataRow row in dataTable.Rows)
                {
                    FuncionarioDTO funcionarioDTO = new FuncionarioDTO();
                    funcionarioDTO.Pessoa = pessoaBLL.PreencherPessoa(row);

                    funcionarioDTO.Filial = filialBLL.ReadId(Convert.ToInt32(dataTable.Rows[0]["IdFilial"]));

                    funcionarioDTO.Cargo.IdCargo = Convert.ToInt32(dataTable.Rows[0]["IdCargo"]);
                    funcionarioDTO.Cargo.DescricaoCargo = dataTable.Rows[0]["DescricaoCargo"].ToString();

                    if (user)
                    {
                        funcionarioDTO.Usuario.Usuario = (row["Usuario"].ToString());
                        funcionarioDTO.Usuario.Senha = (row["Senha"].ToString());
                        funcionarioDTO.Usuario.Situacao = Convert.ToBoolean(row["Situacao"]);
                        funcionarioDTO.Usuario.DescricaoSituacao = row["DescricaoSituacao"].ToString();
                        funcionarioDTO.Usuario.Privilegio.IdPrivilegio = Convert.ToInt32(row["IdPrivilegio"]);
                        funcionarioDTO.Usuario.Privilegio.DescricaoPrivilegio = row["DescricaoPrivilegio"].ToString();
                    }

                    funcionarioCollectionDTO.Add(funcionarioDTO);
                }
                return funcionarioCollectionDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar funcionário por uduário:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }

        #endregion

        #region Utilizados no atendimento

        /// <summary>     
        /// Consulta informações de privilegio por nome.
        /// </summary>     
        /// <param name="idFuncionario">Nome do privilegio que será consultado.</param>
        /// <returns>Informações do privilegio encontrado.</returns>
        public FuncionarioDTO ReadId(int idFuncionario)
        {
            try
            {
                dataBaseAccess.AddParameters("_idFuncionario", idFuncionario);
                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_funcionario_id");

                FuncionarioDTO funcionarioDTO = new FuncionarioDTO();

                foreach (DataRow row in dataTable.Rows)
                {
                    PessoaBLL pessoaBLL = new PessoaBLL();
                    funcionarioDTO.Pessoa = pessoaBLL.PreencherPessoa(row);

                    FilialBLL filialBLL = new FilialBLL();
                    funcionarioDTO.Filial = filialBLL.ReadId(Convert.ToInt32(row["IdFilial"]));

                    funcionarioDTO.Cargo.IdCargo = Convert.ToInt32(row["IdCargo"]);
                    funcionarioDTO.Cargo.DescricaoCargo = row["DescricaoCargo"].ToString();

                    if (!string.IsNullOrEmpty(row["Usuario"].ToString()))
                    {
                        funcionarioDTO.Usuario.Usuario = (row["Usuario"].ToString());
                        funcionarioDTO.Usuario.Senha = (row["Senha"].ToString());
                        funcionarioDTO.Usuario.Situacao = Convert.ToBoolean(row["Situacao"]);
                        funcionarioDTO.Usuario.DescricaoSituacao = row["DescricaoSituacao"].ToString();

                        funcionarioDTO.Usuario.Privilegio.IdPrivilegio = Convert.ToInt32(row["IdPrivilegio"]);
                        funcionarioDTO.Usuario.Privilegio.DescricaoPrivilegio = row["DescricaoPrivilegio"].ToString();
                    }
                }

                return funcionarioDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar funcionario por ID:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }

        #endregion
    }
}