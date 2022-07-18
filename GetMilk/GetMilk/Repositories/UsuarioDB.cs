using GetMilk.Database;
using GetMilk.ModelDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace GetMilk.Repositories
{
    public class UsuarioDB
    {
        public BancoContext Banco { get; set; }

        public UsuarioDB()
        {
            Banco = new BancoContext();
        }

        public async Task<bool> CadastrarAsync(Usuario usuario)
        {
            Usuario usu = Consultar(usuario.userId);

            if (usu == null)
            {
                Banco.Usuarios.Add(usuario);
                int linhas = await Banco.SaveChangesAsync();
                return (linhas > 0) ? true : false;
            }
            else
            {
                return true;
            }
        }

        public Usuario Consultar(String userId)
        {
            return Banco.Usuarios.Where(a => a.userId == userId).FirstOrDefault();
        }
    }
}
