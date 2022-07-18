using System;
using System.Collections.Generic;
using System.Text;

namespace GetMilk.ModelDTO
{
    public class UsuarioServico
    {
        public List<Servico> services { get; set; }

        public bool success { get; set; }

        public String message { get; set; }
    }
}
