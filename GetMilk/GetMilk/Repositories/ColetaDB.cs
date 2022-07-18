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
    public class ColetaDB
    {
        public BancoContext Banco { get; set; }

        public ColetaDB()
        {
            Banco = new BancoContext();
        }

        public async Task<List<Coleta>> PesquisarAsync()
        {
            return await Banco.Coletas.Where(a=> a.sampleNumber == null).ToListAsync();
        }

        public async Task<bool> CadastrarAsync(Coleta coleta)
        {
            Coleta col = ConsultarAsync(coleta.collectId);

            if (col == null)
            {
                coleta.status = 1;
                Banco.Coletas.Add(coleta);
                int linhas = await Banco.SaveChangesAsync();
                return (linhas > 0) ? true : false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> AtualizarAsync(Coleta coleta)
        {
            Banco.Coletas.Update(coleta);
            int linhas = await Banco.SaveChangesAsync();
            return (linhas > 0) ? true : false;
        }

        public Coleta ConsultarAsync(String collectId)
        {
            return Banco.Coletas.Where(a => a.collectId == collectId).FirstOrDefault();
        }

        public async Task<List<Coleta>> PesquisarPendenteIntegrarAsync()
        {
            return await Banco.Coletas.Where(a => (a.status == 2)).ToListAsync();
        }

    }
}
