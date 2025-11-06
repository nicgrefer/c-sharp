using System.Diagnostics;
using System.Windows;

namespace DesayunoSincrono
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MostrarMensaje(string mensaje)
        {
            lstDesayuno.Items.Add(mensaje);
        }

        private void btnOtroHilo_Click(object sender, RoutedEventArgs e)
        {
            lstDesayuno.Items.Clear();
            MostrarMensaje("Hilo principal sigue ejecutándose...");

            // Pasar un callback al hilo secundario
            Thread hilo = new Thread(() => TrabajoEnSegundoPlano(() =>
            {
                // Este código se ejecuta cuando el hilo termina
                Dispatcher.Invoke(() => MostrarMensaje("Callback: El hilo secundario ha terminado."));
            }));
            hilo.Start();

            MostrarMensaje("Hilo principal ha terminado.");
        }

        // Método que acepta un callback
        private void TrabajoEnSegundoPlano(Action callback)
        {
            Dispatcher.Invoke(() => MostrarMensaje("Hilo secundario está trabajando..."));
            Thread.Sleep(3000); // Simula trabajo
            Dispatcher.Invoke(() => MostrarMensaje("Hilo secundario ha terminado su trabajo."));

            // Llamar al callback al finalizar
            callback?.Invoke();
        }
    }
}