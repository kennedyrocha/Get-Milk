using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using GetMilk.ModelDTO;
using Xamarin.Essentials;

namespace GetMilk.Service
{
    public class UsuarioService : Service
    {
        public async Task<String> Login(String cpf, string senha)
        {
            var current = Connectivity.NetworkAccess;
            String respostaConteudo = null;

            if (current == NetworkAccess.Internet)
            {
                HttpContent content = new StringContent(JsonConvert.SerializeObject(new { login = cpf, password = senha }), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(ApiUrlLogin, content);

                if (response.IsSuccessStatusCode)
                {
                    respostaConteudo = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    respostaConteudo = @"{ ""success"": false, ""message"": ""Falha na tentativa de Login"" }";
                }
            }
            else
            {
                respostaConteudo = @"{ ""success"": false, ""message"": ""Falha, sem acesso a internet!"" }";
            }
            return respostaConteudo;
        }
    }
}
