using System.Collections.Generic;

namespace MariEtFemme.DTO
{
    public class CargoDTO
    {
        public int IdCargo { get; set; }
        public string DescricaoCargo { get; set; }
    }

    public class CargoCollectionDTO : List<CargoDTO>
    {
    }
}