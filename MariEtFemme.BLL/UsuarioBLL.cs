using System;
using System.Data;
using System.Text;
using MariEtFemme.Tools;
using MariEtFemme.DAL;
using MariEtFemme.DTO;

namespace MariEtFemme.BLL
{
    public class UsuarioBLL
    {
        /// <summary>
        /// Instância de acesso ao banco.
        /// </summary>
        MySqlDatabaseAccess dataBaseAccess = new MySqlDatabaseAccess();

        /// <summary>     
        /// Cria um novo registro no banco.     
        /// </summary>     
        /// <param name="funcionario">Objeto que contém as informações necessárias para criar o registro no banco.</param>
        /// <returns>Ação a ser tomada pela tela</returns>
        public string Create(FuncionarioDTO funcionario)
        {
            try
            {
                /*funcionario.Usuario.Usuario = Encryption.Encrypt(funcionario.Usuario.Usuario);
                funcionario.Usuario.Senha = Encryption.Encrypt(funcionario.Usuario.Senha);*/

                dataBaseAccess.AddParameters("_idPessoa", funcionario.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_usuario", funcionario.Usuario.Usuario);
                dataBaseAccess.AddParameters("_senha", funcionario.Usuario.Senha);
                dataBaseAccess.AddParameters("_idPrivilegio", funcionario.Usuario.Privilegio.IdPrivilegio);
                dataBaseAccess.AddParameters("_situacao", funcionario.Usuario.Situacao);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_usuario_criar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível cadastrar o usuário: ").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }

        /// <summary>     
        /// Altera o registro no banco.     
        /// </summary>     
        /// <param name="funcionario">Objeto que contém as informações necessárias para alterar o registro no banco.</param>
        /// <returns>Ação a ser tomada pela tela</returns>
        public string Update(FuncionarioDTO funcionario)
        {
            try
            {
                dataBaseAccess.AddParameters("_idPessoa", funcionario.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_usuario", funcionario.Usuario.Usuario);
                dataBaseAccess.AddParameters("_senha", funcionario.Usuario.Senha);
                dataBaseAccess.AddParameters("_idPrivilegio", funcionario.Usuario.Privilegio.IdPrivilegio);
                dataBaseAccess.AddParameters("_situacao", funcionario.Usuario.Situacao);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_usuario_modificar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível alterar o usuário: ").Append(ex.Message);
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
        /// <returns>Ação a ser tomada pela tela</returns>
        public string Delete(FuncionarioDTO funcionario)
        {
            try
            {
                dataBaseAccess.AddParameters("_id", funcionario.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_table_name", "pessoa_usuario");
                dataBaseAccess.AddParameters("_colum_name", "IdPessoa");
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_remover");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível remover o usuário:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }

        /// <summary>     
        /// Autentica um usuário.
        /// </summary>     
        /// <param name="username">Nome do usuário que será autenticado.</param>
        /// <param name="password">Senha que será autenticada.</param>
        /// <returns>Ação a ser tomada pela tela</returns>
        public string AuthenticateUser(string username, string password)
        {
            try
            {
                dataBaseAccess.AddParameters("_usuario", username);
                dataBaseAccess.AddParameters("_senha", password);

                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_autenticar");

                return dataTable.Rows[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível autenticar o usuário:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }
    }
}