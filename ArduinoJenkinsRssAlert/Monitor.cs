using ArduinoJenkinsRssAlert.Modelo.Atom;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ArduinoJenkinsRssAlert
{
    public class Monitor
    {
        public delegate void FeedDelegate(Feed Feed);
        public event FeedDelegate ReceberFeeds;

        /// <summary>
        /// Inicia monitoramento
        /// </summary>
        public async void IniciarAsync(CancellationToken token, string url)
        {
            while (!token.IsCancellationRequested)
            {
                var feed = await AnalizarAsync(url);
                ReceberFeeds?.Invoke(feed);

                Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// Busca jobs quebrados
        /// </summary>
        private async Task<Feed> AnalizarAsync(string url)
        {
            var cliente = new HttpClient();
            var feedXml = await cliente.GetStringAsync(url);
            var buffer = Encoding.UTF8.GetBytes(feedXml);

            var serializador = new XmlSerializer(typeof(Feed));
            using (var leitor = new MemoryStream(buffer))
            {
                var feed = (Feed)serializador.Deserialize(leitor);

                return feed;
            }
        }
    }
}
