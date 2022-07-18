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
    public class ColetaService : Service
    {
        public async Task integrarColetas()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                ColetaDB repo = new ColetaDB();
                String usuario_id = App.Current.Properties["UsuarioId"].ToString();

                List<Coleta> lista = await repo.PesquisarPendenteIntegrarAsync();

                if (lista.Count > 0)
                {
                    foreach (var item in lista)
                    {
                        await IntegraColetaUnico(item);
                    }
                }
            }
        }

        public async Task IntegraColetaUnico(Coleta col)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                var teste = JsonConvert.SerializeObject(new
                {
                    collectId = col.collectId,
                    sampleNumber = col.sampleNumber,
                    volume = col.volume,
                    temperatureTank = col.temperatureTank,
                    alizarolTest = col.alizarolTest,
                    truckCompartment = col.truckCompartment,
                });

                HttpContent content = content = new StringContent(JsonConvert.SerializeObject(new { collectId = col.collectId,
                                                                                                    sampleNumber = col.sampleNumber,
                                                                                                    volume = col.volume,
                                                                                                    temperatureTank = col.temperatureTank,
                                                                                                    alizarolTest = col.alizarolTest,
                                                                                                    truckCompartment = col.truckCompartment,
                                                                                                }), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(ApiControllerColeta, content);

                String respostaConteudo = null;

                if (response.IsSuccessStatusCode)
                {
                    respostaConteudo = await response.Content.ReadAsStringAsync();
                    RespostaAPI respo = JsonConvert.DeserializeObject<RespostaAPI>(respostaConteudo);

                    if (respo.success == true)
                    {
                        ColetaDB repo = new ColetaDB();
                        col.status = 3;
                        await repo.AtualizarAsync(col);
                    }
                }
            }
        }

        public async Task<String> ColetasUsuario(String usuarioId)
        {
            var current = Connectivity.NetworkAccess;
            String respostaConteudo = null;

            if (current == NetworkAccess.Internet)
            {
                HttpResponseMessage response = await _client.GetAsync(ApiGetColetas + "?user_id=" + usuarioId);

                if (response.IsSuccessStatusCode)
                {
                    respostaConteudo = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    respostaConteudo = @"{ ""success"": false, ""message"": ""Falha ao buscar coletas"" }";
                }
            }
            else
            {
                respostaConteudo = @"{ ""success"": false, ""message"": ""Falha ao buscar coletas"" }";
            }

            return respostaConteudo;
        }
    }
}
