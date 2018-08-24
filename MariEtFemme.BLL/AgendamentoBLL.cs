using System;
using System.Drawing;
using System.Text;
using System.Data;
using MariEtFemme.DTO;
using MariEtFemme.DAL;

namespace MariEtFemme.BLL
{
    public class AgendamentoBLL
    {
        /// <summary>     
        /// Instância de acesso ao banco.     
        /// </summary>
        MySqlDatabaseAccess dataBaseAccess = new MySqlDatabaseAccess();

        /// <summary>     
        /// Consulta todas informações no banco.
        /// </summary>     
        /// <returns>Retorna uma coleção de objetos com as informações encontradas no banco.</returns>
        public AgendamentoCollectionDTO ReadDateRange(DateTime startDate, DateTime endDate, int _agenda)
        {
            try
            {
                dataBaseAccess.AddParameters("_dataInicial", startDate.Date);
                dataBaseAccess.AddParameters("_dataFinal", endDate.Date);
                dataBaseAccess.AddParameters("_agenda", _agenda);

                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_agendamento_data");

                AgendamentoCollectionDTO appointments = new AgendamentoCollectionDTO();

                foreach (DataRow row in dataTable.Rows)
                {
                    AgendamentoDTO appointment = new AgendamentoDTO();
                    appointment.IdAgendamento = Convert.ToInt32(row["IdAgendamento"]);

                    int temp;
                    if (Int32.TryParse(row["IdPessoaCliente"].ToString(), out temp))
                    {
                        appointment.Cliente.Pessoa.IdPessoa = temp;
                    }
                    else
                    {
                        appointment.Cliente.Pessoa.IdPessoa = null;
                    }
                    appointment.Cliente.Pessoa.NomePessoa = row["NomePessoa"].ToString();
                    appointment.StartDate = Convert.ToDateTime(row["DataInicio"].ToString());
                    appointment.EndDate = Convert.ToDateTime(row["DataFim"].ToString());
                    appointment.Observacoes = row["Observacoes"].ToString();
                    appointment.Atendido = Convert.ToInt32(row["IdAtendimento"]);

                    string corFundo = row["CorFundo"].ToString();
                    string[] coresFundos = corFundo.Split(',');
                    appointment.Color = Color.FromArgb(Convert.ToInt32(coresFundos[0]), Convert.ToInt32(coresFundos[1]), Convert.ToInt32(coresFundos[2]), Convert.ToInt32(coresFundos[3]));

                    string corBorda = row["CorBorda"].ToString();
                    string[] coresBordas = corBorda.Split(',');
                    appointment.BorderColor = Color.FromArgb(Convert.ToInt32(coresBordas[0]), Convert.ToInt32(coresBordas[1]), Convert.ToInt32(coresBordas[2]), Convert.ToInt32(coresBordas[3]));
                    appointment.Layer = Convert.ToInt32(row["Agenda"]);

                    appointments.Add(appointment);
                }

                return appointments;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível realizar a consulta de agenamento: ").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }

        /// <summary>     
        /// Consulta todas informações no banco.
        /// </summary>     
        /// <returns>Retorna uma coleção de objetos com as informações encontradas no banco.</returns>
        public string ReadeExists(AgendamentoDTO agendamentoDTO)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idAgendamento", agendamentoDTO.IdAgendamento);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_agendamento_existe");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível realizar a consulta de agendamento: ").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }

        }

        /// <summary>     
        /// Cria um novo registro no banco.     
        /// </summary>     
        /// <param name="appointment">Objeto que contém as informações necessárias para criar o registro no banco.</param>
        /// <returns>Retorna o id do objeto se o registro for inserido com sucesso no banco.</returns>
        public string Create(AgendamentoDTO appointment)
        {
            try
            {
                string corFundo = appointment.Color.A + "," + appointment.Color.R + "," + appointment.Color.G + "," + appointment.Color.B;
                string corBorda = appointment.Color.A + "," + appointment.BorderColor.R + "," + appointment.BorderColor.G + "," + appointment.BorderColor.B;
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idPessoaCliente", appointment.Cliente.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_nomeClienteNaoCadastrado", appointment.Cliente.Pessoa.NomePessoa);
                dataBaseAccess.AddParameters("_dataInicio", appointment.StartDate);
                dataBaseAccess.AddParameters("_dataFim", appointment.EndDate);
                dataBaseAccess.AddParameters("_observacoes", appointment.Observacoes);
                dataBaseAccess.AddParameters("_corFundo", corFundo);
                dataBaseAccess.AddParameters("_corBorda", corBorda);
                dataBaseAccess.AddParameters("_agenda", appointment.Layer);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_agendamento_criar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível cadastrar o agendamento: ").Append(ex.Message);
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
        /// <param name="appointment">Objeto que contém as informações necessárias para atualizar o registro no banco.</param>
        public void Update(AgendamentoDTO appointment)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idAgendamento", appointment.IdAgendamento);
                dataBaseAccess.AddParameters("_idPessoaCliente", appointment.Cliente.Pessoa.IdPessoa);

                if(appointment.Cliente.Pessoa.IdPessoa.Equals(null))
                {
                    dataBaseAccess.AddParameters("_nomeClienteNaoCadastrado", appointment.Cliente.Pessoa.NomePessoa);
                }
                else
                {
                    dataBaseAccess.AddParameters("_nomeClienteNaoCadastrado", null);
                }

                string corFundo = appointment.Color.A + "," + appointment.Color.R + "," + appointment.Color.G + "," + appointment.Color.B;
                string corBorda = appointment.BorderColor.A + "," + appointment.BorderColor.R + "," + appointment.BorderColor.G + "," + appointment.BorderColor.B;

                dataBaseAccess.AddParameters("_dataInicio", appointment.StartDate);
                dataBaseAccess.AddParameters("_dataFim", appointment.EndDate);
                dataBaseAccess.AddParameters("_observacoes", appointment.Observacoes);
                dataBaseAccess.AddParameters("_corFundo", corFundo);
                dataBaseAccess.AddParameters("_corBorda", corBorda);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_agendamento_modificar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível alterar o agendamento: ").Append(ex.Message);
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
        /// <param name="appointment">Objeto que contém as informações necessárias para remover o registro do banco.</param>
        /// <returns>Retorna o id do objeto se o registro for removido com sucesso no banco.</returns>
        public void Delete(AgendamentoDTO appointment)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_id", appointment.IdAgendamento);
                dataBaseAccess.AddParameters("_table_name", "agendamento");
                dataBaseAccess.AddParameters("_colum_name", "IdAgendamento");
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_remover");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível excluir o agendamento: ").Append(ex.Message);
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }

        /// <summary>     
        /// Atualiza o registro no banco.     
        /// </summary>     
        /// <param name="appointment">Objeto que contém as informações necessárias para atualizar o registro no banco.</param>
        public void Atendido(AgendamentoDTO appointment, AtendimentoDTO atendimento)
        {
            try
            {
                dataBaseAccess.AddParameters("_idAgendamento", appointment.IdAgendamento);
                dataBaseAccess.AddParameters("_idAtendimento", atendimento.IdAtendimento);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_agendamento_atendido");
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