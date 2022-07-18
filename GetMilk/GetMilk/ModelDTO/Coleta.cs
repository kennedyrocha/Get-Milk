using System;
using System.Collections.Generic;
using System.Text;

namespace GetMilk.ModelDTO
{
    public class Coleta
    {
        public int Id { get; set; }

        public String collectId { get; set; }

        public String farmerId { get; set; }

        public String farmerName { get; set; }

        public String sampleNumber { get; set; }

        public double volume { get; set; }

        public double temperatureTank { get; set; }

        public bool alizarolTest { get; set; }

        public String truckCompartment { get; set; }
        /*
         * 1 - Coletar
         * 2 - Integrar
         * 3 - Não há integrações pendentes
         */
        public int status { get; set; }
    }
}
