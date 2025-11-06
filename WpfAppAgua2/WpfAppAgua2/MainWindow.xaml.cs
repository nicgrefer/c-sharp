using System;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Media;

namespace WpfAppAgua
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Deposito deposito = new(); // creo una instancia de Deposito

        public MainWindow()
        {
            InitializeComponent();
            this .DataContext = deposito; // asigno el DataContext de la ventana al depósito

            //deposito.Cantidad_Changed += Deposito_Cantidad_Changed;

            // deposito.DepositoVacio += Deposito_DepositoVacio;
            //deposito.Llenar(10);
        }

        private void Deposito_DepositoVacio(object sender, EventArgs e)
        {
            // desahabilito el botón cuando salte el evento DepositoVacio
            botonBeber.IsEnabled = false;
            //botonBeber.Background=new SolidColorBrush(Colors.Coral);
            //MessageBox.Show("Se vació el depósito");
        }
/*
        private void Deposito_Cantidad_Changed(object sender, EventArgs e)
        {
            // cada vez que salta el evento... (porque me he suscrito)
            pbDeposito.Value = deposito.Cantidad; // modifica el valor del progressBar
            //botonBeber.IsEnabled = deposito.Cantidad > 0;
        }
*/
        private void botonBeber_Click(object sender, RoutedEventArgs e)
        {
            deposito.Cantidad--; // se ejecutarán los bloques get y set de la propiedad Cantidad
            // equivalente a: deposito.Cantidad= deposito.Cantidad - 1
        }

        private void botonLlenar_Click(object sender, RoutedEventArgs e)
        {
            deposito.Llenar(5);
            botonBeber.IsEnabled = true;
        }
    }
}
