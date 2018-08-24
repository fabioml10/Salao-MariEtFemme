using System;
using System.Data;
using System.Text;
using MariEtFemme.DAL;
using MariEtFemme.DTO;

namespace MariEtFemme.BLL
{
    public class NotaBLL
    {
        /// <summary>
        /// Instância de acesso ao banco.
        /// </summary>
        MySqlDatabaseAccess dataBaseAccess = new MySqlDatabaseAccess();

        /// <summary>     
        /// Consulta informações de privilegio por nome.
        /// </summary>     
        /// <returns>Informações do privilegio encontrado.</returns>
        public NotaCollectionDTO ReadAll()
        {
            NotaCollectionDTO notaCollectionDTO = new NotaCollectionDTO();

            try
            {
                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_nota_todos");

                foreach (DataRow row in dataTable.Rows)
                {
                    NotaDTO notaDTO = new NotaDTO();
                    notaDTO.IdNota = Convert.ToInt32(row["IdNota"]);
                    notaDTO.NumeroNota = row["NumeroNota"].ToString();
                    notaDTO.DataNota = Convert.ToDateTime(row["DataNota"]);

                    notaDTO.Filial = new FilialDTO();
                    notaDTO.Filial.Pessoa.IdPessoa = Convert.ToInt32(row["IdPessoaFilial"]);
                    notaDTO.Filial.Pessoa.NomePessoa = row["NomeFilial"].ToString();

                    notaDTO.Fornecedor = new FornecedorDTO();
                    notaDTO.Fornecedor.Pessoa.IdPessoa = Convert.ToInt32(row["IdPessoaFornecedor"]);
                    notaDTO.Fornecedor.Pessoa.NomePessoa = row["NomeFornecedor"].ToString();
                    notaCollectionDTO.Add(notaDTO);
                }

                return notaCollectionDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar notas fiscais:\n\n").Append(ex.Message);
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

        public string Create(NotaDTO nota)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idPessoaFilial", nota.Filial.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_idPessoaFornecedor", nota.Fornecedor.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_numeroNota", nota.NumeroNota);
                dataBaseAccess.AddParameters("_dataNota", nota.DataNota);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_nota_criar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível cadastrar a nota fiscal: ").Append(ex.Message);
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
        /// <param name="nota">Objeto que contém as informações necessárias para atualizar o registro no banco.</param>
        public string Update(NotaDTO nota)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idNota", nota.IdNota);
                dataBaseAccess.AddParameters("_idPessoaFilial", nota.Filial.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_idPessoaFornecedor", nota.Fornecedor.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_numeroNota", nota.NumeroNota);
                dataBaseAccess.AddParameters("_dataNota", nota.DataNota);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_nota_modificar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível alterar a nota fiscal: ").Append(ex.Message);
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
        /// <param name="nota">Objeto que contém as informações necessárias para remover o registro do banco.</param>
        public string Delete(NotaDTO nota)
        {
            try
            {
                dataBaseAccess.AddParameters("_id", nota.IdNota);
                dataBaseAccess.AddParameters("_table_name", "nota");
                dataBaseAccess.AddParameters("_colum_name", "IdNota");
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_remover");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível remover a nota:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }
    }
}