namespace MariEtFemme.Tools
{
    public enum EnumApplyAction
    {
        Create,
        CreateClient,
        CreateService,
        CreateEmployee,
        Remove,
        Update,
        UpdateClient,
        UpdateService,
        UpdateEmployee
    }

    public enum EnumPermission
    {
        Administrador = 1,
        Serviços = 2,
        Relatórios = 3,
        Financeiro = 4
    }

    public enum DaysToShow
    {
        One = 1,
        Three = 3,
        Five = 5,
        Seven = 7 
    }
}