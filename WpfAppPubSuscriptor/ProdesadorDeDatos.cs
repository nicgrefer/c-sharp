using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppPubSuscriptor
{
    public class ProcesadorDeDatos
    {
        // 2. Definir el delegado y el evento.
        // Usamos el delegado genérico EventHandler<T> para no tener que definir uno propio.
        // <ProcesoCompletadoEventArgs> es el tipo de datos que enviaremos.
        public event EventHandler<ProcesoCompletadoEventArgs> ProcesoCompletado;

        // Método que inicia el trabajo simulado
        public void IniciarProceso()
        {
            // Usamos Task.Run para simular que esto ocurre en otro hilo (background)
            // y no congela la interfaz de usuario (UI).
            Task.Run(() =>
            {
                // Simular un trabajo largo (ej. 5 segundos)
                Thread.Sleep(5000);

                // 3. Lanzar (disparar) el evento
                // Creamos los datos del evento
                ProcesoCompletadoEventArgs args = new ProcesoCompletadoEventArgs("¡Proceso finalizado desde el publicador!");

                // Llamamos al método que dispara el evento
                OnProcesoCompletado(args);
            });
        }

        // Método protegido y virtual (por convención) para lanzar el evento.
        // Comprueba si hay alguien suscrito (si ProcesoCompletado no es null)
        // y luego lo invoca.
        protected virtual void OnProcesoCompletado(ProcesoCompletadoEventArgs e)
        {
            // 'this' es el 'sender' (quién envía el evento)
            ProcesoCompletado?.Invoke(this, e);
        }
    }

}
