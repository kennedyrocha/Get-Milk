using System;
using System.Collections.Generic;
using System.Text;

namespace GetMilk.ModelDTO
{
    public class Veiculo
    {
        public int Id { get; set; }

        public String vehicleManufacturer { get; set; }

        public String vehicleModel { get; set; }

        public String vehicleLicensePlate { get; set; }

        public String vehicleFuel { get; set; }
    }
}
