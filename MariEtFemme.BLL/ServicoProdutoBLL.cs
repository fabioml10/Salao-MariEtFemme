using System;
using System.Data;
using System.Text;
using MariEtFemme.DAL;
using MariEtFemme.DTO;

namespace MariEtFemme.BLL
{
    public class ServicoProdutoBLL
    {
        /// <summary>
        /// Instância de acesso ao banco.
        /// </summary>
        MySqlDatabaseAccess dataBaseAccess = new MySqlDatabaseAccess();

        /// <summary>     
        /// Consulta informações de privilegio por nome.
        /// </summary>     
        /// <param name="servico">Nome do privilegio que será consultado.</param>
        /// <returns>Informações do privilegio encontrado.</returns>
        public ServicoProdutoCollectionDTO ReadService(ServicoDTO servico)
        {
            ServicoProdutoCollectionDTO servicoProdutoCollectionDTO = new ServicoProdutoCollectionDTO();

            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idServico", servico.IdServico);

                DataTable dataTable = new DataTable();
                dataTable = dataBaseAccess.Consult(CommandType.StoredProcedure, "sp_servico_produto_servico");

                foreach (DataRow row in dataTable.Rows)
                {
                    ServicoProdutoDTO servicoProdutoDTO = new ServicoProdutoDTO();
                    servicoProdutoDTO.Servico = new ServicoDTO();
                    servicoProdutoDTO.Servico.IdServico = Convert.ToInt32(row["IdServico"]);
                    servicoProdutoDTO.Servico.DescricaoServico = row["DescricaoServico"].ToString();

                    servicoProdutoDTO.Produto = new ProdutoDTO();
                    servicoProdutoDTO.Produto.IdProduto = Convert.ToInt32(row["IdProduto"]);
                    servicoProdutoDTO.Produto.DescricaoProduto = row["DescricaoProduto"].ToString();
                    servicoProdutoDTO.Produto.Consumo = float.Parse(row["Consumo"].ToString());

                    servicoProdutoDTO.Produto.Unidade = new UnidadeDTO();
                    servicoProdutoDTO.Produto.Unidade.IdUnidade = Convert.ToInt32(row["IdProduto"]);
                    servicoProdutoDTO.Produto.Unidade.SiglaUnidade = row["SiglaUnidade"].ToString();
                    servicoProdutoDTO.Produto.Unidade.DescricaoUnidade = row["DescricaoUnidade"].ToString();

                    servicoProdutoCollectionDTO.Add(servicoProdutoDTO);
                }                

                return servicoProdutoCollectionDTO;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível consultar produto por servico:\n\n").Append(ex.Message);
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

        public string Create(ServicoProdutoDTO servicoProduto)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_idServico", servicoProduto.Servico.IdServico);
                dataBaseAccess.AddParameters("_idProduto", servicoProduto.Produto.IdProduto);
                dataBaseAccess.AddParameters("_consumo", servicoProduto.Produto.Consumo);
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_servico_produto_criar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível cadastrar os produtos no serviço: ").Append(ex.Message);
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
        /// <param name="servico">Objeto que contém as informações necessárias para remover o registro do banco.</param>
        public string Delete(ServicoDTO servico)
        {
            try
            {
                dataBaseAccess.ClearParameters();
                dataBaseAccess.AddParameters("_id", servico.IdServico);
                dataBaseAccess.AddParameters("_table_name", "servico_produto");
                dataBaseAccess.AddParameters("_colum_name", "IdServico");
                dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_remover");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível remover os produtos relacionados ao serviço:\n\n").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }
    }
}