using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WpfAppFactura.Clases
{
    internal class DatosEmpresa
    {
        public string Nombre { get; set; } = "Gregorio Fernandez";
        public string CIF { get; set; } = "B12345678";
        public string Domicilio { get; set; } = "C. Guardería, 1";
        public string Ciudad { get; set; } = "47007 Valladolid";
        public string Telefono { get; set; } = "983 47 16 00";
        public string RutaLogo { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo.png");
    }
}