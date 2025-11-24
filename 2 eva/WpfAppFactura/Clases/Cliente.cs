using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppFactura.Clases
{
    internal class Cliente
    {
        public int IdCliente { get; set; }
        public string NIF { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Domicilio { get; set; } = string.Empty;
    }
}
