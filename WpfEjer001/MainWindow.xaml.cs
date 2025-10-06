using System; // Necesitas este para Random
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfEjre001
{
    
    public partial class MainWindow : Window
    {
        
        private Random rand = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        
        private void ButtonTrigger_Click(object sender, RoutedEventArgs e)
        {
            Color c;
            Rectangle r;
            Ellipse r2;

           
            int numberOfElements = rand.Next(10, 21); 

            for (int i = 0; i < numberOfElements; i++)
            {
                // Generar un color RGB aleatorio
                c = Color.FromRgb((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256));

                // 1. Incorporar rectángulos al StackPanel (Cuadraditos)
                r = new Rectangle();
                r.Width = 40;
                r.Height = 40;
                r.Margin = new Thickness(5);
                r.Fill = new SolidColorBrush(c);

                // Usar el nombre del StackPanel en XAML: Cuadraditos
                Cuadraditos.Children.Add(r);

                // 2. Incorporar círculos al WrapPanel (Circulitos)
                r2 = new Ellipse();
                r2.Width = 40;
                r2.Height = 40;
                r2.Margin = new Thickness(5);
                r2.Fill = new SolidColorBrush(c);

               
                Circulitos.Children.Add(r2);
            }
        }
    }
}