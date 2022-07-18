using GetMilk.ModelDTO;
using GetMilk.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GetMilk.Service
{
    public class ServicoService : Service
    {
        public async Task integrarServicos()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                ServicoDB repo = new ServicoDB();
                String usuario_id = App.Current.Properties["UsuarioId"].ToString();

                List<Servico> lista = await repo.PesquisarPendenteIntegrarAsync();

                if (lista.Count > 0)
                {
                    foreach (var item in lista)
                    {
                        await IntegraServicoUnico(item);
                    }
                }
            }
        }

        public async Task IntegraServicoUnico(Servico ser)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                HttpContent content = null;

                if (ser.status == 1)
                {
                    content = new StringContent(JsonConvert.SerializeObject(new { serviceId = ser.serviceId, odometerStart = ser.odometerStart }), Encoding.UTF8, "application/json");
                }
                else if (ser.status == 2)
                {
                    content = new StringContent(JsonConvert.SerializeObject(new { serviceId = ser.serviceId, odometerEnd = ser.odometerEnd }), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage response = await _client.PostAsync(ser.status == 1 ? ApiStartService : ApiEndService, content);

                String respostaConteudo = null; ;

                if (response.IsSuccessStatusCode)
                {
                    respostaConteudo = await response.Content.ReadAsStringAsync();
                    RespostaAPI respo = JsonConvert.DeserializeObject<RespostaAPI>(respostaConteudo);

                    if (respo.success == true)
                    {
                        ServicoDB repo = new ServicoDB();
                        await repo.UpdateAsync(ser);
                    }
                }
            }
        }

        public async Task<String> ServicosUsuario(String usuarioId)
        {
            var current = Connectivity.NetworkAccess;
            String respostaConteudo = null;

            if (current == NetworkAccess.Internet)
            {
                HttpResponseMessage response = await _client.GetAsync(ApiGetServices + "?user_id=" + usuarioId);

                if (response.IsSuccessStatusCode)
                {
                    respostaConteudo = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    respostaConteudo = @"{ ""success"": false, ""message"": ""Falha ao buscar serviços"" }";
                }
            }
            else
            {
                respostaConteudo = @"{ ""success"": false, ""message"": ""Falha ao buscar serviços"" }";
            }

            return respostaConteudo;
        }
    }
}
