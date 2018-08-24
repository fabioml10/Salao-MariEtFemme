using System;
using System.Data;
using System.Text;
using MariEtFemme.DAL;

namespace MariEtFemme.BLL
{
    public class SessionBLL
    {
        /// <summary>
        /// Instância de acesso ao banco.
        /// </summary>
        MySqlDatabaseAccess dataBaseAccess = new MySqlDatabaseAccess();

        /// <summary>     
        /// Consulta informações do usuário por nome.
        /// </summary>     
        /// <param name="name">Nome do usuário que será consultado.</param>
        /// <returns>Informações do usuário encontrado.</returns>
        public DataTable LastInsertId()
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.Text, "select last_insert_id();");
                return dataTable;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar último ID inserido:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }
    }
}