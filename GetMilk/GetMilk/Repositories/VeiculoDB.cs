using GetMilk.Database;
using GetMilk.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetMilk.Repositories
{
    public class VeiculoDB
    {
        public BancoContext Banco { get; set; }

        public VeiculoDB()
        {
            Banco = new BancoContext();
        }

        public Veiculo ConsultarAsync(String licensePlate)
        {
            return Banco.Veiculos.Where(a => a.vehicleLicensePlate == licensePlate).FirstOrDefault();
        }

        public async Task<bool> CadastrarAsync(Veiculo veiculo)
        {
            Veiculo ve = ConsultarAsync(veiculo.vehicleLicensePlate);

            if (ve == null)
            {
                Banco.Veiculos.Add(veiculo);
                int linhas = await Banco.SaveChangesAsync();
                return (linhas > 0) ? true : false;
            }
            else
            {
                return true;
            }
        }
    }
}
