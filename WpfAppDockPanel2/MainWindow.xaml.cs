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

namespace WpfAppDockPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    // Subscribe to the PreviewKeyDown event
    // (se genera con el tab el método asociado)
    // Los eventos PreviewXXX son eventos tunneling
    // y se propagan de la raíz hasta el senfer
    // en este caso la raíz es el  mismo Window, sin embargo
    // si se generase desde un botón, el evento se propagaría
    // en el arbol de la GUI desde Window hasta el botón
    // en contraposicion a los de tipo burbuja como KeyDown que se
    // propagan hacia arriba hasta la raíz
    // el evento Click nos se propaga en burbuja
    //La troria de los REUTED EVENTS//


    public partial class MainWindow : Window
    {
        private bool permitirSalir = false;
        public MainWindow()
        {
            InitializeComponent();
            this.PreviewKeyDown += Window_PreviewKeyDown;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            permitirSalir = true;
            App.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!permitirSalir)
            {
                e.Cancel = true;
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                MessageBox.Show("¡Has pulsado F3!");
                e.Handled = true;
            }
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            UserControl control = new UserControl1();
            MainContentArea.Content = control;
            MainContentArea.Visibility = Visibility.Visible;
            MainFrame.Visibility = Visibility.Collapsed;

        }

        private void ListBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {
            /*
             * No se puede hacer así porque Window1 no hereda de UserControl, sino de Window.
             * System.InvalidOperationException: 'Window debe ser la raíz del árbol. No se puede agregar Window como elemento secundario de Visual.'
            var control = new Window1();
            MainContentArea.Content = control;
            */

            /*
            Window1 window = new Window1();
           // window.botonCerrar.IsEnabled = false; // No es una ventana modal y no puede contener un DialogResult
            window.Show(); 
            */

            
            var window2 = new Window1();
            window2.Owner = this; // Opcional, para que la ventana hija esté centrada sobre la padre
            bool? respuesta = window2.ShowDialog(); // para que sea modal
            MessageBox.Show(window2.Respuesta);
            
        }

        private void ListBoxItem_Selected_2(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Page1());
            MainContentArea.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
        }
    }
}