using System;
using System.Collections.Generic;
using System.Text;

namespace GetMilk.ModelDTO
{
    public class UsuarioColeta
    {
        public List<Coleta> collects { get; set; }

        public bool success { get; set; }

        public String message { get; set; }
    }
}
