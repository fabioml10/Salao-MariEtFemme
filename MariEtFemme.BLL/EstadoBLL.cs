using System;
using System.Data;
using System.Text;
using MariEtFemme.DAL;
using MariEtFemme.DTO;

namespace MariEtFemme.BLL
{
    public class EstadoBLL
    {
        /// <summary>
        /// Instância de acesso ao banco.
        /// </summary>
        MySqlDatabaseAccess dataBaseAccess = new MySqlDatabaseAccess();

        /// <summary>     
        /// Consulta informações do usuário por nome.
        /// </summary>     
        /// <param name="state">Nome do usuário que será consultado.</param>
        /// <returns>Informações do usuário encontrado.</returns>
        public EstadoCollectionDTO ReadName(string state)
        {
            EstadoCollectionDTO estadoCollectionDTO = new EstadoCollectionDTO();

            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_siglaEstado", state);

                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_estado_sigla");

                foreach (DataRow row in dataTable.Rows)
                {
                    EstadoDTO estadoDTO = new EstadoDTO();
                    estadoDTO.IdEstado = Convert.ToInt32(row["IdEstado"]);
                    estadoDTO.SiglaEstado = row["SiglaEstado"].ToString();
                    estadoDTO.DescricaoEstado = row["DescricaoEstado"].ToString();

                    estadoCollectionDTO.Add(estadoDTO);
                }

                return estadoCollectionDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar estado por nome:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }
    }
}
