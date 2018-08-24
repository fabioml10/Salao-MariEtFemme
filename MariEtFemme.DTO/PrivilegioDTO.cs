using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class PrivilegioDTO
    {
        public int IdPrivilegio { get; set; }
        public string DescricaoPrivilegio { get; set; }
    }

    public class PrivilegioCollectionDTO : List<PrivilegioDTO>
    {
    }
}