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

namespace WpfAppDockpanelFotos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            /* opcion 1: cargar las imagenes desde local
            Char[] letra = {'a','b','c','d','e','f'};
            foreach (Char c in letra)
            {
                Image img = new Image()
                {
                    Source = new BitmapImage(new Uri($"imagenes/{c}.jpeg", UriKind.Relative)),
                    Width = 100,Height=100,
                };
                var items = new ListBoxItem { Content = img };
                lbAves.Items.Add(items);
            }
            */
            // opcion 2: cargar las imagenes desde internet
            for (int i = 0; i<10; i++)
            {
                Image img = new Image()
                {
                    Source = new BitmapImage(new Uri($"https://picsum.photos/id/{i+50}/200/200")),
                    Width = 100,
                    Height = 100,
                };
                var items = new ListBoxItem { Content = img };
                lbAves.Items.Add(items);
            }

        }


        private void lbAves_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbAves.SelectedItem != null) // Recomendable poner porque aveces se carga esta funcion sin que se haya pulsado nada
                                             // de esta forma verificamos si es nulo o no
            {
                ListBox lb = (ListBox)sender;
                ListBoxItem lbi = (ListBoxItem)lbAves.SelectedItem;
                Image img = (Image)lbi.Content;
                WindowDetalle ventanaDetalle = new WindowDetalle(img);
                ventanaDetalle.ShowDialog();
                // al regresar de la ventana detalle, deseleccionamos el item seleccionado
                lb.SelectedItem = null; 
            }
        }
    }
}