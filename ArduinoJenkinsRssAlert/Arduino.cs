using System.IO.Ports;
using System.Threading;

namespace ArduinoJenkinsRssAlert
{
    public class Arduino
    {
        private SerialPort _serial;

        public Arduino(string nomePorta)
        {
            _serial = new SerialPort();
            _serial.PortName = nomePorta;
        }

        public void Abrir()
        {
            _serial.Open();
        }

        public void Fechar()
        {
            _serial.Close();
        }

        public void Inicializar()
        {
            Sucesso();
        }

        public void Sucesso()
        {
            try
            {
                Abrir();
                Enviar(250);
                Thread.Sleep(500);
                Enviar(1);
                Thread.Sleep(500);
            }
            finally
            {
                Fechar();
            }
        }

        public void Falha()
        {
            try
            {
                Abrir();
                Enviar(200);
                Thread.Sleep(500);
                Enviar(180);
                Thread.Sleep(500);
                Fechar();
            }
            finally
            {
                Fechar();
            }
        }

        private void Enviar(int graus)
        {
            if (_serial.IsOpen)
            {
                _serial.WriteLine(graus.ToString());
            }
        }
    }
}