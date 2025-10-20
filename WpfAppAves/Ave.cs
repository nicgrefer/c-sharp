using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppColecFotos
{
    public class Ave
    {
        public string? Nombre { get; set; }
        public string? Foto { get; set; }

        public static ObservableCollection<Ave> GetAves()
        {
            return new ObservableCollection<Ave>
            {
                new Ave { Nombre = "Gorrión", Foto = "Imagenes/a.jpeg" },
                new Ave { Nombre = "Estornino", Foto = "Imagenes/b.jpeg" },
                new Ave { Nombre = "Abejaruco", Foto = "Imagenes/c.jpeg" },
                new Ave { Nombre = "Tórtola", Foto = "Imagenes/d.jpeg" },
                new Ave { Nombre = "Carbonero", Foto = "Imagenes/e.jpeg" },
                new Ave { Nombre = "Jilguero", Foto = "Imagenes/f.jpeg" }
            };
        }
    }
}
