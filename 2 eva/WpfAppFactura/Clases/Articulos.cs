using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppFactura.Clases
{
    internal class Articulos
    {
        public int IdArticulo { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int Stock { get; set; }
        public decimal Precio { get; set; }
    }
}
