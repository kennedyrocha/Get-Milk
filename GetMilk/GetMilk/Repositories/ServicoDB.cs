using GetMilk.Database;
using GetMilk.ModelDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetMilk.Repositories
{
    public class ServicoDB
    {
        public BancoContext Banco { get; set; }

        public ServicoDB()
        {
            Banco = new BancoContext();
        }

        public async Task<bool> CadastrarAsync(Servico servico)
        {
            Servico ser = ConsultarAsync(servico.serviceId);

            if (ser == null)
            {
                VeiculoDB veiRepo = new VeiculoDB();
                Veiculo vei = veiRepo.ConsultarAsync(servico.licensePlate);

                if (vei != null)
                {
                    servico.vehicleModel = vei.vehicleModel;
                    servico.vehicleManufacturer = vei.vehicleManufacturer;
                }

                servico.Fechado = false;
                servico.NomeBotao = "Iniciar";
                Banco.Servicos.Add(servico);
                int linhas = await Banco.SaveChangesAsync();
                return (linhas > 0) ? true : false;
            }
            else
            {
                return true;
            }
        }

        public Servico ConsultarAsync(String serviceId)
        {
            return Banco.Servicos.Where(a => a.serviceId == serviceId).FirstOrDefault();
        }

        public async Task<List<Servico>> PesquisarAsync()
        {
            return await Banco.Servicos.Where(a => a.Fechado == false).ToListAsync();
        }

        public async Task UpdateAsync(Servico servico)
        {
            if (servico.status == 3)
            {
                servico.status = 3;
            }
            else
            {
                if (servico.odometerEnd > 0)
                {
                    servico.Fechado = true;
                    servico.status = 2;
                }
                else
                {
                    servico.Fechado = false;
                    servico.status = 1;
                    servico.NomeBotao = "Finalizar";
                }
            }
            _ = Banco.Servicos.Update(servico);
            _ = await Banco.SaveChangesAsync();
        }

        public async Task DeleteAll()
        {
            List<Servico> lista = await PesquisarAsync();

            foreach (var item in lista)
            {
                Banco.Servicos.Remove(item);
                await Banco.SaveChangesAsync();
            }
        }

        public async Task<List<Servico>> PesquisarPendenteIntegrarAsync()
        {
            return await Banco.Servicos.Where(a => (a.status == 1 || a.status == 2)).ToListAsync();
        }
    }
}
