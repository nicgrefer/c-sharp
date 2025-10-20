using System.Collections.ObjectModel;
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

namespace WpfAppColecFotos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Ave> Aves { get; set; }
        public MainWindow()
        {
            /*
             * Si declaro en XAML el DataContext:
            // DataContext="{Binding RelativeSource={RelativeSource Self}}"
            * tengo que iniciar Aves en el constructor antes de InitializeComponent
            // Aves = Ave.GetAves();
            */
            InitializeComponent();

        }

        private void listBoxFotos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxFotos.SelectedItem != null)
            {
                Ave ave = (Ave)listBoxFotos.SelectedItem;
                var windowDetalle = new WindowDetalle(ave);
                windowDetalle.Owner = this;
                windowDetalle.ShowDialog();
                listBoxFotos.SelectedItem = null;
            }
        }

        private void botonCargarFotos_Click(object sender, RoutedEventArgs e)
        {
            //listBoxFotos.ItemsSource = Ave.GetAves();
            Aves = Ave.GetAves();
            this.DataContext = this;
        }
    }
}