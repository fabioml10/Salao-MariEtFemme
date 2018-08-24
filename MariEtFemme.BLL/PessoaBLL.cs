using System;
using System.Text;
using System.Data;
using MariEtFemme.DAL;
using MariEtFemme.DTO;

namespace MariEtFemme.BLL
{
    public class PessoaBLL
    {
        /// <summary>
        /// Instância de acesso ao banco.
        /// </summary>
        MySqlDatabaseAccess dataBaseAccess = new MySqlDatabaseAccess();
        public PessoaDTO PreencherPessoa(DataRow row)
        {
            PessoaDTO pessoaDTO = new PessoaDTO();
            pessoaDTO.IdPessoa = Convert.ToInt32(row["IdPessoa"]);
            pessoaDTO.NomePessoa = row["NomePessoa"].ToString();
            pessoaDTO.TipoPessoa = Convert.ToBoolean(row["TipoPessoa"]);
            pessoaDTO.DescricaoTipoPessoa = row["DescricaoTipoPessoa"].ToString();
            pessoaDTO.Comentarios = row["Comentarios"].ToString();
            pessoaDTO.DataCadastro = Convert.ToDateTime(row["DataCadastro"]);

            if (pessoaDTO.TipoPessoa)
            {
                pessoaDTO.PessoaFisica.Nascimento = Convert.ToDateTime(row["Nascimento"]);
                pessoaDTO.PessoaFisica.Genero = Convert.ToBoolean(row["Genero"]);
            }
            else
            {
                pessoaDTO.PessoaJuridica.RazaoSocial = row["RazaoSocial"].ToString();
                pessoaDTO.PessoaJuridica.CNPJ = row["CNPJ"].ToString();
            }

            #region Endereço
            pessoaDTO.Endereco.Rua = row["Rua"].ToString();
            pessoaDTO.Endereco.Numero = row["Numero"].ToString();
            pessoaDTO.Endereco.Bairro = row["Bairro"].ToString();
            pessoaDTO.Endereco.Cidade = row["Cidade"].ToString();

            pessoaDTO.Endereco.Estado.IdEstado = Convert.ToInt32(row["IdEstado"]);
            pessoaDTO.Endereco.Estado.SiglaEstado = row["SiglaEstado"].ToString();
            #endregion

            #region Contato
            pessoaDTO.Contato.Telefone1 = row["Telefone1"].ToString();
            pessoaDTO.Contato.WhatsApp1 = Convert.ToBoolean(row["WhatsApp1"]);
            pessoaDTO.Contato.Telefone2 = row["Telefone2"].ToString();
            pessoaDTO.Contato.WhatsApp2 = Convert.ToBoolean(row["WhatsApp2"]);
            pessoaDTO.Contato.Email = row["Email"].ToString();
            pessoaDTO.Contato.Telefone3 = row["Telefone3"].ToString();
            pessoaDTO.Contato.WhatsApp3 = Convert.ToBoolean(row["WhatsApp3"]);

            pessoaDTO.Contato.Operadora1.IdOperadora = Convert.ToInt32(row["IdOperadora1"]);
            pessoaDTO.Contato.Operadora1.DescricaoOperadora = row["DescricaoOperadora1"].ToString();

            pessoaDTO.Contato.Operadora2.IdOperadora = Convert.ToInt32(row["IdOperadora2"]);
            pessoaDTO.Contato.Operadora2.DescricaoOperadora = row["DescricaoOperadora2"].ToString();

            pessoaDTO.Contato.Operadora3.IdOperadora = Convert.ToInt32(row["IdOperadora3"]);
            pessoaDTO.Contato.Operadora3.DescricaoOperadora = row["DescricaoOperadora3"].ToString();
            #endregion

            return pessoaDTO;
        }
        private void PreencherObjetoPessoa(PessoaDTO pessoaDTO)
        {
            dataBaseAccess.AddParameters("_nomePessoa", pessoaDTO.NomePessoa);
            dataBaseAccess.AddParameters("_tipoPessoa", pessoaDTO.TipoPessoa);
            dataBaseAccess.AddParameters("_comentarios", pessoaDTO.Comentarios);

            if (pessoaDTO.TipoPessoa)
            {
                dataBaseAccess.AddParameters("_nascimento", pessoaDTO.PessoaFisica.Nascimento);
                dataBaseAccess.AddParameters("_genero", pessoaDTO.PessoaFisica.Genero);

                dataBaseAccess.AddParameters("_razaoSocial", null);
                dataBaseAccess.AddParameters("_cnpj", null);
            }
            else
            {
                dataBaseAccess.AddParameters("_razaoSocial", pessoaDTO.PessoaJuridica.RazaoSocial);
                dataBaseAccess.AddParameters("_cnpj", pessoaDTO.PessoaJuridica.CNPJ);

                dataBaseAccess.AddParameters("_nascimento", null);
                dataBaseAccess.AddParameters("_genero", null);
            }

            dataBaseAccess.AddParameters("_rua", pessoaDTO.Endereco.Rua);
            dataBaseAccess.AddParameters("_numero", pessoaDTO.Endereco.Numero);
            dataBaseAccess.AddParameters("_bairro", pessoaDTO.Endereco.Bairro);
            dataBaseAccess.AddParameters("_cidade", pessoaDTO.Endereco.Cidade);
            dataBaseAccess.AddParameters("_idEstado", pessoaDTO.Endereco.Estado.IdEstado);

            dataBaseAccess.AddParameters("_telefone1", pessoaDTO.Contato.Telefone1);
            dataBaseAccess.AddParameters("_idOperadora1", pessoaDTO.Contato.Operadora1.IdOperadora);
            dataBaseAccess.AddParameters("_whatsApp1", pessoaDTO.Contato.WhatsApp1);
            dataBaseAccess.AddParameters("_telefone2", pessoaDTO.Contato.Telefone2);
            dataBaseAccess.AddParameters("_idOperadora2", pessoaDTO.Contato.Operadora2.IdOperadora);
            dataBaseAccess.AddParameters("_whatsApp2", pessoaDTO.Contato.WhatsApp2);
            dataBaseAccess.AddParameters("_telefone3", pessoaDTO.Contato.Telefone3);
            dataBaseAccess.AddParameters("_idOperadora3", pessoaDTO.Contato.Operadora3.IdOperadora);
            dataBaseAccess.AddParameters("_whatsApp3", pessoaDTO.Contato.WhatsApp3);
            dataBaseAccess.AddParameters("_email", pessoaDTO.Contato.Email);

            dataBaseAccess.AddParameters("_message", ErrorMessage.MensagemErro);
        }

        /// <summary>     
        /// Cria um novo registro no banco.     
        /// </summary>     
        /// <param name="pessoaDTO">Objeto que contém as informações necessárias para criar o registro no banco.</param>
        public string Create(PessoaDTO pessoaDTO)
        {
            try
            {
                PreencherObjetoPessoa(pessoaDTO);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_pessoa_criar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível cadastrar a pessoa: ").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }

        /// <summary>     
        /// Cria um novo registro no banco.     
        /// </summary>     
        /// <param name="pessoaDTO">Objeto que contém as informações necessárias para criar o registro no banco.</param>
        public string Update(PessoaDTO pessoaDTO)
        {
            try
            {
                dataBaseAccess.AddParameters("_idPessoa", pessoaDTO.IdPessoa);
                PreencherObjetoPessoa(pessoaDTO);
                return dataBaseAccess.ExecuteQuery(CommandType.StoredProcedure, "sp_pessoa_modificar");
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append("Não foi possível modificar a pessoa: ").Append(ex.Message);
                throw new Exception(message.ToString());
            }
            finally
            {
                dataBaseAccess.ClearParameters();
            }
        }
    }
}