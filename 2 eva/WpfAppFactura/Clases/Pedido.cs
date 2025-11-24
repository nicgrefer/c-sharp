using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppFactura.Clases
{
    internal class Pedido
    {
        public int IdPedido { get; set; }
        public DateTime Fecha { get; set; }
        public int IdCliente { get; set; }
    }
}
