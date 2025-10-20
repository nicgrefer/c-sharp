using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppColecFotos
{
    public class Coche
    {
        public string? Nombre { get; set; }
        public string? Foto { get; set; }

        public static ObservableCollection<Coche> GetCoches()
        {
            return new ObservableCollection<Coche>
            {
                new Coche
                {
                    Nombre = "BMW i8",
                    Foto = "https://purepng.com/public/uploads/large/purepng.com-bmw-i8-protonic-red-carcarbmwvehicletransport-961524662432w9cyi.png"
                },
                new Coche
                {
                    Nombre = "Audi TT",
                    Foto = "https://purepng.com/public/uploads/large/purepng.com-orange-audi-r8-caraudicarvehicletransport-961524656080cbae9.png"
                },
                new Coche
                {
                    Nombre = "Ferrari California",
                    Foto = "https://upload.wikimedia.org/wikipedia/commons/e/e9/Ferrari_California_Red.png"
                },
                new Coche
                {
                    Nombre = "Mercedes-Benz AMG GT",
                    Foto = "https://assets.stickpng.com/thumbs/580b585b2edbce24c47b2c94.png"
                }
                
            };
        }
    }
}
