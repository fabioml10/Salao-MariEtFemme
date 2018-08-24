using System;
using System.Text;
using System.Data;
using MariEtFemme.DTO;
using MariEtFemme.DAL;

namespace MariEtFemme.BLL
{
    public class AgendamentoServicoBLL
    {
        /// <summary>     
        /// Instância de acesso ao banco.     
        /// </summary>
        MySqlDatabaseAccess dataBaseAccess = new MySqlDatabaseAccess();

        /// <summary>     
        /// Cria um novo registro no banco.     
        /// </summary>     
        /// <param name="appointment">Objeto que contém as informações necessárias para criar o registro no banco.</param>
        /// <returns>Retorna o id do objeto se o registro for inserido com sucesso no banco.</returns>
        public string Create(AgendamentoDTO appointment, ServicoDTO servico)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idAgendamento", appointment.IdAgendamento);
                dataBaseAccess.AddParameters("_idServico", servico.IdServico);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_agendamento_servico_criar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível cadastrar os servicos relacionados ao agendamento: ").Append(ex.Message);
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
                dataBaseAccess.AddParameters("_id", appointment.IdAgendamento);
                dataBaseAccess.AddParameters("_table_name", "servico_agendamento");
                dataBaseAccess.AddParameters("_colum_name", "IdAgendamento");
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_remover");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível excluir os serviços relacionados ao  agendamento: ").Append(ex.Message);
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
        public ServicoCollectionDTO ReadService(AgendamentoDTO agendamento)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idAgendamento", agendamento.IdAgendamento);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);

                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_agendamento_servico_servico");

                ServicoCollectionDTO servicos = new ServicoCollectionDTO();

                foreach (DataRow row in dataTable.Rows)
                {
                    ServicoDTO servicoDTO = new ServicoDTO();
                    servicoDTO.IdServico = Convert.ToInt32(row["IdServico"]);
                    servicoDTO.DescricaoServico = row["DescricaoServico"].ToString();
                    servicos.Add(servicoDTO);
                }

                return servicos;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível realizar a consulta de serviços por agendamento: ").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }
    }
}