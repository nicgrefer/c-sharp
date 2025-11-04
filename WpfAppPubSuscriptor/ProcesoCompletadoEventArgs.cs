using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppPubSuscriptor
{
    // 1. (Opcional pero recomendado) Definir los datos que el evento enviará.
    // Hereda de EventArgs.
    public class ProcesoCompletadoEventArgs : EventArgs
    {
        public string Mensaje { get; }

        public ProcesoCompletadoEventArgs(string mensaje)
        {
            Mensaje = mensaje;
        }
    }

}
