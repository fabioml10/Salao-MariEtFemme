using System;
using System.Data;
using System.Text;
using MariEtFemme.DAL;
using MariEtFemme.DTO;

namespace MariEtFemme.BLL
{
    public class ProdutoBLL
    {
        /// <summary>
        /// Instância de acesso ao banco.
        /// </summary>
        MySqlDatabaseAccess dataBaseAccess = new MySqlDatabaseAccess();

        /// <summary>     
        /// Consulta informações de privilegio por nome.
        /// </summary>     
        /// <param name="produto">Nome do privilegio que será consultado.</param>
        /// <returns>Informações do privilegio encontrado.</returns>
        public ProdutoCollectionDTO ReadName(string produto)
        {
            ProdutoCollectionDTO produtoCollectionDTO = new ProdutoCollectionDTO();

            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_produto", produto);

                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_produto_nome");

                foreach (DataRow row in dataTable.Rows)
                {
                    ProdutoDTO produtoDTO = new ProdutoDTO();
                    produtoDTO.IdProduto = Convert.ToInt32(row["IdProduto"]);
                    produtoDTO.DescricaoProduto = row["DescricaoProduto"].ToString();

                    produtoDTO.Unidade = new UnidadeDTO();
                    produtoDTO.Unidade.IdUnidade = Convert.ToInt32(row["IdUnidade"]);
                    produtoDTO.Unidade.SiglaUnidade = row["SiglaUnidade"].ToString();
                    produtoDTO.Unidade.DescricaoUnidade = row["DescricaoUnidade"].ToString();

                    produtoCollectionDTO.Add(produtoDTO);
                }

                return produtoCollectionDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar produto por nome:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }

        //Teste do relatório
        public DataTable ReadNameTeste(string produto)
        {
            ProdutoCollectionDTO produtoCollectionDTO = new ProdutoCollectionDTO();

            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_produto", produto);

                DataTable dataTable = new DataTable();
                return dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_produto_nome");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar produto por nome:\n\n").Append(ex.Message);
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
        public string Create(ProdutoDTO produto)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_descricaoProduto", produto.DescricaoProduto);
                dataBaseAccess.AddParameters("_idUnidade", produto.Unidade.IdUnidade);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_produto_criar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível cadastrar o produto: ").Append(ex.Message);
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
        /// <param name="produto">Objeto que contém as informações necessárias para remover o registro do banco.</param>
        public string Delete(ProdutoDTO produto)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_id", produto.IdProduto);
                dataBaseAccess.AddParameters("_table_name", "produto");
                dataBaseAccess.AddParameters("_colum_name", "IdProduto");
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_remover");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível remover o produto:\n\n").Append(ex.Message);
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
        /// <param name="produto">Objeto que contém as informações necessárias para atualizar o registro no banco.</param>
        public string Update(ProdutoDTO produto)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idProduto", produto.IdProduto);
                dataBaseAccess.AddParameters("_descricaoProduto", produto.DescricaoProduto);
                dataBaseAccess.AddParameters("_idUnidade", produto.Unidade.IdUnidade);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_produto_modificar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível alterar o produto: ").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }
    }
}