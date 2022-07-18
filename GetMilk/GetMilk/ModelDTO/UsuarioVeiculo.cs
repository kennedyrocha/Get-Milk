using System;
using System.Collections.Generic;
using System.Text;

namespace GetMilk.ModelDTO
{
    public class UsuarioVeiculo
    {
        public List<Veiculo> vehicles { get; set; }
        
        public bool success { get; set; }
        
        public String message { get; set; }
    }
}
