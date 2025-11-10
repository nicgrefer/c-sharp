using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace WpfAppSurtidorAsync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        // --- Gestión de Estado ---
        private enum EstadoSurtidor
        {
            Listo,          // Esperando. Se puede fijar importe.
            MangueraDescolgada, // Manguera descolgada. Listo para surtir.
            Surtiendo,        // Surtiendo gasolina.
            Completado        // Surtido completado.
        }

        private EstadoSurtidor _currentState;
        private CancellationTokenSource _cts; // Para poder cancelar la tarea de surtir

        // --- Propiedades para Data Binding (INotifyPropertyChanged) ---

        private decimal _importeSolicitado;
        public decimal ImporteSolicitado
        {
            get => _importeSolicitado;
            set { _importeSolicitado = value; OnPropertyChanged(); }
        }

        private decimal _importeSurtido;
        public decimal ImporteSurtido
        {
            get => _importeSurtido;
            set { _importeSurtido = value; OnPropertyChanged(); }
        }
        private decimal ImportePendiente;

        private string _textoEstado;
        public string TextoEstado
        {
            get => _textoEstado;
            set { _textoEstado = value; OnPropertyChanged(); }
        }

        // --- Propiedades de Habilitación de Botones ---

        private bool _puedoASignarImporte;
        public bool PuedoAsignarImporte
        {
            get => _puedoASignarImporte;
            set { _puedoASignarImporte = value; OnPropertyChanged(); }
        }

        private bool _puedoDescolgar;
        public bool PuedoDescolgar
        {
            get => _puedoDescolgar;
            set { _puedoDescolgar = value; OnPropertyChanged(); }
        }

        private bool _puedoSurtir;
        public bool PuedoSurtir
        {
            get => _puedoSurtir;
            set { _puedoSurtir = value; OnPropertyChanged(); }
        }

        private bool _puedoColgar;
        public bool PuedoColgar
        {
            get => _puedoColgar;
            set { _puedoColgar = value; OnPropertyChanged(); }
        }

        // --- Constructor ---
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this; // Imprescindible para que funcionen los {Binding}
            AsignarEstado(EstadoSurtidor.Listo);
        }

        // --- Lógica de los Estados ---
        private void AsignarEstado(EstadoSurtidor newState)
        {
            _currentState = newState;

            // Actualizar estado y habilitar/deshabilitar botones según el estado
            switch (_currentState)
            {
                case EstadoSurtidor.Listo:
                    TextoEstado = "Listo. Fije importe y descuelgue.";
                    PuedoAsignarImporte = true;
                    PuedoDescolgar = true;
                    PuedoSurtir = false;
                    PuedoColgar = false;
                    break;

                case EstadoSurtidor.MangueraDescolgada:
                    TextoEstado = "Manguera descolgada. Pulse 'Surtir'.";
                    PuedoAsignarImporte = true; // Aún puede cambiar el importe
                    PuedoDescolgar = false;
                    PuedoSurtir = ImporteSolicitado > 0; // Solo puede surtir si hay importe
                    PuedoColgar = true;
                    break;

                case EstadoSurtidor.Surtiendo:
                    TextoEstado = "Surtiendo...";
                    PuedoAsignarImporte = false;
                    PuedoDescolgar = false;
                    PuedoSurtir = false;
                    PuedoColgar = true; // Puede colgar para cancelar
                    break;

                case EstadoSurtidor.Completado:
                    TextoEstado = "Surtido completado. Cuelgue la manguera.";
                    PuedoAsignarImporte = false;
                    PuedoDescolgar = false;
                    PuedoSurtir = false;
                    PuedoColgar = true;
                    break;
            }
        }

        // --- Event Handlers de Botones ---

        private void SumarImporte(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Tag.ToString(), out int acumulador))
            {
                ImporteSolicitado += acumulador;
                // Si ya descolgó, habilitar "Surtir"
                if (_currentState == EstadoSurtidor.MangueraDescolgada)
                {
                    PuedoSurtir = ImporteSolicitado > 0;
                }
            }
        }

        private void Descolgar_Click(object sender, RoutedEventArgs e)
        {
            AsignarEstado(EstadoSurtidor.MangueraDescolgada);
        }

        private void Colgar_Click(object sender, RoutedEventArgs e)
        {
            // Si estaba surtiendo, enviar señal de cancelación
            if (_currentState == EstadoSurtidor.Surtiendo)
            {
                _cts?.Cancel(); // Envía la señal de cancelación
                ImportePendiente = ImporteSurtido;
            }

            // Reseteo completo
            ImporteSolicitado = 0;
            ImporteSurtido = 0;
            AsignarEstado(EstadoSurtidor.Listo);
        }

        // --- EL NÚCLEO ASÍNCRONO ---

        private async void Surtir_Click(object sender, RoutedEventArgs e)
        {
            AsignarEstado(EstadoSurtidor.Surtiendo);
            _cts = new CancellationTokenSource(); // Nuevo token para esta operación

            try
            {
                // Llamamos al método asíncrono y esperamos a que termine (await)
                await SurtirAsync(_cts.Token);

                // Si llegamos aquí, la tarea terminó sin ser cancelada
                AsignarEstado(EstadoSurtidor.Completado);
            }
            catch (OperationCanceledException)
            {
                // La tarea fue cancelada (el usuario colgó la manguera)
                TextoEstado = $"Suministro cancelado. [{ImportePendiente:C2}]";
            }
            finally
            {
                _cts.Dispose();
                _cts = null;
            }
        }

        /// <summary>
        /// Simula el proceso de surtir gasolina de forma asíncrona.
        /// </summary>
        private async Task SurtirAsync(CancellationToken token)
        {
            // Simula una tasa de 0.25€ cada 50 milisegundos
            decimal incremento = 0.25m;
            int demora = 50; // simularmdemora en milisegundos

            while (ImporteSurtido < ImporteSolicitado)
            {
                // 1. Comprobar si se ha solicitado la cancelación
                token.ThrowIfCancellationRequested();

                // 2. Simular el paso del tiempo de forma no bloqueante
                await Task.Delay(demora, token);

                // 3. Actualizar el importe surtido
                ImporteSurtido += incremento;

                // Asegurarse de no pasarse del importe deseado
                if (ImporteSurtido > ImporteSolicitado)
                {
                    ImporteSurtido = ImporteSolicitado;
                }
            }
        }

        // --- Implementación de INotifyPropertyChanged ---
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}