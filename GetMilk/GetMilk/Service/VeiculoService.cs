using GetMilk.ModelDTO;
using GetMilk.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GetMilk.Service
{
    public class VeiculoService : Service
    {
        public async Task<String> getVeiculos(String company_id)
        {
            var current = Connectivity.NetworkAccess;
            String respostaConteudo = null;

            if (current == NetworkAccess.Internet)
            {
                HttpResponseMessage response = await _client.GetAsync(ApiGetVeiculos + "?company_id=" + company_id);

                if (response.IsSuccessStatusCode)
                {
                    respostaConteudo = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    respostaConteudo = @"{ ""success"": false, ""message"": ""Falha ao buscar veiculos"" }";
                }
            }
            else
            {
                respostaConteudo = @"{ ""success"": false, ""message"": ""Falha ao buscar veiculos"" }";
            }
            return respostaConteudo;
        }

        public async Task verificaVeiculos(List<string> placas)
        {
            VeiculoDB repo = new VeiculoDB();
            bool atualizar = false;

            foreach (var item in placas)
            {
                var veicu = repo.ConsultarAsync(item);

                if (veicu == null)
                {
                    atualizar = true;
                    break;
                }
            }

            if (atualizar)
            {
                await atualizarVeiculos();
            }
        }

        public async Task atualizarVeiculos()
        {
            VeiculoDB repo = new VeiculoDB();

            String usuario_id = App.Current.Properties["UsuarioId"].ToString();

            Usuario usu = new UsuarioDB().Consultar(usuario_id);

            String resposta = await getVeiculos(usu.companyId);


            UsuarioVeiculo veiculos = JsonConvert.DeserializeObject<UsuarioVeiculo>(resposta);

            if (veiculos.success == true)
            {
                foreach (var item in veiculos.vehicles.ToList<Veiculo>())
                {
                    _ = await repo.CadastrarAsync(item);
                }
            }

        }
    }
}
