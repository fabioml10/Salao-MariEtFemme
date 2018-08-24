using System;
using System.Data;
using System.Text;
using MariEtFemme.DAL;
using MariEtFemme.DTO;

namespace MariEtFemme.BLL
{
    public class AtendimentoServicoBLL
    {
        /// <summary>
        /// Instância de acesso ao banco.
        /// </summary>
        MySqlDatabaseAccess dataBaseAccess = new MySqlDatabaseAccess();

        /// <summary>     
        /// Consulta informações de privilegio por nome.
        /// </summary>     
        /// <param name="atendimento">Nome do privilegio que será consultado.</param>
        /// <returns>Informações do privilegio encontrado.</returns>
        public AtendimentoServicoCollectionDTO ReadAttendance(AtendimentoDTO atendimento)
        {
            AtendimentoServicoCollectionDTO atendimentoServicoCollectionDTO = new AtendimentoServicoCollectionDTO();

            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idAtendimento", atendimento.IdAtendimento);

                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_atendimento_servico_atendimento");

                foreach (DataRow row in dataTable.Rows)
                {
                    AtendimentoServicoDTO atendimentoServicoDTO = new AtendimentoServicoDTO();

                    AtendimentoBLL atendimentoBLL = new AtendimentoBLL();
                    atendimentoServicoDTO.Atendimento = atendimentoBLL.ReadId(Convert.ToInt32(row["IdAtendimento"]));

                    atendimentoServicoDTO.Servico = new ServicoDTO();
                    atendimentoServicoDTO.Servico.IdServico = Convert.ToInt32(row["IdServico"]);

                    atendimentoServicoCollectionDTO.Add(atendimentoServicoDTO);
                }

                return atendimentoServicoCollectionDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar servicos por atendimento:\n\n").Append(ex.Message);
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
        public string Create(AtendimentoServicoDTO atendimentoServico)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idAtendimento", atendimentoServico.Atendimento.IdAtendimento);
                dataBaseAccess.AddParameters("_idServico", atendimentoServico.Servico.IdServico);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_atendimento_servico_criar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível cadastrar os servicos no atendimento: ").Append(ex.Message);
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
                dataBaseAccess.AddParameters("_table_name", "atendimento_servico");
                dataBaseAccess.AddParameters("_colum_name", "IdAtendimento");
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_remover");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível remover os servicos relacionados ao atendimento:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }
    }
}