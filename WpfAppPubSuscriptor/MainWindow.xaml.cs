using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppPubSuscriptor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // 1. Crear una instancia del Publicador
        private readonly ProcesadorDeDatos _procesador;
        public MainWindow()
        {
            InitializeComponent();

            _procesador = new ProcesadorDeDatos();

            // 2. Suscribirse al evento
            // Usamos el operador '+=' para añadir nuestro método manejador
            _procesador.ProcesoCompletado += Procesador_ProcesoCompletado;

        }
        // 3. Método que inicia el proceso (controlador de clic del botón)
        private void BotonIniciar_Click(object sender, RoutedEventArgs e)
        {
            TextBlockResultado.Text = "Procesando...";
            _procesador.IniciarProceso();
        }

        // 4. El Método Manejador (Handler)
        // Este método se ejecutará CUANDO el procesador lance el evento.
        // Debe coincidir con la firma del delegado (object sender, ProcesoCompletadoEventArgs e)
        private void Procesador_ProcesoCompletado(object sender, ProcesoCompletadoEventArgs e)
        {
            // ¡Atención! El evento se lanzó desde un hilo de fondo (Task.Run)
            // No podemos actualizar la UI (TextBlock) directamente desde otro hilo.
            // Debemos usar el 'Dispatcher' de WPF para "enviar" la actualización
            // de vuelta al hilo principal de la UI.

            Dispatcher.Invoke(() =>
            {
                // Ahora sí es seguro actualizar la UI.
                // Usamos los datos que vinieron en el evento (e.Mensaje)
                TextBlockResultado.Text = e.Mensaje;
            });
        }

        // (Opcional pero buena práctica) Darse de baja del evento al cerrar
        // para evitar fugas de memoria si el publicador vive más que el suscriptor.
        protected override void OnClosed(EventArgs e)
        {
            _procesador.ProcesoCompletado -= Procesador_ProcesoCompletado;
            base.OnClosed(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ayuda no disponible por el momento.", "Ayuda", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}