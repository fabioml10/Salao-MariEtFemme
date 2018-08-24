using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public static class Session
    {
        public static FuncionarioDTO LoggedUser { get; set; }
    }

    public static class ErrorMessage
    {
        public static string MensagemErro { get ; set; }
    }

    public static class SessionServices
    {
        public static List<string> ListaDeServicos { get; set; }

        public static ServicoCollectionDTO ListaDeServicosObjeto { get; set; }

        public static ClienteCollectionDTO ListaClientes { get; set; }
    }
}
