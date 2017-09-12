using ArduinoJenkinsAlert.Modelo;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace ArduinoJenkinsAlert
{
    class Program
    {
        static bool situacaoAtualQuebrado = false;
        static Arduino _arduino;

        static void Main(string[] args)
        {
            var cancelar = new CancellationTokenSource();

            var monitor = new Monitor();
            monitor.ReceberFeeds += Receber;
            monitor.IniciarAsync(cancelar.Token, args[0]);

            _arduino = new Arduino(args[1]);
            _arduino.Inicializar();

            Console.Read();
            cancelar.Cancel();
        }

        static void Receber(List<Job> jobs)
        {
            // Percorre todos os jobs se algum diferente de 'blue' está com falha
            var quebrou = jobs.Where(item => item.Cor != "disabled")
                              .Any(item => !item.Cor.Contains("blue"));

            // Só aciona o robo se mudou a situação...
            if (situacaoAtualQuebrado == quebrou)
            {
                return;
            }

            situacaoAtualQuebrado = quebrou;
            if (quebrou)
            {
                Console.WriteLine("Falha");
                _arduino.Falha();
            }
            else
            {
                Console.WriteLine("Ambiente restaurado");
                _arduino.Sucesso();
            }
        }
    }
}
