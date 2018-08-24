using System;
using MySql.Data.MySqlClient;
using System.Data;

namespace MariEtFemme.DAL
{
    public class MySqlDatabaseAccess
    {
        /// <summary>     
        /// Cria uma conexão com o banco.     
        /// </summary>         
        /// <returns>String de conexão com o banco.</returns>
        private MySqlConnection Connect()
        {
            try
            {
                return new MySqlConnection(Properties.Settings.Default.connectionString);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
            }

        }

        /// <summary>     
        /// Instância da coleção de parâmetros.     
        /// </summary>  
        private MySqlParameterCollection mySqlParameterCollection = new MySqlCommand().Parameters;

        /// <summary>     
        /// Limpa a coleção de parâmetros.     
        /// </summary>
        public void ClearParameters()
        {
            mySqlParameterCollection.Clear();
        }

        /// <summary>     
        /// Adiciona parâmetros na coleção de parâmetros.     
        /// </summary>     
        /// <param name="parameterName">Nome do parâmetro</param>
        /// <param name="parameterValue">Valor do parâmetro</param>
        public void AddParameters(string parameterName, object parameterValue)
        {
            mySqlParameterCollection.Add(new MySqlParameter(parameterName, parameterValue));
        }

        /// <summary>     
        /// Executa um comando no banco.     
        /// </summary>     
        /// <param name="commandType">Define se é um comando SQL ou uma Stored Procedure</param>
        /// <param name="command">Comando SQL ou nome da Stored Procedure</param>
        /// <returns>Retorna um objeto sinalizando que a execução foi bem sucedida.</returns>
        public string ExecuteQuery(CommandType commandType, string command)
        {
            MySqlConnection mySqlConnection = null;
            MySqlCommand mySqlCommand = null;
            try
            {
                mySqlConnection = Connect();
                mySqlConnection.Open();
                mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandType = commandType;
                mySqlCommand.CommandText = command;
                mySqlCommand.CommandTimeout = 7200;

                foreach (MySqlParameter mySqlParameter in mySqlParameterCollection)
                {
                    mySqlCommand.Parameters.Add(new MySqlParameter(mySqlParameter.ParameterName, mySqlParameter.Value));
                }

                mySqlCommand.Parameters["_message"].Direction = ParameterDirection.Output;
                mySqlCommand.ExecuteNonQuery();
                return mySqlCommand.Parameters["_message"].Value.ToString();
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>     
        /// Faz uma consulta no banco.     
        /// </summary>     
        /// <param name="commandType">Define se é um comando SQL ou uma Stored Procedure</param>
        /// <param name="command">Comando SQL ou nome da Stored Procedure</param>
        /// <returns>Retorna uma tabela com os dados solicitados.</returns>
        public DataTable Consult(CommandType commandType, string command)
        {
            MySqlConnection mySqlConnection = null;
            MySqlCommand mySqlCommand = null;
            try
            {
                mySqlConnection = Connect();
                mySqlConnection.Open();
                mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandType = commandType;
                mySqlCommand.CommandText = command;
                mySqlCommand.CommandTimeout = 7200;

                foreach (MySqlParameter mySqlParameter in mySqlParameterCollection)
                {
                    mySqlCommand.Parameters.Add(new MySqlParameter(mySqlParameter.ParameterName, mySqlParameter.Value));
                }

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
                DataTable dataTable = new DataTable();
                mySqlDataAdapter.Fill(dataTable);

                return dataTable;
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}