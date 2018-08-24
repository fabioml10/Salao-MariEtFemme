using System;
using System.Data;
using System.Text;
using MariEtFemme.DAL;
using MariEtFemme.DTO;

namespace MariEtFemme.BLL
{
    public class CargoBLL
    {
        /// <summary>
        /// Instância de acesso ao banco.
        /// </summary>
        MySqlDatabaseAccess dataBaseAccess = new MySqlDatabaseAccess();

        /// <summary>     
        /// Consulta informações do usuário por nome.
        /// </summary>     
        /// <param name="post">Nome do usuário que será consultado.</param>
        /// <returns>Informações do usuário encontrado.</returns>
        public CargoCollectionDTO ReadName(string post)
        {
            CargoCollectionDTO cargoCollectionDTO = new CargoCollectionDTO();

            try
            {
                dataBaseAccess.AddParameters("_cargo", post);

                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_cargo_nome");

                foreach (DataRow row in dataTable.Rows)
                {
                    CargoDTO cargoDTO = new CargoDTO();
                    cargoDTO.IdCargo = Convert.ToInt32(row["IdCargo"]);
                    cargoDTO.DescricaoCargo = row["DescricaoCargo"].ToString();

                    cargoCollectionDTO.Add(cargoDTO);
                }

                return cargoCollectionDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar cargos por nome:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }

        /// <summary>     
        /// Consulta informações do usuário por nome.
        /// </summary>     
        /// <param name="_id">Nome do usuário que será consultado.</param>
        /// <returns>Informações do usuário encontrado.</returns>
        public CargoDTO ReadId(int _id)
        {
            try
            {
                dataBaseAccess.AddParameters("_cargo", _id);

                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_cargo_id");

                CargoDTO cargoDTO = new CargoDTO();
                cargoDTO.IdCargo = Convert.ToInt32(dataTable.Rows[0]["IdCargo"]);
                cargoDTO.DescricaoCargo = dataTable.Rows[0]["DescricaoCargo"].ToString();

                return cargoDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar cargos por id:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }
    }
}