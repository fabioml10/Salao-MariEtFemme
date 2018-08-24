using System;
using System.Data;
using System.Text;
using MariEtFemme.DAL;
using MariEtFemme.DTO;

namespace MariEtFemme.BLL
{
    public class PrivilegioBLL
    {
        /// <summary>
        /// Instância de acesso ao banco.
        /// </summary>
        MySqlDatabaseAccess dataBaseAccess = new MySqlDatabaseAccess();

        /// <summary>     
        /// Consulta informações de privilegio por nome.
        /// </summary>     
        /// <param name="privilege">Nome do privilegio que será consultado.</param>
        /// <returns>Informações do privilegio encontrado.</returns>
        public PrivilegioCollectionDTO ReadName(string privilege)
        {
            PrivilegioCollectionDTO privilegioCollectionDTO = new PrivilegioCollectionDTO();

            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_privilegio", privilege);

                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_privilegio_nome");

                foreach (DataRow row in dataTable.Rows)
                {
                    PrivilegioDTO privilegioDTO = new PrivilegioDTO();
                    privilegioDTO.IdPrivilegio = Convert.ToInt32(row["IdPrivilegio"]);
                    privilegioDTO.DescricaoPrivilegio = row["DescricaoPrivilegio"].ToString();

                    privilegioCollectionDTO.Add(privilegioDTO);
                }

                return privilegioCollectionDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar privilegio por nome:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }
    }
}