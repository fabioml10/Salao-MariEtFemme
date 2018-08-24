using System;
using System.Data;
using System.Text;
using MariEtFemme.DAL;
using MariEtFemme.DTO;

namespace MariEtFemme.BLL
{
    public class NotaProdutoBLL
    {
        /// <summary>
        /// Instância de acesso ao banco.
        /// </summary>
        MySqlDatabaseAccess dataBaseAccess = new MySqlDatabaseAccess();

        /// <summary>     
        /// Consulta informações de privilegio por nome.
        /// </summary>     
        /// <param name="nota">Nome do privilegio que será consultado.</param>
        /// <returns>Informações do privilegio encontrado.</returns>
        public NotaProdutoCollectionDTO ReadInvoice(NotaDTO nota)
        {
            NotaProdutoCollectionDTO notaProdutoCollectionDTO = new NotaProdutoCollectionDTO();

            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idNota", nota.IdNota);

                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_nota_produto_nota");

                foreach (DataRow row in dataTable.Rows)
                {
                    NotaProdutoDTO notaProdutoDTO = new NotaProdutoDTO();
                    notaProdutoDTO.Nota = new NotaDTO();
                    notaProdutoDTO.Nota.IdNota = Convert.ToInt32(row["IdNota"]);
                    notaProdutoDTO.QuantidadeComprada = float.Parse(row["QuantidadeComprada"].ToString());
                    notaProdutoDTO.ValorUnitario = Convert.ToDecimal(row["ValorUnitario"]);
                    notaProdutoDTO.ValorTotal = Convert.ToDecimal(row["ValorTotal"]);

                    notaProdutoDTO.Nota.Filial = new FilialDTO();
                    notaProdutoDTO.Nota.Filial.Pessoa.IdPessoa = Convert.ToInt32(row["IdPessoaFilial"]);
                    notaProdutoDTO.Nota.Filial.Pessoa.NomePessoa = row["NomeFilial"].ToString();

                    notaProdutoDTO.Nota.Fornecedor = new FornecedorDTO();
                    notaProdutoDTO.Nota.Fornecedor.Pessoa.IdPessoa = Convert.ToInt32(row["IdPessoaFornecedor"]);
                    notaProdutoDTO.Nota.Fornecedor.Pessoa.NomePessoa = row["NomeFornecedor"].ToString();

                    notaProdutoDTO.Produto = new ProdutoDTO();
                    notaProdutoDTO.Produto.IdProduto = Convert.ToInt32(row["IdProduto"]);
                    notaProdutoDTO.Produto.DescricaoProduto = row["DescricaoProduto"].ToString();

                    notaProdutoDTO.Produto.Unidade = new UnidadeDTO();
                    notaProdutoDTO.Produto.Unidade.IdUnidade = Convert.ToInt32(row["IdProduto"]);
                    notaProdutoDTO.Produto.Unidade.SiglaUnidade = row["SiglaUnidade"].ToString();

                    notaProdutoCollectionDTO.Add(notaProdutoDTO);
                }

                return notaProdutoCollectionDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar produto por nota:\n\n").Append(ex.Message);
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

        public string Create(NotaProdutoDTO notaProduto)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idNota", notaProduto.Nota.IdNota);
                dataBaseAccess.AddParameters("_idProduto", notaProduto.Produto.IdProduto);
                dataBaseAccess.AddParameters("_quantidadeComprada", notaProduto.QuantidadeComprada);
                dataBaseAccess.AddParameters("_valorUnitario", notaProduto.ValorUnitario);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_nota_produto_criar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível cadastrar os produtos na nota fiscal: ").Append(ex.Message);
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
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_id", nota.IdNota);
                dataBaseAccess.AddParameters("_table_name", "nota_produto");
                dataBaseAccess.AddParameters("_colum_name", "IdNota");
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_remover");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível remover os produtos relacionados à nota:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }
    }
}