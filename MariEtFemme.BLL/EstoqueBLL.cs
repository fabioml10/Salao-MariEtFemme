using System;
using System.Data;
using System.Text;
using MariEtFemme.DAL;
using MariEtFemme.DTO;

namespace MariEtFemme.BLL
{
    public class EstoqueBLL
    {
        /// <summary>
        /// Instância de acesso ao banco.
        /// </summary>
        MySqlDatabaseAccess dataBaseAccess = new MySqlDatabaseAccess();
        /// <summary>     
        /// Consulta informações de privilegio por nome.
        /// </summary>     
        /// <param name="filial">Nome do privilegio que será consultado.</param>
        /// <returns>Informações do privilegio encontrado.</returns>
        public EstoqueCollectionDTO ReadFilial()
        {
            EstoqueCollectionDTO estoqueCollectionDTO = new EstoqueCollectionDTO();

            try
            {
                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "spEstoque_Todos");

                foreach (DataRow row in dataTable.Rows)
                {
                    EstoqueDTO estoqueDTO = new EstoqueDTO();
                    estoqueDTO.Quantidade = float.Parse(row["Quantidade"].ToString());

                    estoqueDTO.Filial = new FilialDTO();
                    estoqueDTO.Filial.Pessoa.IdPessoa = Convert.ToInt32(row["IdPessoa"]);
                    estoqueDTO.Filial.Pessoa.NomePessoa = row["NomePessoa"].ToString();

                    estoqueDTO.Produto = new ProdutoDTO();
                    estoqueDTO.Produto.IdProduto = Convert.ToInt32(row["IdProduto"]);
                    estoqueDTO.Produto.DescricaoProduto = row["DescricaoProduto"].ToString();

                    estoqueDTO.Produto.Unidade = new UnidadeDTO();
                    estoqueDTO.Produto.Unidade.SiglaUnidade = row["SiglaUnidade"].ToString();

                    estoqueCollectionDTO.Add(estoqueDTO);
                }

                return estoqueCollectionDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar estoque:\n\n").Append(ex.Message);
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

        public string Create(EstoqueDTO estoque)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idPessoa", estoque.Filial.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_idProduto", estoque.Produto.IdProduto);
                dataBaseAccess.AddParameters("_quantidade", estoque.Quantidade);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_estoque_adicionar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível cadastrar no estoque: ").Append(ex.Message);
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
        /// <param name="estoque">Objeto que contém as informações necessárias para remover o registro do banco.</param>
        public string Delete(EstoqueDTO estoque)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idPessoa", estoque.Filial.Pessoa.IdPessoa);
                dataBaseAccess.AddParameters("_idProduto", estoque.Produto.IdProduto);
                dataBaseAccess.AddParameters("_quantidade", estoque.Quantidade);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_estoque_remover");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível remover no estoque:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }
    }
}
