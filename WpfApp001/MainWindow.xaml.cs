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

namespace WpfApp001
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

        private void Button_click(object sender, RoutedEventArgs e)
        {
            inkCanvas1.Strokes.Clear();
            Wp1.Children.Add(new Button{ // Aquí se añade un botón al WrapPanel
                Content = "Botoncillo",
                Height = 80, Padding = new Thickness (20,5,20,5),  
                //Background = new SolidColorBrush (Color.FromRgb (224, 96, 96)),
                Background = Brushes.Crimson}); 
        }

    }
}