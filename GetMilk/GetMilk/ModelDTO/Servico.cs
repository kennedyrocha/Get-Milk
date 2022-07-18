using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace GetMilk.ModelDTO
{
    public class Servico
    {
        public int Id { get; set; }

        public String serviceId { get; set; }

        public String licensePlate { get; set; }

        public bool Fechado { get; set; }

        public int odometerStart { get; set; }

        public int odometerEnd { get; set; }

        public String vehicleManufacturer { get; set; }

        public String vehicleModel { get; set; }

        /*
         * 1 - Integrar inicio
         * 2 - Integrar fim
         * 3 - Não há integrações pendentes
         */
        public int status { get; set; }

        public String NomeBotao { get; set; }
    }
}
