using System;
using System.Collections.Generic;
using System.Text;

namespace GetMilk.ModelDTO
{
    public class Usuario
    {
        public int Id { get; set; }

        public String documentCPF { get; set; }

        public String password { get; set; }

        public String userId { get; set; }

        public String userName { get; set; }

        public String userLastname { get; set; }

        public String companyId { get; set; }

        public DateTime time { get; set; }

        public Boolean success { get; set; }

        public String message { get; set; }
    }
}
