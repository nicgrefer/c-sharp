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