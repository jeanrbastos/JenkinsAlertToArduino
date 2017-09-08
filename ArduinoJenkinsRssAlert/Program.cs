using ArduinoJenkinsRssAlert.Modelo.Atom;
using System;
using System.Threading;

namespace ArduinoJenkinsRssAlert
{
    class Program
    {
        static bool situacaoAtualQuebrado = false;
        static Arduino _arduino;

        static void Main(string[] args)
        {
            //https://ci.jenkins.io/rssFailed
            //https://ci.jenkins.io/rssLatest

            var cancelar = new CancellationTokenSource();

            var monitor = new Monitor();
            monitor.ReceberFeeds += Receber;
            monitor.IniciarAsync(cancelar.Token, args[0]);

            _arduino = new Arduino(args[1]);
            _arduino.Inicializar();
            
            Console.Read();
            cancelar.Cancel();
        }

        static void Receber(Feed feed)
        {
            // Verifica se a situação mudou...
            var quebrou = feed.Entry.Count > 0;

            if (situacaoAtualQuebrado == quebrou)
            {
                return;
            }
            situacaoAtualQuebrado = quebrou;

            if (quebrou)
            {
                foreach (var item in feed.Entry)
                {
                    Console.Write(item.Title);
                }
                _arduino.Falha();
            }
            else
            {
                _arduino.Sucesso();
            }
        }
    }
}
