using ArduinoJenkinsAlert.Modelo;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ArduinoJenkinsAlert
{
    public class Monitor
    {
        public delegate void FeedDelegate(List<Job> jobs);

        public event FeedDelegate ReceberFeeds;

        /// <summary>
        /// Inicia monitoramento
        /// </summary>
        public async void IniciarAsync(CancellationToken token, string url)
        {
            while (!token.IsCancellationRequested)
            {
                Thread.Sleep(5000);

                var jobs = await AnalizarAsync(url);
                ReceberFeeds?.Invoke(jobs);
            }
        }

        /// <summary>
        /// Busca jobs quebrados
        /// </summary>
        private async Task<List<Job>> AnalizarAsync(string url)
        {
            try
            {
                var cliente = new HttpClient();
                var json = await cliente.GetStringAsync(url);

                dynamic consulta = JsonConvert.DeserializeObject(json);

                var saida = new List<Job>();
                foreach (var item in consulta.jobs)
                {
                    var novo = new Job()
                    {
                        Nome = item.name,
                        Cor = item.color
                    };
                    saida.Add(novo);
                }

                return saida;
            }
            catch
            {
                return null;
            }
        }
    }
}