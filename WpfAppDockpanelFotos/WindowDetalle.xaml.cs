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

namespace WpfAppDockpanelFotos
{
    /// <summary>
    /// Lógica de interacción para WindowDetalle.xaml
    /// </summary>
    public partial class WindowDetalle : Window
    {
        public WindowDetalle(Image image)
        {
            InitializeComponent();
            imgDetalle.Source = image.Source;
        }

        private void imgDetalle_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
