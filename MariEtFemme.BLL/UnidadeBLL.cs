using System;
using System.Data;
using System.Text;
using MariEtFemme.DAL;
using MariEtFemme.DTO;

namespace MariEtFemme.BLL
{
    public class UnidadeBLL
    {
        /// <summary>
        /// Instância de acesso ao banco.
        /// </summary>
        MySqlDatabaseAccess dataBaseAccess = new MySqlDatabaseAccess();

        /// <summary>     
        /// Consulta informações do usuário por nome.
        /// </summary>     
        /// <param name="unidade">Nome do usuário que será consultado.</param>
        /// <returns>Informações do usuário encontrado.</returns>
        public UnidadeCollectionDTO ReadName(string unidade)
        {
            UnidadeCollectionDTO unidadeCollectionDTO = new UnidadeCollectionDTO();

            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_siglaUnidade", unidade);

                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_unidade_nome");

                foreach (DataRow row in dataTable.Rows)
                {
                    UnidadeDTO unidadeDTO = new UnidadeDTO();
                    unidadeDTO.IdUnidade = Convert.ToInt32(row["IdUnidade"]);
                    unidadeDTO.SiglaUnidade = row["SiglaUnidade"].ToString();
                    unidadeDTO.DescricaoUnidade = row["DescricaoUnidade"].ToString();

                    unidadeCollectionDTO.Add(unidadeDTO);
                }

                return unidadeCollectionDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar unidade por nome:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }
    }
}