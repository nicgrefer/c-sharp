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
        private async void btnOtroHilo_Click(object sender, RoutedEventArgs e)
        {
            lstDesayuno.Items.Clear();
            MostrarMensaje("Hilo principal sigue ejecutándose...");
            await TrabajoEnSegundoPlanoAsync();
            MostrarMensaje("Hilo principal ha terminado.");
        }

        // es recomendable terminar los métodos asincrónicos con "Async" para facilitar su identificación
        // el Task indica que el método es asincrónico y no devuelve ningún valor (una promesa)
        // en vez de poner void, se pone Task para métodos asincrónicos
        private async Task TrabajoEnSegundoPlanoAsync()
        {
            MostrarMensaje("Hilo secundario está trabajando...");
            // Se pone await para indicar que se espera a que termine la tarea asincrónica
            // y así no bloquear el hilo principal (UI)
            await Task.Delay(3000); // Simula trabajo asincrónico (no bloquea la UI)
            MostrarMensaje("Hilo secundario ha terminado su trabajo.");
        }

    }

}