using System;
using System.Data;
using System.Text;
using MariEtFemme.DAL;
using MariEtFemme.DTO;

namespace MariEtFemme.BLL
{
    public class AtendimentoBLL
    {
        /// <summary>
        /// Instância de acesso ao banco.
        /// </summary>
        MySqlDatabaseAccess dataBaseAccess = new MySqlDatabaseAccess();

        /// <summary>     
        /// Consulta informações de atendimentos.
        /// </summary>     
        /// <returns>Informações dos atendimentos encontrados.</returns>
        public AtendimentoCollectionDTO ReadAll()
        {
            AtendimentoCollectionDTO atendimentoCollectionDTO = new AtendimentoCollectionDTO();

            try
            {
                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_atendimento_todos");

                foreach (DataRow row in dataTable.Rows)
                {
                    AtendimentoDTO atendimentoDTO = new AtendimentoDTO();
                    atendimentoDTO.IdAtendimento = Convert.ToInt32(row["IdAtendimento"]);
                    atendimentoDTO.DataAtendimento = Convert.ToDateTime(row["DataAtendimento"]);
                    atendimentoDTO.ComenariosAtendimento = row["ComentariosAtendimento"].ToString();

                    ClienteBLL clienteBLL = new ClienteBLL();
                    atendimentoDTO.Cliente = clienteBLL.ReadId(Convert.ToInt32(row["IdCliente"]));

                    FuncionarioBLL funcionarioBLL = new FuncionarioBLL();
                    atendimentoDTO.Funcionario = funcionarioBLL.ReadId(Convert.ToInt32(row["IdFuncionario"]));

                    atendimentoCollectionDTO.Add(atendimentoDTO);
                }

                return atendimentoCollectionDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar os atendimentos:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }

        /// <summary>     
        /// Consulta informações de atendimentos.
        /// </summary>     
        /// <returns>Informações dos atendimentos encontrados.</returns>
        public AtendimentoDTO ReadId(int id)
        {
            try
            {
                dataBaseAccess.AddParameters("_idAtendimento", id);
                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_atendimento_id");

                AtendimentoDTO atendimentoDTO = new AtendimentoDTO();
                atendimentoDTO.IdAtendimento = Convert.ToInt32(dataTable.Rows[0]["IdAtendimento"]);
                atendimentoDTO.DataAtendimento = Convert.ToDateTime(dataTable.Rows[0]["DataAtendimento"]);
                atendimentoDTO.ComenariosAtendimento = dataTable.Rows[0]["ComentariosAtendimento"].ToString();

                FuncionarioBLL funcionarioBLL = new FuncionarioBLL();
                atendimentoDTO.Funcionario = funcionarioBLL.ReadId(Convert.ToInt32(dataTable.Rows[0]["IdFuncionario"]));

                ClienteBLL clienteBLL = new ClienteBLL();
                atendimentoDTO.Cliente = clienteBLL.ReadId(Convert.ToInt32(dataTable.Rows[0]["IdCliente"]));

                return atendimentoDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar atendimento:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }

        /// <summary>     
        /// Cria um registro no banco.
        /// </summary>     
        /// <returns>Ação a ser tomada pelo form.</returns>
        public string Create(AtendimentoDTO atendimento)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idFuncionario", atendimento.Funcionario.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_idCliente", atendimento.Cliente.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_dataAtendimento", atendimento.DataAtendimento);
                dataBaseAccess.AddParameters("_comentariosAtendimento", atendimento.ComenariosAtendimento);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_atendimento_criar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível cadastrar o atendimento: ").Append(ex.Message);
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
        /// <param name="atendimento">Objeto que contém as informações necessárias para remover o registro do banco.</param>
        public string Delete(AtendimentoDTO atendimento)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_id", atendimento.IdAtendimento);
                dataBaseAccess.AddParameters("_table_name", "atendimento");
                dataBaseAccess.AddParameters("_colum_name", "IdAtendimento");
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_remover");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível remover o atendimento:\n\n").Append(ex.Message);
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
        /// <param name="atendimento">Objeto que contém as informações necessárias para atualizar o registro no banco.</param>
        public string Update(AtendimentoDTO atendimento)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idAtendimento", atendimento.IdAtendimento);
                dataBaseAccess.AddParameters("_idFuncionario", atendimento.Funcionario.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_idCliente", atendimento.Cliente.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_dataAtendimento", atendimento.DataAtendimento);
                dataBaseAccess.AddParameters("_comentariosAtendimento", atendimento.ComenariosAtendimento);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_atendimento_modificar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível alterar o atendimento: ").Append(ex.Message);
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
        /// <param name="atendimento">Objeto que contém as informações necessárias para atualizar o registro no banco.</param>
        public string Atendido(AtendimentoDTO atendimento, bool atendido)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idAtendimento", atendimento.IdAtendimento);
                dataBaseAccess.AddParameters("_atendido", atendimento.Funcionario.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_atendido");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível efetuar o atendimento: ").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }
    }
}