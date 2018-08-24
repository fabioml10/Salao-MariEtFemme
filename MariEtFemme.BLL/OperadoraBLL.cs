using System;
using System.Data;
using System.Text;
using MariEtFemme.DAL;
using MariEtFemme.DTO;

namespace MariEtFemme.BLL
{
    public class OperadoraBLL
    {
        /// <summary>
        /// Instância de acesso ao banco.
        /// </summary>
        MySqlDatabaseAccess dataBaseAccess = new MySqlDatabaseAccess();

        /// <summary>     
        /// Consulta informações do usuário por nome.
        /// </summary>     
        /// <param name="operadora">Nome do usuário que será consultado.</param>
        /// <returns>Informações do usuário encontrado.</returns>
        public OperadoraCollectionDTO ReadName(string operadora)
        {
            OperadoraCollectionDTO operadoraCollectionDTO = new OperadoraCollectionDTO();

            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_descricaoOperadora", operadora);

                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_operadora_descricao");

                foreach (DataRow row in dataTable.Rows)
                {
                    OperadoraDTO operadoraDTO = new OperadoraDTO();
                    operadoraDTO.IdOperadora = Convert.ToInt32(row["IdOperadora"]);
                    operadoraDTO.DescricaoOperadora = row["DescricaoOperadora"].ToString();

                    operadoraCollectionDTO.Add(operadoraDTO);
                }

                return operadoraCollectionDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar operadora por nome:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }
    }
}
