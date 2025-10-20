using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfAppColecFotos
{
    /// <summary>
    /// Lógica de interacción para WindowDetalle.xaml
    /// </summary>
    public partial class WindowDetalle : Window
    {
        public WindowDetalle(Ave ave)
        {
            InitializeComponent();

            this.DataContext= ave; // mi contexto es el ave recibido como parámetro 
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
