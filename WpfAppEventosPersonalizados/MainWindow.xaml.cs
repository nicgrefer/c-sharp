using System;
using System.Windows;
using System.Windows.Media;

namespace WpfAppEventosPersonalizados
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Caldera caldera = new Caldera();
        Color[] colores = { Colors.Green, Colors.Gold, Colors.Red }; //Correcto,Alerta,Peligro
        public MainWindow()
        {
            InitializeComponent();

            IniciarCaldera();
        }

        private void IniciarCaldera()
        {
            lbInforme.Items.Add("Inicio " + DateTime.Now.ToLongTimeString());
            caldera.CambioTemperatura += caldera2_CambioTemperatura;
            caldera.CambioEstado += caldera_CambioEstado;
            caldera.StopCaldera += Caldera_StopCaldera;
        }

        private void Caldera_StopCaldera(object sender, EventArgs e)
        {
            lbInforme.Items.Add("STOP " + DateTime.Now.ToLongTimeString());
            caldera.CambioTemperatura -= caldera2_CambioTemperatura;
            caldera.CambioEstado -= caldera_CambioEstado;
            caldera.StopCaldera -= Caldera_StopCaldera;
        }

        void caldera_CambioEstado(object sender, CalderaArgumentosEvento e)
        {
            lbInforme.Items.Add($"{e.EstadoCaldera,10}\t{e.Hora.ToLongTimeString()}");
        }

        void caldera2_CambioTemperatura(object sender, CalderaArgumentosEvento e)
        {
            txtbTemperatura.Text = e.GradosC.ToString("00");
            txtbTemperatura.Background = new SolidColorBrush(colores[(int)e.EstadoCaldera]);
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            caldera.Temperatura = (int)e.NewValue;
        }
    }
}
