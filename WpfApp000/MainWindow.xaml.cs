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

namespace WpfApp000
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Simulamos una base de datos
        List<string> provincias = new List<string>() { "Salamanca", "Burgos", "Valladolid", "etc"};

        public MainWindow()
        {
            InitializeComponent();
            LlenarComboProvincias();
        }

        private void LlenarComboProvincias()
        {
           /* foreach (string provincia in provincias)
            {
                ComboBoxItem prov = new ComboBoxItem();
                prov.Content = provincia;
                cbProvincias.Items.Add(prov);
            }
           */
           cbProvincias.ItemsSource = provincias;
        }

        private void botSalir_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            //button.Content = "Pulsaste";
            button.Background = Brushes.Red;
            MessageBoxResult respuesta = MessageBox.Show("Desea salir de la aplicación?", "Atencion", MessageBoxButton.YesNo,MessageBoxImage.Question);
            if (respuesta == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
            else
            {
                button.Background = Brushes.LightGray;
            }

        }
    }
}