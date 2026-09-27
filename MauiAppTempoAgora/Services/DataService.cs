using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisaoDoTempo(string cidade)
        {
            // Atividade 2: Verificar conexão com a Internet
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                throw new Exception("SemConexao");
            }

            string appId = "SUA_CHAVE_API_AQUI";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&units=metric&appid={appId}&lang=pt_br";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                // Atividade 2: Tratamento do StatusCode HTTP
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new Exception("CidadeNaoEncontrada");
                }

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var rascunho = JObject.Parse(json);

                    Tempo tempo = new Tempo
                    {
                        Title = (string)rascunho["name"],
                        Temperature = (string)rascunho["main"]["temp"],
                        Wind = (string)rascunho["wind"]["speed"],       // Atividade 1
                        Humidity = (string)rascunho["main"]["humidity"],
                        Visibility = (string)rascunho["visibility"],   // Atividade 1
                        Description = (string)rascunho["weather"][0]["description"], // Atividade 1
                        Sunrise = (string)rascunho["sys"]["sunrise"],
                        Sunset = (string)rascunho["sys"]["sunset"]
                    };

                    return tempo;
                }
                else
                {
                    throw new Exception("ErroNaRequisicao");
                }
            }
        }
    }
}