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

        // 1. Declara el evento en la clase
        public event EventHandler? TrabajoEnSegundoPlanoTerminado;

        private void btnOtroHilo_Click(object sender, RoutedEventArgs e)
        {
            lstDesayuno.Items.Clear();
            MostrarMensaje("Hilo principal sigue ejecutándose...");

            // 2. Suscribe el manejador al evento
            TrabajoEnSegundoPlanoTerminado += MainWindow_TrabajoEnSegundoPlanoTerminado;

            Thread hilo = new Thread(TrabajoEnSegundoPlanoConEvento);
            hilo.Start();

            MostrarMensaje("Hilo principal ha terminado.");
        }

        // 3. Método que lanza el evento al terminar
        private void TrabajoEnSegundoPlanoConEvento()
        {
            Dispatcher.Invoke(() => MostrarMensaje("Hilo secundario está trabajando..."));
            Thread.Sleep(3000); // Simula trabajo
            Dispatcher.Invoke(() => MostrarMensaje("Hilo secundario ha terminado su trabajo."));

            // Lanza el evento
            TrabajoEnSegundoPlanoTerminado?.Invoke(this, EventArgs.Empty);
        }

        // 4. Manejador del evento
        private void MainWindow_TrabajoEnSegundoPlanoTerminado(object? sender, EventArgs e)
        {
            // Siempre usa Dispatcher para actualizar la UI desde el hilo secundario
            Dispatcher.Invoke(() => MostrarMensaje("Evento: El hilo secundario ha notificado su finalización."));
        }
    }
}