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
            var error = false;

            do
            {
                try
                {
                    var monitor = new Monitor();
                    monitor.ReceberFeeds += Receber;
                    monitor.IniciarAsync(cancelar.Token, args[0]);

                    _arduino = new Arduino(args[1]);
                    _arduino.Inicializar();
                    error = false;
                }
                catch
                {
                    error = true;
                    Console.WriteLine("Erro ao inicializar. Tentando novamente em 60 segundos...");
                    Thread.Sleep(60000);
                }
            } while (error);
            Console.Read();
            cancelar.Cancel();
        }

        static void Receber(List<Job> jobs)
        {
            try
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
            catch
            {
                Console.WriteLine("Erro ao consultar situação do Jenkins.");
            }
        }
    }
}
