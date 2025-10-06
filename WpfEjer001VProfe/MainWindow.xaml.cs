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

namespace WpfAppScrollView
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
        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var rand = new Random();
            Color c;
            Rectangle r;
            Ellipse r2;
            for (int i = 0; i < rand.Next(20); i++)
            {
                c = Color.FromRgb((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256));
                //incorporar rectángulos al StackPanel
                r = new Rectangle();
                r.Width = 40; r.Height = 40;
                r.Margin = new Thickness(5);
                r.Fill = new SolidColorBrush(c);
                sp1.Children.Add(r);
                //incorporar círculos al WrapPanel
                r2 = new Ellipse();
                r2.Width = 40; r2.Height = 40;
                r2.Margin = new Thickness(5);
                r2.Fill = new SolidColorBrush(c);
                wp1.Children.Add(r2);
            }
        }

    }
}