using GetMilk.ModelDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetMilk.Database
{
    public class BancoContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Coleta> Coletas { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }

        public BancoContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={Constants.DatabasePath}");
        }
    }
}
